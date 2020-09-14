using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WpfArkanoid.Models;

namespace WpfArkanoid.ViewModels
{
    /// <summary>
    /// Converts "game over" state into a visibility value.
    /// </summary>
    public class GameOverToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((PlayerStats.GameStates) value == PlayerStats.GameStates.game_over)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
