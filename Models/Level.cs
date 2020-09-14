using System.Collections.ObjectModel;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// One level of the game. Is loaded from an external XML file.
    /// </summary>
    public class Level
    {
        /// <summary>
        /// Individual rows in the level
        /// </summary>
        public ObservableCollection<BrickRow> BrickRows { get; set; } = new ObservableCollection<BrickRow>();
        /// <summary>
        /// Additional (optional) walls specific for a level
        /// </summary>
        public ObservableCollection<RectangleObstacle> Walls { get; set; } = new ObservableCollection<RectangleObstacle>();
        /// <summary>
        /// Additional (optional) death triggers for a level
        /// </summary>
        public ObservableCollection<RectangleObstacle> DeathTriggers { get; set; } = new ObservableCollection<RectangleObstacle>();
        public Level()
        {
        }

        public void LoadSampleValues() {
            for (int i = BrickRows.Count - 1; i >= 0; i--)
                BrickRows.RemoveAt(i);

            for (int i = 0; i < 6; i++)
            {
                BrickRow row = new BrickRow();
                row.LoadSampleValues(6, offsetX: 35, offsetY: (30 * i) + 30);
                BrickRows.Add(row);
            }
        }

        public bool IsAllEmpty() {
            foreach (var row in BrickRows) {
                if (row.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
