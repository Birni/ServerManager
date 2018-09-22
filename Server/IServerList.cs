using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Server
{
  public sealed class IServerList 
    {
        private static IServerList mIServerList = null;
        private static readonly object padlock = new object();
        static ConcurrentDictionary<string, IServer> mServerList = new ConcurrentDictionary<string, IServer>();


        public static IServerList MIServerList
        {
            get
            {
                lock (padlock)
                {
                    if (mIServerList == null)
                    {
                        mIServerList = new IServerList();
                    }
                    return mIServerList;
                }

            }
        }

        public ConcurrentDictionary<string, IServer> GetserverList()
        {
            return mServerList;
        }

        public void ChangeKey(string currentkey, string newkey)
        {
            IServer retrievedValue;

            /* mServerList may not contain the new key  */
            if (false == mServerList.TryGetValue(newkey, out retrievedValue))
            {
                /* determine the current server information*/
                if (true == mServerList.TryGetValue(currentkey, out retrievedValue))
                {
                    retrievedValue.ServerName = newkey;
                    AddOrUpdateServer(retrievedValue);

                    mServerList.TryRemove(currentkey, out retrievedValue);
                }
            }
        }


        public void AddOrUpdateServer(IServer server)
        {

            mServerList.AddOrUpdate(server.ServerName, server,
                (key, existingVal) =>
                    {

                        if (server != existingVal)
                        {
                            throw new ArgumentException("Duplicate server names names are not allowed: {0}.", server.ServerName);
                        }
                        existingVal.Map = server.Map;
                        existingVal.maxPlayer = server.maxPlayer;
                        existingVal.Port  = server.Port;
                        existingVal.QueryPort = server.QueryPort;
                        existingVal.RconEnabled = server.RconEnabled;
                        existingVal.RconPort = server.RconPort;
                        existingVal.ServerIp = server.ServerIp;
                        existingVal.ServerStartArgument = server.ServerStartArgument;

                        return existingVal;
                });

        }


        public bool UpdateServer(IServer name)
        {



            return true; 
        }
    }
}
