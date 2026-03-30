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

            // ViewModel Helpers
            services.AddTransient<INavigationService, NavigationService>();

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

            return services;
        }
    }
}
