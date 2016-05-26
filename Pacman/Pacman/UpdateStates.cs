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
        private static Timer timerMaze = new Timer(1000);
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
                if (Menu.HighScoreRectangle.Contains(new Point(PacmanGame.mouse.X, PacmanGame.mouse.Y)))
                {
                    PacmanGame.gameState = GameState.HighScore;
                    HighScore = true;
                }
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
            if (timerMaze.TimeMilliseconds >= timerMaze.Interval && Map.Ghosts.Count < 5)
            {
                Map.Ghosts.Add(new Ghost());
                timerMaze.reset();
            }

            if (PacmanGame.keyboard.IsKeyDown(Keys.W))
                Map.Paddles[0].MoveUp();
            if (PacmanGame.keyboard.IsKeyDown(Keys.D))
                Map.Paddles[0].MoveRight();
            if (PacmanGame.keyboard.IsKeyDown(Keys.S))
                Map.Paddles[0].MoveDown();
            if (PacmanGame.keyboard.IsKeyDown(Keys.A))
                Map.Paddles[0].MoveLeft();


            if (PacmanGame.keyboard.IsKeyDown(Keys.Up))
                Map.Paddles[1].MoveUp();
            if (PacmanGame.keyboard.IsKeyDown(Keys.Right))
                Map.Paddles[1].MoveRight();
            if (PacmanGame.keyboard.IsKeyDown(Keys.Down))
                Map.Paddles[1].MoveDown();
            if (PacmanGame.keyboard.IsKeyDown(Keys.Left))
                Map.Paddles[1].MoveLeft();

            foreach (Paddle paddle in Map.Paddles)
            {
                paddle.CatchPacman();
            }
            if (PacmanGame.pacman.PlayerCaught != null)
            {
                timerCatch.tick(gameTime);
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
                timerMaze.reset();
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
        }
        public static void UpdateEnterName()
        {
            //Reset maze value to true so next time maze start it is reset
            Maze = true;
            if (EnterName)
            {
                using (StreamReader sr = new StreamReader("Content/Text Files/Name.txt"))
                {
                    string line = sr.ReadLine();
                    if (line == null)
                        Highscore.currentName = "";
                    else
                        Highscore.currentName = line;
                }
                EnterName = false;
            }
            if (Highscore.currentName.Length < 20) 
            {
                foreach (Keys key in PacmanGame.keyboard.GetPressedKeys())
                {
                    Console.WriteLine(key.ToString());
                    if (key >= Keys.A && key <= Keys.Z && PacmanGame.oldKeyboard.IsKeyUp(key))
                    {
                        Highscore.currentName += key.ToString();
                    }
                    if (key == Keys.Tab)
                    {
                        if (Highscore.currentName.Trim() == "")
                        {
                            enterNameError = true;
                        }
                        else
                        {
                            using (StreamWriter sw = new StreamWriter("Content/Text Files/Name.txt"))
                            {
                                sw.WriteLine(Highscore.currentName);
                            }
                            Highscore.addScore(new Score(Highscore.currentName.Trim(), PacmanGame.pacman.Score));
                            PacmanGame.gameState = GameState.Menu;
                            HighScore = true;
                            EnterName = true;
                        }
                    }
                    if (key == Keys.Enter)
                    {
                        if (Highscore.currentName.Trim() == "")
                        {
                            enterNameError = true;
                        }
                        else
                        {
                            using (StreamWriter sw = new StreamWriter("Content/Text Files/Name.txt"))
                            {
                                sw.WriteLine(Highscore.currentName);
                            }
                            Highscore.addScore(new Score(Highscore.currentName.Trim(), PacmanGame.pacman.Score));
                            PacmanGame.gameState = GameState.Maze;
                        }
                    }
                    /*if (PacmanGame.oldKeyboard.IsKeyUp(key))
                    {
                        switch (key)
                        {
                            case Keys.A:
                                Highscore.currentName += Keys.A.ToString();
                                Highscore.currentName += "A";
                                break;
                            case Keys.B:
                                Highscore.currentName += "B";
                                break;
                            case Keys.C:
                                Highscore.currentName += "C";
                                break;
                            case Keys.D:
                                Highscore.currentName += "D";
                                break;
                            case Keys.E:
                                Highscore.currentName += "E";
                                break;
                            case Keys.F:
                                Highscore.currentName += "F";
                                break;
                            case Keys.G:
                                Highscore.currentName += "G";
                                break;
                            case Keys.H:
                                Highscore.currentName += "H";
                                break;
                            case Keys.I:
                                Highscore.currentName += "I";
                                break;
                            case Keys.J:
                                Highscore.currentName += "J";
                                break;
                            case Keys.K:
                                Highscore.currentName += "K";
                                break;
                            case Keys.L:
                                Highscore.currentName += "L";
                                break;
                            case Keys.M:
                                Highscore.currentName += "M";
                                break;
                            case Keys.N:
                                Highscore.currentName += "N";
                                break;
                            case Keys.O:
                                Highscore.currentName += "O";
                                break;
                            case Keys.P:
                                Highscore.currentName += "P";
                                break;
                            case Keys.Q:
                                Highscore.currentName += "Q";
                                break;
                            case Keys.R:
                                Highscore.currentName += "R";
                                break;
                            case Keys.S:
                                Highscore.currentName += "S";
                                break;
                            case Keys.T:
                                Highscore.currentName += "T";
                                break;
                            case Keys.U:
                                Highscore.currentName += "U";
                                break;
                            case Keys.V:
                                Highscore.currentName += "V";
                                break;
                            case Keys.W:
                                Highscore.currentName += "W";
                                break;
                            case Keys.X:
                                Highscore.currentName += "X";
                                break;
                            case Keys.Y:
                                Highscore.currentName += "Y";
                                break;
                            case Keys.Z:
                                Highscore.currentName += "Z";
                                break;
                            case Keys.Space:
                                Highscore.currentName += " ";
                                break;
                            case Keys.Tab:
                                if (Highscore.currentName.Trim() == "")
                                {
                                    enterNameError = true;
                                }
                                else
                                {
                                    using (StreamWriter sw = new StreamWriter("Content/Text Files/Name.txt"))
                                    {
                                        sw.WriteLine(Highscore.currentName);
                                        sw.Close();
                                    }
                                    Highscore.addScore(new Score(Highscore.currentName.Trim(), PacmanGame.pacman.Score));
                                    PacmanGame.gameState = GameState.Menu;
                                    HighScore = true;
                                    EnterName = true;
                                }
                                break;
                            case Keys.Enter:
                                if (Highscore.currentName.Trim() == "")
                                {
                                    enterNameError = true;
                                }
                                else
                                {
                                    using (StreamWriter sw = new StreamWriter("Content/Text Files/Name.txt"))
                                    {
                                        sw.WriteLine(Highscore.currentName);
                                        sw.Close();
                                    }
                                    Highscore.addScore(new Score(Highscore.currentName.Trim(), PacmanGame.pacman.Score));
                                    PacmanGame.gameState = GameState.Maze;
                                }
                                break;
                        }
                    }*/
                }
            }
            if (PacmanGame.keyboard.IsKeyDown(Keys.Back))
            {
                if (Highscore.currentName.Length > 0)
                    Highscore.currentName = Highscore.currentName.Remove(Highscore.currentName.Length - 1);
            }
        }

        public static void UpdateHighScore()
        {
            if (HighScore)
            {
                Highscore.ReadFromFile();
                HighScore = false;
            }
            if (PacmanGame.keyboard.IsKeyDown(Keys.Tab))
                PacmanGame.gameState = GameState.Menu;
        }
    }
}
