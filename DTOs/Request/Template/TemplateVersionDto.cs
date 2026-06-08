using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request.Template
{
    public class TemplateVersionDto
    {
        public int VersionNumber { get; set; }
        public string FrontLayoutJson { get; set; } = string.Empty;
        public string BackLayoutJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? PreviewImageBase64 { get; set; }
    }
}