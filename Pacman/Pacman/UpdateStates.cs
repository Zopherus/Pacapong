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

        private static int currentSelectedButton = 0;
        public static int CurrentSelectedButton { get { return currentSelectedButton; } } //make currentSelectedButton accessible for when drawing buttons

        private static List<Keys> previousKeysDown = new List<Keys>(); //list of keys previously down (to check for key clicks, not just presses)

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
            //keyboard functionality to menu
            if (!PacmanGame.keyboard.IsKeyDown(Keys.Down) && previousKeysDown.Contains(Keys.Down)) //only register press when releasing
            {
                currentSelectedButton += 1; //plus because buttons are numbered as decreasing going bottom -> up
                currentSelectedButton %= 4; //3 buttons, so reset back to zero every 3 presses
                previousKeysDown.Remove(Keys.Down);
            }
            if(PacmanGame.keyboard.IsKeyDown(Keys.Down) && !previousKeysDown.Contains(Keys.Down)) { previousKeysDown.Add(Keys.Down); }

            if (!PacmanGame.keyboard.IsKeyDown(Keys.Up) && previousKeysDown.Contains(Keys.Up)) //only register press when releasing
            {
                currentSelectedButton -= 1; //minus because buttons are numbered as increasing going top -> down
                if (currentSelectedButton == -1) { currentSelectedButton = 3; } //if it goes negative, bring it back to 3
                currentSelectedButton %= 4; //3 buttons, so reset back to zero every 3 presses
                previousKeysDown.Remove(Keys.Up);
            }
            if (PacmanGame.keyboard.IsKeyDown(Keys.Up) && !previousKeysDown.Contains(Keys.Up)) { previousKeysDown.Add(Keys.Up); }

            if (PacmanGame.keyboard.IsKeyDown(Keys.Enter))
            {
                switch (currentSelectedButton)
                {
                    case 1:
                        PacmanGame.gameState = GameState.Maze; //play button selected
                        currentSelectedButton = 0; //reset for later use
                        break;
                    case 2:
                        PacmanGame.gameState = GameState.HighScore; //high score button selected
                        currentSelectedButton = 0; //reset for later use
                        break;
                    case 3:
                        Program.game.Exit();
                        break;
                }
            }

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
            //keyboard functionality to high score screen
            if (PacmanGame.keyboard.IsKeyDown(Keys.Down)) { currentSelectedButton = 1; } //only 1 button, so can only equal 1
            if (PacmanGame.keyboard.IsKeyDown(Keys.Up)) { currentSelectedButton = 1; } //only 1 button, so can only equal 1
            if (PacmanGame.keyboard.IsKeyDown(Keys.Enter))
            {
                if(currentSelectedButton == 1)
                {
                    PacmanGame.gameState = GameState.Menu; //back button selected
                    currentSelectedButton = 0; //reset for later use
                }
            }
            if (PacmanGame.keyboard.IsKeyDown(Keys.Escape))
                PacmanGame.gameState = GameState.Menu;
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
                highScores.Reverse(); //reverse, since sort does it backwards
                highScores.RemoveAt(5); //remove the sixth element (we only want five)

                using (StreamWriter writer = new StreamWriter("HighScores.txt"))
                {
                    foreach (int i in highScores) //rewrite the list to file
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
}
