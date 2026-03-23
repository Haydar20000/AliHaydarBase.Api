using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Blogs
{
    public class BlogImages
    {
        public Guid Id { get; set; }

        // 🌐 Image storage path or CDN link
        public required string ImageUrl { get; set; }

        // 📝 Optional caption/description
        public string? Caption { get; set; }

        // 🔗 Foreign key to parent blog
        public Guid BlogId { get; set; }
        public required Blogs Blog { get; set; }

        // 📁 Is this image part of the main content or a gallery?
        public bool IsGallery { get; set; }

        // 👤 User who added this image
        public required Guid UserId { get; set; }

        // ⏰ When the image was added
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // ⏰ When the category was last updated
        public DateTime? UpdatedAt { get; set; }
    }
}