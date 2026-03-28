using Chess.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.View.Views
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        public Help()
        {
            InitializeComponent();
        }

        private void WatchVideo_Click(object sender, RoutedEventArgs e)
        {
            VideoGrid.Visibility = Visibility.Visible;
        }

        private async void SendEmailToDeveloper_Click(object sender, RoutedEventArgs e)
        {
            string subject = EmailSubject.Text;
            string body = EmailBody.Text;

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                MessageBox.Show("Please fill out both the subject and the message before sending.", "Missing Info");
                return;
            }

            // TODO: remove ts and connect to SendEmail() from servicw when in vm
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(AppConstants.APP_EMAIL, AppConstants.APP_KEY),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(AppConstants.APP_EMAIL),
                    Subject = $"[Custom Chess Feedback] {subject}",
                    Body = $"Message from User:\n\n{body}",
                };

                mailMessage.To.Add(AppConstants.APP_EMAIL);
                await smtpClient.SendMailAsync(mailMessage);

                EmailSubject.Clear();
                EmailBody.Clear();

                MessageBox.Show("Message sent successfully! Thank you for your feedback.", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not send message. Check your internet connection.\n\nError: {ex.Message}", "Failed to Send");
            }
        }

        private void CollapseVideo_Click(object sender, RoutedEventArgs e)
        {
            TutorialVideo.Stop();
            VideoGrid.Visibility = Visibility.Collapsed;
        }

        private void Forwards5Sec_Click(object sender, RoutedEventArgs e)
        {
            var duration = TutorialVideo.NaturalDuration.TimeSpan;
            var nextPosition = TutorialVideo.Position.Add(TimeSpan.FromSeconds(5));

            if (nextPosition > duration)
            {
                nextPosition = duration;
            }

            TutorialVideo.Position = nextPosition;
        }

        private void Backwards5Sec_Click(object sender, RoutedEventArgs e)
        {
            var zero = TimeSpan.FromSeconds(0);
            var nextPosition = TutorialVideo.Position.Subtract(TimeSpan.FromSeconds(5));

            if (nextPosition < zero)
            {
                nextPosition = zero;
            }

            TutorialVideo.Position = nextPosition;
        }

        private void PlayVideo_Click(object sender, RoutedEventArgs e) => TutorialVideo.Play();
        private void PauseVideo_Click(object sender, RoutedEventArgs e) => TutorialVideo.Pause();
    }
}
