using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Src.Application.Service
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;
        private readonly ILogger<JwtService> _logger;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _secretKey = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
            _expireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]);
            _logger = logger;

            _logger.LogInformation("Jwt Config -> Issuer: {Issuer}, Audience: {Audience}, Expire: {Expire}", _issuer, _audience, _expireMinutes);
        }

        public string GenerateToken(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var key = new[] { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)) };
            var validationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = key,
                ValidateLifetime = true
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                var jwt = (JwtSecurityToken)validatedToken;
                _logger.LogInformation("JWT recibido:");
                _logger.LogInformation("Issuer: {Issuer}", jwt.Issuer);
                _logger.LogInformation("Audience: {Audience}", string.Join(", ", jwt.Audiences));
                _logger.LogInformation("Expires: {Expiration}", jwt.ValidTo);
                _logger.LogInformation("Claims:");
                foreach (var claim in jwt.Claims)
                {
                    _logger.LogInformation(" - {Type}: {Value}", claim.Type, claim.Value);
                }
                return true;
            } catch (SecurityTokenValidationException ex) {
                _logger.LogWarning(ex, "Fallo la validación del token JWT: {Message}", ex.Message);
                return false;
            }
        }
    }
}
