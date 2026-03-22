using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class UpdateCategoryRequestDto
    {
        public required string Name { get; set; }
        public string IconName { get; set; } = "app";
        public string? Description { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }
    }
}