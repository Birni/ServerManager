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
        private static readonly string FolderName = "Logs";
        private static readonly string DataFormat = ".log";

        private readonly string datetimeFormat;
        private Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        string logFilename;
        string fullfilename;
        StreamWriter file = null;
       // List<LogData> logs = null;
        int currentlogId = 0;

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

           //  logs = new List<LogData>();

        }
        public ObservableCollection<LogData> GetLogs()
        {
            return logs;
        }


        string GenerateLogString(LogData data)
        {
            return data.LogTime + "\t" + data.LogType.ToString() + "\t" + data.LogMessage; 
        }


        public void LogServerInformation(string text)
        {
            LogData infoLog = new LogData
            {
                LodId = ++currentlogId,
                LogMessage = text,
                LogTime = DateTime.Now,
                LogType = LogType.Information
            };

            Action action = () => { logs.Add(infoLog); };
            dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(action));

            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }

        public void LogServerCritical(string text)
        {
            LogData infoLog = new LogData
            {
                LodId = ++currentlogId,
                LogMessage = text,
                LogTime = DateTime.Now,
                LogType = LogType.Critical
            };

            Action action = () => { logs.Add(infoLog); };
            dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(action));
            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }

        public void LogServerDeveloper(string text)
        {
            LogData infoLog = new LogData
            {
                LodId = ++currentlogId,
                LogMessage = text,
                LogTime = DateTime.Now,
                LogType = LogType.Developer
            };

            Action action = () => { logs.Add(infoLog); };
            dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(action));
            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }

        public void LogServerError(string text)
        {
            LogData infoLog = new LogData
            {
                LodId = ++currentlogId,
                LogMessage = text,
                LogTime = DateTime.Now,
                LogType = LogType.Error
            };

            Action action = () => { logs.Add(infoLog); };
            dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(action));
            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }
    }
}
