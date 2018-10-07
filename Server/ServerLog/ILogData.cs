using System;
using System.Collections.Generic;
using System.Text;

namespace ArkServer.Logging
{
    interface LogData
    {
        DateTime LogTime { get; set; }
        LogType  LogType { get; set; }
        string LogMessage { get; set; }
    }

}
