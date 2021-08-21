using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class Logger
    {
        private string LogString;
        private StringBuilder LogBuilder;

        public Logger(string TargetString)
        {
            LogString = TargetString;
            LogBuilder = new StringBuilder();
        }

        public void Add(string log)
        {
            LogBuilder.AppendLine(log);
            LogString = LogBuilder.ToString();
        }
    }
}
