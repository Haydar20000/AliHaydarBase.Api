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
        [Required(ErrorMessage = DkString.EmailError01)]
        [EmailAddress(ErrorMessage = DkString.EmailError02)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = DkString.Email)]
        public string? Email { get; set; } = string.Empty;


        [Required(ErrorMessage = DkString.PasswordError01)]
        [StringLength(256, ErrorMessage = DkString.PasswordError02, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = DkString.Password)]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? Password { get; set; } = string.Empty;

        [Display(Name = DkString.RememberMe)]
        public bool RememberMe { get; set; }
    }
}