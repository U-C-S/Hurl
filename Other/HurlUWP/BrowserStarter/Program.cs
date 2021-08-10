using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BrowserStarter
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            string x = ApplicationData.Current.LocalSettings.Values["browser"] as string;
            //Console.Title = "okok";
            //Console.WriteLine("This process has access to the entire public desktop API surface -" + x);
            //Console.ReadLine();

            Process.Start(x, @"https://google.com");
        }
    }
}
