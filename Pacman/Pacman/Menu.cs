using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    /// <summary>
    /// Main menu screen
    /// </summary>
    class Menu
    {
        //rectangle for drawing play button
        private static Rectangle playRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        //rectangle for drawing high score button
        private static Rectangle highScoreRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, 2 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        //rectangle for drawing help button
        private static Rectangle helpRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, 3 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        //rectangle for drawing quit button
        private static Rectangle quitRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, 4 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        /// <summary>
        /// Rectangle for drawing play button
        /// </summary>
        public static Rectangle PlayRectangle
        {
            get { return playRectangle; }
        }
    
        /// <summary>
        /// Rectangle for drawing high score button
        /// </summary>
        public static Rectangle HighScoreRectangle
        {
            get { return highScoreRectangle; }
        }

        /// <summary>
        /// Rectangle for drawing help button
        /// </summary>
        public static Rectangle HelpRectangle
        {
            get { return helpRectangle; }
        }

        /// <summary>
        /// Rectangle for drawing quit button
        /// </summary>
        public static Rectangle QuitRectangle
        {
            get { return quitRectangle; }
        }
    }
}
