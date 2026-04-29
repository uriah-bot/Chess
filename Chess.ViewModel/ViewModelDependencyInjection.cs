using Chess.ViewModel.Stores;
using Chess.ViewModel.ViewModelHelper;
using Microsoft.Extensions.DependencyInjection;

namespace Chess.ViewModel
{
    public static class ViewModelDependencyInjection
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Stores
            services.AddSingleton<IUserStore, UserStore>();
            services.AddSingleton<INavigationStore, NavigationStore>();
            services.AddSingleton<IGameHistoryStore, GameHistoryStore>();
            services.AddSingleton<ISettingsStore, SettingsStore>();
            services.AddSingleton<IDecorStore, DecorStore>();

            // ViewModel Helpers
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IGameManagerService, GameManagerService>();

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<AppBaseViewModel>();
            services.AddTransient<AdventureViewModel>();
            services.AddTransient<ClassicalViewModel>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<StatsViewModel>();
            services.AddTransient<HelpViewModel>();
            services.AddTransient<LeaderboardViewModel>();

            services.AddTransient<GameOverMenuViewModel>();
            services.AddTransient<ModifiedGamePausedMenuViewModel>();
            services.AddTransient<PromotionMenuViewModel>();
            services.AddTransient<GamePausedMenuViewModel>();

            return services;
        }
    }
}
