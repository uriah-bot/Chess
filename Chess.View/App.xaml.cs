using Chess.Data;
using Chess.Service;
using Chess.View.Menus;
using Chess.View.Services;
using Chess.ViewModel;
using Chess.ViewModel.ViewModelHelper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IWindowService, WindowService>();
            services.AddTransient<MainWindow>();
            services.AddTransient<AppBase>();
            services.AddTransient<PopupWindow>();

            // Maps Func<Type, ViewModelBase> (NavigateCommand) to ServiceProvider's GetRequiredService method
            services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
                        viewModelType => (ViewModelBase)provider.GetRequiredService(viewModelType));

            services.AddViewModels();
            services.AddRepositories();
            services.AddServices();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var windowService = _serviceProvider.GetRequiredService<IWindowService>();
            var navService = _serviceProvider.GetRequiredService<INavigationService>();

            //windowService.SwitchWindow<AppBaseViewModel>();
            navService.NavigateTo<LoginViewModel>();
            windowService.SwitchWindow<MainViewModel>();
        }
    }
}
