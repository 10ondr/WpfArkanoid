using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfArkanoid.ViewModels
{
    /// <summary>
    /// Converts ball damage value to color. Higher values than 5 are all converted to the same color.
    /// </summary>
    class BallDamageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value) {
                case 1:
                    return Colors.Yellow;
                case 2:
                    return Colors.Orange;
                case 3:
                    return Colors.Red;
                case 4:
                    return Colors.Purple;
                case 5:
                    return Colors.Violet;
            }
            return Colors.Violet;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
