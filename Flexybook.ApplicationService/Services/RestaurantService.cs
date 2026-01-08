using Flexybook.Domain.Responses.Restaurant;
using Flexybook.Infrastructure.Repositories;

namespace Flexybook.ApplicationService.Services
{
    /// <summary>
    /// Service for managing restaurant data and operations.
    /// </summary>
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        /// <summary>
        /// Retrieves a single restaurant with all related data.
        /// </summary>
        /// <param name="id">The unique identifier of the restaurant.</param>
        /// <returns>The restaurant response, or null if not found.</returns>
        public async Task<RestaurantResponse?> GetAsync(Guid id)
        {
            var restaurant = await _restaurantRepository.GetFullAsync(id);
            return restaurant?.ToResponse();
        }

        /// <summary>
        /// Retrieves all restaurants with their related data.
        /// </summary>
        /// <returns>A collection of restaurant responses.</returns>
        public async Task<IEnumerable<RestaurantResponse>> GetAllAsync()
        {
            var restaurants = await _restaurantRepository.GetAllFullAsync();
            return restaurants.Select(r => r.ToResponse());
        }

        /// <summary>
        /// Updates an existing restaurant's information.
        /// </summary>
        /// <param name="restaurant">The restaurant data to update.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdateAsync(RestaurantResponse restaurant)
        {
            var restaurantEntity = restaurant.ToEntity();
            return await _restaurantRepository.UpdateAsync(restaurantEntity);
        }
    }
}
