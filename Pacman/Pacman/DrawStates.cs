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
            if (!PacmanGame.pacman.IsPowerUp)
            {
                foreach (Ghost ghost in Map.Ghosts)
                {
                    PacmanGame.spriteBatch.Draw(PacmanGame.GhostTexture, ghost.Position, Color.White);
                }
            }
            else
            {
                foreach (Ghost ghost in Map.Ghosts)
                {
                    PacmanGame.spriteBatch.Draw(PacmanGame.GhostPowerupTexture, ghost.Position, Color.White);
                }
            }
            foreach (Paddle paddle in Map.Paddles)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.BlackTexture, paddle.Position, Color.White);
            }
            PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTexture, PacmanGame.pacman.Position, Color.White);
            //PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, PacmanGame.pacman.Score.ToString(), new Vector2(0, 0), Color.White);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, Map.Paddles[0].Score.ToString(), new Vector2(0, 0), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, Map.Paddles[1].Score.ToString(), new Vector2(PacmanGame.screenWidth - PacmanGame.spriteFont.MeasureString(Map.Paddles[1].Score.ToString()).X, 0), Color.Black);
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