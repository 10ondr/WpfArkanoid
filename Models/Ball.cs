using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// A Simple representation of a 2D circle object
    /// </summary>
    public class Ball : INotifyPropertyChanged
    {
        public const double defaultWidth = 20.0;
        public const double defaultHeight = 20.0;

        public event PropertyChangedEventHandler PropertyChanged;

        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
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

        public Vector Direction;
        public double Speed { get; set; }

        public Ball(double X, double Y, double Width = defaultWidth, double Height = defaultHeight, double Speed = 1)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Speed = Speed;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Check 2D collision between circle (this) and one rectangle shape
        /// </summary>
        /// <param name="brick">Rectangle shape to check the collision against</param>
        /// <param name="direction">Direction vector of the hit</param>
        /// <returns>True if the two intersect, false otherwise</returns>
        public bool Intersect(RectangleObstacle brick, ref Vector direction)
        {
            double ballX = this.X + this.Width / 2;
            double ballY = this.Y + this.Height / 2;
            double testX = ballX;
            double testY = ballY;

            if (ballX < brick.X) testX = brick.X;
            else if (ballX > brick.X + brick.Width) testX = brick.X + brick.Width;
            if (ballY < brick.Y) testY = brick.Y;
            else if (ballY > brick.Y + brick.Height) testY = brick.Y + brick.Height;

            direction.X = testX - ballX;
            direction.Y = testY - ballY;

            double distance = direction.Length;

            if (distance <= this.Width / 2)
                return true;
            return false;
        }
        public void UpdatePosition()
        {
            X += Direction.X * Speed;
            Y += Direction.Y * Speed;
        }

        public bool IsMovingUp() {
            return Direction.Y < 0;
        }

        public bool IsMovingLeft() {
            return Direction.X > 0;
        }
    }
}
