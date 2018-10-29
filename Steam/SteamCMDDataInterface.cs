using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;




namespace Steam
{
    [Serializable]
    public class SteamCMDDataInterface
    {
        [NonSerialized()] private static readonly string SaveFolderName = "Steam";
        [NonSerialized()] private static readonly string FileName = "SteamCMD";
        [NonSerialized()] private static readonly string SaveDataFormat = ".dat";

        private static SteamCMDDataInterface mSteamCMDDataInterface = null;
        private static readonly object padlock = new object();
        public  string SteamCmdPath { get; set; }
        public  string LoginName { get; set; }
        public  int SteamAppId { get; set; }

        private SteamCMDDataInterface()
        {
        }

        public static SteamCMDDataInterface MSteamCMDDataInterface
        {
            get
            {
                lock (padlock)
                {
                    if (mSteamCMDDataInterface == null)
                    {
                        mSteamCMDDataInterface = new SteamCMDDataInterface();
                        mSteamCMDDataInterface = mSteamCMDDataInterface.LoadFromFile();

                        if (mSteamCMDDataInterface == null)
                        {
                            mSteamCMDDataInterface = new SteamCMDDataInterface();
                            mSteamCMDDataInterface.LoginName = "anonymous";
                            mSteamCMDDataInterface.SteamAppId = 346110;
                        }
                    }
                    return mSteamCMDDataInterface;
                }

            }
        }

        public void SaveToFile()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName)))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName));
            }

             BinaryFormatter formatter = new BinaryFormatter();
             FileStream writerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName , (FileName + SaveDataFormat)), FileMode.Create, FileAccess.Write);
             formatter.Serialize(writerFileStream, mSteamCMDDataInterface);
             writerFileStream.Close();

        }

        private SteamCMDDataInterface LoadFromFile()
        {
            SteamCMDDataInterface obj = null;

            if (true == File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat))))
            {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream readerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat)), FileMode.Open, FileAccess.Read);
            obj = (SteamCMDDataInterface)formatter.Deserialize(readerFileStream);

            }
            return obj;
        }
    }
}
