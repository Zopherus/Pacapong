using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //Maybe inherit off of class dot?
    class Powerup
    {
        private Rectangle position;

        public Powerup(Rectangle position)
        {
            this.position = position;
        }

        public Rectangle Position
        {
            get { return position; }
        }
    }
}
