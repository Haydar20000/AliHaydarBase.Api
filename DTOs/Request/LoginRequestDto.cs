using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string DeviceId { get; set; }
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
    }
}