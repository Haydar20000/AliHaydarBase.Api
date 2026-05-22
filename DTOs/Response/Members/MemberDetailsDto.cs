using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response.Members
{
    public class MemberDetailsDto
    {
        public Guid Id { get; set; }

        public string FullNameArabic { get; set; }
        public string FullNameEnglish { get; set; }

        public string Stage { get; set; }
        public string RegisterNumber { get; set; }
        public string City { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }

        public string LastYearIdentityRenewal { get; set; }
        public string ImageUrl { get; set; }

        public bool IsIdPrinted { get; set; }
        public bool IsBlockedByAdmin { get; set; }

        public List<PrintHistoryRecord> PrintHistories { get; set; }
    }

}