
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AliHaydarBase.Api.Core.Interfaces;
using AliHaydarBase.Api.Core.Models;
using AliHaydarBase.Api.DTOs.Request;
using AliHaydarBase.Api.DTOs.Response;
using Microsoft.IdentityModel.Tokens;

namespace AliHaydarBase.Api.Core.Repositories
{
    public class JwtRepository : IJwtRepository
    {
        private readonly IConfiguration _configuration;


        public JwtRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public JwtResponseDto GenerateAccessToken(JwtRequestDto request)
        {
            try
            {
                var credentials = GetSigningCredentials();
                var claims = BuildClaims(request.User, request.Roles);
                var token = CreateJwtToken(credentials, claims);

                return new JwtResponseDto
                {
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(30),
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    IsSuccessful = true
                };
            }
            catch (Exception ex)
            {
                return new JwtResponseDto
                {
                    IsSuccessful = false,
                    Errors = [ex.Message]
                };
            }
        }

        public JwtResponseDto GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return new JwtResponseDto
            {
                RefreshToken = Convert.ToBase64String(randomBytes),
                IsSuccessful = true
            };
        }

        public bool ReadJwtToken(string token)
        {
            try
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return jwt.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }

        // 🔐 Helpers

        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration["Jwt:secretKey"]
                ?? throw new InvalidOperationException("Missing JWT secret key");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> BuildClaims(User user, IList<string> roles)
        {
            var now = DateTime.UtcNow;
            var expiry = now.AddMinutes(30);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("email_verified", user.EmailConfirmed.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiry).ToUnixTimeSeconds().ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;

        }

        public bool IsTokenValid(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var now = DateTime.UtcNow;

            var exp = jwt.Payload.Expiration;
            var nbf = jwt.Payload.NotBefore;

            return exp.HasValue && nbf.HasValue &&
                   now >= DateTimeOffset.FromUnixTimeSeconds(nbf.Value).UtcDateTime &&
                   now <= DateTimeOffset.FromUnixTimeSeconds(exp.Value).UtcDateTime;

        }

        private JwtSecurityToken CreateJwtToken(SigningCredentials credentials, IEnumerable<Claim> claims)
        {
            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Missing JWT issuer");
            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("Missing JWT audience");
            var expiryMinutes = _configuration.GetValue<int>("Jwt:ExpirationInMinutes");

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );
        }
    }
}