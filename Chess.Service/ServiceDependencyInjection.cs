using Microsoft.Extensions.DependencyInjection;

namespace Chess.Service
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Auth
            services.AddSingleton<AuthService>();

            // Stores
            services.AddSingleton<UserStore>();

            return services;
        }
    }
}
