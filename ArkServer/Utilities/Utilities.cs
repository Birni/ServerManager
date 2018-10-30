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
    class Utilities
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



        public async Task ServerRestart(int TimeInMin, string reason)
        {
            if (mServer != null)
            {
                Webhook webhook = new Webhook(WebhookDataInterface.MWebhookDataInterface.WebhoockLink);
                bool firstannouncementgiven = false;


                if (TimeInMin >= 60)
                {
                    await webhook.Send(mServer.ServerName+ " server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                    firstannouncementgiven = true;
                    await BroadcastMessageAsync("server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                    mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                    await Task.Delay((int)TimeSpan.FromMinutes(15).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 45)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                        firstannouncementgiven = true;
                    }

                    await BroadcastMessageAsync("server restart. Reason:" + reason +". server will shutdown in 45 minutes");
                    mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 45 minutes");
                    await Task.Delay((int)TimeSpan.FromMinutes(15).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 30)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                        firstannouncementgiven = true;
                    }

                    await BroadcastMessageAsync(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 30 minutes");
                    mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 45 minutes");
                    await Task.Delay((int)TimeSpan.FromMinutes(15).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 15)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                        firstannouncementgiven = true;
                    }

                    await BroadcastMessageAsync("server restart. Reason:" + reason + ". server will shutdown in 15 minutes");
                    mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 15 minutes");
                    await Task.Delay((int)TimeSpan.FromMinutes(5).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 10)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                        firstannouncementgiven = true;
                    }

                    await BroadcastMessageAsync(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 10 minutes");
                    mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 10 minutes");
                    await Task.Delay((int)TimeSpan.FromMinutes(5).TotalMilliseconds).ConfigureAwait(false);
                }
                if (TimeInMin >= 5)
                {
                    if (!firstannouncementgiven)
                    {
                        await webhook.Send(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 60 minutes");
                        firstannouncementgiven = true;
                    }

                    await BroadcastMessageAsync(mServer.ServerName + " server restart. Reason:" + reason + ". server will shutdown in 5 minutes");
                    mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 5 minutes");
                    await Task.Delay((int)TimeSpan.FromMinutes(4).TotalMilliseconds).ConfigureAwait(false);
                }

                await BroadcastMessageAsync(mServer.ServerName + " server restart for. Reason:" + reason + ". server will shutdown in 1 minute");
                mServer.logs.AddLog(LogType.Information, "server restart. Reason:" + reason + ". server will shutdown in 1 minutes");
                await Task.Delay((int)TimeSpan.FromMinutes(1).TotalMilliseconds).ConfigureAwait(false);

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

                await Task.Delay((int)TimeSpan.FromSeconds(3).TotalMilliseconds).ConfigureAwait(false);

                mServer.StartServerHandler();
            }
        }
    }
}
