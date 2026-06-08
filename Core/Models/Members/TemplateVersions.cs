using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Members
{
    public class TemplateVersion
    {
        public Guid Id { get; set; }

        // Foreign key to the main template
        public Guid TemplateId { get; set; }

        // Incremental version number (1, 2, 3, ...)
        public int VersionNumber { get; set; }

        // JSON layouts stored as text
        public string FrontLayoutJson { get; set; } = string.Empty;
        public string BackLayoutJson { get; set; } = string.Empty;

        // When this version was created
        public DateTime CreatedAt { get; set; }

        // Who created it (optional)
        public Guid? CreatedBy { get; set; }

        // Base64 preview image (optional)
        public string? PreviewImageBase64 { get; set; }
    }

}