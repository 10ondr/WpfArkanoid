using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// Simple representation of a 2D rectangle object
    /// </summary>
    public class RectangleObstacle : INotifyPropertyChanged
    {
        public const double defaultWidth = 50.0;
        public const double defaultHeight = 10.0;

        public event PropertyChangedEventHandler PropertyChanged;

        private double _x;

        public double X
        {
            get { return _x; }
            set {
                _x = value;
                OnPropertyChanged();
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        private double _width;

        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        private double _height;

        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        private RectangleObstacle() {
        }

        public RectangleObstacle(double X, double Y, double Width = defaultWidth, double Height = defaultHeight)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
