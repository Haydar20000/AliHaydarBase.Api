using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Members
{
    public class MemberRowDto
    {
        public Guid Id { get; set; }
        public string FullNameArabic { get; set; }
        public string FullNameEnglish { get; set; }
        public string FullName => FullNameArabic; // backward compatibility

        public string Stage { get; set; }
        public string RegisterNumber { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastYearIdentityRenewal { get; set; }

        public string Status { get; set; }
        public string ImageBase64 { get; set; }

        public bool IsIdPrinted { get; set; }
        public bool IsBlockedByAdmin { get; set; }
    }

}