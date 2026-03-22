using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Blogs
{
    public class BlogResponseDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Content { get; set; }

        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public required string CategoryIcon { get; set; }

        public required string UserId { get; set; }
        public string? PublishedBy { get; set; }

        public bool IsVisible { get; set; }
        public string? TargetRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }

        public bool AuditLevel1Approved { get; set; }
        public bool AuditLevel2Approved { get; set; }
        public bool AuditLevel3Approved { get; set; }

        public List<BlogImageResponseDto> Images { get; set; } = [];
    }
}