using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Blogs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}