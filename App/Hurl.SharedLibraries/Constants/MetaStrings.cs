﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.SharedLibraries.Constants
{
    public class MetaStrings
    {
        public const string NAME = "Hurl";
        public const string DESCRIPTION = "Hurl - A tool to select the browsers dynamically";
        public const string URLAssociations = "HandleURL3721";
        public const string VERSION = "0.2.1";

        public static string SettingsFilePath = Path.Combine(OtherStrings.ROAMING, "Hurl", "UserSettings.json");

    }
}