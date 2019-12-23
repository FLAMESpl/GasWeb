using GasWeb.Domain;
using GasWeb.Server.Authentication;
using GasWeb.Server.Schedulers;
using GasWeb.Server.Settings;
using GasWeb.Server.Users;
using GasWeb.Server.Validation;
using GasWeb.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GasWeb.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var postgreSqlSettings = Configuration.GetSection(nameof(PostgreSqlSettings)).Get<PostgreSqlSettings>();
            var bootstrap = new GasWebBootstrap();
            bootstrap.Configure(services);

            services.AddSingleton(postgreSqlSettings);
            services.AddScoped<HttpClientProvider>();
            services.AddHttpClient<HttpClientProvider>();
            services.AddScoped(sp => sp.GetRequiredService<HttpClientProvider>().Get());

            services.AddHostedService<InitializationHostedService>();
            services.AddHostedService<SchedulersHostedService>();
            services.AddScoped<IFacebookAuthenticator, FacebookAuthenticator>();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(new ValidateModelAttribute());
                options.Filters.Add(new ValidationExceptionTranslatorAttribute());
            })
            .AddNewtonsoftJson();

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            services.AddDbContext<GasWebDbContext>(options =>
            {
                options.UseNpgsql(postgreSqlSettings.ConnectionString, npgsql => npgsql
                    .UseNetTopologySuite()
                    .MigrationsAssembly(typeof(GasWebDbContext).Assembly.FullName));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "GasWeb",
                    Contact = new OpenApiContact()
                    {
                        Name = "£ukasz Szafirski",
                        Email = "lukasza568@student.polsl.pl"
                    }
                });

            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = ctx => HandleWithStatusCode(ctx, HttpStatusCode.Forbidden);
                    options.Events.OnRedirectToAccessDenied = ctx => HandleWithStatusCode(ctx, HttpStatusCode.Unauthorized);
                })
                .AddCookie("TempCookie")
                .AddFacebook(options =>
                {
                    options.AppId = "903162020039531";
                    options.AppSecret = "1962fbb0753ad5383a67f7a6b1a29436";
                    options.SignInScheme = "TempCookie";
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Client.Startup>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserContextMiddleware>();
            app.UseRouting();
            app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }

        private Task HandleWithStatusCode(RedirectContext<CookieAuthenticationOptions> context, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;
            return Task.CompletedTask;
        }
    }
}
