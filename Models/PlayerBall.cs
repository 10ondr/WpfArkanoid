using System.Windows;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// Represents one player ball. Its width and speed can be changed by collectables.
    /// </summary>
    public class PlayerBall : Ball
    {
        public const double defaultSpeed = 2;

        public bool IsPinned { get; set; }

        private int _damage;

        public int Damage
        {
            get { return _damage; }
            set {
                _damage = value;
                OnPropertyChanged();
            }
        }


        public PlayerBall(double X, double Y, double Width = defaultWidth, double Height = defaultHeight, double Speed = defaultSpeed, int Damage = 1) : base(X, Y, Width, Height, Speed)
        {
            this.Damage = Damage;
            IsPinned = true;
        }

        /// <summary>
        /// Releases the ball when it is in the pinned down state.
        /// </summary>
        /// <param name="initialDirection">Starting direction vector of the ball after release</param>
        public void Release(Vector initialDirection) {
            if (IsPinned)
            {
                Direction = initialDirection;
                Direction.Normalize();
                IsPinned = false;
                Speed = PlayerBall.defaultSpeed;
            }
        }

        /// <summary>
        /// Bouces the ball with respect to the provided directional vector.
        /// </summary>
        /// <param name="hitDirection">Directional vector of the hit.</param>
        public void Bounce(ref Vector hitDirection)
        {
            hitDirection.Normalize();

            // TODO: Simplified. Wouldn't work with angled walls
            if ((hitDirection.Y < 0 && Direction.Y < 0) || (hitDirection.Y > 0 && Direction.Y > 0))
            {
                Direction.Y = -Direction.Y;
            }
            else if ((hitDirection.X < 0 && Direction.X < 0) || (hitDirection.X > 0 && Direction.X > 0))
            {
                Direction.X = -Direction.X;
            }
        }

        /// <summary>
        /// Offsets the ball's X direction value. Will never breach certain threshold.
        /// </summary>
        /// <param name="valueX">Value by how much to deflect the ball direction</param>
        public void DeflectX(double valueX)
        {
            // Ensure the ball never gets too horizontal trajectory
            if ((valueX < 0 && Direction.X > -0.9) || (valueX > 0 && Direction.X < 0.9))
            {
                Direction.X += valueX;
                Direction.Normalize();
            }
        }

        /// <summary>
        /// Creates a new ball with the same position as the original one but mirrored X movement direction.
        /// </summary>
        /// <returns>New cloned ball instance</returns>
        public PlayerBall CreateClone()
        {
            var b = new PlayerBall(X, Y, Width, Height)
            {
                Direction = new Vector(-this.Direction.X, this.Direction.Y),
                IsPinned = false
            };
            return b;
        }
    }
}
