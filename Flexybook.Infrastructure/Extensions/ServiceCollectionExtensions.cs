using Microsoft.Extensions.DependencyInjection;
using Flexybook.Infrastructure.Repositories;

namespace Flexybook.Infrastructure.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection to register infrastructure services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all infrastructure-level services including repository implementations.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
