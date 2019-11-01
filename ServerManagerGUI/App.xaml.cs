using DiscordBot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ServerManagerGUI
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
 
            string datetimeFormat = "yyyy-MM-dd-HH-mm-ss-fff";



            string logFilename = "ServerManagerCrash" + datetimeFormat + ".txt"; 


            StreamWriter w = File.AppendText(logFilename);


            Log("An unexpected application exception occurred" + args.Exception , w);

            MessageBox.Show("An unexpected exception has occurred. Shutting down the application. Please check the log file for more details." + args.Exception);

            // Prevent default unhandled exception processing
            args.Handled = true;

            Environment.Exit(0);
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }


    }
}
