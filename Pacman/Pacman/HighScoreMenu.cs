using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class HighScoreMenu
    {
        private static Rectangle backToMenuRectangle = new Rectangle(2 * PacmanGame.screenWidth / 3,  4 * PacmanGame.screenHeight / 5, PacmanGame.screenWidth / 4, PacmanGame.screenHeight / 8);

        public static Rectangle BackToMenuRectangle
        {
            get { return backToMenuRectangle; }
        }

        public static List<int> GetHighScores()
        {
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
    }
}
