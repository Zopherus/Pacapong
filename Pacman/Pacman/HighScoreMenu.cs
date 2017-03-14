using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    /// <summary>
    /// High score menu (which simply displays a list of high scores to the player to player)
    /// </summary>
    class HighScoreMenu
    {
        //rectangle for drawing back button
        private static Rectangle backToMenuRectangle = new Rectangle(2 * PacmanGame.screenWidth / 3,  4 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        /// <summary>
        /// Rectangle for drawing back button
        /// </summary>
        public static Rectangle BackToMenuRectangle
        {
            get { return backToMenuRectangle; }
        }

        /// <summary>
        /// Read the high scores from the file
        /// </summary>
        /// <returns>List of high scores</returns>
        public static List<int> GetHighScores()
        {
            try
            {
                ///read from file and extract high scores as ints
                using (StreamReader reader = new StreamReader("HighScores.txt"))
                {
                    List<int> highScores = new List<int>(new int[] { 0, 0, 0, 0, 0 });
                    string fileRead = reader.ReadLine();
                    int iterator = 0;
                    while (fileRead != null)
                    {
                        highScores[iterator] = int.Parse(fileRead);
                        iterator++;
                        fileRead = reader.ReadLine();
                    }
                    return highScores;
                }
            }
            catch { }
            return null;
        }
    }
}
