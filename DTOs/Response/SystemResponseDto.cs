using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response
{
    public class SystemResponseDto
    {
        public bool IsSuccessful { get; set; } = true;
        public IEnumerable<string> Errors { get; set; } = [];
    }
}