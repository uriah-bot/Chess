using Chess.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class HelpViewModel : ViewModelBase
    {
        private readonly IEmailService _emailService;
        
        public HelpViewModel(IEmailService emailService)
        {
            _emailService = emailService;

            ShowVideoCommand = new RelayCommand(o => ShowVideo());
            CollapseVideoCommand = new RelayCommand(o => CollapseVideo());
            BackFiveSecCommand = new RelayCommand(o => BackFiveSec());
            ForwardsFiveSecCommand = new RelayCommand(o => ForwardsFiveSec());
            CloseVideoCommand = new RelayCommand(o => CloseVideo());
            PlayVideoCommand = new RelayCommand(o => PlayVideo());
            SendEmailCommand = new RelayCommand(o => SendEmail());
        }

        public ICommand ShowVideoCommand { get; }
        public ICommand CollapseVideoCommand { get; }
        public ICommand BackFiveSecCommand { get; }
        public ICommand ForwardsFiveSecCommand { get; }
        public ICommand CloseVideoCommand { get; }
        public ICommand PlayVideoCommand { get; }
        public ICommand SendEmailCommand { get; }

        private async void SendEmail()
        {
            //await _emailService.SendEmail();
            return;
        }

        private void PlayVideo()
        {
            throw new NotImplementedException();
        }

        private void CloseVideo()
        {
            throw new NotImplementedException();
        }

        private void ForwardsFiveSec()
        {
            throw new NotImplementedException();
        }

        private void BackFiveSec()
        {
            throw new NotImplementedException();
        }

        private void CollapseVideo()
        {
            throw new NotImplementedException();
        }

        private void ShowVideo()
        {
            throw new NotImplementedException();
        }
    }
}
