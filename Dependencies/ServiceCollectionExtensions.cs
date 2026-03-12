using System.Text;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Core.Repositories;
using AliHaydarBase.Api.Dependencies;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAliHaydarBaseServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        // 🔗 Connection String
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        // 🧱 DbContext
        services.AddDbContext<AliHaydarDbContext>(options =>
            options.UseSqlServer(connectionString));

        // 🔐 Identity Configuration
        services.AddIdentity<User, Role>(options =>
        {
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
        })
        .AddEntityFrameworkStores<AliHaydarDbContext>()
        .AddDefaultTokenProviders();

        // 🔐 JWT + Google Auth
        var jwtKey = configuration["Jwt:secretKey"]
            ?? throw new InvalidOperationException("Missing JWT secret key");
        var jwtIssuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Missing JWT issuer");
        var jwtAudience = configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Missing JWT audience");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                ClockSkew = TimeSpan.Zero
            };
        })
        .AddGoogle(options =>
        {
            options.ClientId = configuration["Google:ClientId"]
                ?? throw new InvalidOperationException("Missing Google ClientId");
            options.ClientSecret = configuration["Google:ClientSecret"]
                ?? throw new InvalidOperationException("Missing Google ClientSecret");
            options.CallbackPath = "/signin-google";
        });

        services.AddAuthorization();



        // 🧩 Repositories & Services
        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IAuthRepository, AuthRepository>()
            .AddScoped<IExternalLoginRepository, ExternalLoginRepository>()
            .AddScoped<IEmailServicesRepository, EmailServicesRepository>()
            .AddScoped<IJwtRepository, JwtRepository>()
            .AddScoped<IAuditLoggerRepository, AuditLoggerRepository>();
        services.AddHttpClient();           // 📡 HTTP Client
        services.AddHttpContextAccessor();  // 📡 HTTP Context

        // 🌐 CORS Configuration
//         services.AddCors(options =>
// {
//     options.AddPolicy("AllowDevClient", policy =>
//     {
//         policy
//             .SetIsOriginAllowed(origin =>
//                 origin.StartsWith("http://localhost") ||
//                 origin.StartsWith("http://127.0.0.1"))
//             .AllowAnyHeader()
//             .AllowAnyMethod();
//     });
// });
services.AddCors(options =>
{
    options.AddPolicy("AllowFlutter",
        policy => policy.WithOrigins("http://localhost:5000", "http://192.168.0.101:5164") // Your flutter port
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
        var allowedOrigins = env.IsDevelopment()
    ? new[] { "http://localhost:57636", "http://192.168.0.101:5164" }
    : new[] { "https://your-production-client.com" };

// services.AddCors(options =>
// {
//     options.AddPolicy("AllowWebClient", policy =>
//     {
//         policy.WithOrigins(allowedOrigins)
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//     });
// });

        services.Configure<IdentityOptions>(options =>
        {
            // 🔐 Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // 🚫 Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // 👤 User settings
            options.User.RequireUniqueEmail = true;
        });

        // 🛡️ Rate Limiting / Throttling
        services.AddMemoryCache(); // Required for rate limiting
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        return services;
    }
}