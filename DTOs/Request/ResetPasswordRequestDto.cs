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
        [Required(ErrorMessage = DkString.PasswordError01)]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = DkString.Password)]
        public string? Password { get; set; }
        [Required(ErrorMessage = DkString.PasswordError03)]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = DkString.ConfirmPassword)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = DkString.OtpError01)]
        [Display(Name = DkString.Otp)]
        public string? Otp { get; set; }

        [Required(ErrorMessage = DkString.EmailError01)]
        [EmailAddress(ErrorMessage = DkString.EmailError02)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = DkString.Email)]
        public string? Email { get; set; }
    }
}