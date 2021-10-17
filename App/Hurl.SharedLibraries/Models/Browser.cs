using Hurl.SharedLibraries.Services;
using System.Text.Json;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Text.Json.Serialization;

namespace Hurl.SharedLibraries.Models
{
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

        [JsonInclude]
        public BrowserSourceType SourceType { get; set; }

        [JsonInclude]
        public string Name { get; set; }

        [JsonInclude]
        public string ExePath { get; set; }

        [JsonInclude]
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

        [JsonIgnore]
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
