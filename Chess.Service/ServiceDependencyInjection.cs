using Chess.Model;
using Chess.ViewModel.ViewModelHelper;
using Microsoft.Extensions.DependencyInjection;

namespace Chess.Service
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // services
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddTransient<StockfishCommunicationService>();
            services.AddSingleton<IGameManagerService, GameManagerService>();

            // Customizable decors
            services.AddSingleton<ICustomizableDecorManager<RadioChannelEntity>, RadioPlayer>();

            return services;
        }
    }
}
