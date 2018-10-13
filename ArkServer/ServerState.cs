using System;
using System.Collections.Generic;
using System.Text;

namespace ArkServer
{
    enum ServerState
    {
        Running = 0, 
        Started = 1,
        Stopped = 2,
        UnexpectedStop = 3  
    }

}
