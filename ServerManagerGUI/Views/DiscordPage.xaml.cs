using System.Windows.Controls;
using DiscordWebhook;


namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für SteamPage.xaml
    /// </summary>
    public partial class DiscordPage: UserControl
    {
        /* this needs to lock the ChangedEventHandler when the components get initialize */
        private bool isInitialized = false;

        public DiscordPage()
        {
            InitializeComponent();

            TextBox_Webhook.Text = WebhookDataInterface.MWebhookDataInterface.WebhoockLink;
            isInitialized = true;
        }

        private void TextBox_WebhookChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (true == isInitialized)
            {
                WebhookDataInterface.MWebhookDataInterface.WebhoockLink = TextBox_Webhook.Text;
                WebhookDataInterface.MWebhookDataInterface.SaveToFile();
            }
        }
    }
}
