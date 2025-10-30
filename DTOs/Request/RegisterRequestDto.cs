using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = DkString.EmailError01)]
        [EmailAddress]
        [Display(Name = DkString.Email)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = DkString.PasswordError01)]
        [DataType(DataType.Password)]
        [Display(Name = DkString.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = DkString.PasswordError03)]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = DkString.PasswordError02, MinimumLength = 8)]
        [Compare("Password", ErrorMessage = DkString.PasswordError04)]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}