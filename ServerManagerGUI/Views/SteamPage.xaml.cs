using System;
using System.Windows.Controls;
using SteamWeb;
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

            TextBox_SteamPath.Text = SteamCMDDataInterface.MSteamCMDDataInterface.SteamCmdPath;
            TextBox_SteamLogin.Text = SteamCMDDataInterface.MSteamCMDDataInterface.LoginName;
            TextBox_SteamAppID.Text = SteamCMDDataInterface.MSteamCMDDataInterface.SteamAppId.ToString();

            TextBox_SteamApiKey.Text = SteamWebDataInterface.MSteamWebDataInterface.ApiKey;

            isInitialized = true;
        }


        private void TextBox_SteamPathChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamCMDDataInterface.MSteamCMDDataInterface.SteamCmdPath = TextBox_SteamPath.Text;
                SteamCMDDataInterface.MSteamCMDDataInterface.SaveToFile();
            }
        }

        private void TextBox_SteamLoginChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamCMDDataInterface.MSteamCMDDataInterface.LoginName = TextBox_SteamLogin.Text;
                SteamCMDDataInterface.MSteamCMDDataInterface.SaveToFile();
            }
        }

        private void TextBox_SteamAppIDChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamCMDDataInterface.MSteamCMDDataInterface.SteamAppId = Int32.Parse(TextBox_SteamAppID.Text);
                SteamCMDDataInterface.MSteamCMDDataInterface.SaveToFile();
            }
        }

        private void TextBox_SteamApiKeyChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                SteamWebDataInterface.MSteamWebDataInterface.ApiKey = TextBox_SteamApiKey.Text;
                SteamWebDataInterface.MSteamWebDataInterface.SaveToFile();
            }
        }

    }
}
