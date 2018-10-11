using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerManagerGUI;
using ServerManagerGUI.ViewModels;
using ArkServer;

namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für ServerSettingsPage.xaml
    /// </summary>
    partial class ServerSettingsPage : UserControl 
    {
        Server mServer;
        ShellViewModel mShellViewModel = null;

        // ServerList ServerList = ;

        internal ServerSettingsPage(ShellViewModel ShellViewModel ,  Server server)
        {
            mServer = server;
            mShellViewModel = ShellViewModel;

            InitializeComponent();

            TextBox_ServerName.Text = mServer.ServerName;
            TextBox_ServerIp.Text = mServer.ServerIp;
            TextBox_ServerPath.Text = mServer.ArkSurvivalFolder;
            TextBox_ServerPort.Text = mServer.Port.ToString();
            TextBox_ServerQuerryPort.Text = mServer.QueryPort.ToString();
            TextBox_ServerRconPort.Text = mServer.RconPort.ToString();
            TextBox_ServerStartArgument.Text = mServer.ServerStartArgument.ToString();
        }



        private async void ServerSettingsSave_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TextBox_ServerName.Text))
            {
                mServer.ServerIp = TextBox_ServerIp.Text;
                mServer.ArkSurvivalFolder = TextBox_ServerPath.Text;
                mServer.Port = Int32.Parse(TextBox_ServerPort.Text);
                mServer.QueryPort = Int32.Parse(TextBox_ServerQuerryPort.Text);
                mServer.RconPort = Int32.Parse(TextBox_ServerRconPort.Text);
                mServer.ServerStartArgument = TextBox_ServerStartArgument.Text;

                if (mServer.ServerName != TextBox_ServerName.Text)
                {
                    if (ServerCollection.MServerCollection.IsAlreadyInCollection(TextBox_ServerName.Text))
                    {
                        var metroWindow = (Application.Current.MainWindow as MetroWindow);

                        var mySettings = new MetroDialogSettings()
                        {
                            AnimateShow = true,
                            AffirmativeButtonText = "OK",
                            ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
                        };

                        MessageDialogResult result = await metroWindow.ShowMessageAsync("Invalid server name !", "There is already a server named" + TextBox_ServerName.Text,
                            MessageDialogStyle.Affirmative, mySettings);
                    }
                    else
                    {
                        /* save file is named like the server ? server renamed -> new save file  */ 
                        mServer.DeliteSaveFile();
                        ServerCollection.MServerCollection.RemoveServer(mServer);

                        mServer.ServerName = TextBox_ServerName.Text;

                        ServerCollection.MServerCollection.AddServer(mServer);
                        mServer.SaveToFile();
                        /* Server name changed -> refresh menu items */
                        mShellViewModel.RefreshMenu();
                    }
                }
                else
                {
                    mServer.SaveToFile();
                    ServerCollection.MServerCollection.UpdateServer(mServer);

                }
            }
            Navigation.Navigation.GoBack();
        }

        private void ServerSettingsCancel_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Navigation.GoBack();
        }
    }
}
