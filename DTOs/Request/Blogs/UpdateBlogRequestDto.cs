using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class UpdateBlogRequestDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Guid CategoryId { get; set; }

        public bool IsVisible { get; set; }
        public string? TargetRole { get; set; }
        public DateTime? ExpireAt { get; set; }

        public bool AuditLevel1Approved { get; set; }
        public bool AuditLevel2Approved { get; set; }
        public bool AuditLevel3Approved { get; set; }
    }
}