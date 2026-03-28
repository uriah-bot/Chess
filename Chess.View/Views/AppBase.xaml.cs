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

        private Adventure _adventureView;
        private Classical _classicalView;

        public AppBase()
        {
            InitializeComponent();
            _homeView = new Home();
            _homeView.AdventureRequested += AdventureRequested;
            _homeView.ClassicalRequested += ClassicalRequested;
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

        public void AdventureRequested(object sender, EventArgs e)
        {
            if (_adventureView == null) _adventureView = new Adventure();
            MainContentArea.Content = _adventureView;
        }

        public void ClassicalRequested(object sender, EventArgs e)
        {
            if (_classicalView == null)
            {
                _classicalView = new Classical();
                _classicalView.SettingsRequested += SettingsRequested;
            }

            MainContentArea.Content = _classicalView;
        }

        public void SettingsRequested(object sender, EventArgs e)
        {
            if (_settingsView == null) _settingsView = new Settings();

            SettingsRadioButton.IsChecked = true;
            MainContentArea.Content = _settingsView;
        }
    }
}
