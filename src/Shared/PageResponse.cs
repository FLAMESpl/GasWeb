using System.Collections.Generic;

namespace GasWeb.Shared
{
    public class PageResponse<T>
    {
        public PageResponse() { }

        public PageResponse(IReadOnlyCollection<T> results, PagingInfo paging)
        {
            Results = results;
            Paging = paging;
        }

        public IReadOnlyCollection<T> Results { get; set; }
        public PagingInfo Paging { get; set; }
    }

    public class PagingInfo
    {
        public PagingInfo() { }

        public PagingInfo(int pageNumber, int pageSize, long totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
    }
}
