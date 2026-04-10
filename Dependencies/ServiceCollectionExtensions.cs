using System.Text;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Mapper.Members;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.Core.Repositories;
using AliHaydarBase.Api.Core.Services;
using AliHaydarBase.Api.Dependencies;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAliHaydarBaseServices(
    this IServiceCollection services,
    IConfiguration configuration,
    IWebHostEnvironment env)
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

        // 🔐 JWT
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
        });

        services.AddAuthorization();

        // 🧩 Repositories & Services
        services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IAuthRepository, AuthRepository>()
            .AddScoped<IExternalLoginRepository, ExternalLoginRepository>()
            .AddScoped<IEmailServicesRepository, EmailServicesRepository>()
            .AddScoped<IJwtRepository, JwtRepository>()
            .AddScoped<IAuditLoggerRepository, AuditLoggerRepository>()
            .AddScoped<IRefreshTokenEntryRepository, RefreshTokenEntryRepositories>(); // ✅ FIX ADDED HERE

        services.AddHttpClient();
        services.AddHttpContextAccessor();

        // 🧩 AutoMapper
        services.AddAutoMapper(typeof(IdCardMappingProfile));

        // 🧹 Background service
        services.AddHostedService<TokenCleanupService>();

        // 🌐 CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFlutter", policy =>
                policy.WithOrigins("http://localhost:5000", "http://192.168.0.101:5164")
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        // 🛡️ Rate Limiting
        services.AddMemoryCache();
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        return services;
    }
}