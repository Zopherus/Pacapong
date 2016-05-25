using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Paddle
    {
        public const int speed = 5;
        public Rectangle Position { get; private set; }
        
        public Paddle(Rectangle Position)
        {
            this.Position = Position;
        }

        public void MoveUp()
        {
            Position = new Rectangle(Position.X, Position.Y - 5, Position.Width, Position.Height);
        }

        public void MoveRight()
        {
            Position = new Rectangle(Position.X - 5, Position.Y, Position.Width, Position.Height);
        }

        public void MoveDown()
        {
            Position = new Rectangle(Position.X, Position.Y + 5, Position.Width, Position.Height);
        }

        public void MoveLeft()
        {
            Position = new Rectangle(Position.X + 5, Position.Y, Position.Width, Position.Height);
        }
    }
}
