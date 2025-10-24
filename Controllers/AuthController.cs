
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.DTOs.Request;
using Microsoft.AspNetCore.Mvc;


namespace AliHaydarBase.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthRepository _authRepository;
        private readonly IExternalLoginRepository _externalLogin;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IAuthRepository authRepository, IExternalLoginRepository externalLogin, IConfiguration configuration)
        {
            _logger = logger;
            _authRepository = authRepository;
            _externalLogin = externalLogin;
            _configuration = configuration;
        }
        [HttpGet("check")]
        //[Authorize(Roles = "Admin, visitor")]
        public IActionResult Check()
        {
            var key1 = _configuration["Google:ClientId"];
            var z = _configuration["Google:ANDROIDd"];

            return Ok(z + "     " + key1 ?? "No Key");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var response = await _authRepository.RegisterAsync(model);
            return !response.IsSuccessful ? BadRequest(response) : Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var response = await _authRepository.LoginAsync(model);
            return !response.IsSuccessful ? Unauthorized(response) : Ok(response);
        }

        [HttpPost("validate")]
        public IActionResult Validate([FromBody] string token)
        {
            var response = _authRepository.ValidateToken(token);
            return !response.IsSuccessful ? Unauthorized(response) : Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string code)
        {
            var response = await _authRepository.LoginWithRefreshToken(code);
            return !response.IsSuccessful ? Unauthorized(response) : Ok(response);
        }

        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmailAsync([FromBody] VerifyEmailRequestDto verifyEmailRequest)
        {
            var response = await _authRepository.VerifyEmailAsync(verifyEmailRequest);
            return !response.IsSuccessful ? BadRequest(response) : Ok(response);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequestDto forgotPasswordRequest)
        {
            var response = await _authRepository.ForgotPasswordAsync(forgotPasswordRequest);
            return !response.IsSuccessful ? BadRequest(response) : Ok(response);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var response = await _authRepository.ResetPasswordAsync(resetPasswordRequestDto);
            return !response.IsSuccessful ? BadRequest(response) : Ok(response);
        }
        [HttpPost("resendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmationAsync([FromBody] ResendEmailConfirmationRequestDto request)
        {
            var response = await _authRepository.ResendEmailConfirmation(request);
            return !response.IsSuccessful ? BadRequest(response) : Ok(response);
        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDto request)
        {
            var response = await _externalLogin.GoogleLogin(request);
            return !response.IsSuccessful ? BadRequest(response) : Ok(response);
            //"http://10.0.2.2:5003",
        }
    }
}