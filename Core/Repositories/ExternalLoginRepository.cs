
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class ExternalLoginRepository : IExternalLoginRepository
    {
        //private GoogleJsonWebSignature.Payload? _googlePayload;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IJwtRepository _jwtRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ExternalLoginRepository(IConfiguration configuration, UserManager<User> userManager, IJwtRepository jwtRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _userManager = userManager;
            _jwtRepository = jwtRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AuthResponseDto> Login(ExternalLoginRequestDto request)
        {
            return request.Provider switch
            {
                "Google" => await GoogleLogin(request.IdToken, request.DeviceId),
                // "Facebook" => await FacebookLogin(request.IdToken, request.DeviceId),
                //"Apple" => await AppleLogin(request.IdToken, request.DeviceId),
                _ => new AuthResponseDto
                {
                    IsSuccessful = false,
                    Errors = new List<string> { "Unsupported provider" }
                }
            };
        }

        public async Task<AuthResponseDto> GoogleLogin(string idToken, string deviceId)
        {
            var response = new AuthResponseDto();

            if (string.IsNullOrWhiteSpace(idToken))
            {
                response.Errors.Add("Invalid Authentication");
                response.IsSuccessful = false;
                return response;
            }

            try
            {
                // 1️⃣ Validate Google token
                var audience = _configuration.GetSection("Google")["Web1"];
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { audience }
                };

                var googlePayload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                if (googlePayload == null || string.IsNullOrWhiteSpace(googlePayload.Email))
                {
                    response.Errors.Add("Invalid Authentication");
                    response.IsSuccessful = false;
                    return response;
                }

                // 2️⃣ Find or create user
                var user = await _userManager.FindByEmailAsync(googlePayload.Email);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = googlePayload.Email,
                        Email = googlePayload.Email,
                        FirstName = googlePayload.Name ?? "Google User"
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, "Visitor");

                    if (!createResult.Succeeded)
                    {
                        response.Errors.AddRange(createResult.Errors.Select(e => e.Description));
                        response.IsSuccessful = false;
                        return response;
                    }
                }

                // 3️⃣ Generate access token
                var roles = await _userManager.GetRolesAsync(user);
                var jwtResponse = _jwtRepository.GenerateAccessToken(new JwtRequestDto
                {
                    User = user,
                    Roles = roles,
                    DeviceId = deviceId
                });

                if (!jwtResponse.IsSuccessful || string.IsNullOrWhiteSpace(jwtResponse.Token))
                {
                    response.IsSuccessful = false;
                    response.Errors.Add("Access token generation failed");
                    response.Errors.AddRange(jwtResponse.Errors);
                    return response;
                }

                // 4️⃣ Generate refresh token
                var refreshTokenResult = _jwtRepository.GenerateRefreshToken();
                if (!refreshTokenResult.IsSuccessful || string.IsNullOrWhiteSpace(refreshTokenResult.RefreshToken))
                {
                    response.IsSuccessful = false;
                    response.Errors.Add("Refresh token generation failed");
                    response.Errors.AddRange(refreshTokenResult.Errors);
                    return response;
                }

                // 5️⃣ Persist refresh token in both User and RefreshTokens table
                user.RefreshToken = refreshTokenResult.RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    response.IsSuccessful = false;
                    response.Errors.Add("Failed to update user with refresh token");
                    return response;
                }

                var refreshTokenEntry = new RefreshTokenEntry
                {
                    Token = refreshTokenResult.RefreshToken,
                    DeviceId = deviceId,
                    UserId = user.Id,
                    ExpiryTime = DateTime.UtcNow.AddDays(7),
                    IsRevoked = false
                };

                await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntry);
                await _unitOfWork.Complete();

                // 6️⃣ Build response
                response.Token = jwtResponse.Token;
                response.RefreshToken = refreshTokenResult.RefreshToken;
                response.IsSuccessful = true;
                response.Errors = new List<string> { "Ok" };

                return response;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.Errors.Add(e.Message);
                return response;
            }
        }

    }
}