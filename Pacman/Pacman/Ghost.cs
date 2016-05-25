using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The Ghost that chases Pacman
    public class Ghost
    {
        private const int speed = 5; //Bugs happen when the speed doesn't divide the gridSize

        private Random random = new Random();
        private Rectangle position;
        private Direction oldDirection;

        public Ghost()
        {
            Random random = new Random();
            int index = random.Next(Map.Nodes.Count);
            position = Map.Nodes.ElementAt(index).Position;
        }
        
        public Rectangle Position
        {
            get { return position; }
        }

        public void move()
        {
            Node startingNode = nodeClosest(position);
            Node destinationNode = nodeClosest(PacmanGame.pacman.Position);
            LinkedList<Node> path = new LinkedList<Node>();
            Node pathNode = startingNode;
            while (pathNode.distanceFromSource > 0)
            {
                if (!path.Contains(pathNode))
                {
                    path.AddFirst(pathNode);
                }
                foreach (Node node in pathNode.adjacentNodes)
                {
                    if (node.distanceFromSource < pathNode.distanceFromSource)
                    {
                        pathNode = node;
                    }
                }
            }
            path.AddFirst(pathNode);
            if (path.Count > 1)
            {
                if (path.ElementAt<Node>(path.Count - 2).Position.Y < position.Y)
                {
                    moveUp();
                }
                else if (path.ElementAt<Node>(path.Count - 2).Position.Y > position.Y)
                {
                    moveDown();
                }
                if (path.ElementAt<Node>(path.Count - 2).Position.X < position.X)
                {
                    moveLeft();
                }
                else if (path.ElementAt<Node>(path.Count - 2).Position.X > position.X)
                {
                    moveRight();
                }
            }
        }

        //Used when in powerup game state
        public void moveOpposite()
        {
            int value = random.Next(4);
            switch(value)
            {
                case 0:
                    if (oldDirection != Direction.Down)
                        moveUp();
                    break;
                case 1:
                    if (oldDirection != Direction.Left)
                        moveRight();
                    break;
                case 2:
                    if (oldDirection != Direction.Right)
                        moveLeft();
                    break;
                case 3:
                    if (oldDirection != Direction.Up)
                        moveDown();
                    break;
            }
        }

        private void moveUp()
        {
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X, position.Y - speed, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.Y -= speed;
                oldDirection = Direction.Up;
            }
            else
            {
                position.Y = value.Item2.Bottom;
            }
        }

        private void moveRight()
        {
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X + speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.X += speed;
                oldDirection = Direction.Right;
            }
            else
            {
                position.X = value.Item2.Left - position.Width;
            }
        }

        private void moveDown()
        {
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X, position.Y + speed, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.Y += speed;
                oldDirection = Direction.Down;
            }
            else
            {
                position.Y = value.Item2.Top - position.Height;
            }
        }

        private void moveLeft()
        {
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X - speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.X -= speed;
                oldDirection = Direction.Left;
            }
            else
            {
                position.X = value.Item2.Right;
            }
        }

        //Used by the move methods to stop pacman from going through a wall
        //the bool value states if pacman intersects a wall or not
        //the rectangle value is the rectangle of the wall that it intersected
        private Tuple<bool, Rectangle> checkIntersectionWalls(Rectangle position)
        {
            foreach (Wall wall in Map.Walls)
            {
                if (wall.Position.Intersects(position))
                {
                    return new Tuple<bool, Rectangle>(false, wall.Position);
                }
            }
            return new Tuple<bool, Rectangle>(true, new Rectangle(0, 0, 0, 0));
        }

        //Finds the node closest to a certain rectangle
        private Node nodeClosest(Rectangle position)
        {
            double lowestDistance = 1000000000;
            int indexOfNode = 0;
            for (int counter = 0; counter < Map.Nodes.Count; counter++ )
            {
                if (Math.Pow(position.X - Map.Nodes[counter].Position.X, 2) + Math.Pow(position.Y - Map.Nodes[counter].Position.Y, 2) < lowestDistance)
                {
                    indexOfNode = counter;
                    lowestDistance = Math.Pow(position.X - Map.Nodes[counter].Position.X, 2) + Math.Pow(position.Y - Map.Nodes[counter].Position.Y, 2);
                }
            }
            return Map.Nodes[indexOfNode];
        }
    }
}
