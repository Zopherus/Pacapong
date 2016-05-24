using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //To be used in Queues for Ghost pathfinding
    class Node
    {
        public int distanceFromSource;
        public List<Node> adjacentNodes = new List<Node>();
        private int number;
        private Rectangle position;

        public Node(Rectangle position)
        {
            this.position = position;
        }

        public Node(int number, Rectangle position)
        {
            this.number = number;
            this.position = position;
        }

        public int Number
        {
            get { return number; }
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
            Node node = obj as Node;
            if ((Object)node == null)
                return false;

            //Return true if the two rectangles match
            return position == node.position;
        }

        public bool Equals(Node node)
        {
            return position == node.position;
        }
    }
}
