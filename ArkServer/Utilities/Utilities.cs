using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rcon;
using ArkServer.Logging;
using DiscordWebhook;
using System.Threading;

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

        public async Task ServerStopOrRestart(int TimeInMin, string reason, bool restartagain, CancellationToken cancellationToken)
        {
            if (mServer != null)
            {
                Webhook webhook = new Webhook(WebhookDataInterface.MWebhookDataInterface.WebhoockLink);
                bool firstannouncementgiven = false;
                string broadcastmessage;
                string cancelbroadcastmessage;
                string discordnotification;
                string canceldiscordnotification;
                TimeSpan WaitTime = TimeSpan.Zero;


                mServer.serverState = ServerState.RestartInProgress;

                if (restartagain)
                {
                    broadcastmessage = "Server restart. Reason: " + reason + ". server will shutdown in {0} minutes";
                    discordnotification = "```" + mServer.ServerName + ": " + "Server restart. Reason: " + reason + ". server will shutdown in {0} minutes" + "```";
                    cancelbroadcastmessage = "Server restart canceled";
                    canceldiscordnotification = "```" + mServer.ServerName + ": " + "Server restart canceled" + "```";
                }
                else
                {
                    broadcastmessage = "Server stop. Reason: " + reason + ". server will shutdown in {0} minutes";
                    discordnotification = "```" + mServer.ServerName + ": " + "Server stop. Reason: " + reason + ". server will shutdown in {0} minutes" + "```";
                    cancelbroadcastmessage = "Server stop canceled";
                    canceldiscordnotification = "```" + mServer.ServerName + ": " + "Server stop canceled" + "```";
                }

                try

                {

                    if (TimeInMin >= 60)
                    {
                        await webhook.Send(string.Format(discordnotification, 60));
                        firstannouncementgiven = true;
                        await BroadcastMessageAsync(string.Format(broadcastmessage, 60));
                        mServer.logs.AddLog(LogType.Information, string.Format(broadcastmessage, 60));

                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(15))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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

                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(15))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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

                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(15))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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


                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(5))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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

                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(5))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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

                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(4))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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

                        WaitTime = TimeSpan.Zero;
                        while (WaitTime < TimeSpan.FromMinutes(1))
                        {
                            await Task.Delay((int)TimeSpan.FromMilliseconds(200).TotalMilliseconds).ConfigureAwait(false);
                            WaitTime += TimeSpan.FromMilliseconds(200);
                            if (cancellationToken.IsCancellationRequested)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                        }
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
                catch (OperationCanceledException)
                {
                    await webhook.Send(canceldiscordnotification);
                    await BroadcastMessageAsync(cancelbroadcastmessage);
                    mServer.logs.AddLog(LogType.Information, cancelbroadcastmessage);

                    mServer.serverState = ServerState.Running;
                }
            }
        }
    }
}
