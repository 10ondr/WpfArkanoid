using System;
using System.Globalization;
using System.Windows.Data;
using WpfArkanoid.Models;

namespace WpfArkanoid.ViewModels
{
    /// <summary>
    /// Converts collectable type to a human readable string description.
    /// </summary>
    public class CollectableToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Collectable.CollectableType) value)
            {
                case Collectable.CollectableType.clone_all:
                    return "All balls cloned!";
                case Collectable.CollectableType.decrease_speed:
                    return "Speed decreased!";
                case Collectable.CollectableType.increase_width:
                    return "Paddle width increased!";
                case Collectable.CollectableType.decrease_width:
                    return "Paddle width decreased!";
                case Collectable.CollectableType.add_life:
                    return "Bonus life!";
                case Collectable.CollectableType.increase_damage:
                    return "Damage increased!";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
