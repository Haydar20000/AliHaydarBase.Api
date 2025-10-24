using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AliHaydarBase.Api.Core.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ThirdName { get; set; }
        public string? ForthName { get; set; }
        public string? SureName { get; set; }
        public string? FullName => $"{FirstName} {LastName} {ThirdName} {ForthName} {SureName}".Trim();
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}