using Flexybook.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Flexybook.Infrastructure.Seeders
{
    /// <summary>
    /// Seeds user data into the database.
    /// </summary>
    public static class UserSeeder
    {
        private const string DefaultUsername = "Flexybook";
        private const string DefaultEmail = "flexybook@example.com";
        private const string DefaultPassword = "Flexybook1234";
        private const string DefaultFirstName = "Flexy";
        private const string DefaultLastName = "Book";

        /// <summary>
        /// Seeds the default user if it doesn't already exist.
        /// </summary>
        /// <param name="userManager">The UserManager for managing users.</param>
        /// <param name="favouriteRestaurantId">Optional restaurant ID to add to user's favourites.</param>
        public static async Task SeedAsync(UserManager<UserEntity> userManager, Guid? favouriteRestaurantId = null)
        {
            if (await UserExistsAsync(userManager))
                return;

            var user = CreateDefaultUser(favouriteRestaurantId);
            await CreateUserAsync(userManager, user);
        }

        private static async Task<bool> UserExistsAsync(UserManager<UserEntity> userManager)
        {
            var existingUser = await userManager.FindByNameAsync(DefaultUsername);
            return existingUser != null;
        }

        private static UserEntity CreateDefaultUser(Guid? favouriteRestaurantId)
        {
            var user = new UserEntity
            {
                UserName = DefaultUsername,
                Email = DefaultEmail,
                FirstName = DefaultFirstName,
                LastName = DefaultLastName,
                Created = DateTime.Now,
                EmailConfirmed = true
            };

            if (favouriteRestaurantId.HasValue)
            {
                user.FavouredRestaurants = new List<Guid> { favouriteRestaurantId.Value };
            }

            return user;
        }

        private static async Task CreateUserAsync(UserManager<UserEntity> userManager, UserEntity user)
        {
            var result = await userManager.CreateAsync(user, DefaultPassword);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
        }
    }
}
