using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class ResetPasswordRequestDto
    {
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Otp { get; set; }
        public string? Email { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceId { get; set; }
    }
}