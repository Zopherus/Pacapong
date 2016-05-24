using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The walls that restrict pacman's movements
    class Wall
    {
        private Rectangle position;
        public Wall(Rectangle position)
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
                return false;

            //If parameter cannot be cast to Rectangle return false
            Wall wall = obj as Wall;
            if ((Object)wall == null)
                return false;

            //Return true if the two rectangles match
            return position == wall.position;
        }

        public bool Equals(Wall wall)
        {
            return position == wall.position;
        }
    }
}
