using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;




namespace DiscordWebhook
{
    [Serializable]
    public class WebhookDataInterface
    {
        [NonSerialized()] private static readonly string SaveFolderName = "Discord";
        [NonSerialized()] private static readonly string FileName = "Webhoock";
        [NonSerialized()] private static readonly string SaveDataFormat = ".dat";

        private static WebhookDataInterface mWebhookDataInterface = null;
        private static readonly object padlock = new object();
        public string WebhoockLink { get; set; }

        private WebhookDataInterface()
        {
        }

        public static WebhookDataInterface MWebhookDataInterface
        {
            get
            {
                lock (padlock)
                {
                    if (mWebhookDataInterface == null)
                    {
                        mWebhookDataInterface = new WebhookDataInterface();
                        mWebhookDataInterface = mWebhookDataInterface.LoadFromFile();

                        if (mWebhookDataInterface == null)
                        {
                            mWebhookDataInterface = new WebhookDataInterface();
                            mWebhookDataInterface.WebhoockLink = "link";
                        }
                    }
                    return mWebhookDataInterface;
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
            formatter.Serialize(writerFileStream, mWebhookDataInterface);
            writerFileStream.Close();

        }

        private WebhookDataInterface LoadFromFile()
        {
            WebhookDataInterface obj = null;

            if (true == File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat))))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream readerFileStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SaveFolderName, (FileName + SaveDataFormat)), FileMode.Open, FileAccess.Read);
                obj = (WebhookDataInterface)formatter.Deserialize(readerFileStream);

            }
            return obj;
        }
    }
}
