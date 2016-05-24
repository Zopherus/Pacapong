using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //Used in case I want to change the background squares to something different
    class EmptySquare
    {
        private Rectangle position;

        public EmptySquare(Rectangle position)
        {
            this.position = position;
        }

        public Rectangle Position
        {
            get { return position; }
        }
    }
}
