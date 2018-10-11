using System.Windows.Controls;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ArkServer;
using ServerManagerGUI.ViewModels;
using System.Threading.Tasks;

namespace ServerManagerGUI.Views
{
    /// <summary>
    /// Interaktionslogik für AboutPage.xaml
    /// </summary>
    partial class NewServerPage : UserControl
    {

        ShellViewModel mShellViewModel = null;

        internal NewServerPage(ShellViewModel ShellViewModel)
        {
            InitializeComponent();

            mShellViewModel = ShellViewModel;


            this.Loaded += (sender, args) => DialogServerName();
            

        }

        private async void DialogServerName()
        {

            var metroWindow = (Application.Current.MainWindow as MetroWindow);

            var mySettings = new MetroDialogSettings()
            {
                AnimateShow = false,
                AffirmativeButtonText = "Ok",
                NegativeButtonText = "Cancel",
                ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
            };

            var input = await metroWindow.ShowInputAsync("Server Name", "Name the new server?", mySettings);

            if (input == null) //user pressed cancel
            {
                Navigation.Navigation.GoBack();

            }
            else
            {
                if (!ServerCollection.MServerCollection.IsAlreadyInCollection(input))
                {
                    Server server = new Server(input);
                    ServerCollection.MServerCollection.AddServer(server);

                    mShellViewModel.RefreshMenu();

                    Navigation.Navigation.Navigate(mShellViewModel.GetItemByText(server.ServerName));

                }
                else
                {
                    var mSettings = new MetroDialogSettings()
                    {
                        AnimateShow = false,
                        AffirmativeButtonText = "Ok",
                        ColorScheme = metroWindow.MetroDialogOptions.ColorScheme
                    };
                    await metroWindow.ShowMessageAsync("Server Name", "There is already are server named " + input + "!" + " The server name must be unique!", MessageDialogStyle.Affirmative, mSettings);

                    Navigation.Navigation.GoBack();
                }
            }

        }
    }
}
