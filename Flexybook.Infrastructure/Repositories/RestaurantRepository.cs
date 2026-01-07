using Flexybook.Domain.Entities.Restaurant;
using Microsoft.EntityFrameworkCore;

namespace Flexybook.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantContext _context;

        public RestaurantRepository(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<Restaurant?> Get(Guid id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Restaurant?> GetFull(Guid id)
        {
            return await _context.Restaurants
                .Include(r => r.Images)
                .Include(r => r.OpeningHours)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Restaurant>> GetAll()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllFull()
        {
            return await _context.Restaurants
                .Include(r => r.Images)
                .Include(r => r.OpeningHours)
                .ToListAsync();
        }
    }
}
