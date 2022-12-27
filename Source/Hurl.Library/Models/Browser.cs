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

        [JsonInclude]
        public string Name { get; set; }

        [JsonInclude]
        public string ExePath { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string LaunchArgs { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Hidden { get; set; } = false;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public AlternateLaunch[] AlternateLaunches { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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
                if (AlternateLaunches == null || AlternateLaunches.Length == 0)
                    return Visibility.Hidden;
                else return Visibility.Visible;
            }
        }

        [JsonIgnore]
        public string Linkx { get; set; }
    }

    public class AlternateLaunch
    {
        public AlternateLaunch() { }

        public AlternateLaunch(string ItemName, string LaunchArgs)
        {
            this.ItemName = ItemName;
            this.LaunchArgs = LaunchArgs;
        }

        [JsonInclude]
        public string ItemName { get; set; }

        [JsonInclude]
        public string LaunchArgs { get; set; }
    }
}
