using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;

namespace AliHaydarBase.Api.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request);
        Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
        Task<SystemResponseDto> ResendEmailConfirmation(ResendEmailConfirmationRequestDto request);
        Task<AuthResponseDto> LoginWithRefreshToken(RefreshTokenRequestDto request);
        Task<AuthResponseDto> LogoutDeviceAsync(RefreshTokenRequestDto request);
        Task<AuthResponseDto> LogoutAllDevicesAsync(LogoutAllRequestDto request);
        // till here all is well

        JwtResponseDto ValidateToken(string Token);
        Task<RefreshTokenIntrospectionResponseDto> IntrospectRefreshTokenAsync(string token);
        Task<IEnumerable<ClaimDefinitionResponseDto>> GetAllClaimDefinitionsAsync();
    }
}