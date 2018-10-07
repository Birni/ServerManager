using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArkServer.Logging
{
    class ServerLog 
    {
        private readonly string datetimeFormat;
        string logFilename;
        StreamWriter file = null;
        List<LogData> logs = null;  

        public ServerLog(string Filename)
        {
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

            this.logFilename = System.DateTime.Now.ToString(datetimeFormat) + Filename + ".log" ;
            this.file = new System.IO.StreamWriter(@logFilename);
        }

        string GenerateLogString(LogData data)
        {
            return data.LogTime + "\t" + data.LogType.ToString() + "\t" + data.LogMessage; 
        }


        public void LogServerInformation(string text)
        {
            LogData infoLog = null;

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Information;

            this.logs.Add(infoLog);
            file.WriteLine(GenerateLogString(infoLog));
        }

        public void LogServerCritical(string text)
        {
            LogData infoLog = null;

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Critical;

            this.logs.Add(infoLog);
            file.WriteLine(GenerateLogString(infoLog));
        }

        public void LogServerDeveloper(string text)
        {
            LogData infoLog = null;

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Developer;

            this.logs.Add(infoLog);
            file.WriteLine(GenerateLogString(infoLog));
        }

        public void LogServerError(string text)
        {
            LogData infoLog = null;

            infoLog.LogMessage = text;
            infoLog.LogTime = DateTime.Now;
            infoLog.LogType = LogType.Error;

            this.logs.Add(infoLog);
            file.WriteLine(GenerateLogString(infoLog));
        }
    }
}
