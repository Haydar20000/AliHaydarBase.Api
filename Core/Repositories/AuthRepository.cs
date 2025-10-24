
using System.Net.Mail;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Mapper;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Dependencies;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtRepository _jwtRepository;
        private readonly IEmailServicesRepository _emailServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthRepository(AliHaydarDbContext context,
            UserManager<User> userManager,
            IJwtRepository jwtRepository,
            IEmailServicesRepository emailServices,
            IConfiguration configuration,
            IUnitOfWork unitOfWork) : base(context)
        {
            _userManager = userManager;
            _jwtRepository = jwtRepository;
            _emailServices = emailServices;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<SystemResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var error = new List<string>();
            var response = new SystemResponseDto();

            if (request is null || string.IsNullOrWhiteSpace(request.Email))
            {
                error.Add("Invalid request or missing email.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.FullName))
            {
                error.Add("User not found or profile incomplete.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var emails = new List<MailAddress> { new MailAddress(user.Email) };
            var variables = new List<string>
            {
                user.FullName,
                token,
                "لاسترجاع كلمة السر"
            };

            var emailRequest = new EmailRequestDto
            {
                Receptors = emails,
                Subject = "استرجاع كلمة السر",
                MessageVariables = variables
            };

            var emailResponse = await _emailServices.ConfirmEmailTemp(emailRequest);
            if (!emailResponse.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Errors = emailResponse.Errors;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var response = new AuthResponseDto();

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                response.Errors.Add("Email and password are required.");
                response.IsSuccessful = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.Errors.Add("Invalid Authentication");
                response.IsSuccessful = false;
                return response;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                response.Errors.Add("You Need to Confirm Your Email");
                response.IsSuccessful = false;
                response.Code = 1;
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);
            var jwtResponse = _jwtRepository.GenerateAccessToken(new JwtRequestDto
            {
                User = user,
                Roles = roles
            });

            if (!jwtResponse.IsSuccessful || string.IsNullOrWhiteSpace(jwtResponse.RefreshToken))
            {
                response.IsSuccessful = false;
                response.Errors.AddRange(jwtResponse.Errors);
                return response;
            }

            var refreshToken = _jwtRepository.GenerateRefreshToken();
            if (!refreshToken.IsSuccessful || string.IsNullOrWhiteSpace(refreshToken.RefreshToken))
            {
                response.IsSuccessful = false;
                response.Errors.AddRange(refreshToken.Errors);
                return response;
            }

            user.RefreshToken = refreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpirationInDays"));

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.IsSuccessful = false;
                response.Errors.Add("Invalid Authentication");
                return response;
            }

            response.Token = jwtResponse.RefreshToken;
            response.RefreshToken = refreshToken.RefreshToken;
            response.IsSuccessful = true;
            return response;
        }

        public async Task<AuthResponseDto> LoginWithRefreshToken(RefreshTokenRequestDto request)
        {
            var response = new AuthResponseDto();

            if (string.IsNullOrWhiteSpace(request.RefreshToken) || string.IsNullOrWhiteSpace(request.DeviceId))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Missing token or device ID");
                return response;
            }

            var tokenEntry = await _unitOfWork.RefreshTokens.SingleOrDefault(t =>
                t.Token == request.RefreshToken &&
                t.DeviceId == request.DeviceId &&
                !t.IsRevoked &&
                t.ExpiryTime > DateTime.UtcNow);

            if (tokenEntry is null)
            {
                response.IsSuccessful = false;
                response.Errors.Add("Invalid or expired refresh token");
                return response;
            }

            var user = await _userManager.FindByIdAsync(tokenEntry.UserId);
            if (user is null)
            {
                response.IsSuccessful = false;
                response.Errors.Add("User not found");
                return response;
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

            // 🔄 Rotate refresh token
            tokenEntry.IsRevoked = true;

            var refreshTokenResult = _jwtRepository.GenerateRefreshToken();
            if (!refreshTokenResult.IsSuccessful || string.IsNullOrWhiteSpace(refreshTokenResult.RefreshToken))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Failed to generate new refresh token");
                return response;
            }

            var newToken = new RefreshTokenEntry
            {
                Token = refreshTokenResult.RefreshToken,
                DeviceId = request.DeviceId,
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.RefreshTokens.AddAsync(newToken);
            await _unitOfWork.Complete();

            response.Token = jwtResponse.Token;
            response.RefreshToken = newToken.Token;
            response.IsSuccessful = true;
            return response;
        }
        public async Task<SystemResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var response = new SystemResponseDto();
            var error = new List<string>();

            if (request is null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                error.Add("Invalid request or missing email/password.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var alreadyUser = await _userManager.FindByEmailAsync(request.Email);
            if (alreadyUser != null)
            {
                if (alreadyUser.PasswordHash is null)
                {
                    error.Add("Use Google Sign In");
                    response.Errors = error;
                    response.IsSuccessful = false;
                    return response;
                }

                error.Add("انت مشترك سابقا يرجى تسجيل الدخول");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var user = AppMapper.MapUserFromRegisterRequest(request);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.Errors = result.Errors.Select(e => e.Description);
                response.IsSuccessful = false;
                return response;
            }

            await _userManager.AddToRoleAsync(user, "Visitor");

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.FullName))
            {
                error.Add("User profile is incomplete.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var emails = new List<MailAddress> { new MailAddress(user.Email) };
            var variables = new List<string>
            {
                user.FullName,
                emailConfirmationToken,
                "لتفعيل الاشتراك"
            };

            var emailRequest = new EmailRequestDto
            {
                Receptors = emails,
                Subject = "تفعيل الاشنراك",
                MessageVariables = variables
            };

            var emailResponse = await _emailServices.ConfirmEmailTemp(emailRequest);
            if (!emailResponse.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Errors = emailResponse.Errors;
                return response;
            }

            error.Add("تم التسجيل بنجاح");
            response.Errors = error;
            response.IsSuccessful = true;
            return response;
        }

        public async Task<SystemResponseDto> ResendEmailConfirmation(ResendEmailConfirmationRequestDto request)
        {
            var error = new List<string>();
            var response = new SystemResponseDto();

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                error.Add("Email is required.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                error.Add("Invalid Request");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            if (user.EmailConfirmed)
            {
                error.Add("Email Already Confirmed");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.FullName))
            {
                error.Add("User profile is incomplete.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emails = new List<MailAddress> { new MailAddress(user.Email) };
            var variables = new List<string>
            {
                user.FullName,
                token,
                "لتفعيل الاشتراك"
            };

            var emailRequest = new EmailRequestDto
            {
                Receptors = emails,
                Subject = "تفعيل الاشنراك",
                MessageVariables = variables
            };

            var emailResponse = await _emailServices.ConfirmEmailTemp(emailRequest);
            if (!emailResponse.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Errors = emailResponse.Errors;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public async Task<SystemResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var error = new List<string>();
            var response = new SystemResponseDto();

            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.Otp))
            {
                error.Add("Email, password, and OTP are required.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                error.Add("Invalid Request");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Otp, request.Password);
            if (!result.Succeeded)
            {
                response.Errors = result.Errors.Select(e => e.Description);
                response.IsSuccessful = false;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public JwtResponseDto ValidateToken(string token)
        {
            var response = new JwtResponseDto();
            var error = new List<string>();

            try
            {
                var isValid = _jwtRepository.ReadJwtToken(token);
                response.IsSuccessful = isValid;
                response.Errors = new List<string> { isValid ? "ok" : "Not ok" };
                return response;
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                error.Add(e.Message);
                response.Errors = error;
                return response;
            }
        }

        public async Task<SystemResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request)
        {
            var error = new List<string>();
            var response = new SystemResponseDto();

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Code))
            {
                error.Add("Email and verification code are required.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                error.Add("Invalid Request");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Code);
            if (!result.Succeeded)
            {
                response.Errors = result.Errors.Select(e => e.Description);
                response.IsSuccessful = false;
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        public async Task<SystemResponseDto> LogoutDeviceAsync(RefreshTokenRequestDto request)
        {
            var response = new SystemResponseDto();

            if (string.IsNullOrWhiteSpace(request.RefreshToken) || string.IsNullOrWhiteSpace(request.DeviceId))
            {
                response.IsSuccessful = false;
                response.Errors = ["Missing token or device ID"];
                return response;
            }

            var tokenEntry = await _unitOfWork.RefreshTokens.SingleOrDefault(t =>
                t.Token == request.RefreshToken &&
                t.DeviceId == request.DeviceId &&
                !t.IsRevoked);

            if (tokenEntry is null)
            {
                response.IsSuccessful = false;
                response.Errors = ["No active session found for this device"];
                return response;
            }

            tokenEntry.IsRevoked = true;
            await _unitOfWork.Complete();

            response.IsSuccessful = true;
            response.Errors = ["Device logged out successfully"];
            return response;
        }

        public async Task<SystemResponseDto> LogoutAllDevicesAsync(RefreshTokenRequestDto request)
        {
            var response = new SystemResponseDto();

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                response.IsSuccessful = false;
                response.Errors = ["Missing refresh token"];
                return response;
            }

            var tokenEntry = await _unitOfWork.RefreshTokens.SingleOrDefault(t =>
                t.Token == request.RefreshToken &&
                !t.IsRevoked);

            if (tokenEntry is null)
            {
                response.IsSuccessful = false;
                response.Errors = ["Invalid or expired refresh token"];
                return response;
            }

            var allTokens = await _unitOfWork.RefreshTokens.FindAsync(t =>
                t.UserId == tokenEntry.UserId &&
                !t.IsRevoked);

            if (!allTokens.Any())
            {
                response.IsSuccessful = false;
                response.Errors = ["No active sessions found"];
                return response;
            }

            foreach (var token in allTokens)
                token.IsRevoked = true;

            await _unitOfWork.Complete();

            response.IsSuccessful = true;
            response.Errors = ["All sessions revoked"];
            return response;
        }

        public SystemResponseDto ValidateToken(TokenValidationRequestDto request)
        {
            var response = new SystemResponseDto();

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                response.IsSuccessful = false;
                response.Errors = ["Missing token"];
                return response;
            }

            var isValid = _jwtRepository.IsTokenValid(request.Token);
            response.IsSuccessful = isValid;
            response.Errors = isValid ? [] : ["Token is invalid or expired"];
            return response;
        }

        public async Task<RefreshTokenIntrospectionResponseDto> IntrospectRefreshTokenAsync(string token)
        {
            var entry = await _unitOfWork.RefreshTokens.SingleOrDefault(t => t.Token == token);

            if (entry is null)
            {
                return new RefreshTokenIntrospectionResponseDto
                {
                    IsValid = false,
                    Reason = "Token not found"
                };
            }

            return new RefreshTokenIntrospectionResponseDto
            {
                IsValid = !entry.IsRevoked && entry.ExpiryTime > DateTime.UtcNow,
                UserId = entry.UserId,
                DeviceId = entry.DeviceId,
                ExpiryTime = entry.ExpiryTime,
                CreatedAt = entry.CreatedAt,
                IsRevoked = entry.IsRevoked,
                Reason = entry.IsRevoked ? "Revoked" : (entry.ExpiryTime < DateTime.UtcNow ? "Expired" : "Active")
            };
        }
    }
}