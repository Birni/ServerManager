using System;
using System.Collections.Generic;
using System.Text;

namespace ArkServer
{
    public enum ServerState
    {
        Running = 0,
        Initialize = 1,
        Updating = 2,
        Booting = 3,
        Started = 4,
        RestartInProgress = 5,
        Stopped = 6,
        Crashed = 7  
    }

}
