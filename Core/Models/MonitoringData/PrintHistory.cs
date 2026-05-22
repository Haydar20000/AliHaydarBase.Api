using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.Members;

namespace AliHaydarBase.Api.Core.Models.MonitoringData
{
    public class PrintHistory
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid MemberId { get; set; }
        public Member Member { get; set; } = default!;
        public string MemberName { get; set; } = string.Empty;

        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;

        public string PrintMode { get; set; } = string.Empty; // "FrontOnly", "ManualFlip", "Duplex"

        public DateTime PrintedAtUtc { get; set; }

        public string FrontThumbnailBase64 { get; set; } = string.Empty;
        public string BackThumbnailBase64 { get; set; } = string.Empty;
    }

}