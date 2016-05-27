using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace Pacman
{
    //The different elements of the map
    class Map
    {
        private static List<Wall> walls = new List<Wall>();
        private static List<Node> nodes = new List<Node>();
        private static List<Invader> invaders = new List<Invader>();
        //subtracting 2 to ignore the borders of screen
        private static bool[,] wallMap = new bool[PacmanGame.mapWidth / PacmanGame.gridSize, PacmanGame.mapHeight / PacmanGame.gridSize];
        // 0 is left paddle, 1 is right paddle
        private static Paddle[] paddles = new Paddle[2];
        private static List<EmptySquare> emptySquares = new List<EmptySquare>(); 
        private static List<Dot> dots = new List<Dot>();
        private static List<Powerup> powerups = new List<Powerup>();
        private static List<Ghost> ghosts = new List<Ghost>();


        public static List<Wall> Walls
        {
            get { return walls; }
        }

        public static List<Node> Nodes
        {
            get { return nodes; }
        }

        public static List<Invader> Invaders
        {
            get { return invaders; }
        }

        public static List<EmptySquare> EmptySquares
        {
            get { return emptySquares; }
        }

        public static List<Dot> Dots
        {
            get { return dots; }
        }

        public static List<Powerup> Powerups
        {
            get { return powerups; }
        }

        public static List<Ghost> Ghosts
        {
            get { return ghosts; }
        }

        public static Paddle[] Paddles
        {
            get { return paddles; }
        }


        public static void InitializeMap()
        {
            walls.Clear();
            nodes.Clear();
            emptySquares.Clear();
            dots.Clear();
            powerups.Clear();
            ghosts.Clear();
            //Initializes border of map
            string line = "";
            int number = 0;
            using (StreamReader sr = new StreamReader("Content/Text Files/pacman1.txt"))
            {
                while (true)
                {
                    line = sr.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    for (int counter = 0; counter < line.Length; counter++)
                    {
                        wallMap[counter, number] = line[counter].ToString() == "0";
                    }
                    number++;
                }
            }
            //Fills in rest of map using two dimensional bool array wallMap
            for (int x = 0; x < wallMap.GetLength(0); x++)
            {
                for (int y = 0; y < wallMap.GetLength(1); y++)
                {
                    if (!wallMap[x,y])
                    {
                        walls.Add(new Wall(new Rectangle(x * PacmanGame.gridSize + PacmanGame.horizontalSpace, y * PacmanGame.gridSize + PacmanGame.verticalSpace, PacmanGame.gridSize, PacmanGame.gridSize)));
                    }
                }
            }
            //Fills map with empty squares, dots and nodes in all places without walls
            //variable used to count nodes
            int variable = 0;
            for (int y = 1; y < PacmanGame.mapHeight / PacmanGame.gridSize - 1; y++)
            {
                 for (int x = 1; x < PacmanGame.mapWidth / PacmanGame.gridSize - 1; x++)
                 {
                     Rectangle rectangle = new Rectangle(PacmanGame.gridSize * x +PacmanGame.horizontalSpace, PacmanGame.gridSize * y + PacmanGame.verticalSpace, PacmanGame.gridSize, PacmanGame.gridSize);
                     bool value = true;
                     foreach (Wall wall in walls)
                     {
                         if (wall.Position == rectangle)
                         {
                             value = false;
                         }
                     }
                     if (value)
                     {
                         emptySquares.Add(new EmptySquare(rectangle));
                         dots.Add(new Dot(rectangle));
                         nodes.Add(new Node(variable, rectangle));
                         variable++;
                     }
                 }
             }
            //Remove the dot where the pacman starts
            dots.Remove(new Dot(new Rectangle(PacmanGame.gridSize, PacmanGame.gridSize, PacmanGame.gridSize, PacmanGame.gridSize)));
            //Adds powerups
            Random random = new Random();
            int[] values = Enumerable.Range(0, Map.Nodes.Count).OrderBy(x => random.Next()).ToArray();
            for (int counter = 0; counter < 4; counter++)
            {
                int index = values[counter];
                Rectangle rectangleValue = Map.Nodes.ElementAt(index).Position;
                powerups.Add(new Powerup(rectangleValue));
                //Remove the dot where the powerup is
                dots.Remove(new Dot(rectangleValue));
            }

            createAdjacencyList();

            paddles[0] = new Paddle(new Rectangle(PacmanGame.horizontalSpace / 2, PacmanGame.screenHeight / 2, PacmanGame.gridSize, PacmanGame.gridSize * 5), Player.Left);
            paddles[1] = new Paddle(new Rectangle(PacmanGame.screenWidth - PacmanGame.horizontalSpace / 2, PacmanGame.screenHeight / 2, PacmanGame.gridSize, PacmanGame.gridSize * 5), Player.Right);
        }

        //Creates the adjacency list for the nodes for use in path finding for ghost
        private static void createAdjacencyList()
        {
            //for each node, checks if there are nodes adjacent to them and adds to adjacency list if there are
            foreach (Node node in nodes)
            {
                //Node to the right
                if (nodes.Contains(new Node(new Rectangle(node.Position.X + PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(nodes[nodes.IndexOf(new Node(new Rectangle(node.Position.X + PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height)))]);
                }
                //Node below
                if (nodes.Contains(new Node(new Rectangle(node.Position.X, node.Position.Y + PacmanGame.gridSize, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(nodes[nodes.IndexOf(new Node(new Rectangle(node.Position.X, node.Position.Y + PacmanGame.gridSize, node.Position.Width, node.Position.Height)))]);
                }
                //Node to the left
                if (nodes.Contains(new Node(new Rectangle(node.Position.X - PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(nodes[nodes.IndexOf(new Node(new Rectangle(node.Position.X - PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height)))]);
                }
                //Node above
                if (nodes.Contains(new Node(new Rectangle(node.Position.X, node.Position.Y - PacmanGame.gridSize, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(nodes[nodes.IndexOf(new Node(new Rectangle(node.Position.X, node.Position.Y - PacmanGame.gridSize, node.Position.Width, node.Position.Height)))]);
                }
            }
        }
    }
}
