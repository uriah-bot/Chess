using Chess.Service;
using Chess.ViewModel.Stores;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class HelpViewModel : ValidatableViewModel
    {
        private readonly IEmailService _emailService;
        public readonly IDecorStore _decorStore;
        public double UserVolume = 0;
        
        public HelpViewModel(IEmailService emailService, IDecorStore decorStore)
        {
            _emailService = emailService;
            _decorStore = decorStore;

            ShowVideoCommand = new RelayCommand(o => ShowVideo());
            SendEmailCommand = new RelayCommand(o => SendEmail(), o => !string.IsNullOrEmpty(Subject) && !string.IsNullOrEmpty(Body));
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

                ClearErrors();
                if (string.IsNullOrWhiteSpace(Subject))
                {
                    AddError("\"Email Subject\" is a required field.");
                }
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

                ClearErrors();
                if (string.IsNullOrWhiteSpace(Body))
                {
                    AddError("\"Email Body\" is a required field.");
                }
            }
        }

        private async void SendEmail()
        {
            Subject = string.Empty;
            Body = string.Empty;
            await _emailService.SendEmail(Subject, Body);
            ClearErrors();
        }

        private void ShowVideo()
        {
            IsVideoVisible = true;
            UserVolume = _decorStore.CurrentVolume;
        }
    }
}
