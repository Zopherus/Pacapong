﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Menu
    {
        private static Rectangle playRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);
        private static Rectangle highScoreRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, 2 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);
        private static Rectangle helpRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, 3 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);
        private static Rectangle quitRectangle = new Rectangle(3 * PacmanGame.screenWidth / 8, 4 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        public static Rectangle PlayRectangle
        {
            get { return playRectangle; }
        }
    
        public static Rectangle HighScoreRectangle
        {
            get { return highScoreRectangle; }
        }

        public static Rectangle HelpRectangle
        {
            get { return helpRectangle; }
        }

        public static Rectangle QuitRectangle
        {
            get { return quitRectangle; }
        }
    }
}
