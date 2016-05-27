using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Pacman
{
    class Paddle
    {
        public const int speed = 6;
        public Rectangle Position { get; private set; }
        public Player Player { get; private set; }
        public int Score;
        public Dictionary<Keys, Direction> DirectionByKey { get; private set; }
        


        public Paddle(Rectangle Position, Player Player)
        {
            this.Position = Position;
            this.Player = Player;
            DirectionByKey = new Dictionary<Keys, Direction>();
            if (Player == Player.Left)
            {
                DirectionByKey.Add(Keys.W, Direction.Up);
                DirectionByKey.Add(Keys.A, Direction.Left);
                DirectionByKey.Add(Keys.S, Direction.Down);
                DirectionByKey.Add(Keys.D, Direction.Right);
            }
            if (Player == Player.Right)
            {
                DirectionByKey.Add(Keys.Up, Direction.Up);
                DirectionByKey.Add(Keys.Left, Direction.Left);
                DirectionByKey.Add(Keys.Down, Direction.Down);
                DirectionByKey.Add(Keys.Right, Direction.Right);
            }
        }

        public void Move(Direction direction){
            if (direction == Direction.Up)
            {
                if (Position.Y - speed >= 0)
                    Position = new Rectangle(Position.X, Position.Y - speed, Position.Width, Position.Height);
            }
            else if (direction == Direction.Left)
            {
                if (Player == Player.Left && Position.X - speed < 0)
                    return;
                if (Player == Player.Right && Position.X - speed < PacmanGame.screenWidth - PacmanGame.horizontalSpace + PacmanGame.gridSize)
                    return;
                Position = new Rectangle(Position.X - speed, Position.Y, Position.Width, Position.Height);
            }
            else if (direction == Direction.Down)
            {
                if (Position.Y + speed + Position.Height <= PacmanGame.screenHeight)
                    Position = new Rectangle(Position.X, Position.Y + speed, Position.Width, Position.Height);
            }
            else if (direction == Direction.Right)
            {
                if (Player == Player.Right && Position.X + speed + Position.Width > PacmanGame.screenWidth)
                    return;
                if (Player == Player.Left && Position.X + speed + Position.Width > PacmanGame.horizontalSpace - PacmanGame.gridSize)
                    return;
                Position = new Rectangle(Position.X + speed, Position.Y, Position.Width, Position.Height);
            }
        }

        public void CatchPacman()
        {
            if (PacmanGame.pacman.PlayerCaught != null)
                return;
            if (Position.Intersects(PacmanGame.pacman.Position))
            {
                PacmanGame.pacman.currentControl = Player;
                PacmanGame.pacman.PlayerCaught = Player;
                PacmanGame.pacman.CatchDifferential = Position.Y - PacmanGame.pacman.Position.Y;
            }
        }
    }
}
