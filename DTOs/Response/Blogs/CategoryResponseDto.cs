using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Blogs
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? IconName { get; set; }
        public string? Description { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}