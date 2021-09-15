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
        // Create a interface the _logbox so that you can choose wherever you want to log the text.
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
            if (isSubLog)
            {
                AppendText(" - " + log + Environment.NewLine);
            }
        }

        public void writeRaw(string log)
        {
            AppendText("-> " + log + Environment.NewLine);
        }

        public void Start(string message, bool dontLog = false)
        {
            if (!dontLog)
            {
                AppendText("===== " + message + " =====" + Environment.NewLine);
                isSubLog = true;
            }
        }

        public void Stop()
        {
            AppendText(Environment.NewLine + "===========" + Environment.NewLine);
            isSubLog = false;
        }
    }
}
