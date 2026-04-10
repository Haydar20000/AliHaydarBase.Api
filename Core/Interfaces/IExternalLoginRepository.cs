using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using AliHaydarBase.Api.DTOs.Response.Authentication;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IExternalLoginRepository
    {
        Task<AuthResponseDto> Login(SocialLoginRequestDto request);
        Task<AuthResponseDto> GoogleLogin(string idToken, string deviceId, string ipAddress, string userAgent);
    }
}