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
            var now = DateTime.UtcNow;
            var expire = now.AddMinutes(_expireMinutes);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expire,
                NotBefore = now,
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("Token generado para usuario {UserId}: {Token}", userId, tokenString);

            return tokenString;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                _logger.LogInformation("Validando token...");

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                _logger.LogInformation("Token válido para usuario {UserId}", principal.Identity?.Name ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);

                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogWarning("Token expirado.");
                return false;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex, "Error validando token.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado validando token.");
                return false;
            }
        }

        public void InspectToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                _logger.LogInformation("Inspectando token:");
                foreach (var claim in jwtToken.Claims)
                {
                    _logger.LogInformation("  Claim Type: {Type}, Value: {Value}", claim.Type, claim.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inspeccionando token.");
            }
        }
    }
}
