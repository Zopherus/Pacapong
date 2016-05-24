using System;

namespace Pacman
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static PacmanGame game;
        static void Main(string[] args)
        {
            using (game = new PacmanGame())
            {
                game.Run();
            }
        }
    }
#endif
}

