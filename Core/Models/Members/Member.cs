using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models.MonitoringData;

namespace AliHaydarBase.Api.Core.Models.Members
{
    public class Member
    {
        public Guid Id { get; set; }
        // Names
        public string FullNameArabic { get; set; } = string.Empty;
        public string FullNameEnglish { get; set; } = string.Empty;
        // Basic info
        public string Stage { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string RegisterNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        // Security flags
        public bool IsIdPrinted { get; set; }          // Prevent double printing
        public bool IsBlockedByAdmin { get; set; }     // Admin freeze

        // Issuance info
        public DateTime IssuanceDate { get; set; }
        public DateTime LastYearIdentityRenewal { get; set; }

        // Image
        public string ImageUrl { get; set; }

        // Relations
        public ICollection<PrintHistory> PrintHistories { get; set; }
    }

}