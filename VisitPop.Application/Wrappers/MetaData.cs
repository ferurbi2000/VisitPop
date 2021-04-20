

using System.Collections.Generic;

namespace VisitPop.Application.Wrappers
{
    public class MetaData
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPageSize { get; set; }

        public string UrlParams { get; set; }
        public int LinksPerPage { get; set; }

        public int CurrentStartIndex => TotalCount == 0 ? 0 : (PageNumber * PageSize) - PageSize + 1;
        public int CurrentEndIndex => TotalCount == 0 ? 0 : CurrentStartIndex + CurrentPageSize - 1;

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        //public string Filters { get; set; }
    }
}
