using System;
using System.Collections.Generic;
using System.Text;

namespace ArkServer.Logging
{
    public class LogData
    {
       public  DateTime LogTime { get; set; }
       public LogType  LogType { get; set; }
       public string LogMessage { get; set; }
    }

}
