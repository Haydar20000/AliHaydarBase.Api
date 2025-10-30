using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class RegisterRequestDtoOld
    {
        [Required(ErrorMessage = DkString.UserFullNameError01)]
        [Display(Name = DkString.UserFullName)]
        public string UserFullName { get; set; } = string.Empty;

        [Required(ErrorMessage = DkString.EmailError01)]
        [EmailAddress]
        [Display(Name = DkString.Email)]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = DkString.PasswordError01)]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = DkString.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = DkString.PasswordError03)]
        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = DkString.PasswordError02, MinimumLength = 8)]
        [Compare("Password", ErrorMessage = DkString.PasswordError04)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = DkString.JobTitleError01)]
        [Display(Name = DkString.JobTitle)]
        public string? Job { get; set; }

        [Required(ErrorMessage = DkString.PhoneNumberError01)]
        [Display(Name = DkString.PhoneNumber)]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [Display(Name = DkString.IdNumber)]
        public string? IdNumber { get; set; }

        [Display(Name = DkString.IdPhoneNumber)]
        [DataType(DataType.PhoneNumber)]
        public string? RPhoneNumber { get; set; }
        // [Required(ErrorMessage = "First Name is required")]
        // [Display(Name = "First Name")]
        // public required string FirstName { get; set; }

        // [Required(ErrorMessage = "Last Name is required")]
        // [Display(Name = "Last Name")]
        // public required string LastName { get; set; }

        //public string? ClientUri { get; set; }
    }
}