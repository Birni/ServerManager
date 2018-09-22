﻿using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using System.Collections.Generic;
using ServerManagerGUI;
using ServerManagerGUI.Views;
using Server;

namespace ServerManagerGUI.ViewModels
{
    public class MainViewModel : PropertyChangedViewModel
    {
        private HamburgerMenuItemCollection _menuItems;
        private HamburgerMenuItemCollection _menuOptionItems;

        IServerList ServerList = new IServerList();


        public MainViewModel()
        {
            ServerList.LoadFromFile();

            /* TODO: remove testing stuff */
            IServer server1 = new IServer("Ragnarok");
            ServerList.AddOrUpdateServer(server1);

            IServer server2 = new IServer("awsome server");
            ServerList.AddOrUpdateServer(server2);

            CreateMenuItems();
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

            foreach (KeyValuePair<string, IServer> server in ServerList.GetserverList())
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