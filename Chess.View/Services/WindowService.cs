using Chess.ViewModel;
using Chess.ViewModel.ViewModelHelper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Chess.View.Services
{
    public class WindowService : IWindowService
    {
        private readonly IServiceProvider _serviceProvider;
        private Window _currentWindow;

        public WindowService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SwitchWindow<TViewModel>() where TViewModel : ViewModelBase
        {
            Window oldWindow = _currentWindow;

            Window window = null;
            if (typeof(TViewModel) == typeof(MainViewModel)) window = _serviceProvider.GetRequiredService<MainWindow>();
            if (typeof(TViewModel) == typeof(AppBaseViewModel)) window = _serviceProvider.GetRequiredService<AppBase>();

            if (window != null)
            {
                window.DataContext = _serviceProvider.GetRequiredService<TViewModel>();
                window.Show();

                CloseCurrentWindow();

                _currentWindow = window;
            }
        }

        private void CloseCurrentWindow()
        {
            if (_currentWindow?.DataContext is IDisposable window)
            {
                window.Dispose();

                _currentWindow.DataContext = null;
            }

            _currentWindow?.Close();
            _currentWindow = null;
        }
    }
}
