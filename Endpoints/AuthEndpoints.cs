
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.DTOs.Request;

namespace AliHaydarBase.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/auth");

            group.MapGet("/check", (IConfiguration config) =>
            {
                var key1 = config["Google:ClientId"];
                var z = config["Google:ANDROIDd"];
                return Results.Ok($"{z}     {key1 ?? "No Key"}");
            });

            group.MapPost("/register", async (RegisterRequestDto model, IAuthRepository repo) =>
            {
                var response = await repo.RegisterAsync(model);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            group.MapPost("/login", async (LoginRequestDto model, IAuthRepository repo) =>
            {
                var response = await repo.LoginAsync(model);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
            });

            group.MapPost("/validate", (string token, IAuthRepository repo) =>
            {
                var response = repo.ValidateToken(token);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
            });

            group.MapPost("/refresh", async (RefreshTokenRequestDto request, IAuthRepository repo) =>
            {
                var response = await repo.LoginWithRefreshToken(request);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.Json(response, statusCode: StatusCodes.Status401Unauthorized);
            });

            group.MapPost("/verifyEmail", async (VerifyEmailRequestDto dto, IAuthRepository repo) =>
            {
                var response = await repo.VerifyEmailAsync(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            group.MapPost("/forgotPassword", async (ForgotPasswordRequestDto dto, IAuthRepository repo) =>
            {
                var response = await repo.ForgotPasswordAsync(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            group.MapPost("/resetPassword", async (ResetPasswordRequestDto dto, IAuthRepository repo) =>
            {
                var response = await repo.ResetPasswordAsync(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            group.MapPost("/resendEmailConfirmation", async (ResendEmailConfirmationRequestDto dto, IAuthRepository repo) =>
            {
                var response = await repo.ResendEmailConfirmation(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            group.MapPost("/google-login", async (GoogleLoginRequestDto dto, IExternalLoginRepository externalLogin) =>
            {
                var response = await externalLogin.GoogleLogin(dto);
                return response.IsSuccessful
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            });

            group.MapPost("/logout-device", async (RefreshTokenRequestDto request, IAuthRepository authRepo) =>
            {
                var result = await authRepo.LogoutDeviceAsync(request);
                return Results.Ok(result);
            });

            group.MapPost("/logout-all", async (RefreshTokenRequestDto request, IAuthRepository authRepo) =>
            {
                var result = await authRepo.LogoutAllDevicesAsync(request);
                return Results.Ok(result);
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