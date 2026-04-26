using Chess.View.Menus;
using Chess.ViewModel;
using Chess.ViewModel.ViewModelHelper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Chess.View.Services
{
    public class WindowService : IWindowService
    {
        private static readonly Dictionary<Type, Type> mappings = new Dictionary<Type, Type>();
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

        public void ShowDialog<TViewModel>() where TViewModel : DialogViewModel
        {
            var dialogWindow = _serviceProvider.GetRequiredService<PopupWindow>();

            var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

            viewModel.RequestClose = () => dialogWindow.Close();
            dialogWindow.DataContext = viewModel;

            dialogWindow.ShowDialog();
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
