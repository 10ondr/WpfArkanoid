using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace WpfArkanoid.Models
{
    /// <summary>
    /// The main representation of the game logic. Controls the game flow, progress and events.
    /// </summary>
    public class ArkanoidGame : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double GameAreaWidth { get; set; } = 500;
        public double GameAreaHeight { get; set; } = 300;

        public const double WallSize = 15;

        /// <summary>
        /// Walls that are copied as addition to all level specific walls
        /// </summary>
        public ObservableCollection<RectangleObstacle> PermanentWalls { get; set; } = new ObservableCollection<RectangleObstacle>();
        /// <summary>
        /// Death triggers that are copied as addition to all level specific ones
        /// </summary>
        public ObservableCollection<RectangleObstacle> PermanentDeathTriggers { get; set; } = new ObservableCollection<RectangleObstacle>();
        public ObservableCollection<Collectable> Collectables { get; set; } = new ObservableCollection<Collectable>();
        public ObservableCollection<PlayerBall> Balls { get; set; } = new ObservableCollection<PlayerBall>();
        public Paddle PlayerPaddle { get; set; } = new Paddle(0, 0);

        private string[] levelFileNames;
        private int currentLevelFileIndex = 0;

        private Level _currentLevel;

        public Level CurrentLevel
        {
            get { return _currentLevel; }
            set {
                _currentLevel = value;
                OnPropertyChanged();
            }
        }


        public PlayerStats CurrentPlayerStats { get; set; } = new PlayerStats();

        private Random _randomGenerator;
        private ulong _currentTick;
        private readonly System.Windows.Threading.DispatcherTimer gameTickTimer = new System.Windows.Threading.DispatcherTimer();

        /// <summary>
        /// Constructor retrieves levels from the provided path and starts the game.
        /// </summary>
        /// <param name="levelFilePath">Path to the directory containing XML files with level descriptions</param>
        public ArkanoidGame(string levelFilePath=@"Levels\")
        {
            try
            {
                levelFileNames = Directory.GetFiles(levelFilePath, "*.xml");
            }
            catch (Exception)
            {
                levelFileNames = new string[]{ "RandomlyGenerated" };
            }
            

            PermanentWalls.Add(new RectangleObstacle(0, 0, GameAreaWidth, WallSize));
            PermanentWalls.Add(new RectangleObstacle(GameAreaWidth - WallSize, 0, WallSize, GameAreaHeight));
            PermanentWalls.Add(new RectangleObstacle(0, GameAreaHeight - WallSize, GameAreaWidth, WallSize));
            PermanentWalls.Add(new RectangleObstacle(0, 0, WallSize, GameAreaHeight - WallSize));

            PermanentDeathTriggers.Add(new RectangleObstacle(WallSize, GameAreaHeight - 1.5 * WallSize, GameAreaWidth - 2 * WallSize, WallSize / 2));

            gameTickTimer.Tick += GameLoop;
            gameTickTimer.Interval = TimeSpan.FromSeconds(0.01);

            StartNewGame();
        }

        private Level LoadLevel(string path) {
            Level lvl;
            XmlSerializer xs = new XmlSerializer(typeof(Level));
            try
            {
                TextReader txtReader = new StreamReader(path);
                lvl = (Level)xs.Deserialize(txtReader);
                txtReader.Close();
            }
            catch (Exception) {
                lvl = new Level();
                lvl.LoadSampleValues();
            }
            return lvl;
        }
        private void SaveLevel(Level lvl, string path) {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Level));
                TextWriter txtWriter = new StreamWriter(path);
                xs.Serialize(txtWriter, lvl);
                txtWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        private void NextLevel() {
            ReinitializeGame();

            if (currentLevelFileIndex >= levelFileNames.Length)
            {
                GameWon();
                return;
            }

            CurrentLevel = LoadLevel(levelFileNames[currentLevelFileIndex++]);
            foreach (var wall in PermanentWalls)
                CurrentLevel.Walls.Add(wall);
            foreach (var trigger in PermanentDeathTriggers)
                CurrentLevel.DeathTriggers.Add(trigger);

            gameTickTimer.Start();
        }

        private void ReinitializeGame()
        {
            // Remove game object instances
            for (int i = Collectables.Count - 1; i >= 0; i--)
                Collectables.RemoveAt(i);
            for (int i = Balls.Count - 1; i >= 0; i--)
                Balls.RemoveAt(i);

            // Reinitialize Player paddle
            PlayerPaddle.Width = Paddle.defaultWidth;
            PlayerPaddle.Height = Paddle.defaultHeight;
            PlayerPaddle.X = GameAreaWidth / 2 - PlayerPaddle.Width / 2;
            PlayerPaddle.Y = GameAreaHeight - 50;

            _currentTick = 0;

            Balls.Add(new PlayerBall(0, 0));
        }
        private void StartNewGame()
        {
            // New random generator with the same initial seed
            _randomGenerator = new Random(465089);

            // Reinitialize Player stats
            CurrentPlayerStats.GameState = PlayerStats.GameStates.game_running;
            CurrentPlayerStats.Lives = 3;
            CurrentPlayerStats.Score = 0;

            currentLevelFileIndex = 0;
            NextLevel();
        }

        private void GameWon() {
            CurrentPlayerStats.LastCollectable = Collectable.CollectableType.none;
            CurrentPlayerStats.GameState = PlayerStats.GameStates.game_won;
            gameTickTimer.Stop();
        }

        private void GameOver()
        {
            CurrentPlayerStats.LastCollectable = Collectable.CollectableType.none;
            CurrentPlayerStats.GameState = PlayerStats.GameStates.game_over;
            gameTickTimer.Stop();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            _currentTick++;
            PlayerPaddle.UpdatePosition(WallSize, GameAreaWidth - WallSize);

            CheckCollectables();
            CheckBalls();

            // Constantly increase speed as the game goes on
            if (_currentTick % 200 == 0)
                IncreaseSpeed(0.1);
        }

        /// <summary>
        /// Checks interactions with collectables (bonus drops).
        /// </summary>
        private void CheckCollectables() {
            for (int i = 0; i < Collectables.Count; i++)
            {
                var collectable = Collectables[i];
                collectable.UpdatePosition();

                Vector hitDirection = new Vector();

                if (collectable.Intersect(PlayerPaddle, ref hitDirection))
                {
                    CurrentPlayerStats.LastCollectable = collectable.Type;
                    switch (collectable.Type)
                    {
                        case Collectable.CollectableType.clone_all:
                            List<PlayerBall> tempList = new List<PlayerBall>();
                            foreach (var ball in Balls)
                            {
                                tempList.Add(ball.CreateClone());
                            }
                            foreach (var clonedBall in tempList)
                            {
                                Balls.Add(clonedBall);
                            }
                            break;
                        case Collectable.CollectableType.increase_width:
                            if(PlayerPaddle.Width < GameAreaWidth / 2)
                                PlayerPaddle.Width += 20;
                            break;
                        case Collectable.CollectableType.decrease_width:
                            if (PlayerPaddle.Width > 40)
                                PlayerPaddle.Width -= 20;
                            break;
                        case Collectable.CollectableType.decrease_speed:
                            IncreaseSpeed(-0.5);
                            break;
                        case Collectable.CollectableType.add_life:
                            CurrentPlayerStats.Lives += 1;
                            break;
                        case Collectable.CollectableType.increase_damage:
                            foreach (var ball in Balls)
                            {
                                ball.Damage += 1;
                            }
                            break;

                    }
                    Collectables.Remove(collectable);
                }

                // Collectable is destroyed when lands on death trigger
                foreach (var trigger in CurrentLevel.DeathTriggers)
                {
                    if (collectable.Intersect(trigger, ref hitDirection))
                    {
                        Collectables.Remove(collectable);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Checks interactions between balls and other game instances (death triggers, player paddle, walls, bricks).
        /// TODO: Could be optimized - when ball is moving from a certain position in a certain direction,
        /// not all bricks have to be checked for collision.
        /// </summary>
        private void CheckBalls() {
            for (int i = 0; i < Balls.Count; i++)
            {
                var ball = Balls[i];
                if (ball.IsPinned)
                {
                    Vector pinnedPosition = PlayerPaddle.GetPinnedPosition();
                    ball.X = pinnedPosition.X - ball.Width / 2;
                    ball.Y = pinnedPosition.Y - ball.Height;
                    continue;
                }
                ball.UpdatePosition();

                Vector hitDirection = new Vector();

                // Interaction with player paddle
                if (ball.Intersect(PlayerPaddle, ref hitDirection))
                {
                    ball.Bounce(ref hitDirection);
                    if (PlayerPaddle.CurrentPlayerDirection == Paddle.PlayerDirection.left)
                    {
                        ball.DeflectX(-0.85);
                    }
                    else if (PlayerPaddle.CurrentPlayerDirection == Paddle.PlayerDirection.right)
                    {
                        ball.DeflectX(0.85);
                    }
                }

                // Interaction with walls
                foreach (var wall in CurrentLevel.Walls)
                {
                    if (ball.Intersect(wall, ref hitDirection))
                        ball.Bounce(ref hitDirection);
                }

                // Interaction with death triggers
                foreach (var trigger in CurrentLevel.DeathTriggers)
                {
                    if (ball.Intersect(trigger, ref hitDirection)) {
                        Balls.Remove(ball);
                        if (Balls.Count < 1)
                        {
                            for (int j = Collectables.Count - 1; j >= 0; j--) {
                                Collectables.RemoveAt(i);
                            }
                            RemoveLife();
                        }
                        continue;
                    }
                }
                
                // Interaction with bricks
                foreach (var brickRow in CurrentLevel.BrickRows)
                {
                    for (int j = brickRow.Count - 1; j >= 0; j--)
                    {
                        Brick brick = brickRow[j];

                        if (ball.Intersect(brick, ref hitDirection))
                        {
                            brick.Hit(ball.Damage);
                            if (!brick.Alive())
                            {
                                CurrentPlayerStats.Score += brick.BrickPoints;

                                int rnd = _randomGenerator.Next(0, 100);
                                if (rnd < 10)
                                    Collectables.Add(new Collectable(Collectable.CollectableType.clone_all, brick.X, brick.Y));
                                else if (rnd > 10 && rnd < 15)
                                    Collectables.Add(new Collectable(Collectable.CollectableType.increase_width, brick.X, brick.Y));
                                else if (rnd > 15 && rnd < 20)
                                    Collectables.Add(new Collectable(Collectable.CollectableType.decrease_width, brick.X, brick.Y));
                                else if (rnd > 20 && rnd < 25)
                                    Collectables.Add(new Collectable(Collectable.CollectableType.decrease_speed, brick.X, brick.Y));
                                else if (rnd > 25 && rnd < 30)
                                    Collectables.Add(new Collectable(Collectable.CollectableType.add_life, brick.X, brick.Y));
                                else if(rnd > 30 && rnd < 35)
                                    Collectables.Add(new Collectable(Collectable.CollectableType.increase_damage, brick.X, brick.Y));
                                

                                brickRow.Remove(brick);

                                if (CurrentLevel.IsAllEmpty()) {
                                    NextLevel();
                                }
                            }
                            ball.Bounce(ref hitDirection);
                        }
                    }
                }
            }
        }

        private void IncreaseSpeed(double value)
        {
            foreach (var ball in Balls)
            {
                if (value < 0 && ball.Speed < 1)
                    continue;
                ball.Speed += value;
            }
        }

        private void RemoveLife() {
            CurrentPlayerStats.Lives -= 1;
            if (CurrentPlayerStats.Lives > 0)
            {
                Balls.Add(new PlayerBall(0, 0));
            }
            else {
                GameOver();
            }
        }

        public void ProcessKeyDown(KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;
            switch (e.Key)
            {
                case Key.Enter:
                    if (CurrentPlayerStats.GameState == PlayerStats.GameStates.game_over ||
                        CurrentPlayerStats.GameState == PlayerStats.GameStates.game_won)
                        StartNewGame();
                    break;

                case Key.Left:
                    PlayerPaddle.CurrentPlayerDirection = Paddle.PlayerDirection.left;
                    break;
                case Key.Right:
                    PlayerPaddle.CurrentPlayerDirection = Paddle.PlayerDirection.right;
                    break;
            }
        }

        public void ProcessKeyUp(KeyEventArgs e)
        {
            if (e.IsRepeat)
                return;
            switch (e.Key)
            {
                case Key.Up:
                case Key.Space:
                    foreach (var ball in Balls)
                    {
                        if (ball.IsPinned)
                        {
                            double x = ((double)_randomGenerator.Next(-10, 10)) / 10;
                            double y = ((double)_randomGenerator.Next(-10, -7)) / 10;
                            ball.Release(new Vector(x, y));
                        }
                    }
                    break;

                case Key.Left:
                    if (!Keyboard.IsKeyDown(Key.Right))
                        PlayerPaddle.CurrentPlayerDirection = Paddle.PlayerDirection.none;
                    break;
                case Key.Right:
                    if (!Keyboard.IsKeyDown(Key.Left))
                        PlayerPaddle.CurrentPlayerDirection = Paddle.PlayerDirection.none;
                    break;
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
