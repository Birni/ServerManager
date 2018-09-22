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



namespace Server
{
  public sealed class IServerList 
    {
        private static IServerList mIServerList = null;
        private static readonly object padlock = new object();
        static ConcurrentDictionary<string, IServer> mServerList = new ConcurrentDictionary<string, IServer>();
        private const string DATA_FILENAME = "ServerManagerData.dat";

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
            SaveToFile();
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
            SaveToFile();
        }

        public void SaveToFile()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream writerFileStream =
                new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, mServerList);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                /*TODO: no action planned ?*/
                //  throw new ArgumentException("Unable to save our information");
            }
        }

        public void LoadFromFile()
        {
            if (true == File.Exists(DATA_FILENAME))
            {

                try
                {
                   BinaryFormatter formatter = new BinaryFormatter(); 
                   FileStream readerFileStream = new FileStream(DATA_FILENAME,FileMode.Open, FileAccess.Read);
                   mServerList = (ConcurrentDictionary<String, IServer>) formatter.Deserialize(readerFileStream);
                   readerFileStream.Close();

            }
            catch (Exception)
            {
                /*TODO: no action planned ?*/
                //  throw new ArgumentException("Can not load data from file");
            }

        } 

        } 
    }

}
