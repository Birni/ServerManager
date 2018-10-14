using System;
using System.Collections.Generic;
using System.Text;

namespace ArkServer
{
    public enum ServerState
    {
        Running = 0,
        Updating = 1,
        Started = 2,
        Stopped = 3,
        Crashed = 4  
    }

}
