using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Members
{
    public class IdCardTemplateDto
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;

        public int Year { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public string FrontImage { get; set; } = string.Empty;
        public string BackImage { get; set; } = string.Empty;

        public string FrontLayoutJson { get; set; } = string.Empty;
        public string BackLayoutJson { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public double CardWidthMm { get; set; }
        public double CardHeightMm { get; set; }
        public int Dpi { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}