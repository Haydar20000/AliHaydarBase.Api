using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class ExternalLoginRequestDto
    {
        public string IdToken { get; set; }
        public string Provider { get; set; }   // "Google", "Facebook", "Apple"
        public string DeviceId { get; set; }   // optional, for binding refresh tokens to device
    }
}