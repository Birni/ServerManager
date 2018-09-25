using System;
using System.Windows.Controls;
using Steam;


namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für SteamPage.xaml
    /// </summary>
    public partial class SteamPage : UserControl
    {
        /* this needs to lock the ChangedEventHandler when the components get initialize */
        private bool isInitialized = false;

        public SteamPage()
        {
            InitializeComponent();

            TextBox_SteamPath.Text = SteamCMDInterface.MSteamCMDInterface.SteamCmdPath;
            TextBox_SteamLogin.Text = SteamCMDInterface.MSteamCMDInterface.LoginName;
            TextBox_SteamAppID.Text = SteamCMDInterface.MSteamCMDInterface.SteamAppId.ToString();

            isInitialized = true;
        }


        private void TextBox_SteamPathChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamCMDInterface.MSteamCMDInterface.SteamCmdPath = TextBox_SteamPath.Text;
                SteamCMDInterface.MSteamCMDInterface.SaveToFile();
            }
        }

        private void TextBox_SteamLoginChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamCMDInterface.MSteamCMDInterface.LoginName = TextBox_SteamLogin.Text;
                SteamCMDInterface.MSteamCMDInterface.SaveToFile();
            }
        }

        private void TextBox_SteamAppIDChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamCMDInterface.MSteamCMDInterface.SteamAppId = Int32.Parse(TextBox_SteamAppID.Text);
                SteamCMDInterface.MSteamCMDInterface.SaveToFile();
            }
        }

    }
}
