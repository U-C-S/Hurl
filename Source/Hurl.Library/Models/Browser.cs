using System;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hurl.Library.Models
{
    public class Browser
    {
        public Browser(string Name, string ExePath)
        {
            this.Name = Name;
            this.ExePath = ExePath;
        }
        public string Name { get; set; }

        public string ExePath { get; set; }

        public string LaunchArgs { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Hidden { get; set; } = false;

        public AlternateLaunch[] AlternateLaunches { get; set; }

        public string CustomIconPath { get; set; }

        [JsonIgnore]
        public ImageSource GetIcon
        {
            get
            {
                if (!string.IsNullOrEmpty(CustomIconPath))
                {
                    return new BitmapImage(new Uri(CustomIconPath));
                }
                else if (!string.IsNullOrEmpty(ExePath))
                {
                    Icon RawIcon = ExePath.StartsWith('"'.ToString())
                                ? IconExtractor.FromFile(ExePath.Substring(1, ExePath.Length - 2))
                                : IconExtractor.FromFile(ExePath);

                    return IconUtilites.ToImageSource(RawIcon);
                }
                else
                    return null;
            }
        }

        [JsonIgnore]
        public Visibility ShowAdditionalBtn
        {
            get
            {
                return AlternateLaunches == null || AlternateLaunches.Length == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        // public string[] Rules { get; set; }
    }

    public class AlternateLaunch
    {
        public AlternateLaunch() { }

        public AlternateLaunch(string ItemName, string LaunchArgs)
        {
            this.ItemName = ItemName;
            this.LaunchArgs = LaunchArgs;
        }

        public string ItemName { get; set; }

        public string LaunchArgs { get; set; }
    }
}
