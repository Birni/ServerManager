using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System.Collections.Generic;
using ServerManagerGUI;
using ServerManagerGUI.Views;
using Server;
using Rcon;
using SteamWeb;

namespace ServerManagerGUI.ViewModels
{
    public class MainViewModel : PropertyChangedViewModel
    {
        private HamburgerMenuItemCollection _menuItems;
        private HamburgerMenuItemCollection _menuOptionItems;



        public MainViewModel()
        {

            RconTest();
            TestSteamRequest();

            ///* TODO: remove testing stuff */
            //IServer server1 = new IServer("Ragnarok");
            //ServerList.AddOrUpdateServer(server1);

            //IServer server2 = new IServer("test");
            ////ServerList.AddOrUpdateServer(server2);

            //IServerList.MIServerList.AddOrUpdateServer(server2);

            CreateMenuItems();
        }

        public async void TestSteamRequest()
        {

       //     var parameters = new List<SteamWebRequestParameter>();

       //     parameters.Add(new SteamWebRequestParameter("itemcount", "1"));
       //     parameters.Add(new SteamWebRequestParameter("publishedfileids[0]", "1373937944"));

       //     // List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();
       //     // parameters.AddIfHasValue("publishedfileids[0]", "1373937944");


       //     // this will map to the ISteamUser endpoint
            var steamInterface = new SteamWebInterface("https://api.steampowered.com/" , "<secret>");
            steamInterface.SteamWebGetPublishedFileDetails("1373937944");

       //     await steamInterface.PostAsync<dynamic>("GetPublishedFileDetails",1, parameters);



            ////     await steamInterface.PostAsync<dynamic>("GetPublishedFileDetails" , 1 , param);





        }


        public async void RconTest()
        {

            INetworkSocket socket = new RconSocket();


            RconMessenger messenger = new RconMessenger(socket);

            bool isConnected = await messenger.ConnectAsync("185.38.149.15", 32330);

            bool authenticated = await messenger.AuthenticateAsync("password");
            if (authenticated)
            {

                var response = await messenger.ExecuteCommandAsync("listplayers");
            }



        }



        public void RefreshMenu()
        {
            CreateMenuItems();
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

            foreach (KeyValuePair<string, IServer> server in IServerList.MIServerList.GetserverList())
            {

                MenuItems.Add(new HamburgerMenuIconItem()
                {
                    Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ServerSolid },
                    Label = server.Key,
                    ToolTip = server.Key + " settings.",
                    Tag = new ServerPage(this , server.Value)
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