using System;
#if NET4
using System.Threading.Tasks;
#endif
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerManagerGUI.ViewModels;
using ArkServer;

namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für ServerPage.xaml
    /// </summary>
    /// 
    public partial class ServerPage : UserControl 
    {
        Server mServer;

        public ServerPage(Server server)
        {
            mServer = server;

            InitializeComponent();

            if (null != server.ServerName)
            {
                Label_ServerName.Content = server.ServerName;
            }

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

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            mServer.StartAndUpdateServer();
        }

        private void SaveServer_Click(object sender, RoutedEventArgs e)
        {
            /* TODO: add implementation */
        }

        private async void DeleteServer_Click(object sender, RoutedEventArgs e)
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);


            var mySettings = new MetroDialogSettings()
            {
                AnimateShow = true,
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
                ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
            };


            MessageDialogResult result = await metroWindow.ShowMessageAsync("Delete Server!", "Are you sure to delete the server?",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
               // ServerList ServerList = ServerList.MIServerList;

              //  ServerList.DeleteServer(mServer);
             //   _MainView.RefreshMenu();
            }
        }


        private void StopSettings_Click(object sender, RoutedEventArgs e)
        {

           Navigation.Navigation.Navigate(new ServerSettingsPage(mServer));
        }
    }
}

