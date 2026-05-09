using Chess.ViewModel.ViewModelHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class GamePausedMenuViewModel : ViewModelBase, IDialogViewModel
    {
        public Action RequestClose { get; set; }

        public ICommand ContinueCommand { get; }
        public ICommand ResignCommand { get; }

        public GamePausedMenuViewModel()
        {
            ContinueCommand = new RelayCommand(o => RequestClose?.Invoke());
        }
    }
}
