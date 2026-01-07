using Microsoft.Extensions.DependencyInjection;
using Flexybook.ApplicationService.Services;

namespace Flexybook.ApplicationService.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRestaurantService, RestaurantService>();
            return services;
        }
    }
}
