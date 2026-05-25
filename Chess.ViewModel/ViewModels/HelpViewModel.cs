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
            SendEmailCommand = new RelayCommand(o => SendEmail());
        }

        public ICommand ShowVideoCommand { get; }
        public ICommand SendEmailCommand { get; }

        public bool _isVideoVisible = false;
        public bool IsVideoVisible
        {
            get => _isVideoVisible;
            set
            {
                _isVideoVisible = value;
                OnPropertyChanged(nameof(IsVideoVisible));
            }
        }

        private string _subject;
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        private string _body;
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                OnPropertyChanged(nameof(Body));
            }
        }

        private async void SendEmail()
        {
            await _emailService.SendEmail(Subject, Body);
            Subject = string.Empty;
            Body = string.Empty;
            return;
        }

        private void ShowVideo()
        {
            IsVideoVisible = true;
        }
    }
}
