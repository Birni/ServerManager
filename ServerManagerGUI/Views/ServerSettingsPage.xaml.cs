using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Server;

namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für ServerSettingsPage.xaml
    /// </summary>
    public partial class ServerSettingsPage : UserControl
    {
        IServer mServer;

        public ServerSettingsPage(ref IServer server)
        {
            mServer = server;

            InitializeComponent();

            TextBox_ServerName.Text = mServer.GetServerName();
            TextBox_ServerIp.Text = mServer.GetServerIp();
            TextBox_ServerPath.Text = mServer.GetServerArkSurvivalFolder();
            TextBox_ServerPort.Text = mServer.GetServerPort().ToString();
            TextBox_ServerQuerryPort.Text = mServer.GetServerPort().ToString();
            TextBox_ServerRconPort.Text = mServer.GetServerRconPort().ToString();
            TextBox_ServerStartArgument.Text = mServer.GetServerStartArgument().ToString();
        }



        private void ServerSettingsSave_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_ServerName.Text != null)
            {
   
            }


        }

        private void ServerSettingsCancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
