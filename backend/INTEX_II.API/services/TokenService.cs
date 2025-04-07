// File: /backend/CineNiche.Auth/Services/TokenService.cs
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CineNiche.Auth.Configuration;

namespace CineNiche.Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly StytchConfig _config;
        
        public TokenService(IOptions<StytchConfig> config)
        {
            _config = config.Value;
        }
        
        public string GenerateJwtToken(string userId, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.JwtSigningKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var token = new JwtSecurityToken(
                issuer: _config.ProjectId,
                audience: _config.ProjectId,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.JwtExpiryMinutes),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public bool ValidateJwtToken(string token, out string userId, out string role)
        {
            userId = null;
            role = null;
            
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config.JwtSigningKey);
                
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config.ProjectId,
                    ValidateAudience = true,
                    ValidAudience = _config.ProjectId,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                
                var jwtToken = (JwtSecurityToken)validatedToken;
                userId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
                role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool ReadTokenInfo(string token, out string userId, out string role)
        {
            userId = null;
            role = null;
            
            try
            {
                if (string.IsNullOrEmpty(token))
                    return false;
                
                var tokenHandler = new JwtSecurityTokenHandler();
                
                if (!tokenHandler.CanReadToken(token))
                    return false;
                
                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                userId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
                role = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
                
                return !string.IsNullOrEmpty(userId);
            }
            catch
            {
                return false;
            }
        }
    }
}