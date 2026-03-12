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
        private Classical _classicalView;
        private Adventure _adventureView;
        private Stats _statsView;
        private Settings _settingsView;
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

                    case "Classical":
                        if (_adventureView == null) _classicalView = new Classical();
                        MainContentArea.Content = _classicalView;
                        break;

                    case "Adventure":
                        if (_adventureView == null) _adventureView = new Adventure();
                        MainContentArea.Content = _adventureView;
                        break;

                    case "Statistics":
                        if (_statsView == null) _statsView = new Stats();
                        MainContentArea.Content = _statsView;
                        break;

                    case "Settings":
                        if (_settingsView == null) _settingsView = new Settings();
                        MainContentArea.Content = _settingsView;
                        break;

                    //case "Info":
                    //    if (_settingsView == null) _infoView = new Info();
                    //    MainContentArea.Content = _infoView;
                    //    break;
                }
            }
        }
    }
}
