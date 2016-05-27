using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Pacman
{
    class UpdateStates
    {
        private static Timer timerMaze = new Timer(5000);
        private static Timer timerPowerup = new Timer(5000);
        private static Timer timerCatch = new Timer(2000);


        //Used when EnterName state is entered to pull the last name from the text file
        private static bool EnterName = true;

        //Used to reset the maze
        private static bool Maze = true;

        //Used when HighScore state is entered to pull the high scores from the text file
        private static bool HighScore = true;

        //Becomes true if player wins
        public static bool win = false;

        public static bool enterNameError = false;

        public static Timer TimerMaze
        {
            get { return timerMaze; }
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
                    PacmanGame.gameState = GameState.Maze; 
                if (Menu.QuitRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                    Program.game.Exit();
            }
        }

        public static void UpdateMaze(GameTime gameTime)
        {
            //reset enterNameError
            enterNameError = false;
            if (Maze)
            {
                Program.game.Start();
                Maze = false;
            }
            timerMaze.tick(gameTime);
            if (timerMaze.TimeMilliseconds >= timerMaze.Interval && Map.Ghosts.Count < 4)
            {
                Map.Ghosts.Add(new Ghost());
                timerMaze.reset();
            }

            foreach (Keys key in PacmanGame.keyboard.GetPressedKeys())
            {
                foreach (Paddle paddle in Map.Paddles)
                {
                    Direction d;
                    if (paddle.DirectionByKey.TryGetValue(key, out d))
                    {
                        paddle.Move(d);
                    }
                }
            }
               

            foreach (Paddle paddle in Map.Paddles)
            {
                paddle.CatchPacman();
            }

            if (PacmanGame.pacman.PlayerCaught != null)
            {
                timerCatch.tick(gameTime);
            }

            PacmanGame.pacman.oldMovementDirection = PacmanGame.pacman.movementDirection;
            if (PacmanGame.keyboard.IsKeyDown(Keys.Escape))
                Program.game.Exit();

            if (PacmanGame.pacman.currentControl == Player.Right)
            {
                if (PacmanGame.keyboard.IsKeyDown(Keys.Right) && PacmanGame.oldKeyboard.IsKeyUp(Keys.Right))
                    PacmanGame.pacman.changeDirectionRight();
                if (PacmanGame.keyboard.IsKeyDown(Keys.Up) && PacmanGame.oldKeyboard.IsKeyUp(Keys.Up))
                    PacmanGame.pacman.changeDirectionUp();
                if (PacmanGame.keyboard.IsKeyDown(Keys.Left) && PacmanGame.oldKeyboard.IsKeyUp(Keys.Left))
                    PacmanGame.pacman.changeDirectionLeft();
                if (PacmanGame.keyboard.IsKeyDown(Keys.Down) && PacmanGame.oldKeyboard.IsKeyUp(Keys.Down))
                    PacmanGame.pacman.changeDirectionDown();
            }
            else if (PacmanGame.pacman.currentControl == Player.Left)
            {
                if (PacmanGame.keyboard.IsKeyDown(Keys.D) && PacmanGame.oldKeyboard.IsKeyUp(Keys.D))
                    PacmanGame.pacman.changeDirectionRight();
                if (PacmanGame.keyboard.IsKeyDown(Keys.W) && PacmanGame.oldKeyboard.IsKeyUp(Keys.W))
                    PacmanGame.pacman.changeDirectionUp();
                if (PacmanGame.keyboard.IsKeyDown(Keys.A) && PacmanGame.oldKeyboard.IsKeyUp(Keys.A))
                    PacmanGame.pacman.changeDirectionLeft();
                if (PacmanGame.keyboard.IsKeyDown(Keys.S) && PacmanGame.oldKeyboard.IsKeyUp(Keys.S))
                    PacmanGame.pacman.changeDirectionDown();
            }

            if (timerCatch.TimeMilliseconds >= timerCatch.Interval)
            {
                if (PacmanGame.pacman.PlayerCaught == Player.Left)
                {
                    PacmanGame.pacman.PlayerCaught = null;
                    PacmanGame.pacman.changeDirectionRight();
                }
                else if (PacmanGame.pacman.PlayerCaught == Player.Right)
                {
                    PacmanGame.pacman.PlayerCaught = null;
                    PacmanGame.pacman.changeDirectionLeft();
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
    }
}
