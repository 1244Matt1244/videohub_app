using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class JwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtService(IConfiguration config)
        {
            _key = config["Jwt:Key"] ?? throw new ArgumentNullException("JWT Key missing in config");
            _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("JWT Issuer missing in config");
            _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException("JWT Audience missing in config");
            _expireMinutes = int.TryParse(config["Jwt:ExpireMinutes"], out int exp) ? exp : 60;
        }

        public string GenerateToken(string userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Opcionalno: Metoda za validaciju tokena može ići ovdje ako želiš
    }
}
