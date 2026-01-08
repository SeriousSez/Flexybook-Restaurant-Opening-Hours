using Flexybook.Domain.Entities;
using Flexybook.Domain.Responses;
using Flexybook.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Flexybook.ApplicationService.Services
{
    /// <summary>
    /// Service for managing user data and operations.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;

        public UserService(IUserRepository userRepository, UserManager<UserEntity> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The user response, or null if not found.</returns>
        public async Task<UserResponse?> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.ToResponse();
        }

        /// <summary>
        /// Updates a user's favourite restaurants list.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="restaurantId">The restaurant ID to add or remove from favourites.</param>
        /// <param name="isFavourite">True to add to favourites; false to remove.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateUserFavouritesAsync(string userId, Guid restaurantId, bool isFavourite)
        {
            var user = await FindUserByIdAsync(userId);
            if (user == null)
                return false;

            UpdateUserFavouritesList(user, restaurantId, isFavourite);

            return await _userRepository.UpdateAsync(user);
        }

        private async Task<UserEntity?> FindUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        private static void UpdateUserFavouritesList(UserEntity user, Guid restaurantId, bool isFavourite)
        {
            var favourites = user.FavouredRestaurants?.ToList() ?? new List<Guid>();

            if (isFavourite)
            {
                if (!favourites.Contains(restaurantId))
                {
                    favourites.Add(restaurantId);
                }
            }
            else
            {
                favourites.Remove(restaurantId);
            }

            user.FavouredRestaurants = favourites;
        }
    }
}
