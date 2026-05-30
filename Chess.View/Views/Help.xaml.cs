using Chess.Model;
using Chess.ViewModel;
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

namespace Chess.View
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

        private void CollapseVideo_Click(object sender, RoutedEventArgs e)
        {
            TutorialVideo.Stop();
            if (DataContext is HelpViewModel viewModel)
            {
                viewModel.IsVideoVisible = false;
                viewModel._decorStore.CurrentVolume = viewModel.UserVolume;
            }
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

        private void PlayVideo_Click(object sender, RoutedEventArgs e) => Play();
        private void PauseVideo_Click(object sender, RoutedEventArgs e) => Pause();

        public void Pause()
        {
            TutorialVideo.Pause();
            if (DataContext is HelpViewModel viewModel)
            {
                viewModel._decorStore.CurrentVolume = viewModel.UserVolume;
            }
        }

        public void Play()
        {
            TutorialVideo.Play();
            if (DataContext is HelpViewModel viewModel)
            {
                viewModel._decorStore.CurrentVolume = 0;
            }
        }
    }
}
