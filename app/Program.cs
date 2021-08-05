using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using app.Browser;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RuntimeInformation.OSDescription + " " + RuntimeInformation.OSArchitecture);
            BList ls = BList.InitalGetList();

            while (true)
            {
                String inp = Console.ReadLine();
                if (inp.Equals("exit"))
                {
                    Console.WriteLine("exitted");
                    break;
                }
                if (inp.Equals("1"))
                {
                    Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "google.com");
                }
                Console.WriteLine(inp);

            }

            // Console.WriteLine(x);
            // Console.WriteLine(RuntimeInformation.FrameworkDescription);
        }
    }
}
