using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.DTOs.Response
{
    public class AuthResponseDto
    {
        public bool IsSuccessful { get; set; } = true;
        public List<string> Errors { get; set; } = [];
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public int Code { get; set; } = 0;
    }
}