using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        private string GenerateToken(IEnumerable<Claim> claims, TimeSpan lifetime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(lifetime),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenPair GenerateTokenPair(UserProfile profile, IEnumerable<MenuItem> menu)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, profile.Id),
                new Claim("name", profile.Name),
                new Claim("email", profile.Email),
                new Claim("menu", System.Text.Json.JsonSerializer.Serialize(menu))
            };
            var access = GenerateToken(claims, TimeSpan.FromMinutes(15));
            claims.Add(new Claim("typ", "refresh"));
            var refresh = GenerateToken(claims, TimeSpan.FromDays(7));
            return new TokenPair(access, refresh);
        }

        public ClaimsPrincipal? ValidateRefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = key,
                ValidateLifetime = true
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, parameters, out _);
                if (principal.FindFirst("typ")?.Value != "refresh") return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
