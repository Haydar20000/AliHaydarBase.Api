using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.Core.Models.Members
{
    public class IdCardTemplate
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public string CompanyName { get; set; }
        public byte[]? FrontImage { get; set; }
        public byte[]? BackImage { get; set; }
        public string FrontLayoutJson { get; set; }
        public string BackLayoutJson { get; set; }
        public bool IsActive { get; set; }
        public string TemplateName { get; set; }
        public int CardWidthMm { get; set; }
        public int CardHeightMm { get; set; }
        public int Dpi { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<PrintHistory> PrintHistories { get; set; }
    }
}