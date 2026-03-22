using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class BlogQueryParametersRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int? CategoryId { get; set; }
        public string? Role { get; set; }
    }
}