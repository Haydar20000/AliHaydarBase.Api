
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

        public ExternalLoginRepository(IConfiguration configuration, UserManager<User> userManager, IJwtRepository jwtRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _jwtRepository = jwtRepository;
        }
        public async Task<AuthResponseDto> GoogleLogin(GoogleLoginRequestDto request)
        {
            var response = new AuthResponseDto();

            if (string.IsNullOrWhiteSpace(request.idToken))
            {
                response.Errors.Add("Invalid Authentication");
                response.IsSuccessful = false;
                return response;
            }

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>
            {
                _configuration["Google:ClientId"] ?? string.Empty,
                _configuration["Google:ANDROIDd"] ?? string.Empty,
                _configuration["Google:Web"] ?? string.Empty
            }
                };

                var googlePayload = await GoogleJsonWebSignature.ValidateAsync(request.idToken, settings);
                if (googlePayload == null || string.IsNullOrWhiteSpace(googlePayload.Email))
                {
                    response.Errors.Add("Invalid Authentication");
                    response.IsSuccessful = false;
                    return response;
                }

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

                var roles = await _userManager.GetRolesAsync(user);
                var jwtResponse = _jwtRepository.GenerateAccessToken(new JwtRequestDto
                {
                    User = user,
                    Roles = roles
                });

                if (!jwtResponse.IsSuccessful || string.IsNullOrWhiteSpace(jwtResponse.Token))
                {
                    response.IsSuccessful = false;
                    response.Errors.Add("Access token generation failed");
                    response.Errors.AddRange(jwtResponse.Errors);
                    return response;
                }

                var refreshToken = _jwtRepository.GenerateRefreshToken();
                if (!refreshToken.IsSuccessful || string.IsNullOrWhiteSpace(refreshToken.RefreshToken))
                {
                    response.IsSuccessful = false;
                    response.Errors.Add("Refresh token generation failed");
                    response.Errors.AddRange(refreshToken.Errors);
                    return response;
                }

                user.RefreshToken = refreshToken.RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    response.IsSuccessful = false;
                    response.Errors.Add("Invalid Authentication");
                    return response;
                }

                response.Token = jwtResponse.Token;
                response.RefreshToken = refreshToken.RefreshToken;
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