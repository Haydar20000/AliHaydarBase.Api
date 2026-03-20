using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Blogs
{
    public class Categories
    {
        public int Id { get; set; }

        // 🏷️ Category name
        public required string Name { get; set; }

        // 📝 Optional description
        public string? Description { get; set; }

        // 🖼️ Icon name for Flutter (CupertinoIcons)
        public required string IconName { get; set; }

        // 👀 Admin toggle: show/hide category in UI
        public bool IsVisible { get; set; } = true;

        // ⛔ If true, all blogs in this category are hidden
        public bool IsDisabled { get; set; } = false;

        // 👤 User who created/modified this category
        public required string UserId { get; set; }

        // ⏰ When the category was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔗 Relationship: blogs under this category
        public ICollection<Blogs> Blogs { get; set; } = new List<Blogs>();
    }
}