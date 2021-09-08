﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurl.Constants
{
    public class MetaStrings
    {
        public const string NAME = "Hurl";
        public const string DESCRIPTION = "Hurl - A tool to select the browsers dynamically";
        public const string URLAssociations = "HandleURL3721";
        public const string VERSION = "0.2.0";

        public static string APP_LAUNCH_PATH = Environment.GetCommandLineArgs()[0];
        public static string APP_PARENT_DIR = Environment.CurrentDirectory;
    }
}