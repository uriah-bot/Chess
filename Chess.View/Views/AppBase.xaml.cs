using Chess.View.Views;
using System.Windows;
using System.Windows.Controls;

namespace Chess.View
{
    /// <summary>
    /// Interaction logic for AppBase.xaml
    /// </summary>
    public partial class AppBase : Window
    {
        private Home _homeView;
        private Stats _statsView;
        private Settings _settingsView;
        private Help _helpView;
        //private Infp _infoView;

        public AppBase()
        {
            InitializeComponent();
            _homeView = new Home();
            MainContentArea.Content = _homeView;
        }

        private void Navigate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                string viewName = radioButton.Uid;

                switch (viewName)
                {
                    case "Home":
                        if (_homeView == null) _homeView = new Home();
                        MainContentArea.Content = _homeView;
                        break;

                    case "Statistics":
                        if (_statsView == null) _statsView = new Stats();
                        MainContentArea.Content = _statsView;
                        break;

                    case "Settings":
                        if (_settingsView == null) _settingsView = new Settings();
                        MainContentArea.Content = _settingsView;
                        break;

                    //case "AdvancedSettings":
                    //    if (_advancedSettingsView == null) _advancdSettingsView = new AdvancedSettings();
                    //    MainContentArea.Content = _advancdSettingsView;
                    //    break;

                    case "Help":
                        if (_helpView == null) _helpView = new Help();
                        MainContentArea.Content = _helpView;
                        break;
                }
            }
        }
    }
}
