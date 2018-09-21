using System;
#if NET4
using System.Threading.Tasks;
#endif
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Server;

namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für ServerPage.xaml
    /// </summary>
    /// 
    public partial class ServerPage : UserControl
    {
        IServer mServer;

        public ServerPage(IServer server)
        {
            mServer = server;

            InitializeComponent(); 
        }

        private async void StopServer_Click(object sender, RoutedEventArgs e)
        {

            var metroWindow = (Application.Current.MainWindow as MetroWindow);


            var mySettings = new MetroDialogSettings()
            {
                AnimateShow = true,
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
            };


            MessageDialogResult result = await metroWindow.ShowMessageAsync("Stop Server!", "Stop server without saving!",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result != MessageDialogResult.Affirmative)
            {
                //TODO: call Server Stop function
            }
        }

        private void StopSettings_Click(object sender, RoutedEventArgs e)
        {

            Window window = new Window
            {
                Content = new ServerSettingsPage(ref mServer)
            };

            window.ShowDialog();

        }
    }
}
