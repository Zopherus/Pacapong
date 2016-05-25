using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The character that the player uses
    public class Pacman
    {
        private const int speed = 6; //Bugs happen if the speed doesn't divide the gridSize

        public Direction? oldMovementDirection;
        public Direction? movementDirection;
        private int score;
        private double oldDistanceMoved;
        private double distanceMoved;
        //Used to continue the pacman in the direction already going
        private Rectangle oldPosition;
        private Rectangle position;
        //If player pressed down and cannot go down because of a wall
        //tryingDirection will be used to make the pacman turn downwards as soon as possible
        private Direction? tryingDirection;
        private Queue<Node> nodeQueue = new Queue<Node>();


        private Player currentControl = Player.Left;

        public Pacman() { }
        public Pacman(Rectangle position)
        {
            this.position = position;
        }

        public int Score
        {
            get { return score; }
        }

        public Rectangle Position
        {
            get { return position; }
        }
        
        public double DistanceMoved
        {
            get { return distanceMoved; }
        }

        public double OldDistanceMoved
        {
            get { return oldDistanceMoved; }
        }


        public void update()
        {
            oldDistanceMoved = distanceMoved;
            calculateDistanceMoved();
            oldPosition = position;
            clearDirection();
            moveOppositeDirection();
            move();
            checkIntersectionDots();
            checkIntersectionPowerup();
            moveTryingDirection();
            labelDistanceNodes();
        }
        //Checks if the pacman intersects with a ghost
        public void checkIntersectionGhost()
        {
            foreach (Ghost ghost in Map.Ghosts)
            {
                if (ghost.Position.Intersects(position))
                {
                    PacmanGame.gameState = GameState.EnterName;
                }
            }
        }

        //Kills ghost during powerup
        public void checkIntersectionGhostPowerup()
        {
            foreach (Ghost ghost in Map.Ghosts)
            {
                if (position.Intersects(ghost.Position))
                {
                    Map.Ghosts.Remove(ghost);
                    score += 200;
                    break;
                }
            }
        }

        private void move()
        {
            switch(movementDirection)
            {
                case Direction.Up:
                    moveUp();
                    break;
                case Direction.Right:
                    moveRight();
                    break;
                case Direction.Down:
                    moveDown();
                    break;
                case Direction.Left:
                    moveLeft();
                    break;
            }
        }
                                                    
        //stops the pacman from going through walls
        //sets the pacman immediately adjacent to the wall it ran into
        private void moveUp()
        {
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X, position.Y - speed, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.Y -= speed;
                movementDirection = Direction.Up;
            }
            else
            {
                movementDirection = oldMovementDirection;
                tryingDirection = Direction.Up;
                position.Y = value.Item2.Bottom;
            }
        }
        
        private void moveRight()
        {
            if (currentControl == Player.Right)
                return;
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X + speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.X += speed;
                movementDirection = Direction.Right;
            }
            else
            {
                movementDirection = oldMovementDirection;
                tryingDirection = Direction.Right;
                position.X = value.Item2.Left - position.Width;
            }
        }

        private void moveDown()
        {
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X, position.Y + speed, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.Y += speed;
                movementDirection = Direction.Down;
            }
            else
            {
                movementDirection = oldMovementDirection;
                tryingDirection = Direction.Down;
                position.Y = value.Item2.Top - position.Height;
            }
        }

        private void moveLeft()
        {
            if (currentControl == Player.Left)
                return;
            Tuple<bool, Rectangle> value = checkIntersectionWalls(new Rectangle(position.X - speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize));
            if (value.Item1)
            {
                position.X -= speed;
                movementDirection = Direction.Left;
            }
            else
            {
                movementDirection = oldMovementDirection;
                tryingDirection = Direction.Left;
                position.X = value.Item2.Right;
            }
        }

        //Used by the move methods to stop pacman from going through a wall
        //the bool value states if pacman intersects a wall or not
        //the rectangle value is the rectangle of the wall that it intersected
        private Tuple<bool,Rectangle> checkIntersectionWalls(Rectangle position)
        {
            foreach (Wall wall in Map.Walls)
            {
                if (wall.Position.Intersects(position))
                {
                    return new Tuple<bool, Rectangle>(false, wall.Position);
                }
            }
            return new Tuple<bool,Rectangle>(true, new Rectangle(0,0,0,0));
        }

        // Used to change the movement direction of the pacman
        public void changeDirectionUp()
        {
            movementDirection = Direction.Up;
        }

        public void changeDirectionRight()
        {
            if (currentControl == Player.Right)
                return;
            movementDirection = Direction.Right;
        }

        public void changeDirectionDown()
        {
            movementDirection = Direction.Down;
        }

        public void changeDirectionLeft()
        {
            if (currentControl == Player.Left)
                return;
            movementDirection = Direction.Left;
        }

        //Used to remove the dots when pacman intersects them
        private void checkIntersectionDots()
        {
            foreach (Dot dot in Map.Dots)
            {
                if (dot.Position.Intersects(position))
                {
                    Map.Dots.Remove(dot);
                    score += 10;
                    return;
                }
            }
        }

        //Changes the gamestate to powerup
        private void checkIntersectionPowerup()
        {
            foreach (Powerup powerup in Map.Powerups)
            {
                if (powerup.Position.Intersects(position))
                {
                    Map.Powerups.Remove(powerup);
                    score += 50;
                    PacmanGame.gameState = GameState.Powerup;
                    return;
                }
            }
        }

        private void calculateDistanceMoved()
        {
            distanceMoved = Math.Pow(Math.Pow(position.X - oldPosition.X, 2) + Math.Pow(position.Y - oldPosition.Y, 2), 0.5);
        }

        //Attempt to move in the tryingDirection
        private void moveTryingDirection()
        {
            switch (tryingDirection)
            {
                case Direction.Up:
                    if (checkIntersectionWalls(new Rectangle(position.X, position.Y - speed, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        movementDirection = Direction.Up;
                        tryingDirection = null;
                    }
                    break;
                case Direction.Right:
                    if (checkIntersectionWalls(new Rectangle(position.X + speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        movementDirection = Direction.Right;
                        tryingDirection = null;
                    }
                    break;
                case Direction.Down:
                    if (checkIntersectionWalls(new Rectangle(position.X, position.Y + speed, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        movementDirection = Direction.Down;
                        tryingDirection = null;
                    }
                    break;
                case Direction.Left:
                    if (checkIntersectionWalls(new Rectangle(position.X - speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        movementDirection = Direction.Left;
                        tryingDirection = null;
                    }
                    break;
            }
        }

        //Clears both directions if player is stopped at a wall
        private void clearDirection()
        {
            if (distanceMoved == 0 && oldDistanceMoved == 0)
            {
                tryingDirection = null;
            }
        }

        //Allows the player to move in the direction perpendicular to a wall immediately after touching it
        private void moveOppositeDirection()
        {
            if ((movementDirection == Direction.Up && tryingDirection == Direction.Down) ||
                (movementDirection == Direction.Right && tryingDirection == Direction.Left) ||
                (movementDirection == Direction.Left && tryingDirection == Direction.Right) ||
                (movementDirection == Direction.Down && tryingDirection == Direction.Up))
            {
                tryingDirection = null;
            }
        }

        private void labelDistanceNodes()
        {
            List<Node> unvisitedNodes = new List<Node>(Map.Nodes);
            nodeQueue.Clear();
            Node startingNode = nodeClosest(PacmanGame.pacman.Position);
            Node destinationNode = nodeClosest(position);
            foreach (Node node in unvisitedNodes)
            {
                node.distanceFromSource = 100; //Infinite Distance
            }
            startingNode.distanceFromSource = 0;
            nodeQueue.Enqueue(startingNode);
            while (unvisitedNodes.Count > 0)
            {
                Node nodePosition = nodeQueue.Dequeue();
                unvisitedNodes.Remove(nodePosition);
                foreach (Node node in nodePosition.adjacentNodes)
                {
                    if (nodePosition.distanceFromSource + 1 < node.distanceFromSource)
                    {
                        node.distanceFromSource = nodePosition.distanceFromSource + 1;
                    }
                    if (unvisitedNodes.Contains(node))
                    {
                        nodeQueue.Enqueue(node);
                    }
                }
            }
        }

        private Node nodeClosest(Rectangle position)
        {
            double lowestDistance = 1000000000;
            int indexOfNode = 0;
            for (int counter = 0; counter < Map.Nodes.Count; counter++)
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
