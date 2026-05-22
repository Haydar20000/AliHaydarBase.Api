using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;

namespace AliHaydarBase.Api.DTOs.Response.Members
{
    public class PrintHistoryRecord
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public Member Member { get; set; } = default!;
        public Guid TemplateId { get; set; }
        public IdCardTemplate Template { get; set; } = default!;
        public string FrontThumbnailBase64 { get; set; } = string.Empty;
        public string BackThumbnailBase64 { get; set; } = string.Empty;
        public string PrintMode { get; set; } = string.Empty;
        public DateTime PrintedAtUtc { get; set; }
    }

}