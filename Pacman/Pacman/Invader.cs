using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The Invader that attacks the paddle
    public class Invader
    {
        private const int speed = 3; //Bugs happen when the speed doesn't divide the gridSize

        private Random random = new Random();
        private Rectangle position;
        private Direction oldDirection;

        public Invader()
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
            Tuple<bool, Rectangle> value = checkIntersectionPaddles(new Rectangle(position.X, position.Y + speed, PacmanGame.gridSize, PacmanGame.gridSize));
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

        //Used by the move methods to stop pacman from going through a wall
        //the bool value states if pacman intersects a wall or not
        //the rectangle value is the rectangle of the wall that it intersected
        private Tuple<bool, Rectangle> checkIntersectionPaddles(Rectangle position)
        {
            foreach (Paddle paddle in Map.Paddles)
            {
                if (paddle.Position.Intersects(position))
                {
                    return new Tuple<bool, Rectangle>(false, paddle.Position);
                }
            }
            return new Tuple<bool, Rectangle>(true, new Rectangle(0, 0, 0, 0));
        }
    }
}
