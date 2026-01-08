using Microsoft.EntityFrameworkCore;
using Flexybook.Domain.Entities;

namespace Flexybook.Infrastructure.Repositories
{
    /// <summary>
    /// Base repository providing common CRUD operations for entities.
    /// </summary>
    /// <typeparam name="T">The entity type that inherits from BaseEntity.</typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly RestaurantContext _context;
        protected internal DbSet<T> _dbSet;

        public BaseRepository(RestaurantContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="baseEntity">The entity to create.</param>
        /// <returns>The created entity with generated values.</returns>
        public async Task<T> Create(T baseEntity)
        {
            var entity = await _dbSet.AddAsync(baseEntity);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public async Task<T?> Get(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Retrieves all entities of the specified type.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
