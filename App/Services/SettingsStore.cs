using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Hurl.Services
{
    class SettingsStore
    {
        public string StorePath;

        public SettingsStore(string Path)
        {
            StorePath = Path;
        }

        public XDocument GetSettings()
        {
            if (File.Exists(StorePath))
            {
                return XDocument.Load(StorePath);
            }
            else
            {
                return Initalize();
            }
        }

        private XDocument Initalize()
        {
            XDocument settingsFile = new XDocument(
                new XElement("root",
                                new XElement("settings", "ok"),
                                new XElement("browsers", "edge")
                            )
            );
            settingsFile.Save(StorePath);
            return settingsFile;
        }
    }
}
