using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class CreateBlogImageRequestDto
    {
        public string Url { get; set; }
        public string? Caption { get; set; }
    }
}