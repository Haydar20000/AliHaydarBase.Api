using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

    }
}