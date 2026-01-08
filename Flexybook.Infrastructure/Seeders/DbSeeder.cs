using Flexybook.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Flexybook.Infrastructure.Seeders
{
    /// <summary>
    /// Coordinates database seeding operations for restaurants and users.
    /// </summary>
    public static class DbSeeder
    {
        /// <summary>
        /// Seeds the database with initial data including restaurants and users.
        /// </summary>
        /// <param name="db">The restaurant database context.</param>
        /// <param name="userManager">The UserManager for managing users.</param>
        public static async Task SeedAsync(RestaurantContext db, UserManager<UserEntity> userManager)
        {
            RestaurantSeeder.Seed(db);
            await UserSeeder.SeedAsync(userManager, RestaurantSeeder.GetOdenseRestaurantId());
            
            await db.SaveChangesAsync();
        }
    }
}
