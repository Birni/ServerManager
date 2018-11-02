using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArkServer.Logging;
using DiscordWebhook;

namespace ArkServer.ServerMods
{
    public class ModCollection
    {
        List<Mod> mModCollection = new List<Mod>();

        ServerLog mlog = null;

        public ModCollection(ServerLog log)
        {
            if (null != log)
            {
                mlog = log;
            }

        }

        public async Task DetermineMods(string Serverpath)
        {
            StreamReader file;
            string line;
            List<int> ActiveMods = new List<int>();
            List<int> ModUpdater = new List<int>();

            if (true == File.Exists(Path.Combine(Serverpath, "ShooterGame", "Saved", "Config", "WindowsServer", "GameUserSettings.ini")))
            {
                file = new StreamReader(Path.Combine(Serverpath, "ShooterGame", "Saved", "Config", "WindowsServer", "GameUserSettings.ini"));

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("ActiveMods"))
                    {
                        String pattern = @"(\d+)";
                        foreach (Match m in Regex.Matches(line, pattern))
                        {
                            ActiveMods.Add(Int32.Parse(m.Groups[0].Value));
                        }
                        break;
                    }
                }
                file.Close();
            }
            if (true == File.Exists(Path.Combine(Serverpath, "ShooterGame", "Saved", "Config", "WindowsServer", "Game.ini")))
            {
                file = new StreamReader(Path.Combine(Serverpath, "ShooterGame", "Saved", "Config", "WindowsServer", "Game.ini"));

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("ModIDS"))
                    {
                        String pattern = @"(\d+)";
                        foreach (Match m in Regex.Matches(line, pattern))
                        {
                            ModUpdater.Add(Int32.Parse(m.Groups[0].Value));
                        }
                    }
                }
                file.Close();
            }

            foreach (int modId in ActiveMods)
            {
                if (ModUpdater.Contains(modId))
                {
                    Mod mod =  await Mod.CreateAsync(modId);
                    mModCollection.Add(mod);
                    ModUpdater.Remove(modId);
                    mlog.AddLog(LogType.Information, ("ModId: " + modId + " ModName: " + mod.ModName + " added to server mod collection "));
                }
                else
                {
                    Mod mod = await Mod.CreateAsync(modId);
                    mlog.AddLog(LogType.Critical, ("ModId: "+ modId+" ModName: "+ mod.ModName + " is not added to the modInstaller section in the game.ini" ));
                }
            }

            if (ModUpdater.Count > 0)
            {
                foreach (int Id in ModUpdater)
                {
                    Mod mod = await Mod.CreateAsync(Id);
                    mlog.AddLog(LogType.Critical, ("ModId: " + Id + " ModName: " + mod.ModName + " is not listed as active mod in gameusersettings.ini"));
                }
            }
        }

        public async Task<List<string>> CheckModsForUpdates()
        {
            bool needupdate = false;
            Webhook webhook = new Webhook(WebhookDataInterface.MWebhookDataInterface.WebhoockLink);

            List<string> list = new List<string>();

            foreach (Mod mod in mModCollection)
            {
                if (await mod.IsUpaded())
                {
                    mlog.AddLog(LogType.Information, ("ModId: " + mod.ModId + " ModName: " + mod.ModName + "need a update"));
                    list.Add(mod.ModName);
                    needupdate = true;
                }
            }

            if (!needupdate)
            {
                mlog.AddLog(LogType.Information, ("All mods are up to date "));
            }

            return list;
        }

    }
}
