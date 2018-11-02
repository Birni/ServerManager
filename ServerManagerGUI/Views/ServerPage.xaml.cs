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
using DiscordWebhook;
using ArkServer.ServerUtilities;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

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

        internal ServerPage(ShellViewModel ShellViewModel, Server server)
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
            TextBox_ServerRconPassword.Text = mServer.RconPassword;

            List<string> Restarttimes = new List<string>();

            Restarttimes.Add("60 min");
            Restarttimes.Add("45 min");
            Restarttimes.Add("30 min");
            Restarttimes.Add("15 min");
            Restarttimes.Add("10 min");
            Restarttimes.Add("5 min");
            Restarttimes.Add("1 min");
            Restarttimes.Add("now");

            RestartTimerButton.ItemsSource = Restarttimes;
            ServerStopTimerButton.ItemsSource = Restarttimes;
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
            mServer.StartServerHandler();
        }

        private void SaveServer_Click(object sender, RoutedEventArgs e)
        {
            mServer.CheckforUpdatesAsync(sender, e);
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
                mServer.RconPassword = TextBox_ServerRconPassword.Text;

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

        private async void UserBroadcast_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Broadcast_Textbox.Text))
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);

                var mySettings = new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AffirmativeButtonText = "OK",
                    ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
                };

                MessageDialogResult result = await metroWindow.ShowMessageAsync("No broadcast message", "Please enter a message to broadcast",
                    MessageDialogStyle.Affirmative, mySettings);
            }
            else
            {
                if (mServer.serverState == ServerState.Running)
                {
                    Utilities util = new ArkServer.ServerUtilities.Utilities(mServer);
                    bool successfully = await  util.BroadcastMessageAsync(Broadcast_Textbox.Text);



                    if (true == NotifyDiscord_Button.IsChecked)
                    {
                        Webhook webhook = new Webhook(WebhookDataInterface.MWebhookDataInterface.WebhoockLink);
                        await webhook.Send("```"+ mServer.ServerName + " broadcast: " + Broadcast_Textbox.Text + "```");
                    }
                    Broadcast_Textbox.Text = "";

                }
                else
                {
                    var metroWindow = (Application.Current.MainWindow as MetroWindow);

                    var mySettings = new MetroDialogSettings()
                    {
                        AnimateShow = true,
                        AffirmativeButtonText = "OK",
                        ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
                    };

                    MessageDialogResult result = await metroWindow.ShowMessageAsync("Server is not running", "The server has to run to broadcast messages",
                        MessageDialogStyle.Affirmative, mySettings);
                }
            }



        }

        private async void RestartServer_Click(object sender, RoutedEventArgs e)
        {
            string reason = "unknown";

            if (!string.IsNullOrEmpty(Restart_Textbox.Text))
            {
                reason = Restart_Textbox.Text;
            }
            Utilities util = new Utilities(mServer);

            switch (RestartTimerButton.SelectedIndex)
            {
                case 0:
                    await util.ServerStop(60, reason, true);
                    break;
                case 1:
                    await util.ServerStop(45, reason, true);
                    break;
                case 2:
                    await util.ServerStop(30, reason, true);
                    break;
                case 3:
                    await util.ServerStop(15, reason, true);
                    break;
                case 4:
                    await util.ServerStop(10, reason, true);
                    break;
                case 5:
                    await util.ServerStop(5, reason, true);
                    break;
                case 6:
                    await util.ServerStop(1, reason, true);
                    break;
                case 7:
                    await util.ServerStop(0, reason, true);
                    break;
                default:
                    break;
            }
        }

        private async void ServerStop_Click(object sender, RoutedEventArgs e)
        {
            string reason = "unknown";

            if (!string.IsNullOrEmpty(ServerStop_Textbox.Text))
            {
                reason = ServerStop_Textbox.Text;
            }
            Utilities util = new Utilities(mServer);

            switch (ServerStopTimerButton.SelectedIndex)
            {
                case 0:
                    await util.ServerStop(60, reason, false);
                    break;
                case 1:
                    await util.ServerStop(45, reason, false);
                    break;
                case 2:
                    await util.ServerStop(30, reason, false);
                    break;
                case 3:
                    await util.ServerStop(15, reason, false);
                    break;
                case 4:
                    await util.ServerStop(10, reason, false);
                    break;
                case 5:
                    await util.ServerStop(5, reason, false);
                    break;
                case 6:
                    await util.ServerStop(1, reason, false);
                    break;
                case 7:
                    await util.ServerStop(0, reason, false);
                    break;
                default:
                    break;
            }
        }

        private void SplitButton_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ((Selector)sender).SelectedIndex;
            var item = ((Selector)sender).SelectedItem;
            var value = ((Selector)sender).SelectedValue;
        }

    }
}

