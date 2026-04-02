namespace Chess.ViewModel.ViewModelHelper
{
    public interface IWindowService
    {
        void SwitchWindow<TViewModel>() where TViewModel : ViewModelBase;
    }
}
