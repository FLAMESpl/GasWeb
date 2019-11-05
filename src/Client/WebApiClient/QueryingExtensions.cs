using GasWeb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GasWeb.Client.WebApiClient
{
    internal static class QueryingExtensions
    {
        public static async Task<ServerResponse<List<T>>> GetAllPages<T>(
            Func<int, int, Task<ServerResponse<PageResponse<T>>>> queryExecutor) where T : class
        {
            var pageSize = int.MaxValue;
            var currentPage = 1;
            var results = new List<T>();

            while (true)
            {
                var response = await queryExecutor(currentPage, pageSize);
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
}
