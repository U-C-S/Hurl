using System;
using System.Runtime.InteropServices;

namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(RuntimeInformation.OSDescription + " " + RuntimeInformation.OSArchitecture);

            browserlist.getlist();
            // Console.WriteLine(x);
            // Console.WriteLine(RuntimeInformation.FrameworkDescription);
        }
    }
}
