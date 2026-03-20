using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class VerifyEmailRequestDto
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceId { get; set; }
    }
}