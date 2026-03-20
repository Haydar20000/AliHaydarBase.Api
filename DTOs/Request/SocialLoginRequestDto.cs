using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class SocialLoginRequestDto
    {
        public string Provider { get; set; }
        public string IdToken { get; set; }
        public string DeviceId { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }

    }
}