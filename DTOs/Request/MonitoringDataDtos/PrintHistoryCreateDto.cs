using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.MonitoringDataDtos
{
    public class PrintHistoryCreateDto
    {
        public Guid MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string PrintMode { get; set; } = string.Empty;
        public string FrontThumbnailBase64 { get; set; } = string.Empty;
        public string BackThumbnailBase64 { get; set; } = string.Empty;
    }

}