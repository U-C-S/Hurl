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
            this.RawIcon = ExePath.StartsWith('"'.ToString())
                    ? IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2))
                    : IconExtractor.FromFile(ExePath);
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

    [JsonObject(MemberSerialization.OptOut)]
    public class AlternateLaunch
    {
        public string Name { get; set; }
        public string LaunchCommand {  get; set;}
        public bool IsPath { get; set; }
    }

    public enum BrowserSourceType
    {
        Registry,
        User
    }
}
