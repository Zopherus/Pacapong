using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    /// <summary>
    /// Help menu (which simply displays help text to player)
    /// </summary>
    class HelpMenu
    {
        //rectangle for back button
        private static Rectangle backToMenuRectangle = new Rectangle(2 * PacmanGame.screenWidth / 3, 4 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        //text to display on help screen
        private static string[] helpText = { "Welcome Player! Take control of the paddle on the left and move it",
                                             "using the 'Up' and 'Down' arrow keys! Pacman will be launched on",
                                             "his own, after which you can direct his movement with the 'Up', ",
                                             "'Down', and 'Right' arrow keys.",
                                             "", //line break in help text
                                             "Have pacman collect more black dots than Player 2 before time ",
                                             "runs out to win! Can you beat all 3 levels?" };
        /// <summary>
        /// Rectangle for back button
        /// </summary>
        public static Rectangle BackToMenuRectangle
        {
            get { return backToMenuRectangle; }
        }

        /// <summary>
        /// Text to display on help screen
        /// </summary>
        public static string[] HelpText
        {
            get { return helpText; }
        }
    }
}
