using GasWeb.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GasWeb.Client.WebApiClient
{
    public class Endpoint
    {
        private readonly HttpClient httpClient;

        public Endpoint(HttpClient httpClient, string route)
        {
            this.httpClient = httpClient;
            Route = route;
        }

        public string Route { get; }

        public Task Create(object model)
        {
            return httpClient.PostJsonAsync(Route, model);
        }

        public Task Update(long id, object model)
        {
            return httpClient.PutJsonAsync($"{Route}/{id}", model);
        }

        public Task Delete(long id)
        {
            return httpClient.DeleteAsync($"{Route}/{id}");
        }

        public Task<T> Get<T>(long id)
        {
            return httpClient.GetJsonAsync<T>($"{Route}/{id}");
        }

        public Task<IReadOnlyCollection<T>> GetList<T>(object queryParameters = null)
        {
            return httpClient.Get<IReadOnlyCollection<T>>(Route, queryParameters);
        }

        public Task<PageResponse<T>> GetPage<T>(int pageNumber, int pageSize, object queryParameters = null)
        {
            return httpClient.Get<PageResponse<T>>(Route, new RouteValueDictionary(queryParameters)
            {
                { "pageNumber", pageNumber },
                { "pageSize", pageSize }
            });
        }

        public async Task<IReadOnlyCollection<T>> GetAllPages<T>(object queryParameters = null)
        {
            var results = new List<T>();
            var currentPage = 1;
            var pageSize = int.MaxValue;

            while(true)
            {
                var pageResponse = await GetPage<T>(currentPage, pageSize, queryParameters);
                results.AddRange(pageResponse.Results);

                if (results.Count < pageResponse.Paging.TotalCount && pageResponse.Results.Any())
                {
                    currentPage++;
                }
                else
                {
                    break;
                }

            }

            return results;
        }
    }

    public class Endpoint<T> : Endpoint
    {
        public Endpoint(HttpClient httpClient, string route) : base(httpClient, route)
        {
        }

        public Task<T> Get(long id) => Get<T>(id);
        public Task<IReadOnlyCollection<T>> GetList(object queryParameters = null) => GetList<T>(queryParameters);
        public Task<PageResponse<T>> GetPage(int number, int size, object queryParameters = null) => GetPage<T>(number, size, queryParameters);
        public Task<IReadOnlyCollection<T>> GetAllPages(object queryParameters = null) => GetAllPages<T>(queryParameters);
    }
}
