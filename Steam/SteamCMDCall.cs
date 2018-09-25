using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace Steam
{
    public class SteamCMDCall
    {
        public SteamCMDCall()
        {
        }


        public async Task<bool> UpdateServer(string installdir)
        {
            if (Directory.Exists(installdir))
            {
                return await Task.Run(() =>
                {
                    Process pProcess = new Process();
                    pProcess.StartInfo.FileName = SteamCMDInterface.MSteamCMDInterface.SteamCmdPath;
                    pProcess.StartInfo.Arguments = "+login " + SteamCMDInterface.MSteamCMDInterface.LoginName + " +force_install_dir " + installdir + " +app_update " + SteamCMDInterface.MSteamCMDInterface.SteamAppId + " +exit ";
                    pProcess.Start();
                    pProcess.WaitForExit();
                    pProcess.Close();
                    return true;
                });
            }
            return false;
        }

    }
}
