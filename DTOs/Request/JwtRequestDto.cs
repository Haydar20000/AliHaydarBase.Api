using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Models;

namespace AliHaydarBase.Api.DTOs.Request
{
    public class JwtRequestDto
    {
        public required User User { get; set; }
        public required IList<string> Roles { get; set; }
    }
}