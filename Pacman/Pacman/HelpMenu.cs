using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class HelpMenu
    {
        private static Rectangle backToMenuRectangle = new Rectangle(2 * PacmanGame.screenWidth / 3, 4 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        private static string[] helpText = { "You are Pacman", "Go be great!" };

        public static Rectangle BackToMenuRectangle
        {
            get { return backToMenuRectangle; }
        }

        public static string[] HelpText
        {
            get { return helpText; }
        }
    }
}
