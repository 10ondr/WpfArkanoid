using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfArkanoid.ViewModels
{
    /// <summary>
    /// Converts brick health value to color. Values greater than 10 are converted to the same color.
    /// </summary>
    class BrickHealthToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 1:
                    return Colors.White;
                case 2:
                    return Colors.Blue;
                case 3:
                    return Colors.Red;
                case 4:
                    return Colors.Purple;
                case 5:
                    return Colors.Beige;
                case 6:
                    return Colors.Aqua;
                case 7:
                    return Colors.Brown;
                case 8:
                    return Colors.Coral;
                case 9:
                    return Colors.Cornsilk;
                case 10:
                    return Colors.Crimson;
            }
            return Colors.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
