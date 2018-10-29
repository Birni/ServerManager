using System;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;



namespace ArkServer.Logging
{
    public class ServerLog 
    {
        [NonSerialized()] private readonly object _lock = new object();

        private static readonly string FolderName = "Logs";
        private static readonly string DataFormat = ".log";

        private readonly string datetimeFormat;
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        string logFilename;
        string fullfilename;
        StreamWriter file = null;


        public ObservableCollection<LogData> logs { get; } = new ObservableCollection<LogData>();

        public ServerLog(string Filename)
        {
            datetimeFormat = "yyyy-MM-dd-HH-mm-ss-fff";

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + FolderName))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + FolderName);
            }

            this.logFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderName);
            this.fullfilename = Filename + System.DateTime.Now.ToString(datetimeFormat) + DataFormat;

        }
        public ObservableCollection<LogData> GetLogs()
        {
            return logs;
        }


        string GenerateLogString(LogData data)
        {
            return data.LogTime + "\t" + data.LogType.ToString() + "\t" + data.LogMessage; 
        }


        public void AddLog(LogType type, string text)
        {
            lock (_lock)
            {
                LogData infoLog = new LogData
                {
                    LogMessage = text,
                    LogTime = DateTime.Now,
                    LogType = type
                };

                Action action = () => { logs.Add(infoLog); };
                dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(action));

                file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
                file.WriteLine(GenerateLogString(infoLog));
                file.Close();
            }
        }
    }
}
