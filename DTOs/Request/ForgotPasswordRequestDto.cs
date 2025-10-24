using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class ForgotPasswordRequestDto
    {
        [Required(ErrorMessage = DkString.EmailError01)]
        [EmailAddress(ErrorMessage = DkString.EmailError02)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = DkString.Email)]
        public string Email { get; set; } = string.Empty;
    }
}