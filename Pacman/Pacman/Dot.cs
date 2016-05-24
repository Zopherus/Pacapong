using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The dots that Pacman picks up
    class Dot
    {
        private Rectangle position;

        public Dot(Rectangle position)
        {
            this.position = position;
        }

        public Rectangle Position
        {
            get { return position; } 
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            //If parameter cannot be cast to Rectangle return false
            Dot dot = obj as Dot;
            if ((System.Object)dot == null)
            {
                return false;
            }

            //Return true if the two rectangles match
            return (position == dot.position);
        }

        public bool Equals(Dot dot)
        {
            return position == dot.position;
        }
    }
}
