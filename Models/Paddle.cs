using System.Windows;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// Represents a player paddle. Has always constant speed, but its width can be changed with collectables.
    /// </summary>
    public class Paddle : RectangleObstacle
    {
        public enum PlayerDirection { left, right, none };

        public new const double defaultWidth = 150.0;
        public new const double defaultHeight = 15.0;
        public double Speed { get; set; }
        public PlayerDirection CurrentPlayerDirection { get; set; }
        public Paddle(double X, double Y, double Width = defaultWidth, double Height = defaultHeight, double Speed = 3) : base(X, Y, Width, Height)
        {
            this.Speed = Speed;
            CurrentPlayerDirection = PlayerDirection.none;
        }

        public Vector GetPinnedPosition() {
            return new Vector(X + Width / 2, Y);
        }

        public void UpdatePosition(double minX, double maxX) {
            switch (CurrentPlayerDirection)
            {
                case PlayerDirection.left:
                    if(X - Speed > minX)
                        X -= Speed;
                    break;
                case PlayerDirection.right:
                    if(X + Width + Speed < maxX)
                        X += Speed;
                    break;
            }
        }
    }
}
