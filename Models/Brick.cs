namespace WpfArkanoid.Models
{
    /// <summary>
    /// Representation of one brick in the game. Can have different health and score reward points.
    /// </summary>
    public class Brick : RectangleObstacle
    {
        private int _health;

        public int Health
        {
            get { return _health; }
            set {
                _health = value;
                OnPropertyChanged();
            }
        }

        public int BrickPoints { get; set; }
        public Brick() : base(0, 0, 0, 0)
        {
        }

        public void SetValues(double X, double Y, double Width = 50, double Height = 10, int points = 10, int health = 1)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            BrickPoints = points;
            Health = health;
        }

        public void Hit(int damage) {
            Health -= damage;
        }
        public bool Alive() {
            return Health > 0;
        }
    }
}
