using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The Invader that attacks the paddle
    public class Invader
    {
        private const int speed = 2;

        private Random random = new Random();
        public Rectangle Position { get; private set; }
        private Player side;

        public Invader(Player side)
        {
            this.side = side;
            if (side == Player.Left)
            {
                int positionX = random.Next(PacmanGame.horizontalSpace - 2 * PacmanGame.gridSize);
                Position = new Rectangle(positionX, -PacmanGame.gridSize, PacmanGame.gridSize, PacmanGame.gridSize);
            }
            else
            {
                int positionX = random.Next(PacmanGame.screenWidth - PacmanGame.horizontalSpace + PacmanGame.gridSize, PacmanGame.screenWidth - PacmanGame.gridSize);
                Position = new Rectangle(positionX, -PacmanGame.gridSize, PacmanGame.gridSize, PacmanGame.gridSize);
            }
        }

        public void Update()
        {
            move();
            CheckCollisionPaddles();
        }

        public void CheckCollisionPaddles()
        {
            if (side == Player.Left)
            {
                if (Position.Intersects(Map.Paddles[0].Position))
                {
                    Map.Paddles[0].Score -= 25;
                    Position = new Rectangle(0, PacmanGame.screenHeight, 0, 0);
                }
            }
            else
            {
                if (Position.Intersects(Map.Paddles[1].Position))
                {
                    Map.Paddles[1].Score -= 25;
                    Position = new Rectangle(0, PacmanGame.screenHeight, 0, 0);
                }
            }
        }

        public void move()
        {
            Position = new Rectangle(Position.X, Position.Y + speed, Position.Width, Position.Height);
        }
    }
}
