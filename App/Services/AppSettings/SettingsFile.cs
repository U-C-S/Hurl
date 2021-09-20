using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Hurl.Constants;
using System.Xml.Linq;

namespace Hurl.Services.AppSettings
{
    internal class SettingsFile
    {
        private XDocument FileX;

        public bool DataExists {  get; set; } = false;

        public SettingsFile()
        {
            if (!File.Exists(MetaStrings.SettingsFilePath))
            {
                Directory.CreateDirectory(OtherStrings.ROAMING + "\\Hurl");
                //File.Create(MetaStrings.SettingsFilePath);
                FileX = new XDocument(new XElement("root"));
                FileX.Save(MetaStrings.SettingsFilePath);
            }
            else
            {
                FileX = XDocument.Load(MetaStrings.SettingsFilePath);
            }
        }
    }
}
