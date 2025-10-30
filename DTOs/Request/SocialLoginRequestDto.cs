using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class SocialLoginRequestDto
    {
        public string Provider { get; set; } = string.Empty; // "Google", "Facebook"
        public string ProviderId { get; set; } = string.Empty; // e.g., Google UID
        public string AccessToken { get; set; } = string.Empty; // optional
        public string Email { get; set; } = string.Empty; // optional fallback

    }
}