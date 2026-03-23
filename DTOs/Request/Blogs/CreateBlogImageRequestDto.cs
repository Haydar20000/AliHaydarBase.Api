using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class CreateBlogImageRequestDto
    {
        public required string Url { get; set; }
        public bool IsGallery { get; set; } = false;
        public string? Caption { get; set; }
    }
}