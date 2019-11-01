using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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
                    pProcess.StartInfo.FileName = SteamCMDDataInterface.MSteamCMDDataInterface.SteamCmdPath;
                    pProcess.StartInfo.Arguments = "+login " + SteamCMDDataInterface.MSteamCMDDataInterface.LoginName + " +force_install_dir " + installdir + " +app_update " + SteamCMDDataInterface.MSteamCMDDataInterface.SteamAppId + " +exit ";
                    pProcess.Start();
                    pProcess.WaitForExit();
                    pProcess.Close();
                    return true;
                });
            }
            return false;
        }

        public async Task<AppInfo> AppInfo()
        {

                return await Task.Run(() =>
                {
                    Process pProcess = new Process();
                    pProcess.StartInfo.FileName = SteamCMDDataInterface.MSteamCMDDataInterface.SteamCmdPath;
                    pProcess.StartInfo.Arguments = "+login " + SteamCMDDataInterface.MSteamCMDDataInterface.LoginName + " +app_info_print " + SteamCMDDataInterface.MSteamCMDDataInterface.SteamAppId + " +exit ";
                    pProcess.StartInfo.UseShellExecute = false;
                    pProcess.StartInfo.CreateNoWindow = true;
                    pProcess.StartInfo.RedirectStandardError = true;
                    pProcess.StartInfo.RedirectStandardInput = true;
                    pProcess.StartInfo.RedirectStandardOutput = true;


                    pProcess.Start();

                    pProcess.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds);


                    StreamReader reader = pProcess.StandardOutput;
                    string outputcmd = reader.ReadToEnd();
                    String output = null;

                    bool branchesfound = false;
                    bool branchpublic = false;
                    bool endsequence = false;

                    foreach (var line in outputcmd.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {

                        if (line.Contains("\"branches\""))
                        {
                            branchesfound = true;
                        }

                        if ((branchesfound == true) && (line.Contains("\"public\"")))
                        {
                            branchpublic = true;
                        }
                        if ((branchpublic == true) && (line.Contains("}")))
                        {
                            endsequence = true;
                        }

                        if ((branchpublic == true) && (endsequence == false))
                        {
                            output += line;
                        }
                    }

                    var appinfo = new AppInfo();


                    if (!string.IsNullOrEmpty(output))
                    {
                        output = Regex.Replace(output, @"\t|\n|\r|\{", "");
                        var splitoutput = output.Split('\"');

                        if (!string.IsNullOrEmpty(splitoutput[5]))
                        {
                            if (true == Int32.TryParse(splitoutput[5], out int id))
                            {
                                if (id != 0)
                                {
                                    appinfo.buildid = id;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(splitoutput[9]))
                        {
                            if (true == Int32.TryParse(splitoutput[9], out int time))
                            {
                                if (time != 0)
                                {
                                    appinfo.timeupdated = time;
                                }
                            }
                        }
                    }


                    pProcess.Close();
                    return appinfo;
                });

        }


    }
}
