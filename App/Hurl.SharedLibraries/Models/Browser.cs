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

        private Icon RawIcon { get; set; }

        public ImageSource GetIcon
        {
            get
            {
                if (ExePath != null)
                    return IconUtilites.ToImageSource(RawIcon);
                else return null;
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
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class AlternateLaunch
    {
        private AlternateLaunch() { }

        public string Name { get; set; }
        public string LaunchArgs { get; set; }
        private bool IsPath { get; set; }
        private string LaunchExe = null;

        [JsonIgnore]
        public string LaunchExecutable
        {
            get
            {
                if (IsPath)
                    return LaunchExe;
                else
                    return null;
            }
            set
            {
                LaunchExe = value;
            }
        }

        public static AlternateLaunch FromDiffExe(string Name, string ExePath, string LaunchArgs)
        {
            return new AlternateLaunch()
            {
                Name = Name,
                LaunchExecutable = ExePath,
                LaunchArgs = LaunchArgs,
                IsPath = true
            };
        }

        public static AlternateLaunch WithDiffArgs(string Name, string LaunchArgs)
        {
            return new AlternateLaunch()
            {
                Name = Name,
                LaunchArgs = LaunchArgs,
                IsPath = false
            };
        }
    }

    public enum BrowserSourceType
    {
        Registry,
        User
    }
}
