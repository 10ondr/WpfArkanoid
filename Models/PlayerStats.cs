using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// Holds information about player progress and statistics
    /// </summary>
    public class PlayerStats : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public enum GameStates { game_running, game_over, game_won };

        private GameStates _gameState;

        public GameStates GameState
        {
            get { return _gameState; }
            set
            {
                _gameState = value;
                OnPropertyChanged();
            }
        }

        private int _score;

        public int Score
        {
            get { return _score; }
            set { 
                _score = value;
                OnPropertyChanged();
            }
        }

        private int _lives;

        public int Lives
        {
            get { return _lives; }
            set { 
                _lives = value;
                OnPropertyChanged();
            }
        }

        private Collectable.CollectableType _lastCollectable;

        public Collectable.CollectableType LastCollectable
        {
            get { return _lastCollectable; }
            set {
                _lastCollectable = value;
                OnPropertyChanged();
            }
        }


        public PlayerStats(int Score = 0, int Lives = 3)
        {
            this.Score = Score;
            this.Lives = Lives;
            LastCollectable = Collectable.CollectableType.none;
            GameState = GameStates.game_running;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
