using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class UploadBlogImageRequestDto
    {
        public required IFormFile File { get; set; }
        public bool IsGallery { get; set; } = false;
    }
}