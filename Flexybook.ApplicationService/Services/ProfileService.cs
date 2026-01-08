using Flexybook.ApplicationService.JwtFeatures;
using Flexybook.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Flexybook.ApplicationService.Services
{
    /// <summary>
    /// Service for managing user profile and authentication operations.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private const string DefaultUsername = "Flexybook";
        
        private readonly UserManager<UserEntity> _userManager;
        private readonly JwtHandler _jwtHandler;

        public ProfileService(UserManager<UserEntity> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        /// <summary>
        /// Authenticates the default user and generates a JWT token.
        /// </summary>
        /// <returns>A JWT token string if authentication is successful; otherwise, null.</returns>
        public async Task<string?> LoginAsync()
        {
            var user = await FindUserByUsernameAsync(DefaultUsername);
            if (user == null || !await IsUserEmailConfirmedAsync(user))
                return null;

            return await GenerateJwtTokenAsync(user);
        }

        private async Task<UserEntity?> FindUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        private async Task<bool> IsUserEmailConfirmedAsync(UserEntity user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        private async Task<string> GenerateJwtTokenAsync(UserEntity user)
        {
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = await _jwtHandler.GetClaims(user);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims, rememberMe: true);
            
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
