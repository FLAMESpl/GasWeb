using GasWeb.Shared;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Client.WebApiClient
{
    public class Endpoint
    {
        protected readonly HttpClient httpClient;

        public Endpoint(HttpClient httpClient, string route)
        {
            this.httpClient = httpClient;
            Route = route;
        }

        public string Route { get; }

        public Task<ServerResponse> Create(object model)
        {
            return httpClient.SendJsonAsync(HttpMethod.Post, Route, model);
        }

        public Task<ServerResponse<T>> Create<T>(object model) where T : class
        {
            return httpClient.SendJsonAsync<T>(HttpMethod.Post, Route, model);
        }

        public Task<ServerResponse> Update(long id, object model)
        {
            return httpClient.SendJsonAsync(HttpMethod.Put, $"{Route}/{id}", model);
        }

        public Task<ServerResponse> Delete(long id)
        {
            return httpClient.SendJsonAsync(HttpMethod.Delete, $"{Route}/{id}");
        }

        public Task<ServerResponse<T>> Get<T>(long id) where T : class
        {
            return httpClient.Get<T>($"{Route}/{id}");
        }

        public Task<ServerResponse<IReadOnlyCollection<T>>> GetList<T>(object queryParameters = null)
        {
            return httpClient.Get<IReadOnlyCollection<T>>(Route, queryParameters);
        }

        public Task<ServerResponse<PageResponse<T>>> GetPage<T>(int pageNumber, int pageSize, object queryParameters = null)
        {
            return httpClient.Get<PageResponse<T>>(Route, new RouteValueDictionary(queryParameters)
            {
                { "pageNumber", pageNumber },
                { "pageSize", pageSize }
            });
        }

        public async Task<ServerResponse<List<T>>> GetAllPages<T>(object queryParameters = null)
        {
            var results = new List<T>();
            var currentPage = 1;
            var pageSize = int.MaxValue;

            while(true)
            {
                var response = await GetPage<T>(currentPage, pageSize, queryParameters);
                if (!response.Successful)
                    return response.To<List<T>>();

                var page = response.Result;
                results.AddRange(page.Results);

                if (results.Count < page.Paging.TotalCount && page.Results.Any())
                {
                    currentPage++;
                }
                else
                {
                    break;
                }

            }

            return ServerResponse.Success(results);
        }
    }

    public class Endpoint<T> : Endpoint where T : class
    {
        public Endpoint(HttpClient httpClient, string route) : base(httpClient, route)
        {
        }

        public Task<ServerResponse<T>> Get(long id) => Get<T>(id);
        public Task<ServerResponse<IReadOnlyCollection<T>>> GetList(object queryParameters = null) => GetList<T>(queryParameters);
        public Task<ServerResponse<PageResponse<T>>> GetPage(int number, int size, object queryParameters = null) => GetPage<T>(number, size, queryParameters);
        public Task<ServerResponse<List<T>>> GetAllPages(object queryParameters = null) => GetAllPages<T>(queryParameters);
    }
}
