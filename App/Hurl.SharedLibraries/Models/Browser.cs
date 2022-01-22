using Hurl.SharedLibraries.Services;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
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
            if (ExePath != null)
            {
                this.RawIcon = ExePath.StartsWith('"'.ToString())
                        ? IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2))
                        : IconExtractor.FromFile(ExePath);
            }
        }

        [JsonProperty]
        public BrowserSourceType SourceType { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string ExePath { get; set; }

        [JsonProperty]
        public string LaunchArgs { get; set; }

        [JsonProperty]
        public bool Hidden { get; set; } = false;

        [JsonProperty]
        public AlternateLaunch[] AlternateLaunches { get; set; }

        private Icon RawIcon { get; set; }

        public ImageSource GetIcon
        {
            get
            {
                if (ExePath != null && RawIcon != null)
                    return IconUtilites.ToImageSource(RawIcon);
                else return null;
            }
        }

        /*
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
        */
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class AlternateLaunch
    {
        private AlternateLaunch() { }

        public AlternateLaunch(string ItemName, string LaunchArgs)
        {
            this.ItemName = ItemName;
            this.LaunchArgs = LaunchArgs;
            this.IsPath = false;
        }

        public AlternateLaunch(string ItemName, string ExePath, string LaunchArgs)
        {
            this.ItemName = ItemName;
            this.LaunchExe = ExePath;
            this.LaunchArgs = LaunchArgs;
            this.IsPath = true;
        }

        public string ItemName { get; set; }
        public string LaunchExe { get; set; }
        public string LaunchArgs { get; set; }
        public bool IsPath { get; set; }
    }

    public enum BrowserSourceType
    {
        Registry,
        User
    }
}
