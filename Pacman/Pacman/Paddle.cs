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
        public Rectangle Rectangle { get; private set; }
        
        public void MoveUp()
        {
            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y - 5, Rectangle.Width, Rectangle.Height);
        }

        public void MoveRight()
        {
            Rectangle = new Rectangle(Rectangle.X - 5, Rectangle.Y, Rectangle.Width, Rectangle.Height);
        }

        public void MoveDown()
        {
            Rectangle = new Rectangle(Rectangle.X, Rectangle.Y + 5, Rectangle.Width, Rectangle.Height);
        }

        public void MoveLeft()
        {
            Rectangle = new Rectangle(Rectangle.X + 5, Rectangle.Y, Rectangle.Width, Rectangle.Height);
        }
    }
}
