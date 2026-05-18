using Chess.Model;
using Microsoft.Extensions.DependencyInjection;
using static Chess.Data.Repositories;

namespace Chess.Data
{
    public static class RepositoryDependencyInjection 
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepo>();
            services.AddSingleton<IGameRepository, GameRepo>();
            services.AddSingleton<IRadioChannelRepository, RadioChannelRepo>();
            services.AddSingleton<ISettingsRepository, SettingsRepo>();
            services.AddSingleton<IJSONRepository<string, ModifierData>, JSONRepository<string, ModifierData>>();

            return services;
        }
    }
}
