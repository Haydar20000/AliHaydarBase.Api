using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IJwtRepository
    {
        JwtResponseDto GenerateAccessToken(JwtRequestDto request);
        JwtResponseDto GenerateRefreshToken();
        bool ReadJwtToken(string token);
        bool IsTokenValid(string token);

    }
}