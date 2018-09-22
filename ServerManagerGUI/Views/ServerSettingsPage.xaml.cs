using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ServerManagerGUI;
using ServerManagerGUI.ViewModels;
using Server;

namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für ServerSettingsPage.xaml
    /// </summary>
    public partial class ServerSettingsPage : UserControl 
    {
        IServer mServer;
        MainViewModel _MainView;

        IServerList ServerList = new IServerList();

        public ServerSettingsPage(MainViewModel MainView , IServer server)
        {
            mServer = server;
            _MainView = MainView;

            InitializeComponent();

            TextBox_ServerName.Text = mServer.ServerName;
            TextBox_ServerIp.Text = mServer.ServerIp;
            TextBox_ServerPath.Text = mServer.ArkSurvivalFolder;
            TextBox_ServerPort.Text = mServer.Port.ToString();
            TextBox_ServerQuerryPort.Text = mServer.QueryPort.ToString();
            TextBox_ServerRconPort.Text = mServer.RconPort.ToString();
            TextBox_ServerStartArgument.Text = mServer.ServerStartArgument.ToString();
        }



        private void ServerSettingsSave_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_ServerName.Text != null)
            {

                mServer.ServerIp = TextBox_ServerIp.Text;
                mServer.ArkSurvivalFolder = TextBox_ServerPath.Text;
                mServer.Port = Int32.Parse(TextBox_ServerPort.Text);
                mServer.QueryPort = Int32.Parse(TextBox_ServerQuerryPort.Text);
                mServer.RconPort = Int32.Parse(TextBox_ServerRconPort.Text);
                mServer.ServerStartArgument = TextBox_ServerStartArgument.Text;



                if (TextBox_ServerName.Text == mServer.ServerName)
                {
                    ServerList.AddOrUpdateServer(mServer);
                }
                else
                {
                    ServerList.ChangeKey(mServer.ServerName, TextBox_ServerName.Text);
                    mServer.ServerName = TextBox_ServerName.Text;
                    ServerList.AddOrUpdateServer(mServer);
                    /* Server name changed refresh menu */
                    _MainView.RefreshMenu();
                }
            }
            else
            {
                throw new NotImplementedException();
            }


        }

        private void ServerSettingsCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
