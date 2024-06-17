using Hurl.Library.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Hurl.BrowserSelector.Converters
{
    internal class AltLaunchParentConverter : IMultiValueConverter
    {
        public class AltLaunchParent
        {
            public AlternateLaunch AltLaunch { get; set; }
            public Browser Browser { get; set; }
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new AltLaunchParent()
            {
                AltLaunch = values[0] as AlternateLaunch,
                Browser = values[1] as Browser
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return [
                (value as AltLaunchParent).AltLaunch,
                (value as AltLaunchParent).Browser
            ];
        }
    }
}
