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
using System.Threading;
using System.Threading.Tasks;
using ArkServer.Logging;

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
        public Task mtask1;
        public Task mtask2;

        internal ServerPage(ShellViewModel ShellViewModel, Server server)
        {
            mServer = server;
            mShellViewModel = ShellViewModel;

            InitializeComponent();

            var b1 = new Binding("ServerName");
            var b2 = new Binding("InstalledVersion");
            var b3 = new Binding("serverState");
            var b4 = new Binding("DailyRestartDate");
            b1.Source = mServer;
            b2.Source = mServer;
            b3.Source = mServer;
            b4.Source = mServer;

            Label_ServerName.SetBinding(Label.ContentProperty, b1);
            Version.SetBinding(Label.ContentProperty, b2);
            Label_Status.SetBinding(Label.ContentProperty, b3);
            DailyRestartSelect.SetBinding(DateTimePicker.SelectedDateProperty, b4);


            TextBox_ServerName.Text = mServer.ServerName;
            TextBox_ServerIp.Text = mServer.ServerIp;
            TextBox_ServerPath.Text = mServer.ArkSurvivalFolder;
            TextBox_ServerPort.Text = mServer.Port.ToString();
            TextBox_ServerQuerryPort.Text = mServer.QueryPort.ToString();
            TextBox_ServerRconPort.Text = mServer.RconPort.ToString();
            TextBox_ServerStartArgument.Text = mServer.ServerStartArgument.ToString();
            TextBox_ServerRconPassword.Text = mServer.RconPassword;

            RconEnabled_Button.IsChecked = mServer.RconEnabled;
            AutoUpdateEnabeld_Button.IsChecked = mServer.ArkAutoUpdateIsEnabled;
            AutoModEnabeld_Button.IsChecked = mServer.ModAutoUpdateIsEnabled;
            NotifyDiscordEnabeld_Button.IsChecked = mServer.NotifyDiscordIsEnabled;
            DailyRestartEnabeld_Button.IsChecked = mServer.DailyRestartIsEnabeld;
            TextBox_Affinity.Text = mServer.Affinity;

            if (mServer.DailyRestartDate != DateTime.MinValue)
            {
                DailyRestartSelect.DisplayDate = mServer.DailyRestartDate;
                DailyRestartSelect.SelectedDate = mServer.DailyRestartDate;
            }


            List<string> Restarttimes = new List<string>
            {
                "60 min",
                "45 min",
                "30 min",
                "15 min",
                "10 min",
                "5 min",
                "1 min",
                "now"
            };

            RestartTimerButton.ItemsSource = Restarttimes;
            ServerStopTimerButton.ItemsSource = Restarttimes;

         //   DailyRestartSelect.DisplayDate

        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            LogGrid.Items.Refresh();
            LogGrid.ItemsSource = mServer.logs.GetLogs();

            DailyRestartSelect.DisplayDate = mServer.DailyRestartDate;
            DailyRestartSelect.SelectedDate = mServer.DailyRestartDate;
            

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


            MessageDialogResult result = await metroWindow.ShowMessageAsync("Stop Server!", "Stop server without saving or notification!",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Affirmative)
            {
                Utilities util = new Utilities(mServer);
                util.ServerStopImmediately();
            }
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            mServer.StartServerHandler();
        }

        private async void SaveServer_Click(object sender, RoutedEventArgs e)
        {
            Utilities util = new Utilities(mServer);
            if (await util.SaveServerAsync())
            {
                mServer.logs.AddLog(LogType.Information, "World saved");
            }
            else
            {
                mServer.logs.AddLog(LogType.Information, "World save failed");
            }

            //if (!mServer.RestartTask.IsCompleted)
            //{
            //    mServer.cancellationTokenRestart.Cancel();
            //    mServer.cancellationTokenRestart.Dispose(); ;
            //}
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
                mServer.RconEnabled =  (bool) RconEnabled_Button.IsChecked;
                mServer.ArkAutoUpdateIsEnabled = (bool)AutoUpdateEnabeld_Button.IsChecked;
                mServer.ModAutoUpdateIsEnabled = (bool)AutoModEnabeld_Button.IsChecked;
                mServer.NotifyDiscordIsEnabled = (bool)NotifyDiscordEnabeld_Button.IsChecked;
                mServer.Affinity = TextBox_Affinity.Text;
                mServer.DailyRestartDate = (DateTime) DailyRestartSelect.SelectedDate;
                mServer.DailyRestartIsEnabeld = (bool)DailyRestartEnabeld_Button.IsChecked;


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
            if (mServer.RconEnabled)
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
                        Utilities util = new Utilities(mServer);
                        bool successfully = await util.BroadcastMessageAsync(Broadcast_Textbox.Text);



                        if (true == NotifyDiscord_Button.IsChecked)
                        {
                            Webhook webhook = new Webhook(WebhookDataInterface.MWebhookDataInterface.WebhoockLink);
                            await webhook.Send("```" + mServer.ServerName + " broadcast: " + Broadcast_Textbox.Text + "```");
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
            else
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);

                var mySettings = new MetroDialogSettings()
                {
                    AnimateShow = true,
                    AffirmativeButtonText = "OK",
                    ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
                };

                MessageDialogResult result = await metroWindow.ShowMessageAsync("Server is disabled", "Enable Server Rcon run to broadcast messages",
                    MessageDialogStyle.Affirmative, mySettings);
            }



        }

        private async void RestartServer_Click(object sender, RoutedEventArgs e)
        {
            string reason = "unknown";

            if (mServer.serverState == ServerState.Running)
            {
                if (!string.IsNullOrEmpty(Restart_Textbox.Text))
                {
                    reason = Restart_Textbox.Text;
                }
                Utilities util = new Utilities(mServer);
                mServer.cancellationTokenRestart = new CancellationTokenSource();

                switch (RestartTimerButton.SelectedIndex)
                {
                    case 0:
                        mServer.RestartTask = util.ServerStopOrRestart(45, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 1:
                        mServer.RestartTask = util.ServerStopOrRestart(45, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 2:
                        mServer.RestartTask = util.ServerStopOrRestart(30, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 3:
                        mServer.RestartTask = util.ServerStopOrRestart(15, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 4:
                        mServer.RestartTask = util.ServerStopOrRestart(10, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 5:
                        mServer.RestartTask = util.ServerStopOrRestart(5, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 6:
                        mServer.RestartTask = util.ServerStopOrRestart(1, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    case 7:
                        mServer.RestartTask = util.ServerStopOrRestart(0, reason, true, mServer.cancellationTokenRestart.Token);
                        break;
                    default:
                        break;
                }
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

                MessageDialogResult result = await metroWindow.ShowMessageAsync("Server is not running", "The server has to run to schedule a server restart",
                    MessageDialogStyle.Affirmative, mySettings);

            }
        }


        private async void ServerStop_Click(object sender, RoutedEventArgs e)
        {
            string reason = "unknown";

            if (mServer.serverState == ServerState.Running)
            {

                if (!string.IsNullOrEmpty(ServerStop_Textbox.Text))
                {
                    reason = ServerStop_Textbox.Text;
                }
                Utilities util = new Utilities(mServer);
                mServer.cancellationTokenRestart = new CancellationTokenSource();

                switch (ServerStopTimerButton.SelectedIndex)
                {
                    case 0:
                        mServer.RestartTask = util.ServerStopOrRestart(45, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 1:
                        mServer.RestartTask = util.ServerStopOrRestart(45, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 2:
                        mServer.RestartTask = util.ServerStopOrRestart(30, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 3:
                        mServer.RestartTask = util.ServerStopOrRestart(15, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 4:
                        mServer.RestartTask = util.ServerStopOrRestart(10, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 5:
                        mServer.RestartTask = util.ServerStopOrRestart(5, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 6:
                        mServer.RestartTask = util.ServerStopOrRestart(1, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    case 7:
                        mServer.RestartTask = util.ServerStopOrRestart(0, reason, false, mServer.cancellationTokenRestart.Token);
                        break;
                    default:
                        break;
                }
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

                MessageDialogResult result = await metroWindow.ShowMessageAsync("Server is not running", "The server has to run to schedule a server stop",
                    MessageDialogStyle.Affirmative, mySettings);

            }
        }

        private void SplitButton_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = ((Selector)sender).SelectedIndex;
            var item = ((Selector)sender).SelectedItem;
            var value = ((Selector)sender).SelectedValue;
        }

        private void CancelTask_Click(object sender, RoutedEventArgs e)
        {
            mServer.CancelRunningRestartOrStop();
        }
    }
}

