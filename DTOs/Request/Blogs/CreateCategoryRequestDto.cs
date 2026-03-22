using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class CreateCategoryRequestDto
    {
        public required string Name { get; set; }
        // Default icon name for CupertinoIcons
        public string IconName { get; set; } = "app";
        public string? Description { get; set; }
    }
}