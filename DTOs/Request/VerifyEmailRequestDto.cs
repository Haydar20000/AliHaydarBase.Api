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
        [Required(ErrorMessage = DkString.EmailError01)]
        [EmailAddress]
        [Display(Name = DkString.Email)]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = DkString.OtpError01)]
        [Display(Name = DkString.Otp)]
        public string Code { get; set; } = string.Empty;
    }
}