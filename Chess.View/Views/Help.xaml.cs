using Chess.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private void StartTutorial_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Trigger your interactive tutorial overlay or navigate to a Tutorial view
            MessageBox.Show("Tutorial starting soon!", "Tutorial");
        }

        private void WatchVideo_Click(object sender, RoutedEventArgs e)
        {
            // Opens a YouTube video link in the user's default web browser
            string videoUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = videoUrl,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Could not open the browser. Please visit: " + videoUrl);
            }
        }

        private void SendEmailToDeveloper_Click(object sender, RoutedEventArgs e)
        {
            // Opens the default email client with a pre-filled subject line
            string subject = "Feedback/Bug Report - Custom Chess";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"mailto:{AppConstants.APP_RECEIVER_EMAIL}?subject={subject}",
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Could not open email client. Please email us at: " + AppConstants.APP_RECEIVER_EMAIL);
            }
        }
    }
}
