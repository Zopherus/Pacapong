using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    //The different draw things for certain game states
    class DrawStates
    {
        private const int lineSpacing = 20;

        public static void DrawMenu()
        {
            Color playColor = Color.White;
            Color quitColor = Color.White;
            Color highScoreColor = Color.White;
            Color helpColor = Color.White;
            switch(UpdateStates.CurrentSelectedButton) //change color of button to denote currently selected button (for keyboard controls)
            {
                case 1:
                    playColor = Color.Red;
                    break;
                case 2:
                    highScoreColor = Color.Red;
                    break;
                case 3:
                    helpColor = Color.Red;
                    break;
                case 4:
                    quitColor = Color.Red;
                    break;
            }

            PacmanGame.spriteBatch.Draw(PacmanGame.boxGreen, Menu.PlayRectangle, playColor);
            PacmanGame.spriteBatch.Draw(PacmanGame.boxPink, Menu.QuitRectangle, quitColor);
            PacmanGame.spriteBatch.Draw(PacmanGame.boxYellow, Menu.HelpRectangle, helpColor);
            PacmanGame.spriteBatch.Draw(PacmanGame.boxPurple, Menu.HighScoreRectangle, highScoreColor);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Play", 
                new Vector2(Menu.PlayRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Play").X/2,
                    Menu.PlayRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Play").Y/2), playColor);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "High Scores",
                new Vector2(Menu.HighScoreRectangle.Center.X - PacmanGame.spriteFont.MeasureString("High Scores").X / 2,
                    Menu.HighScoreRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("High Scores").Y / 2), highScoreColor);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Help",
                new Vector2(Menu.HelpRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Help").X / 2,
                    Menu.HelpRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Help").Y / 2), helpColor);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Quit",
                new Vector2(Menu.QuitRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Quit").X / 2,
                    Menu.QuitRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Quit").Y / 2), quitColor);
        }

        public static void DrawHighScore()
        {
            Color buttonColor = Color.White;
            if(UpdateStates.CurrentSelectedButton == 1) //change color of button to denote currently selected button (for keyboard controls)
            {
                buttonColor = Color.Red;
            }

            PacmanGame.spriteBatch.Draw(PacmanGame.boxPurple, HighScoreMenu.BackToMenuRectangle, buttonColor);

            List<int> highScores = HighScoreMenu.GetHighScores(); //print the high scores
            for(int i = 0; i < 5; i++)
            {
                string stringToDraw = (i + 1).ToString() + ". " + highScores[i].ToString();
                PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, stringToDraw,
                new Vector2(PacmanGame.screenWidth / 2 - PacmanGame.spriteFont.MeasureString(stringToDraw).X / 2,
                    (i + 1) * PacmanGame.screenHeight / 7 - PacmanGame.spriteFont.MeasureString(stringToDraw).Y / 2), Color.Black);
            }

            //draw the back button
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Back",
                new Vector2(HighScoreMenu.BackToMenuRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Back").X / 2,
                    HighScoreMenu.BackToMenuRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Back").Y / 2), buttonColor);
        }

        public static void DrawHelp()
        {
            Color buttonColor = Color.White;
            if (UpdateStates.CurrentSelectedButton == 1) //change color of button to denote currently selected button (for keyboard controls)
            {
                buttonColor = Color.Red;
            }

            PacmanGame.spriteBatch.Draw(PacmanGame.boxPurple, HighScoreMenu.BackToMenuRectangle, buttonColor);

            int numLines = HelpMenu.HelpText.Length; //number of lines of text to display
            for (int i = 0; i < HelpMenu.HelpText.Length; i++) 
            {
                string lineToDisplay = HelpMenu.HelpText[i]; //line of help text to draw

                //scrren height + offset
                float textHeight = (PacmanGame.screenHeight / 4) + (i * PacmanGame.spriteFont.MeasureString(lineToDisplay).Y * 1.5f);
                PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, lineToDisplay,
                new Vector2(PacmanGame.screenWidth / 2 - PacmanGame.spriteFont.MeasureString(lineToDisplay).X / 2,
                   textHeight - PacmanGame.spriteFont.MeasureString(lineToDisplay).Y / 2), Color.Black);
            }

            //draw the back button
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Back",
                new Vector2(HighScoreMenu.BackToMenuRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Back").X / 2,
                    HighScoreMenu.BackToMenuRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Back").Y / 2), buttonColor);
        }

        /// <summary>
        /// Draws the pacman maze (including walls, dots, powerups, ghosts, etc)
        /// </summary>
        public static void DrawMaze()
        {
            //draw walls
            foreach (Wall wall in Map.Walls)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.WallTexture, wall.Position, Color.White);
            }

            //draw dots
            foreach (Dot dot in Map.Dots)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.DotTexture, dot.Position, Color.White);
            }

            //draw powerups
            foreach (Powerup powerup in Map.Powerups)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.PowerupTexture, powerup.Position, Color.White);
            }

            //draw ghosts
            if (!PacmanGame.pacman.IsPowerUp)
            {
                foreach (Ghost ghost in Map.Ghosts)
                {
                    PacmanGame.spriteBatch.Draw(ghost.Texture, ghost.Position, Color.White);
                }
            }
            else
            {
                foreach (Ghost ghost in Map.Ghosts)
                {
                    PacmanGame.spriteBatch.Draw(PacmanGame.GhostPowerupTexture, ghost.Position, Color.White);
                }
            }

            //draw paddles
            foreach (Paddle paddle in Map.Paddles)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.paddleBox, paddle.Position, Color.White);
            }
            
            //draw invaders
            foreach (Invader invader in Map.Invaders)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.InvaderTexture, invader.Position, Color.White);
            }

            //draw shots
            foreach (Shot shot in Map.Shots)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.DotTexture, shot.position, Color.White);
            }
            PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTextures[(int)PacmanGame.pacman.movementDirection - 1, Convert.ToInt32(PacmanGame.pacman.IsMouthOpen)], PacmanGame.pacman.Position, Color.White);

           //draw stings (level, etc.)
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, Map.Paddles[0].Score.ToString(), new Vector2(0, 0), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, Map.Paddles[1].Score.ToString(), new Vector2(PacmanGame.screenWidth - PacmanGame.spriteFont.MeasureString(Map.Paddles[1].Score.ToString()).X, 0), Color.Black);
            string message = "Level: " + (PacmanGame.mapNumber + 1).ToString();
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, message, new Vector2(PacmanGame.screenWidth - PacmanGame.spriteFont.MeasureString(message).X, 
                PacmanGame.screenHeight - PacmanGame.spriteFont.MeasureString(message).Y), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, ((UpdateStates.timerGame.Interval - UpdateStates.timerGame.TimeMilliseconds) / 1000).ToString(), new Vector2(PacmanGame.screenWidth / 2, 0), Color.Black);
            for (int i = 0; i < PacmanGame.pacmanLives; i++)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTextures[0, 0], new Rectangle(i * 40, PacmanGame.screenHeight - 40, 40, 40), Color.White);
            }
        }
        
        /// <summary>
        /// Draw the end level state)
        /// </summary>
        public static void DrawLevelEnd()
        {
            // Draw exact same thing as in maze, but with the winner
            DrawMaze();
            string display = "";
            if (Map.Paddles[0].Score >= Map.Paddles[1].Score)
            {
                display = "You beat the level!";
            }
            else if (Map.Paddles[1].Score > Map.Paddles[0].Score)
            {
                display = "You couldn't beat the level!";
            }

            float stringHeight = PacmanGame.spriteFont.MeasureString(display).Y;
            float stringWidth = PacmanGame.spriteFont.MeasureString(display).X;

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, display, new Vector2((PacmanGame.screenWidth - stringWidth) / 2, PacmanGame.screenHeight - stringHeight), Color.Black);
        }

        /// <summary>
        /// Draw th end game state)
        /// </summary>
        public static void DrawGameEnd()
        {
            string display = "";
            if (PacmanGame.mapNumber > 3)
            {
                display = "You Won!";
            }
            else if (PacmanGame.pacmanLives <= 0)
            {
                display = "You Lost. Try again.";
            }

            float stringHeight = PacmanGame.spriteFont.MeasureString(display).Y;
            float stringWidth = PacmanGame.spriteFont.MeasureString(display).X;

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, display, new Vector2((PacmanGame.screenWidth - stringWidth) / 2, PacmanGame.screenHeight - stringHeight), Color.Black);
        }

        //Used to draw just the outline of a rectangle
        private static void drawRectangleOutline(Rectangle rectangle)
        {
            PacmanGame.spriteBatch.Draw(PacmanGame.BlackTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), Color.Black);
            PacmanGame.spriteBatch.Draw(PacmanGame.BlackTexture, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), Color.Black);
            PacmanGame.spriteBatch.Draw(PacmanGame.BlackTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height + 1), Color.Black);
            PacmanGame.spriteBatch.Draw(PacmanGame.BlackTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1), Color.Black);
        }
    }
}