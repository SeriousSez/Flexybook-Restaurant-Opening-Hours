using Flexybook.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Flexybook.ApplicationService.JwtFeatures
{
    public class JwtHandler
    {
        private readonly IConfigurationSection _jwtSettings;
        private readonly UserManager<UserEntity> _userManager;

        public JwtHandler(IConfiguration configuration, UserManager<UserEntity> userManager)
        {
            _jwtSettings = configuration.GetSection("JwtSettings");
            _userManager = userManager;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var secretKey = GetSecurityKey();
            return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        }

        public async Task<List<Claim>> GetClaims(UserEntity user)
        {
            var claims = BuildBasicClaims(user);
            var roleClaims = await GetRoleClaims(user);
            
            claims.AddRange(roleClaims);
            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims, bool rememberMe)
        {
            return new JwtSecurityToken(
                issuer: GetJwtSetting("validIssuer"),
                audience: GetJwtSetting("validAudience"),
                claims: claims,
                expires: CalculateTokenExpiration(rememberMe),
                signingCredentials: signingCredentials);
        }

        private SymmetricSecurityKey GetSecurityKey()
        {
            var key = Encoding.UTF8.GetBytes(GetJwtSetting("securityKey"));
            return new SymmetricSecurityKey(key);
        }

        private static List<Claim> BuildBasicClaims(UserEntity user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email ?? string.Empty)
            };
        }

        private async Task<List<Claim>> GetRoleClaims(UserEntity user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        }

        private DateTime CalculateTokenExpiration(bool rememberMe)
        {
            if (rememberMe)
                return DateTime.Now.AddDays(30);

            var expiryMinutes = Convert.ToDouble(GetJwtSetting("expiryInMinutes"));
            return DateTime.Now.AddMinutes(expiryMinutes);
        }

        private string GetJwtSetting(string key)
        {
            return _jwtSettings.GetSection(key).Value ?? throw new InvalidOperationException($"JWT setting '{key}' is missing");
        }
    }
}
