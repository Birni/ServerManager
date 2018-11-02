using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rcon;
using ArkServer.Logging;
using DiscordWebhook;

namespace ArkServer.ServerUtilities
{
    public class Utilities
    {
        Server mServer;

        public Utilities(Server server)
        {
            mServer = server;
        }

        public async Task<bool> BroadcastMessageAsync(string message)
        {
            INetworkSocket socket = new RconSocket();
            RconMessenger messenger = new RconMessenger(socket);
            string response = null;

            await messenger.ConnectAsync(mServer.ServerIp, mServer.RconPort);

            bool authenticated = await messenger.AuthenticateAsync(mServer.RconPassword);
            if (authenticated)
            {
                response = await messenger.ExecuteCommandAsync("broadcast " + message);
            }

            messenger.CloseConnection();

            if (response.Contains("Server received, But no response!!"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> SaveServerAsync()
        {
            INetworkSocket socket = new RconSocket();
            RconMessenger messenger = new RconMessenger(socket);
            string response = null;

            await messenger.ConnectAsync(mServer.ServerIp, mServer.RconPort);

            bool authenticated = await messenger.AuthenticateAsync(mServer.RconPassword);
            if (authenticated)
            {
                response = await messenger.ExecuteCommandAsync("saveworld ");
            }

            messenger.CloseConnection();

            if (response.Contains("World Saved"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        public async Task ServerStop(int TimeInMin, string reason, bool restartagain)
        {
            if (mServer != null)
            {
                Webhook webhook = new Webhook(WebhookDataInterface.MWebhookDataInterface.WebhoockLink);
                bool firstannouncementgiven = false;
                string broadcastmessage;
                string discordnotification;

                mServer.serverState = ServerState.RestartInProgress;

                if (restartagain)
                {
                    broadcastmessage = "Server restart. Reason: "+reason+". server will shutdown in {0} minutes";
                    discordnotification = "```" + mServer.ServerName + ": " + "Server restart. Reason: " + reason+". server will shutdown in {0} minutes" + "```";
                }
                else
                {
                    broadcastmessage = "Server stop. Reason: " + reason +". server will shutdown in {0} minutes";
                    discordnotification = "```" + mServer.ServerName + ": " + "Server restart. Reason: " + reason+". server will shutdown in {0} minutes" + "```";
                }



                if (TimeInMin >= 60)
                {
                    await webhook.Send(string.Format(discordnotification, 60));
                    firstannouncementgiven = true;
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 60));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 60));
                    await Task.Delay((int)TimeSpan.FromMinutes(15).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 45)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(string.Format(discordnotification, 45));
                        firstannouncementgiven = true;
                    }
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 45));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 45));
                    await Task.Delay((int)TimeSpan.FromMinutes(15).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 30)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(string.Format(discordnotification, 30));
                        firstannouncementgiven = true;
                    }
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 30));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 30));
                    await Task.Delay((int)TimeSpan.FromMinutes(15).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 15)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(string.Format(discordnotification, 15));
                        firstannouncementgiven = true;
                    }
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 15));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 15));
                    await Task.Delay((int)TimeSpan.FromMinutes(5).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 10)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(string.Format(discordnotification, 10));
                        firstannouncementgiven = true;
                    }
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 10));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 10));
                    await Task.Delay((int)TimeSpan.FromMinutes(5).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 5)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(string.Format(discordnotification, 5));
                        firstannouncementgiven = true;
                    }
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 5));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 5));
                    await Task.Delay((int)TimeSpan.FromMinutes(4).TotalMilliseconds).ConfigureAwait(false);
                }

                if (TimeInMin >= 1)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(string.Format(discordnotification, 1));
                        firstannouncementgiven = true;
                    }
                    await BroadcastMessageAsync(string.Format(broadcastmessage, 1));
                    mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 1));
                    await Task.Delay((int)TimeSpan.FromMinutes(1).TotalMilliseconds).ConfigureAwait(false);
                }

                /* try to save world for max 3 times*/
                for (int i = 0; i < 3; i++)
                {
                    if (await SaveServerAsync())
                    {
                        mServer.logs.AddLog(LogType.Information, "World saved");
                        break;
                    }
                    else
                    {
                        mServer.logs.AddLog(LogType.Information, "World save failed");
                    }
                }

                mServer.serverState = ServerState.Stopped;

                await Task.Delay((int)TimeSpan.FromSeconds(3).TotalMilliseconds).ConfigureAwait(false);

                mServer.ArkProcess.Kill();

                if (restartagain)
                {
                    await Task.Delay((int)TimeSpan.FromSeconds(3).TotalMilliseconds).ConfigureAwait(false);
                    mServer.StartServerHandler();
                }
            }
        }
    }
}
