using System;
using System.Collections.Generic;
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

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for Classical.xaml
    /// </summary>
    public partial class Classical : UserControl
    {
        public event EventHandler SettingsRequested;

        public Classical()
        {
            InitializeComponent();
            SettingsRequested += SettingsRequested;
        }

        private void SettingNav_Click(object sender, RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
