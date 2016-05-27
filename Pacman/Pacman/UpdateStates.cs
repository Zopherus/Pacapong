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
        public static Timer timerGame = new Timer(30000);
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
                if (Menu.QuitRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                    Program.game.Exit();
            }
        }

        public static void UpdateMaze(GameTime gameTime)
        {
            if (Maze)
            {
                Program.game.Start();
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
            }
            foreach (Keys key in PacmanGame.keyboard.GetPressedKeys())
            {
                foreach (Paddle paddle in Map.Paddles)
                {
                    Direction d;
                    if (paddle.DirectionByKey.TryGetValue(key, out d))
                    {
                        paddle.Move(d);
                        if (paddle.Player == PacmanGame.pacman.currentControl && PacmanGame.oldKeyboard.IsKeyUp(key))
                        {
                            PacmanGame.pacman.changeDirection(d);
                        }
                    }
                }
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
                Maze = true;
                timerGame.reset();
                timerNewGame.reset();
                PacmanGame.gameState = GameState.Maze;
            }
        }
    }
}
