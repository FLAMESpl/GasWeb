using System.Collections.Generic;

namespace GasWeb.Shared
{
    public class PageResponse<T>
    {
        public PageResponse(IReadOnlyCollection<T> results, PagingInfo paging)
        {
            Results = results;
            Paging = paging;
        }

        public IReadOnlyCollection<T> Results { get; }
        public PagingInfo Paging { get; }
    }

    public class PagingInfo
    {
        public PagingInfo(int pageNumber, int pageSize, long totalCount)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public long TotalCount { get; }
    }
}
