using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VideoApp.Services
{
    public class JwtService
    {
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpireMinutes;

        public JwtService(IConfiguration config)
        {
            _jwtKey = config["Jwt:Key"] ?? throw new ArgumentNullException("JWT Key missing in config");
            _jwtIssuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("JWT Issuer missing in config");
            _jwtAudience = config["Jwt:Audience"] ?? throw new ArgumentNullException("JWT Audience missing in config");
            _jwtExpireMinutes = int.TryParse(config["Jwt:ExpireMinutes"], out int exp) ? exp : 60;
        }

        public string GenerateToken(string userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
