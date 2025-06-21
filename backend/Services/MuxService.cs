using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mux.Video;
using Mux.Video.Api;
using Mux.Video.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.Services
{
    public class MuxService
    {
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _jwtExpireMinutes;

        private readonly AssetsApi _assetsApi;

        public MuxService(IConfiguration config)
        {
            // JWT config
            _jwtKey = config["Jwt:Key"] ?? throw new ArgumentNullException("JWT Key missing in config");
            _jwtIssuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("JWT Issuer missing in config");
            _jwtAudience = config["Jwt:Audience"] ?? throw new ArgumentNullException("JWT Audience missing in config");
            _jwtExpireMinutes = int.TryParse(config["Jwt:ExpireMinutes"], out int exp) ? exp : 60;

            // Mux config
            var muxTokenId = config["Mux:TokenId"];
            var muxTokenSecret = config["Mux:TokenSecret"];
            if (string.IsNullOrEmpty(muxTokenId) || string.IsNullOrEmpty(muxTokenSecret))
                throw new ArgumentException("Mux API keys are missing");

            Configuration muxConfig = new Configuration
            {
                Username = muxTokenId,
                Password = muxTokenSecret
            };

            _assetsApi = new AssetsApi(muxConfig);
        }

        // 🔐 JWT generacija
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

        // 📹 Upload videa na Mux
        public async Task<Asset> UploadVideoAsync()
        {
            var upload = new CreateUploadRequest
            {
                NewAssetSettings = new CreateAssetRequest
                {
                    PlaybackPolicy = new System.Collections.Generic.List<string> { "public" }
                }
            };

            var uploadResult = await _assetsApi.CreateUploadAsync(upload);

            // Kreira Asset nakon što korisnik zapravo uploada video koristeći upload URL
            return await _assetsApi.GetAssetAsync(uploadResult.Data.AssetId);
        }
    }
}
