using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OrdersManagement.Services
{
    public class JwtService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secretKey;

        public JwtService(IConfiguration configuration)
        {
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        public string GenerateToken(int userId, string role)
        {

          try
            {
                var tokenHandler = new JwtSecurityTokenHandler();  
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),
                        ClaimValueTypes.Integer64)
                };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _issuer,
                        audience: _audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: creds
                    );

                var accessToken = tokenHandler.WriteToken(token);
                return accessToken;
            }
            catch(Exception objEx)
            {
                throw new Exception("Lỗi không thể tạo access token, vui lòng kiểm tra lại!");
            }
        }


        public string GenarateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }
}
