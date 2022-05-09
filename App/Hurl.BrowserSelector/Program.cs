using Microsoft.Windows.ApplicationModel.DynamicDependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.BrowserSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap.Initialize(0x00010000);
            //Console.WriteLine("Hello, World!");
            new App().Run();
            Bootstrap.Shutdown();
        }
    }
}
