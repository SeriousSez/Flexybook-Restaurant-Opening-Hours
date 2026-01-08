using Microsoft.Extensions.DependencyInjection;
using Flexybook.ApplicationService.Services;
using Flexybook.ApplicationService.JwtFeatures;

namespace Flexybook.ApplicationService.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection to register application services.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Registers all application-level services including restaurant, profile, user services, and JWT handling.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The service collection for method chaining.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<JwtHandler>();
            return services;
        }
    }
}
