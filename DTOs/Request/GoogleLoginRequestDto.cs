using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class GoogleLoginRequestDto
    {
        public string idToken { get; set; } = string.Empty;
    }
}