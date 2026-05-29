using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.MonitoringDataDtos
{
    public class PrintHistoryDetailsDto
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string PrintMode { get; set; } = string.Empty;
        public DateTime PrintedAtUtc { get; set; }
        public string FrontThumbnailBase64 { get; set; } = string.Empty;
        public string BackThumbnailBase64 { get; set; } = string.Empty;
        public string ActionType { get; set; } = "Print";
    }

}