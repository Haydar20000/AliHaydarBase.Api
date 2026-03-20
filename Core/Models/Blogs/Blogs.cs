using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Blogs
{
    public class Blogs
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CategoryId { get; set; }
        public required Categories Category { get; set; }

        public ICollection<BlogImages> Images { get; set; } = new List<BlogImages>();

        public bool AuditLevel1Approved { get; set; } = true;
        public bool AuditLevel2Approved { get; set; } = true;
        public bool AuditLevel3Approved { get; set; } = true;

        public string? PublishedBy { get; set; }
        public DateTime? ExpireAt { get; set; }
        public bool IsVisible { get; set; } = true;
        public string? TargetRole { get; set; }
    }
}