using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Blogs
{
    public class BlogImageResponseDto
    {
        public Guid Id { get; set; }
        public required string ImageUrl { get; set; }
        public string? Caption { get; set; }
        public bool IsGallery { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}