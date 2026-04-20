using Microsoft.Extensions.DependencyInjection;

namespace Chess.Service
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Auth
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddSingleton<StockfishCommunicationService>();
            services.AddSingleton<StockfishHelper>();

            return services;
        }
    }
}
