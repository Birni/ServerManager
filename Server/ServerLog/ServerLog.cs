using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArkServer.Logging
{
    public class ServerLog 
    {
        private static readonly string FolderName = "Logs";
        private static readonly string DataFormat = ".log";

        private readonly string datetimeFormat;
        string logFilename;
        string fullfilename;
        StreamWriter file = null;
        List<LogData> logs = null;  

        public ServerLog(string Filename)
        {
            datetimeFormat = "yyyy-MM-dd-HH-mm-ss-fff";

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + FolderName))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + FolderName);
            }

            this.logFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderName);
            this.fullfilename = Filename + System.DateTime.Now.ToString(datetimeFormat) + DataFormat;

             logs = new List<LogData>();

        }

        string GenerateLogString(LogData data)
        {
            return data.LogTime + "\t" + data.LogType.ToString() + "\t" + data.LogMessage; 
        }


        public void LogServerInformation(string text)
        {
            LogData infoLog = new LogData();

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Information;

            this.logs.Add(infoLog);

            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }

        public void LogServerCritical(string text)
        {
            LogData infoLog = new LogData();

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Critical;

            this.logs.Add(infoLog);
            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }

        public void LogServerDeveloper(string text)
        {
            LogData infoLog = new LogData();

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Developer;

            this.logs.Add(infoLog);
            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }

        public void LogServerError(string text)
        {
            LogData infoLog = new LogData();

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Error;

            this.logs.Add(infoLog);
            file = new StreamWriter(Path.Combine(logFilename, fullfilename), true);
            file.WriteLine(GenerateLogString(infoLog));
            file.Close();
        }
    }
}
