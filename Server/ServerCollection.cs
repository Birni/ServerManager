using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Reflection;

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

                        DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\" + SaveFolderName);

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
            return Collection.TryGetValue(servername, out Server server);
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
