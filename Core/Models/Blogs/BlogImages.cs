using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Blogs
{
    public class BlogImages
    {
        public int Id { get; set; }

        // 🌐 Image storage path or CDN link
        public required string Url { get; set; }

        // 📝 Optional caption/description
        public string? Caption { get; set; }

        // 🔗 Foreign key to parent blog
        public int BlogId { get; set; }
        public required Blogs Blog { get; set; }

        // 👤 User who added this image
        public required string UserId { get; set; }

        // ⏰ When the image was added
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}