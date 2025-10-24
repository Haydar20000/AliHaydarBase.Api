using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<SystemResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<SystemResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request);
        Task<SystemResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<SystemResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<SystemResponseDto> ResendEmailConfirmation(ResendEmailConfirmationRequestDto request);
        Task<AuthResponseDto> LoginWithRefreshToken(RefreshTokenRequestDto request);
        Task<SystemResponseDto> LogoutDeviceAsync(RefreshTokenRequestDto request);
        Task<SystemResponseDto> LogoutAllDevicesAsync(RefreshTokenRequestDto request);
        JwtResponseDto ValidateToken(string Token);
        Task<RefreshTokenIntrospectionResponseDto> IntrospectRefreshTokenAsync(string token);
    }
}