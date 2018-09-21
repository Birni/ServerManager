using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using ServerManagerGUI.Views;
using Server;

namespace ServerManagerGUI.ViewModels
{
    public class MainViewModel : PropertyChangedViewModel
    {
        private HamburgerMenuItemCollection _menuItems;
        private HamburgerMenuItemCollection _menuOptionItems;

        public IServerList mIServerList = new IServerList();

        public MainViewModel()
        {
            this.CreateMenuItems();
        }

        public void CreateMenuItems()
        {
            MenuItems = new HamburgerMenuItemCollection
            {
                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.SteamBrands },
                    Label = "Steam",
                    ToolTip = "Steam stuff.",
                    Tag = new SteamPage()
                },
                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconFontAwesome() {Kind =  PackIconFontAwesomeKind.DiscordBrands},
                    Label = "Discord",
                    ToolTip = "Discord stuff.",
                    Tag = new DiscordPage()
                }
            };

             foreach (IServer server in mIServerList)
            {

                MenuItems.Add(new HamburgerMenuIconItem()
                {
                    Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ServerSolid },
                    Label = server.GetServerName(),
                    ToolTip = server.GetServerName() + " settings.",
                    Tag = new ServerPage(server)
                }
                );
            }



            MenuOptionItems = new HamburgerMenuItemCollection
            {
                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconFontAwesome() {Kind =  PackIconFontAwesomeKind.CogsSolid},
                    Label = "Settings",
                    ToolTip = "Settings help.",
                    Tag = new AboutPage()
                },
                new HamburgerMenuIconItem()
                {
                    Icon = new PackIconFontAwesome() {Kind =  PackIconFontAwesomeKind.InfoCircleSolid},
                    Label = "About",
                    ToolTip = "Some help.",
                    Tag = new AboutPage()
                }
            };
        }

        public HamburgerMenuItemCollection MenuItems
        {
            get { return _menuItems; }
            set
            {
                if (Equals(value, _menuItems)) return;
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        public HamburgerMenuItemCollection MenuOptionItems
        {
            get { return _menuOptionItems; }
            set
            {
                if (Equals(value, _menuOptionItems)) return;
                _menuOptionItems = value;
                OnPropertyChanged();
            }
        }
    }
}