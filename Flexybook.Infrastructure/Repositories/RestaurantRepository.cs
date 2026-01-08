using Flexybook.Domain.Entities.Restaurant;
using Microsoft.EntityFrameworkCore;

namespace Flexybook.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing restaurant data operations.
    /// </summary>
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantContext _context;

        public RestaurantRepository(RestaurantContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a restaurant by ID without related data.
        /// </summary>
        /// <param name="id">The unique identifier of the restaurant.</param>
        /// <returns>The restaurant entity if found; otherwise, null.</returns>
        public async Task<Restaurant?> GetAsync(Guid id)
        {
            return await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Retrieves a restaurant by ID including images and opening hours.
        /// </summary>
        /// <param name="id">The unique identifier of the restaurant.</param>
        /// <returns>The restaurant entity with related data if found; otherwise, null.</returns>
        public async Task<Restaurant?> GetFullAsync(Guid id)
        {
            return await _context.Restaurants
                .Include(r => r.Images)
                .Include(r => r.OpeningHours)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Retrieves all restaurants without related data.
        /// </summary>
        /// <returns>A collection of all restaurants.</returns>
        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _context.Restaurants.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Retrieves all restaurants including images and opening hours.
        /// </summary>
        /// <returns>A collection of all restaurants with related data.</returns>
        public async Task<IEnumerable<Restaurant>> GetAllFullAsync()
        {
            return await _context.Restaurants
                .Include(r => r.Images)
                .Include(r => r.OpeningHours)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Updates a restaurant's information in the database.
        /// </summary>
        /// <param name="restaurant">The restaurant entity with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateAsync(Restaurant restaurant)
        {
            var tracked = _context.ChangeTracker.Entries<Restaurant>()
                .FirstOrDefault(e => e.Entity.Id == restaurant.Id);

            if (tracked != null)
                tracked.State = EntityState.Detached;

            _context.Restaurants.Update(restaurant);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
