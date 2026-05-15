using Chess.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Chess.Service
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Auth
            services.AddSingleton<IAuthService, AuthService>();

            // Other services
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddTransient<StockfishCommunicationService>();
            services.AddSingleton<GameLogicHelper>();

            // Customizable decors
            services.AddSingleton<ICustomizableDecorManager<RadioChannelEntity>, RadioPlayer>();

            return services;
        }
    }
}
