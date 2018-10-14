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
using System.Windows.Data;
using System.Windows.Media;

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

            var b1 = new Binding("ServerName");
            var b2 = new Binding("InstalledVersion");
            var b3 = new Binding("serverState");
            b1.Source = mServer;
            b2.Source = mServer;
            b3.Source = mServer;

            Label_ServerName.SetBinding(Label.ContentProperty, b1);
            Version.SetBinding(Label.ContentProperty, b2);
            Label_Status.SetBinding(Label.ContentProperty, b3);


            TextBox_ServerName.Text = mServer.ServerName;
            TextBox_ServerIp.Text = mServer.ServerIp;
            TextBox_ServerPath.Text = mServer.ArkSurvivalFolder;
            TextBox_ServerPort.Text = mServer.Port.ToString();
            TextBox_ServerQuerryPort.Text = mServer.QueryPort.ToString();
            TextBox_ServerRconPort.Text = mServer.RconPort.ToString();
            TextBox_ServerStartArgument.Text = mServer.ServerStartArgument.ToString();


        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            LogGrid.Items.Refresh();
            LogGrid.ItemsSource = mServer.logs.GetLogs();

        }

        private void StatusDataChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Label_Status.ContentStringFormat == "Running")
            {
                Label_Status.Foreground = Brushes.Green;
            }
            else if (Label_Status.ContentStringFormat == "Crashed")
            {
                Label_Status.Foreground = Brushes.Red;
            }
            else
            {
                Label_Status.Foreground = Brushes.Black;
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
                mServer.DeliteSaveFile();
                ServerCollection.MServerCollection.RemoveServer(mServer);
                mServer.Delete();
                mShellViewModel.RefreshMenu();
                Navigation.Navigation.Navigate(new Uri("Views/MainPage.xaml", UriKind.RelativeOrAbsolute));

            }
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

                        mServer.OnPropertyChanged("ServerName");

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
        }
    }
}

