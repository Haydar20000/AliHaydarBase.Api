using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Blogs
{
    public class CreateBlogRequestDto
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Guid CategoryId { get; set; }

        public string? TargetRole { get; set; }
        public DateTime? ExpireAt { get; set; }

        public List<CreateBlogImageRequestDto>? Images { get; set; }
    }
}