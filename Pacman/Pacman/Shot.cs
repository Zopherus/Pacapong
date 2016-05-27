using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Shot
    {
        public const int speed = 5;

        public Rectangle position;
        private Player side;

        public Shot(Player side)
        {
            this.side = side;
            if (side == Player.Left)
            {
                position = new Rectangle(Map.Paddles[0].Position.Center.X, Map.Paddles[0].Position.Y, PacmanGame.gridSize / 2, PacmanGame.gridSize / 2);
            }
            else
            {
                position = new Rectangle(Map.Paddles[1].Position.Center.X, Map.Paddles[1].Position.Y, PacmanGame.gridSize / 2, PacmanGame.gridSize / 2);
            }
        }

        public void Update()
        {
            Move();
            CheckIntersectionInvaders();
        }

        public void CheckIntersectionInvaders()
        {
            for (int counter = 0; counter < Map.Invaders.Count; counter++)
            {
                if (position.Intersects(Map.Invaders[counter].Position))
                {
                    Map.Invaders.RemoveAt(counter);
                    position.X = -100;
                    if (side == Player.Left)
                    {
                        Map.Paddles[0].Score += 25;
                    }
                    else
                    {
                        Map.Paddles[1].Score += 25;
                    }
                    return;
                }
            }
        }

        public void Move()
        {
            position.Y -= speed;
        }
    }
}
