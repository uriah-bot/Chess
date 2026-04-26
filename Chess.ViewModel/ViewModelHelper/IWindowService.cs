namespace Chess.ViewModel.ViewModelHelper
{
    public interface IWindowService
    {
        void SwitchWindow<TViewModel>() where TViewModel : ViewModelBase;
        void ShowDialog<TViewModel>() where TViewModel : DialogViewModel;
    }
}
