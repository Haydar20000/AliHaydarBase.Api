
using System.Net.Mail;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Mapper;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Dependencies;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using Microsoft.AspNetCore.Identity;

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
            List<string> error = [];
            var response = new SystemResponseDto();
            if (request is null)
            {
                error.Add("Invalid Request");
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

            var token = await _userManager.GeneratePasswordResetTokenAsync(user!);
            List<MailAddress> Emails = new List<MailAddress>();
            MailAddress userEmail = new MailAddress(user!.Email!);
            Emails.Add(userEmail);
            List<string> variables = [];
            variables.Add(user.FullName!);
            variables.Add(token);
            variables.Add("لاسترجاع كلمة السر");
            EmailRequestDto emailRequest = new EmailRequestDto
            {
                Receptors = Emails,
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
            return response;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var response = new AuthResponseDto();
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
            JwtRequestDto jwtRequest = new JwtRequestDto
            {
                User = user,
                Roles = roles
            };
            var jwtResponse = _jwtRepository.GenerateAccessToken(jwtRequest);
            if (!jwtResponse.IsSuccessful)
            {
                response.IsSuccessful = false;
                response.Errors.AddRange(jwtResponse.Errors);
                return response;
            }
            var refreshToken = _jwtRepository.GenerateRefreshToken();
            if (!refreshToken.IsSuccessful)
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

            response.Token = jwtResponse.RefreshToken!;
            response.RefreshToken = refreshToken.RefreshToken!;
            response.IsSuccessful = true;
            return response;
        }

        public async Task<JwtResponseDto> LoginWithRefreshToken(string refreshToken)
        {
            var response = new JwtResponseDto();

            var user = await _unitOfWork.User.SingleOrDefault(a => a.RefreshToken == refreshToken);
            if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                response.Errors.Add("Invalid Authentication");
                response.IsSuccessful = false;
                return response;
            }
            var roles = await _userManager.GetRolesAsync(user);
            JwtRequestDto jwtRequest = new JwtRequestDto
            {
                User = user,
                Roles = roles
            };
            var accessToken = _jwtRepository.GenerateAccessToken(jwtRequest);
            response.IsSuccessful = true;
            response.Token = accessToken.Token;
            var newRefreshToken = _jwtRepository.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _unitOfWork.Complete();
            response.RefreshToken = newRefreshToken.RefreshToken;

            return response;
        }

        public async Task<SystemResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var response = new SystemResponseDto();
            List<string> error = [];
            if (request is null)
            {
                error.Add("Invalid Request");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }
            var alreadyUser = await _userManager.FindByEmailAsync(request.Email);
            if (alreadyUser != null)
            {
                if (alreadyUser.PasswordHash is null)
                {
                    error.Add("Use google Sign In");
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
                var errors = result.Errors.Select(e => e.Description);

                response.Errors = errors;
                response.IsSuccessful = false;
                return response;
            }
            await _userManager.AddToRoleAsync(user, "Visitor");

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            List<MailAddress> Emails = new List<MailAddress>();
            MailAddress userEmail = new MailAddress(user.Email!);
            Emails.Add(userEmail);
            List<string> variables = [];
            variables.Add(user.FullName!);
            variables.Add(emailConfirmationToken);
            variables.Add("لتفعيل الاشتراك");
            EmailRequestDto emailRequest = new EmailRequestDto
            {
                Receptors = Emails,
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
            error.Add("تم التسجيل بنجاح'");
            response.Errors = error;
            return response;
        }

        public async Task<SystemResponseDto> ResendEmailConfirmation(ResendEmailConfirmationRequestDto request)
        {
            List<string> error = [];
            var response = new SystemResponseDto();
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
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            List<MailAddress> Emails = new List<MailAddress>();
            MailAddress userEmail = new MailAddress(user.Email!);
            Emails.Add(userEmail);
            List<string> variables = [];
            variables.Add(user.FullName!);
            variables.Add(token);
            variables.Add("لتفعيل الاشتراك");
            EmailRequestDto emailRequest = new EmailRequestDto
            {
                Receptors = Emails,
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
            return response;
        }

        public async Task<SystemResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            List<string> error = [];
            var response = new SystemResponseDto();
            var user = await _userManager.FindByEmailAsync(request.Email);
            //|| !await _userManager.CheckPasswordAsync(user, request.Password)
            if (user is null)
            {
                error.Add("Invalid Request");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user!, request.Otp, request.Password);
            if (!result.Succeeded)
            {
                response.Errors = result.Errors.Select(e => e.Description);
                response.IsSuccessful = false;
                return response;
            }
            response.IsSuccessful = true;
            return response;
        }

        public JwtResponseDto ValidateToken(string Token)
        {
            var response = new JwtResponseDto();
            List<string> error = [];
            try
            {
                var isValid = _jwtRepository.ReadJwtToken(token: Token);
                response.IsSuccessful = isValid;
                if (isValid)
                {
                    response.Errors = ["ok"];
                }
                else
                {
                    response.Errors = ["Not ok"];
                }
                return response;
            }
            catch (System.Exception e)
            {
                response.IsSuccessful = false;
                error.AddRange(e.Message);
                response.Errors = error;
                return response;

            }
        }

        public async Task<SystemResponseDto> VerifyEmailAsync(VerifyEmailRequestDto request)
        {
            List<string> error = [];
            var response = new SystemResponseDto();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                error.Add("Invalid Request");
                response.Errors = error;
                response.IsSuccessful = false;
                return response;
            }
            var result = await _userManager.ConfirmEmailAsync(user!, request.Code);
            if (!result.Succeeded)
            {
                response.Errors = result.Errors.Select(e => e.Description);
                response.IsSuccessful = false;
                return response;
            }
            response.IsSuccessful = true;
            return response;
        }
    }
}