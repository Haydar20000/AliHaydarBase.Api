using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response
{
    public class RefreshTokenIntrospectionResponseDto
    {
        public bool IsValid { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; }
        public string Reason { get; set; } = string.Empty;

    }
}