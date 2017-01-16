﻿using System;
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
            PacmanGame.spriteBatch.Draw(PacmanGame.boxGreen, Menu.PlayRectangle, Color.White);
            PacmanGame.spriteBatch.Draw(PacmanGame.boxPink, Menu.QuitRectangle, Color.White);
            PacmanGame.spriteBatch.Draw(PacmanGame.boxPurple, Menu.HighScoreRectangle, Color.White);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Play", 
                new Vector2(Menu.PlayRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Play").X/2,
                    Menu.PlayRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Play").Y/2), Color.White);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "High Scores",
                new Vector2(Menu.HighScoreRectangle.Center.X - PacmanGame.spriteFont.MeasureString("High Scores").X / 2,
                    Menu.HighScoreRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("High Scores").Y / 2), Color.White);

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Quit",
                new Vector2(Menu.QuitRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Quit").X / 2,
                    Menu.QuitRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Quit").Y / 2), Color.White);
        }

        public static void DrawHighScoreMenu()
        {
            PacmanGame.spriteBatch.Draw(PacmanGame.boxPurple, HighScoreMenu.BackToMenuRectangle, Color.White);

            List<int> highScores = HighScoreMenu.GetHighScores(); //print the high scores
            for(int i = 0; i < 5; i++)
            {
                string stringToDraw = (i + 1).ToString() + ". " + highScores[i].ToString();
                PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, stringToDraw,
                new Vector2(PacmanGame.screenWidth / 2 - PacmanGame.spriteFont.MeasureString(stringToDraw).X / 2,
                    (i + 1) * PacmanGame.screenHeight / 7 - PacmanGame.spriteFont.MeasureString(stringToDraw).Y / 2), Color.White);
            }

            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, "Back",
                new Vector2(HighScoreMenu.BackToMenuRectangle.Center.X - PacmanGame.spriteFont.MeasureString("Back").X / 2,
                    HighScoreMenu.BackToMenuRectangle.Center.Y - PacmanGame.spriteFont.MeasureString("Back").Y / 2), Color.White);
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
            foreach (Paddle paddle in Map.Paddles)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.paddleBox, paddle.Position, Color.White);
            }
            foreach (Invader invader in Map.Invaders)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.InvaderTexture, invader.Position, Color.White);
            }
            foreach (Shot shot in Map.Shots)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.DotTexture, shot.position, Color.White);
            }
            PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTextures[(int)PacmanGame.pacman.movementDirection - 1, Convert.ToInt32(PacmanGame.pacman.IsMouthOpen)], PacmanGame.pacman.Position, Color.White);

           
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, Map.Paddles[0].Score.ToString(), new Vector2(0, 0), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, Map.Paddles[1].Score.ToString(), new Vector2(PacmanGame.screenWidth - PacmanGame.spriteFont.MeasureString(Map.Paddles[1].Score.ToString()).X, 0), Color.Black);
            PacmanGame.spriteBatch.DrawString(PacmanGame.spriteFont, ((UpdateStates.timerGame.Interval - UpdateStates.timerGame.TimeMilliseconds) / 1000).ToString(), new Vector2(PacmanGame.screenWidth / 2, 0), Color.Black);
            for (int i = 0; i < PacmanGame.pacmanLives; i++)
            {
                PacmanGame.spriteBatch.Draw(PacmanGame.PacmanTextures[0, 0], new Rectangle(i * 40, PacmanGame.screenHeight - 40, 40, 40), Color.White);
            }
        }
        
        public static void DrawGameEnd()
        {
            // Draw exact same thing as in maze, but with the winner
            DrawMaze();
            string display = "";
            if (Map.Paddles[0].Score > Map.Paddles[1].Score)
            {
                display = "You Win!";
            }
            else if (Map.Paddles[1].Score > Map.Paddles[0].Score)
            {
                display = "You Lose!";
            }
            else
            {
                display = "It's a Thai!";
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