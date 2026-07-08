using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public int Start { get; set; }
        public int End { get; set; }
        public PagedResult() { } // <-- Add this

        public PagedResult(List<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            Start = (Page - 1) * PageSize + 1;
            End = Math.Min(Page * PageSize, TotalCount);
        }
    }
}