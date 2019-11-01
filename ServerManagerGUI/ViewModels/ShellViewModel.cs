using System;
using System.Linq;
using MahApps.Metro.IconPacks;
using ServerManagerGUI.Views;
using ArkServer;
using Rcon;
using SteamWeb;
using System.Collections.Generic;

namespace ServerManagerGUI.ViewModels
{
    internal class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            BuildMenu();
            BuildOptionsMenuMenu();


        }

        internal void BuildMenu()
        {
            // Build the menu
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.SteamBrands }, Text = "Steam", NavigationDestination = new SteamPage() });
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.DiscordBrands }, Text = "Discord", NavigationDestination = new DiscordPage() });

            foreach (KeyValuePair<string, Server> server in ServerCollection.MServerCollection.GetCollection())
            {
                this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.ServerSolid }, Text = server.Key, NavigationDestination = new ServerPage(this , server.Value) });
            }
            this.Menu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.PlusSolid }, Text = "Add new Server", NavigationDestination = new NewServerPage(this) });


        }
        internal void BuildOptionsMenuMenu()
        {
            this.OptionsMenu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CogsSolid }, Text = "Settings", NavigationDestination = new SettingsPage() });
            this.OptionsMenu.Add(new MenuItem() { Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid }, Text = "About", NavigationDestination = new AboutPage() });
        }


        public void RefreshMenu()
        {
            this.Menu.Clear();
            BuildMenu();
        }

        public object GetItemByText(string text)
        {
            object obj = null;

            if (!string.IsNullOrWhiteSpace(text))
            {
                foreach (var menuitem in Menu)
                {
                    if (menuitem.Text == text)
                    {
                        obj = menuitem.NavigationDestination;
                        break;
                    }
                }
            }
            return obj;
        }

        public object GetItem(object obj)
        {
            return null == obj ? null : this.Menu.FirstOrDefault(m => m.NavigationDestination.Equals(obj));
        }

        public object GetOptionsItem(object obj)
        {
            return null == obj ? null : this.OptionsMenu.FirstOrDefault(m => m.NavigationDestination.Equals(obj));
        }
    }
}