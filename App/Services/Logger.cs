using Hurl.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hurl.Services
{
    public class Logger
    {
        protected TextBox _logbox;
        private bool isSubLog;

        public Logger(TextBox LogBox)
        {
            // A feature turn off the log
            _logbox = LogBox;
        }

        private void AppendText(string text)
        {
            _logbox.Text += text;
        }

        public void Write(string log)
        {
            string prefix = "";
            if (isSubLog) { prefix = "- "; }

            AppendText(prefix + log + Environment.NewLine);
        }

        public void Start(string message)
        {
            AppendText("===== " + message + " =====" + Environment.NewLine);
            isSubLog = true;
        }

        public void Stop()
        {
            AppendText(Environment.NewLine + "===========" + Environment.NewLine);
            isSubLog = false;
        }
    }
}
