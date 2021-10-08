using Hurl.SharedLibraries.Services;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Interop;
using System.Windows.Media;

namespace Hurl.SharedLibraries.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Browser
    {
        public Browser(string Name, string ExePath)
        {
            this.Name = Name;
            this.ExePath = ExePath;
            this.RawIcon = ExePath.StartsWith('"'.ToString())
                    ? IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2))
                    : IconExtractor.FromFile(ExePath);
        }

        [JsonProperty]
        public BrowserSourceType SourceType { get; set; }

        [JsonProperty]
        public string Name { get; }

        [JsonProperty]
        public string ExePath { get; }

        [JsonProperty]
        public bool Hidden { get; set; } = false;

        //[JsonProperty]
        private string IconString
        {
            get
            {
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    RawIcon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    bytes = ms.ToArray();
                }

                string iconString = Convert.ToBase64String(bytes);

                return iconString;
            }
        }

        private Icon RawIcon
        {
            get; set;
        }

        public ImageSource GetIcon => IconUtilites.ToImageSource(RawIcon);

        public Image StringToIcon()
        {
            byte[] byteArray = Convert.FromBase64String(this.IconString);
            Bitmap newIcon;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                newIcon = new Bitmap(stream);
            }

            return newIcon;
        }
    }

    public enum BrowserSourceType
    {
        Registry,
        User
    }
}

/*
    public class BrowserObject
    {
        public string Name { get; set; }
        public string ExePath { get; set; }
        public BrowserSourceType SourceType { get; set; }
        public ImageSource GetIcon
        {
            get
            {
                Icon x = ExePath.StartsWith('"'.ToString())
                    ? IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2))
                    : IconExtractor.FromFile(ExePath);

                return IconUtilites.ToImageSource(x);
            }
        }

        public static string IconToString(Icon icon)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                icon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                bytes = ms.ToArray();
            }

            string iconString = Convert.ToBase64String(bytes);

            return iconString;
        }

        public Image StringToIcon()
        {
            byte[] byteArray = Convert.FromBase64String(this.icon);
            Bitmap newIcon;
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                newIcon = new Bitmap(stream);
            }

            return newIcon;
        }

        public bool Hidden { get; set; } = false;
        public string[] Arguments { get; set; }
        //private string IncognitoArg = null;
    }

    public enum BrowserSourceType
    {
        Registry,
        User
    }
*/