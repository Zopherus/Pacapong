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
        public static int speed = 6; //Bugs happen if the speed doesn't divide the gridSize

        public Direction? oldMovementDirection;
        public Direction? movementDirection = Direction.Right;
        private double oldDistanceMoved;
        private double distanceMoved;
        //Used to continue the pacman in the direction already going
        private Rectangle oldPosition;
        private Rectangle position;
        //If player pressed down and cannot go down because of a wall
        //tryingDirection will be used to make the pacman turn downwards as soon as possible
        private Direction? tryingDirection;
        
        // Which paddle, if any, the pacman is currently caught by
        public Player? PlayerCaught;

        // After pacman is caught, how far from the paddle's top, is this top
        // Ensures that the pacman stays in the same position on the paddle until is shot
        public int CatchDifferential;


        public bool IsMouthOpen = false;
        private int FrameCounter;

        private Queue<Node> nodeQueue = new Queue<Node>();
        public bool IsPowerUp = false;

        public Player currentControl;

        public Pacman() {}
        
        public Pacman(Rectangle position)
        {
            this.position = position;
        }

        public Rectangle Position
        {
            get { return position; }
        }

        public void update()
        {
            // Uses int division to round the y position down to the nearest multiple of the speed to ensure it lines up with the grid
            position.Y = (position.Y / speed) * speed; 
            oldDistanceMoved = distanceMoved;
            calculateDistanceMoved();
            oldPosition = position;
            if (distanceMoved == 0 && oldDistanceMoved == 0)
            {
                movementDirection = ((Direction)(new Random().Next(1, 5))); // If player is stopped at a wall, pacman picks another direction
            }
            moveOppositeDirection();
            move();
            checkIntersectionDots();
            checkIntersectionPowerup();


             // If the pacman is outside the bounds on the maze, always try to move towards it
            if (position.X < PacmanGame.horizontalSpace + PacmanGame.gridSize)
            {
                if (currentControl == Player.Left)
                    tryingDirection = Direction.Right;
                else
                    tryingDirection = Direction.Left;
            }
            if (position.X > PacmanGame.screenWidth - PacmanGame.horizontalSpace - PacmanGame.gridSize)
            {
                if (currentControl == Player.Right)
                    tryingDirection = Direction.Left;
                else
                     tryingDirection = Direction.Right;
            }


            moveTryingDirection();
            labelDistanceNodes();
            IncrementFrameCounter();
        }
        //Checks if the pacman intersects with a ghost
        public void checkIntersectionGhost()
        {
            foreach (Ghost ghost in Map.Ghosts)
            {
                if (ghost.Position.Intersects(position))
                {
                    if (currentControl == Player.Left)
                    {
                        Map.Paddles[0].Score -= 50;
                    }
                    else
                    {
                        Map.Paddles[1].Score -= 50;
                    }

                    // Gives the control over to the other player
                    UpdateStates.timerCatch.reset();
                    PlayerCaught = (Player)((!(((int)currentControl) != 0)) ? 1 : 0);
                }
            }
        }

        // Kills ghost during powerup
        public void checkIntersectionGhostPowerup()
        {
            foreach (Ghost ghost in Map.Ghosts)
            {
                if (position.Intersects(ghost.Position))
                {
                    Map.Ghosts.Remove(ghost);
                    if (currentControl == Player.Left)
                    {
                        Map.Paddles[0].Score += 50;
                        Map.Invaders.Add(new Invader(Player.Right));
                    }
                    else
                    {
                        Map.Paddles[1].Score += 50;
                        Map.Invaders.Add(new Invader(Player.Left));
                    }
                    break;
                }
            }
        }

        private void move()
        {
            // If player goes up or down, the next change in direction has to be to the right
            // If a pacman runs into a wall, make it continue moving, if a player doesn't choose direction, randomly choose it

            // If a player currently holds onto the pacman with the paddle, move along with the paddle
            if (PlayerCaught == Player.Left)
            {
                position.X = Map.Paddles[0].Position.X + Map.Paddles[0].Position.Width;
                position.Y = Map.Paddles[0].Position.Y - CatchDifferential;
                return;
            }
            else if (PlayerCaught == Player.Right)
            {
                position.X = Map.Paddles[1].Position.X - position.Width;
                position.Y = Map.Paddles[1].Position.Y - CatchDifferential;
                return;
            }
            
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
            if (!IsWithinMaze())
                return;
            if (position.Y - speed < PacmanGame.verticalSpace)
                return;
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
                if (currentControl == Player.Left && position.X > PacmanGame.screenWidth)
                {
                    Map.Paddles[1].Score -= 50;

                    PlayerCaught = currentControl;
                    UpdateStates.timerCatch.reset();
                }
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
            if (!IsWithinMaze())
                return;
            if (position.Y + speed > PacmanGame.screenHeight - PacmanGame.verticalSpace - PacmanGame.gridSize)
                return;
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
                if (currentControl == Player.Right && position.X + position.Width < 0)
                {
                    Map.Paddles[1].Score -= 50;

                    PlayerCaught = currentControl;
                    UpdateStates.timerCatch.reset();
                }
            }
            else
            {
                movementDirection = oldMovementDirection;
                tryingDirection = Direction.Left;
                position.X = value.Item2.Right;
            }
        }

        private void IncrementFrameCounter()
        {
            FrameCounter++;
            if (FrameCounter % 10 == 0)
                IsMouthOpen = !IsMouthOpen;

            if (!IsWithinMaze())
                IsMouthOpen = false;
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
        public void changeDirection(Direction direction)
        {
            if (direction == Direction.Up)
            {
                if (oldMovementDirection != Direction.Left && currentControl == Player.Right)
                    return;
                if (oldMovementDirection != Direction.Right && currentControl == Player.Left)
                    return;
                if (!IsWithinMaze())
                    return;
                if(checkIntersectionWalls(new Rectangle(position.X, position.Y - speed, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    movementDirection = Direction.Up;
            }
            else if (direction == Direction.Left)
            {
                if (currentControl == Player.Left)
                    return;
                if (checkIntersectionWalls(new Rectangle(position.X - speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    movementDirection = Direction.Left;
            }
            else if (direction == Direction.Down)
            {
                if (oldMovementDirection != Direction.Left && currentControl == Player.Right)
                    return;
                if (oldMovementDirection != Direction.Right && currentControl == Player.Left)
                    return;
                if (!IsWithinMaze())
                    return;
                if (checkIntersectionWalls(new Rectangle(position.X, position.Y + speed, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    movementDirection = Direction.Down;
            }
            else if (direction == Direction.Right)
            {
                if (currentControl == Player.Right)
                    return;
                if (checkIntersectionWalls(new Rectangle(position.X + speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    movementDirection = Direction.Right;
            }
        }

        //Used to remove the dots when pacman intersects them
        private void checkIntersectionDots()
        {
            foreach (Dot dot in Map.Dots)
            {
                if (dot.Position.Intersects(position))
                {
                    Map.Dots.Remove(dot);
                    if (currentControl == Player.Left)
                    {
                        Map.Paddles[0].Score += 10;
                        Map.Paddles[0].Shoot();
                    }
                    else
                    {
                        Map.Paddles[1].Score += 10;
                        Map.Paddles[1].Shoot();
                    }
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
                    PacmanGame.PowerUpSound.Play();
                    if (currentControl == Player.Left)
                    {
                        Map.Paddles[0].Score += 50;
                    }
                    else
                    {
                        Map.Paddles[1].Score += 50;
                    }
                    IsPowerUp = true;
                    UpdateStates.TimerPowerup.reset();
                    return;
                }
            }
        }

        private bool IsWithinMaze()
        {
            return position.X >= PacmanGame.horizontalSpace - PacmanGame.gridSize && position.X + position.Width <= PacmanGame.screenWidth - PacmanGame.horizontalSpace + PacmanGame.gridSize;
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
                        changeDirection(Direction.Up);
                        tryingDirection = null;
                    }
                    break;
                case Direction.Right:
                    if (checkIntersectionWalls(new Rectangle(position.X + speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        changeDirection(Direction.Right);
                        tryingDirection = null;
                    }
                    break;
                case Direction.Down:
                    if (checkIntersectionWalls(new Rectangle(position.X, position.Y + speed, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        changeDirection(Direction.Down);
                        tryingDirection = null;
                    }
                    break;
                case Direction.Left:
                    if (checkIntersectionWalls(new Rectangle(position.X - speed, position.Y, PacmanGame.gridSize, PacmanGame.gridSize)).Item1)
                    {
                        changeDirection(Direction.Left);
                        tryingDirection = null;
                    }
                    break;
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
                    if (unvisitedNodes.Contains(node) && !nodeQueue.Contains(node))
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
