using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Blogs
{
    public class BlogImageResponseDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}