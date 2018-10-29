using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;




namespace SteamWeb
{
    [Serializable]
    public class SteamWebDataInterface
    {
        [NonSerialized()] private static readonly string SaveFolderName = "Steam";
        [NonSerialized()] private static readonly string FileName = "SteamWeb";
        [NonSerialized()] private static readonly string SaveDataFormat = ".dat";

        private static SteamWebDataInterface mSteamWebDataInterface = null;
        private static readonly object padlock = new object();
        public string ApiKey { get; set; }


        private SteamWebDataInterface()
        {
        }

        public static SteamWebDataInterface MSteamWebDataInterface
        {
            get
            {
                lock (padlock)
                {
                    if (mSteamWebDataInterface == null)
                    {
                        mSteamWebDataInterface = new SteamWebDataInterface();
                        mSteamWebDataInterface = mSteamWebDataInterface.LoadFromFile();

                        if (mSteamWebDataInterface == null)
                        {
                            mSteamWebDataInterface = new SteamWebDataInterface();
                            mSteamWebDataInterface.ApiKey = "Api key";
                        }
                    }
                    return mSteamWebDataInterface;
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
            FileStream writerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat)), FileMode.Create, FileAccess.Write);
            formatter.Serialize(writerFileStream, mSteamWebDataInterface);
            writerFileStream.Close();

        }

        private SteamWebDataInterface LoadFromFile()
        {
            SteamWebDataInterface obj = null;

            if (true == File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat))))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream readerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat)), FileMode.Open, FileAccess.Read);
                obj = (SteamWebDataInterface)formatter.Deserialize(readerFileStream);

            }
            return obj;
        }
    }
}
