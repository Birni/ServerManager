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
    partial class ServerPage : UserControl 
    {
        Server mServer = null;
        ShellViewModel mShellViewModel = null;

        internal ServerPage(ShellViewModel ShellViewModel , Server server)
        {
            mServer = server;
            mShellViewModel = ShellViewModel;

            InitializeComponent();

            if (null != mServer.ServerName)
            {
                Label_ServerName.Content = server.ServerName;
            }

        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            Label_ServerName.Content = mServer.ServerName;
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
                mServer.DeliteSaveFile();
                ServerCollection.MServerCollection.RemoveServer(mServer);
                mServer.Delete();
                mShellViewModel.RefreshMenu();
                Navigation.Navigation.Navigate(new Uri("Views/MainPage.xaml", UriKind.RelativeOrAbsolute));

            }
        }


        private void StopSettings_Click(object sender, RoutedEventArgs e)
        {

           Navigation.Navigation.Navigate(new ServerSettingsPage(mShellViewModel , mServer));
        }
    }
}

