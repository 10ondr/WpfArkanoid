using System.Windows;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// Bonus which may be dropped by destroying a brick. Affects different aspects of the game (positively or negatively).
    /// </summary>
    public class Collectable : Ball
    {
        public enum CollectableType { none, clone_all, decrease_speed, increase_width, decrease_width, add_life, increase_damage };
        public CollectableType Type { get; set; }
        public Collectable(CollectableType Type, double X, double Y, double Width = 15, double Height = 15, double Speed = 1) : base(X, Y, Width, Height, Speed)
        {
            this.Type = Type;
            Direction = new Vector(0, 1);
        }
    }
}
