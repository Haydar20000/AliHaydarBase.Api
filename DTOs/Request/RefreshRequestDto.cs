using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}