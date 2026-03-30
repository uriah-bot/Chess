namespace Chess.ViewModel.ViewModelHelper
{
    public interface IWindowService
    {
        void ShowWindow<TViewModel>() where TViewModel : ViewModelBase;
        void CloseCurrentWindow();
    }
}
