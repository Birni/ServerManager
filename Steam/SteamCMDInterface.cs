using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;



namespace Steam
{
    [Serializable]
    public class SteamCMDInterface
    {
        private static readonly string DATA_FILENAME = "SteamData.dat";
        private static SteamCMDInterface mSteamCMDInterface = null;
        private static readonly object padlock = new object();
        public  string SteamCmdPath { get; set; }
        public  string LoginName { get; set; }
        public  int SteamAppId { get; set; }

        private SteamCMDInterface()
        {
        }

        public static SteamCMDInterface MSteamCMDInterface
        {
            get
            {
                lock (padlock)
                {
                    if (mSteamCMDInterface == null)
                    {
                        mSteamCMDInterface = new SteamCMDInterface();
                        mSteamCMDInterface = mSteamCMDInterface.LoadFromFile();

                        if (mSteamCMDInterface == null)
                        {
                            mSteamCMDInterface = new SteamCMDInterface();
                            mSteamCMDInterface.LoginName = "anonymous";
                            mSteamCMDInterface.SteamAppId = 346110;
                        }
                    }
                    return mSteamCMDInterface;
                }

            }
        }

        public void SaveToFile()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream writerFileStream =
                new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, mSteamCMDInterface);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                /*TODO: no action planned ?*/
                //  throw new ArgumentException("Unable to save our information");
            }
        }

        private SteamCMDInterface LoadFromFile()
        {
            SteamCMDInterface obj = null;

            if (true == File.Exists(DATA_FILENAME))
            {

                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream readerFileStream = new FileStream(DATA_FILENAME, FileMode.Open, FileAccess.Read);
                    obj = (SteamCMDInterface)formatter.Deserialize(readerFileStream);
                }
                catch (Exception)
                {
                    /*TODO: no action planned ?*/
                    //  throw new ArgumentException("Can not load data from file");
                }

            }
            return obj;
        }
    }
}
