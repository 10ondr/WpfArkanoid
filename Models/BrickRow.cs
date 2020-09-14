using System;
using System.Collections.ObjectModel;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// One row of bricks in a game level
    /// </summary>
    public class BrickRow : ObservableCollection<Brick>
    {
        public BrickRow()
        {
        }

        public void LoadSampleValues(int columns, int h=1, int p=10, double spacing = 25, double offsetX = 0, double offsetY = 0) {
            Random rnd = new Random((int)offsetY);

            for (int i = Count - 1; i >= 0; i--)
                RemoveAt(i);          

            for (int i = 0; i < columns; i++)
            {
                Brick b = new Brick();
                int r = rnd.Next(1, 10);
                b.SetValues((i * (Brick.defaultWidth + spacing)) + offsetX, offsetY, points: p * r, health: r);
                Add(b);
            }
        }
    }
}
