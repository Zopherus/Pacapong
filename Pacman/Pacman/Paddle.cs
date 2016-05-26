using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Paddle
    {
        public const int speed = 5;
        public Rectangle Position { get; private set; }
        public Player Player { get; private set; }


        public Paddle(Rectangle Position, Player Player)
        {
            this.Position = Position;
            this.Player = Player;
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

        public void MoveUp()
        {
            if (Position.Y - speed >= 0)
                Position = new Rectangle(Position.X, Position.Y - speed, Position.Width, Position.Height);
        }

        public void MoveRight()
        {
            if (Player == Player.Right && Position.X + speed + Position.Width > PacmanGame.screenWidth)
                return;
            if (Player == Player.Left && Position.X + speed + Position.Width > PacmanGame.horizontalSpace)
                return;
            Position = new Rectangle(Position.X + speed, Position.Y, Position.Width, Position.Height);
        }

        public void MoveDown()
        {
            if (Position.Y + speed + Position.Height <= PacmanGame.screenHeight)
                Position = new Rectangle(Position.X, Position.Y + speed, Position.Width, Position.Height);
        }

        public void MoveLeft()
        {
            if (Player == Player.Left && Position.X - speed < 0)
                return;
            if (Player == Player.Right && Position.X - speed < PacmanGame.screenWidth - PacmanGame.horizontalSpace)
                return;
            Position = new Rectangle(Position.X - speed, Position.Y, Position.Width, Position.Height);
        }
    }
}
