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
            drawRectangleOutline(Menu.PlayRectangle);
            drawRectangleOutline(Menu.HighScoreRectangle);
            drawRectangleOutline(Menu.QuitRectangle);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Play", 
                new Vector2(Menu.PlayRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Play").X/2,
                    Menu.PlayRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Play").Y/2), Color.Black);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "High Scores",
                new Vector2(Menu.HighScoreRectangle.Center.X - PacmanGame.spriteFont.MeasureString("High Scores").X / 2,
                    Menu.HighScoreRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("High Scores").Y / 2), Color.Black);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Quit",
                new Vector2(Menu.QuitRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Quit").X / 2,
                    Menu.QuitRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Quit").Y / 2), Color.Black);
        }

        public static void DrawMaze()
        {
            foreach (Wall wall in Map.Walls)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.WallTexture, wall.Position, Color.White);
            }
            foreach (Dot dot in Map.Dots)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.DotTexture, dot.Position, Color.White);
            }
            foreach (Powerup powerup in Map.Powerups)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.PowerupTexture, powerup.Position, Color.White);
            }
            foreach (Ghost ghost in Map.Ghosts)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.GhostTexture, ghost.Position, Color.White);
            }
            foreach (Paddle paddle in Map.Paddles)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.GhostTexture, paddle.Position, Color.White);
            }
            PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTexture, PacmanGame.pacman.Position, Color.White);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, PacmanGame.pacman.Score.ToString(), new Vector2(0, 0), Color.White);
        }

        public static void DrawPowerup()
        {
            foreach (Wall wall in Map.Walls)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.WallTexture, wall.Position, Color.White);
            }
            foreach (Dot dot in Map.Dots)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.DotTexture, dot.Position, Color.White);
            }
            foreach (Powerup powerup in Map.Powerups)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.PowerupTexture, powerup.Position, Color.White);
            }
            foreach (Ghost ghost in Map.Ghosts)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.GhostPowerupTexture, ghost.Position, Color.White);
            }
            PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTexture, PacmanGame.pacman.Position, Color.White);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, PacmanGame.pacman.Score.ToString(), new Vector2(0, 0), Color.White);
        }

        public static void DrawEnterName()
        {
            string value = "Your Score:" + PacmanGame.pacman.Score.ToString();
            if (UpdateStates.win)
                value = "You Win !!!!! \n" + value;
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, value, new Vector2(0, 0), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Name:" + Highscore.currentName,
                new Vector2(0, PacmanGame.spriteFont.MeasureString(PacmanGame.pacman.Score.ToString()).Y + lineSpacing), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Enter to Retry. Tab for Menu",
                new Vector2(0, PacmanGame.spriteFont.MeasureString(PacmanGame.pacman.Score.ToString()).Y +
            PacmanGame.spriteFont.MeasureString("Name:").Y + 2 * lineSpacing), Color.Black);
            if (UpdateStates.enterNameError)
                PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Please enter in a name", 
                    new Vector2(0, PacmanGame.screenHeight - PacmanGame.spriteFont.MeasureString("Please enter in a name").Y), Color.Black);
        }

        public static void DrawHighScore()
        {
            for (int counter = 1; counter < Highscore.Scores.Length + 1; counter++)
            {
                if (Highscore.Scores[counter - 1] != null)
                {
                    PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, counter.ToString() + ". " + Highscore.Scores[counter - 1].ToString(),
                    new Vector2(0, (counter - 1) * lineSpacing), Color.Black);
                }
            }
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Tab To Go Back To Menu",
                new Vector2(0, PacmanGame.screenHeight - PacmanGame.spriteFont.MeasureString("Tab To Go Back To Menu").Y), Color.Black);
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