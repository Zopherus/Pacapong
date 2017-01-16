using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Pacman
{
    class UpdateStates
    {
        private static Timer timerGhost = new Timer(5000);
        private static Timer timerPowerup = new Timer(5000);
        public static Timer timerGame = new Timer(10000);
        public static Timer timerCatch = new Timer(1000);
        private static Timer timerNewGame = new Timer(5000);

        //Used to reset the maze
        private static bool Maze = true;

        //Becomes true if player wins
        public static bool win = false;

        public static bool enterNameError = false;

        public static Timer TimerMaze
        {
            get { return timerGhost; }
        }

        public static Timer TimerPowerup
        {
            get { return timerPowerup; }
        }

        public static void UpdateMenu()
        {
            if (PacmanGame.keyboard.IsKeyDown(Keys.Escape))
                Program.game.Exit();
            if (PacmanGame.mouse.LeftButton == ButtonState.Pressed)
            {
                if (Menu.PlayRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                {
                    PacmanGame.gameState = GameState.Maze;
                    
                }
                if (Menu.HighScoreRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                {
                    PacmanGame.gameState = GameState.HighScore;

                }
                if (Menu.QuitRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                    Program.game.Exit();
            }
        }

        public static void UpdateHighScoreMenu()
        {
            if (PacmanGame.mouse.LeftButton == ButtonState.Pressed)
            {
                if (HighScoreMenu.BackToMenuRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                {
                    PacmanGame.gameState = GameState.Menu;

                }
            }
        }

        public static void UpdateMaze(GameTime gameTime)
        {
            if (Maze)
            {
                if (PacmanGame.mapNumber == 0)
                    Program.game.Start();
                else
                {
                    UpdateStates.TimerMaze.reset();
                    PacmanGame.pacman = new Pacman(new Rectangle(0, 0, PacmanGame.gridSize, PacmanGame.gridSize));
                    PacmanGame.pacman.PlayerCaught = Player.Left;
                    Map.InitializeMap();
                }
                MediaPlayer.Stop();
                MediaPlayer.Play(PacmanGame.GameSong);
                Maze = false;
            }
            
            timerGhost.tick(gameTime);
            if (timerGhost.TimeMilliseconds >= timerGhost.Interval && Map.Ghosts.Count < 4)
            {
                Map.Ghosts.Add(new Ghost());
                timerGhost.reset();
            }
            timerGame.tick(gameTime);
            
            if (timerGame.TimeMilliseconds >= timerGame.Interval)
            {
                PacmanGame.gameState = GameState.GameEnd;
                MediaPlayer.Stop();
                PacmanGame.GameEndSound.Play();
                if (Map.Paddles[0].Score > Map.Paddles[1].Score)
                {
                    PacmanGame.mapNumber++;
                }
                else if (Map.Paddles[1].Score > Map.Paddles[0].Score)
                {
                    PacmanGame.pacmanLives--;
                }
                else
                {

                }
            }
            foreach (Keys key in PacmanGame.keyboard.GetPressedKeys())
            {
                Direction d;
                if (Map.Paddles[0].DirectionByKey.TryGetValue(key, out d))
                {
                    Map.Paddles[0].Move(d);
                    if (PacmanGame.pacman.currentControl == Map.Paddles[0].Player)
                    {
                        PacmanGame.pacman.changeDirection(d);
                    }
                }
            }
            Map.Paddles[1].MoveAI();
            if (PacmanGame.pacman.currentControl == Map.Paddles[1].Player)
            {
                PacmanGame.pacman.changeDirection((Direction)(new Random().Next(Enum.GetNames(typeof(Direction)).Length)));
            }
            foreach (Paddle paddle in Map.Paddles)
            {
                paddle.CatchPacman();
            }

            foreach (Invader invader in Map.Invaders)
            {
                invader.Update();
            }

            foreach (Shot shot in Map.Shots)
            {
                shot.Update();
            }

            if (PacmanGame.pacman.PlayerCaught != null)
            {
                timerCatch.tick(gameTime);
            }

            PacmanGame.pacman.oldMovementDirection = PacmanGame.pacman.movementDirection;
            if (PacmanGame.keyboard.IsKeyDown(Keys.Escape))
            {
                Program.game.Exit();
            }

            if (timerCatch.TimeMilliseconds >= timerCatch.Interval)
            {
                PacmanGame.pacman.PlayerCaught = null;
                if (PacmanGame.pacman.PlayerCaught == Player.Left)
                {
                    PacmanGame.pacman.changeDirection(Direction.Right);
                }
                else if (PacmanGame.pacman.PlayerCaught == Player.Right)
                {
                    PacmanGame.pacman.changeDirection(Direction.Left);
                }
                timerCatch.reset();
            }
            PacmanGame.pacman.update();
            if (!PacmanGame.pacman.IsPowerUp)
                PacmanGame.pacman.checkIntersectionGhost();
            else
                PacmanGame.pacman.checkIntersectionGhostPowerup();
            if (PacmanGame.pacman.IsPowerUp)
            {
                timerPowerup.tick(gameTime);
                if (timerPowerup.TimeMilliseconds >= timerPowerup.Interval)
                {
                    timerPowerup.reset();
                    PacmanGame.pacman.IsPowerUp = false;
                    PacmanGame.PowerDownSound.Play();
                }
            }



            if (!PacmanGame.pacman.IsPowerUp)
            {
                foreach (Ghost ghost in Map.Ghosts)
                {
                    ghost.move();
                }
            }
            else
            {
                foreach (Ghost ghost in Map.Ghosts)
                {
                    ghost.moveOpposite();
                }
            }
            foreach (Invader invader in Map.Invaders){ invader.move(); }
        }

        public static void UpdateGameEnd(GameTime gameTime)
        {
            timerNewGame.tick(gameTime);
            if (timerNewGame.TimeMilliseconds >= timerNewGame.Interval)
            {
                //save the high score to file if it's in the top 5
                int score = Map.Paddles[0].Score; //read the high score + list
                List<int> highScores = HighScoreMenu.GetHighScores();

                highScores.Add(score); //add the highscore to the list
                highScores.Sort();
                highScores.RemoveAt(5);

                StreamWriter writer = new StreamWriter("HighScores.txt"); //rewrite the list to file
                foreach (int i in highScores)
                {
                    writer.WriteLine(i.ToString());
                }

                Maze = true;
                timerGame.reset();
                timerNewGame.reset();
                PacmanGame.gameState = GameState.Maze;
            }
        }
    }
}
