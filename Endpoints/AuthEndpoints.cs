
using System.ComponentModel.DataAnnotations;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using Microsoft.AspNetCore.Identity;

namespace AliHaydarBase.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("api/auth");

            group.MapGet("/check", (IConfiguration config) =>
            {
                var key1 = config["Google:ClientId"];
                var z = config["Google:ANDROIDd"];
                return Results.Ok($"{z}     {key1 ?? "No Key"}");
            });

            // Register User Endpoint
            group.MapPost("/register", async (RegisterRequestDto model, IAuthRepository repo) =>
            {
                var context = new ValidationContext(model);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(model, context, results, true))
                {
                    var errors = results.Select(r => r.ErrorMessage ?? "Invalid input").ToList();
                    return Results.BadRequest(new AuthResponseDto
                    {
                        IsSuccessful = false,
                        Errors = errors,
                        Code = 422
                    });
                }

                var response = await repo.RegisterAsync(model);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });
            /// Verify Email Endpoint

            group.MapPost("/verifyEmail", async (VerifyEmailRequestDto dto, IAuthRepository repo) =>
                        {
                            var response = await repo.VerifyEmailAsync(dto);
                            return response.IsSuccessful
                                ? Results.Ok(response)
                                : Results.BadRequest(response);
                        });

            /// Forgot Password Endpoint
            group.MapPost("/forgotPassword", async (ForgotPasswordRequestDto dto, IAuthRepository repo) =>
            {
                var response = await repo.ForgotPasswordAsync(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            /// Reset Password Endpoint
            group.MapPost("/resetPassword", async (HttpContext http, ResetPasswordRequestDto dto, IAuthRepository repo) =>
            {
                dto.IpAddress = http.Connection.RemoteIpAddress?.ToString();
                dto.DeviceId ??= http.Request.Headers["Device-Id"].ToString(); // if you want client to send DeviceId in header

                var response = await repo.ResetPasswordAsync(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });


            /// External Login Endpoint Finish Google /external-login
            group.MapPost("/external-login", async (HttpContext http, SocialLoginRequestDto dto, IExternalLoginRepository externalLogin) =>
            {
                dto.IpAddress = http.Connection.RemoteIpAddress?.ToString();
                dto.UserAgent = http.Request.Headers["User-Agent"].ToString();

                var response = await externalLogin.Login(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            // Login Endpoint Email and Password
            group.MapPost("/login", async (HttpContext http, LoginRequestDto request, IAuthRepository repo) =>
            {
                request.IpAddress = http.Connection.RemoteIpAddress?.ToString();
                request.UserAgent = http.Request.Headers["User-Agent"].ToString();

                var response = await repo.LoginAsync(request);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
            });

            // Refresh Token Endpoint
            group.MapPost("/refresh", async (HttpContext http, RefreshTokenRequestDto request, IAuthRepository repo) =>
            {
                // Capture audit info
                request.IpAddress = http.Connection.RemoteIpAddress?.ToString();
                request.UserAgent = http.Request.Headers["User-Agent"].ToString();

                var response = await repo.LoginWithRefreshToken(request);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
            });

            // Logout Device Endpoint
            group.MapPost("/logout", async (RefreshTokenRequestDto request, IAuthRepository authRepo) =>
            {
                var result = await authRepo.LogoutDeviceAsync(request);
                return Results.Ok(result);
            });

            // Logout All Devices Endpoint
            group.MapPost("/logout-all", async (LogoutAllRequestDto request, IAuthRepository authRepo) =>
            {
                var result = await authRepo.LogoutAllDevicesAsync(request);
                return Results.Ok(result);
            });

            // all is well until here


            group.MapPost("/validate", (string token, IAuthRepository repo) =>
            {
                var response = repo.ValidateToken(token);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
            });


            group.MapPost("/resendEmailConfirmation", async (ResendEmailConfirmationRequestDto dto, IAuthRepository repo) =>
            {
                var response = await repo.ResendEmailConfirmation(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });





            group.MapPost("/validate-token", (TokenValidationRequestDto request, IAuthRepository authRepo) =>
            {
                var result = authRepo.ValidateToken(request.Token);
                return Results.Ok(result);
            });

            group.MapPost("/introspect-refresh-token", async (RefreshTokenIntrospectionRequestDto request, IAuthRepository authRepo) =>
            {
                var result = await authRepo.IntrospectRefreshTokenAsync(request.RefreshToken);
                return Results.Ok(result);
            });
        }
    }
}