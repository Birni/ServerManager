using System;
using System.Collections.Concurrent;
using System.IO;


namespace ArkServer
{
  public sealed class ServerCollection
    {
        private static ServerCollection mServerCollection = null;
        private static readonly object padlock = new object();
        static ConcurrentDictionary<string, Server> Collection = new ConcurrentDictionary<string, Server>();
        private static readonly string SaveFolderName = "Server";
        private static readonly string SaveDataFormat = ".dat";

        ServerCollection()
        {
        }

        public static ServerCollection MServerCollection
        {
            get
            {
                lock (padlock)
                {
                    if (mServerCollection == null)
                    {
                        mServerCollection = new ServerCollection();

                        DirectoryInfo d = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , SaveFolderName));

                        foreach (var file in d.GetFiles("*"+SaveDataFormat))
                        {
                            var server = new Server(file);
                        }
                    }
                    return mServerCollection;
                }

            }
        }

        public ConcurrentDictionary<string, Server> GetCollection()
        {
            return Collection;
        }


        public bool AddServer(Server server)
        {
            return Collection.TryAdd(server.ServerName, server);
        }

        public void RemoveServer(Server server)
        {
            Collection.TryRemove(server.ServerName , out server);
        }

        public bool IsAlreadyInCollection(string servername)
        {
            if (!string.IsNullOrWhiteSpace(servername))
            {
                return Collection.TryGetValue(servername, out Server server);
            }
            else
            {
                return true;
            }
        }

        public void UpdateServer(Server server)
        {
            if (Collection.TryGetValue(server.ServerName, out Server olddata))
            {
                Collection.TryUpdate(server.ServerName, server , olddata);
            }
        }

    }

}
