
using System.Net.Mail;
using System.Security.Claims;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Dependencies;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using Microsoft.AspNetCore.Identity;
using AliHaydarBase.Api.Constants.Text;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtRepository _jwt;
        private readonly IEmailServicesRepository _emailServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IAuditLoggerRepository _auditLogger;
        public AuthRepository(AliHaydarDbContext context,
            UserManager<User> userManager,
            IJwtRepository jwtRepository,
            IEmailServicesRepository emailServices,
            IConfiguration configuration,
            IUnitOfWork unitOfWork, IAuditLoggerRepository auditLogger, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _userManager = userManager;
            _jwt = jwtRepository;
            _emailServices = emailServices;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _auditLogger = auditLogger;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary> // Finish
        ///  Registers a new user by creating an account with the provided email and password, assigning a default role, and sending an email confirmation link.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var response = new AuthResponseDto();

            // 1️⃣ Validate input
            if (request is null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                response.IsSuccessful = false;
                response.Errors.Add(DkString.InvalidRequestError);
                response.Code = 400;
                return response;
            }

            // 2️⃣ Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                if (existingUser.PasswordHash is null)
                {
                    response.Errors.Add(DkString.GoogleRegisterError01); // user exists via Google
                }
                else
                {
                    response.Errors.Add(DkString.UserExistError); // user exists with password
                }

                response.IsSuccessful = false;
                response.Code = 409;
                return response;
            }

            // 3️⃣ Create new user
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.IsSuccessful = false;
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                response.Code = 422;
                return response;
            }

            // 4️⃣ Assign default role
            await _userManager.AddToRoleAsync(user, "Visitor");

            // 5️⃣ Send confirmation email
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailRequest = new EmailRequestDto
            {
                Receptors = [new MailAddress(user.Email)],
                Subject = DkString.EmailSubject,
                MessageVariables = [user.FullName!, emailToken, DkString.EmailSubject]
            };

            var emailResponse = await _emailServices.ConfirmEmailTemp(emailRequest);
            if (!emailResponse.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Errors.AddRange(emailResponse.Errors);
                response.Code = 500;
                return response;
            }

            // 6️⃣ Build response (no tokens yet, user must confirm email then login)
            response.UserId = user.Id;
            response.Roles = [.. await _userManager.GetRolesAsync(user)];
            response.IsSuccessful = true;
            response.Code = 200;

            return response;
        }

        /// <summary> // Finish 
        /// Initiates the forgot password process by generating a reset token and sending it via email to the user.
        /// </summary>
        /// <param name="request">The request containing the user's email.</param>
        /// <returns>An AuthResponseDto indicating the success or failure of the operation.</returns>

        public async Task<AuthResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var error = new List<string>();
            var response = new AuthResponseDto();

            if (request is null || string.IsNullOrWhiteSpace(request.Email))
            {
                error.Add("Invalid request or missing email.");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || string.IsNullOrWhiteSpace(user.Email))
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
                response.Errors = [.. emailResponse.Errors];
                return response;
            }

            response.IsSuccessful = true;
            await LogActionAsync(user.Id, "Login", "User logged in", new { request.Email });
            return response;
        }
        /// <summary> // Finish
        /// Resends the email confirmation link to the user if their email is not yet confirmed, allowing them to complete the registration process.
        /// </summary>
        /// <param name="request">The request containing the user's email.</param>
        /// <returns>A response indicating the success or failure of the operation.</returns>
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
            await LogActionAsync(user.Id, "Login", "User logged in", new { user.Email });
            return response;
        }
        /// <summary> // Finish
        /// Resets the user's password using the provided email, new password, and OTP token. Validates the input, checks the user's existence, and updates the password if the OTP is valid.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AuthResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var error = new List<string>();
            var response = new AuthResponseDto();

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
                response.Errors = result.Errors.Select(e => e.Description).ToList();
                response.IsSuccessful = false;
                return response;
            }

            response.IsSuccessful = true;
            await LogActionAsync(user.Id, "Login", "User logged in", new { user.Email });
            return response;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var response = new AuthResponseDto();

            // 1️⃣ Validate input
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                response.Errors.Add("Email and password are required.");
                response.IsSuccessful = false;
                return response;
            }

            // 2️⃣ Find user and check password
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                response.Errors.Add("Invalid Authentication");
                response.IsSuccessful = false;
                return response;
            }

            // 3️⃣ Ensure email confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                response.Errors.Add("You need to confirm your email");
                response.IsSuccessful = false;
                response.Code = 1;
                return response;
            }

            // 4️⃣ Generate access token
            var roles = await _userManager.GetRolesAsync(user);
            var jwtResponse = _jwt.GenerateAccessToken(new JwtRequestDto
            {
                User = user,
                Roles = roles,
                DeviceId = request.DeviceId // make sure LoginRequestDto includes DeviceId
            });

            if (!jwtResponse.IsSuccessful || string.IsNullOrWhiteSpace(jwtResponse.Token))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Access token generation failed");
                response.Errors.AddRange(jwtResponse.Errors);
                return response;
            }

            // 5️⃣ Generate refresh token
            var refreshTokenResult = _jwt.GenerateRefreshToken();
            if (!refreshTokenResult.IsSuccessful || string.IsNullOrWhiteSpace(refreshTokenResult.RefreshToken))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Refresh token generation failed");
                response.Errors.AddRange(refreshTokenResult.Errors);
                return response;
            }

            // 6️⃣ Persist refresh token in both User and RefreshTokens table
            user.RefreshToken = refreshTokenResult.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpirationInDays"));
            await _userManager.UpdateAsync(user);

            var refreshTokenEntry = new RefreshTokenEntry
            {
                Token = refreshTokenResult.RefreshToken,
                DeviceId = request.DeviceId,
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpirationInDays")),
                IsRevoked = false
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntry);
            await _unitOfWork.Complete();

            // 7️⃣ Build response
            response.Token = jwtResponse.Token;
            response.RefreshToken = refreshTokenResult.RefreshToken;
            response.IsSuccessful = true;

            return response;
        }
        public async Task<AuthResponseDto> LoginWithRefreshToken(RefreshTokenRequestDto request)
        {
            var response = new AuthResponseDto();
            //Console.WriteLine($"🔄 Refresh attempt: token={request.RefreshToken}, deviceId={request.DeviceId}");

            // 1️⃣ Validate input
            if (string.IsNullOrWhiteSpace(request.RefreshToken) || string.IsNullOrWhiteSpace(request.DeviceId))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Missing token or device ID");
                return response;
            }

            // 2️⃣ Lookup existing refresh token entry
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

            // 3️⃣ Find user associated with token
            var user = await _userManager.FindByIdAsync(tokenEntry.UserId);
            if (user is null)
            {
                response.IsSuccessful = false;
                response.Errors.Add("User not found");
                return response;
            }

            // 4️⃣ Generate new access token
            var roles = await _userManager.GetRolesAsync(user);
            var jwtResponse = _jwt.GenerateAccessToken(new JwtRequestDto
            {
                User = user,
                Roles = roles,
                DeviceId = request.DeviceId // keep device binding consistent
            });

            if (!jwtResponse.IsSuccessful || string.IsNullOrWhiteSpace(jwtResponse.Token))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Access token generation failed");
                response.Errors.AddRange(jwtResponse.Errors);
                return response;
            }

            // 5️⃣ Rotate refresh token (revoke old one)
            tokenEntry.IsRevoked = true;

            var refreshTokenResult = _jwt.GenerateRefreshToken();
            if (!refreshTokenResult.IsSuccessful || string.IsNullOrWhiteSpace(refreshTokenResult.RefreshToken))
            {
                response.IsSuccessful = false;
                response.Errors.Add("Failed to generate new refresh token");
                return response;
            }

            // 6️⃣ Persist new refresh token entry
            var newToken = new RefreshTokenEntry
            {
                Token = refreshTokenResult.RefreshToken,
                DeviceId = request.DeviceId,
                UserId = user.Id,
                ExpiryTime = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _unitOfWork.RefreshTokens.AddAsync(newToken);
            await _unitOfWork.Complete();

            // 7️⃣ Build response
            response.Token = jwtResponse.Token;
            response.RefreshToken = newToken.Token;
            response.IsSuccessful = true;
            response.Errors = new List<string> { "Ok" };

            // 8️⃣ Audit log
            await LogActionAsync(user.Id, "RefreshLogin", "User refreshed session", new { user.Email });

            return response;
        }

        public JwtResponseDto ValidateToken(string token)
        {
            var response = new JwtResponseDto();
            var error = new List<string>();

            try
            {
                var isValid = _jwt.ReadJwtToken(token);
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

        public async Task<AuthResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request)
        {
            var error = new List<string>();
            var response = new AuthResponseDto();

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
                response.Errors = [.. result.Errors.Select(e => e.Description)];
                response.IsSuccessful = false;
                response.Code = 1001; // Custom code for email verification OTP not matching or expired
                return response;
            }

            response.IsSuccessful = true;
            await LogActionAsync(user.Id, "Login", "User logged in", new { user.Email });
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

            var isValid = _jwt.IsTokenValid(request.Token);
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


        public async Task<IEnumerable<ClaimDefinitionResponseDto>> GetAllClaimDefinitionsAsync()
        {
            var claims = await _unitOfWork.ClaimDefinitions.FindAsync(c => c.IsActive);

            return claims.Select(c => new ClaimDefinitionResponseDto
            {
                Type = c.Type,
                Description = c.Description,
                Category = c.Category,
                UiHint = c.UiHint,
                Scope = c.Scope,
                Group = c.Group,
                IsVisibleToFrontend = c.IsVisibleToFrontend
            });

            /// we can use in flutter like this 
            /// final response = await dio.get('/api/auth/claim-definitions');
            /// final claims = response.data as List;

        }

        private async Task LogActionAsync(string? userId, string action, string description, object? metadata = null, string? fallbackEmail = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // 🌍 IP address
            var ip = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";

            // 📱 Device ID from header or claim
            var deviceId = httpContext?.Request.Headers["X-Device-Id"].FirstOrDefault()
                ?? httpContext?.User.FindFirst("DeviceId")?.Value
                ?? "unknown";

            // 🧠 Infer userId from token if missing
            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? httpContext?.User.FindFirst("sub")?.Value;
            }

            // 📧 Fallback to email if still anonymous
            if (string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(fallbackEmail))
            {
                var user = await _unitOfWork.User.GetByEmailAsync(fallbackEmail);
                userId = user?.Id ?? "anonymous";
            }

            await _auditLogger.LogAsync(userId ?? "anonymous", action, description, ip, deviceId, metadata);
        }


    }
}