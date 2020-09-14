using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
using WpfArkanoid.Models;
using WpfArkanoid.ViewModels.Commands;

namespace WpfArkanoid.ViewModels
{
    /// <summary>
    /// View model of the game
    /// TODO: For demonstation purposes everything runs in one thread. Game logic and UI threads should be separated.
    /// </summary>
    public class GameViewModel : IValueConverter
    {

        public RelayCommand KeyDownCommand { get; set; }
        public RelayCommand KeyUpCommand { get; set; }
        public ArkanoidGame Game { get; }

        public int DisplayAreaWidth {
            set { }
            get { return (int) (Game.GameAreaWidth + ArkanoidGame.WallSize); }
        }
        public int DisplayAreaHeight
        {
            set { }
            get { return (int) (Game.GameAreaHeight + ArkanoidGame.WallSize + 100); }
        }

        public GameViewModel()
        {
            Game = new ArkanoidGame();

            KeyDownCommand = new RelayCommand(KeyPressDown);
            KeyUpCommand = new RelayCommand(KeyPressUp);
        }

        private void KeyPressDown(object parameter)
        {
            Game.ProcessKeyDown(parameter as KeyEventArgs);
        }

        private void KeyPressUp(object parameter)
        {
            Game.ProcessKeyUp(parameter as KeyEventArgs);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}