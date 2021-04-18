using MvvmCross.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MediaExplorer.Wpf.Converters
{
    class ImageSupportsMediaConverter : IValueConverter
    {
        private List<string> _supportedFileEndings = new List<string>()
        {
            "jpeg", "jpg",
            "png",
            "bmp",
            "gif",
            "tiff",
            "ico",
            "svg"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _supportedFileEndings.Any(
                x => (value as string).Split('.').Last().ToLower().Contains(x)) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
