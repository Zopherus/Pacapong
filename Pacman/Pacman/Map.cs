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
        public static List<Wall> Walls { get; private set; } = new List<Wall>();
        public static List<Node> Nodes { get; private set; } = new List<Node>();
        public static List<Invader> Invaders { get; private set; } = new List<Invader>();
        //subtracting 2 to ignore the borders of screen
        private static bool[,] wallMap = new bool[PacmanGame.mapWidth / PacmanGame.gridSize, PacmanGame.mapHeight / PacmanGame.gridSize];
        // 0 is left paddle and player, 1 is right paddle and computer
        public static Paddle[] Paddles { get; private set; } = new Paddle[2];
        public static List<EmptySquare> EmptySquares { get; private set; } = new List<EmptySquare>();
        public static List<Dot> Dots { get; private set; } = new List<Dot>();
        public static List<Powerup> Powerups { get; private set; } = new List<Powerup>();
        public static List<Ghost> Ghosts { get; private set; } = new List<Ghost>();
        public static List<Shot> Shots { get; private set; } = new List<Shot>();
        

        public static void InitializeMap()
        {
            Walls.Clear();
            Nodes.Clear();
            EmptySquares.Clear();
            Dots.Clear();
            Powerups.Clear();
            Ghosts.Clear();
            Invaders.Clear();
            //Initializes border of map
            string line = "";
            int number = 0;
            using (StreamReader sr = new StreamReader("Maps/pacman" + PacmanGame.maps[PacmanGame.mapNumber].ToString() + ".txt"))
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
                        Walls.Add(new Wall(new Rectangle(x * PacmanGame.gridSize + PacmanGame.horizontalSpace, y * PacmanGame.gridSize + PacmanGame.verticalSpace, PacmanGame.gridSize, PacmanGame.gridSize)));
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
                     foreach (Wall wall in Walls)
                     {
                         if (wall.Position == rectangle)
                         {
                             value = false;
                         }
                     }
                     if (value)
                     {
                         EmptySquares.Add(new EmptySquare(rectangle));
                         Dots.Add(new Dot(rectangle));
                         Nodes.Add(new Node(variable, rectangle));
                         variable++;
                     }
                 }
             }
            //Remove the dot where the pacman starts
            Dots.Remove(new Dot(new Rectangle(PacmanGame.gridSize, PacmanGame.gridSize, PacmanGame.gridSize, PacmanGame.gridSize)));
            //Adds powerups
            Random random = new Random();
            int[] values = Enumerable.Range(0, Map.Nodes.Count).OrderBy(x => random.Next()).ToArray();
            for (int counter = 0; counter < 4; counter++)
            {
                int index = values[counter];
                Rectangle rectangleValue = Map.Nodes.ElementAt(index).Position;
                Powerups.Add(new Powerup(rectangleValue));
                //Remove the dot where the powerup is
                Dots.Remove(new Dot(rectangleValue));
            }

            createAdjacencyList();

            Paddles[0] = new Paddle(new Rectangle(PacmanGame.horizontalSpace / 2, PacmanGame.screenHeight / 2, PacmanGame.gridSize, PacmanGame.gridSize * 5), Player.Left);
            Paddles[1] = new Paddle(new Rectangle(PacmanGame.screenWidth - PacmanGame.horizontalSpace / 2, PacmanGame.screenHeight / 2, PacmanGame.gridSize, PacmanGame.gridSize * 5), Player.Right);
        }

        //Creates the adjacency list for the Nodes for use in path finding for ghost
        private static void createAdjacencyList()
        {
            //for each node, checks if there are Nodes adjacent to them and adds to adjacency list if there are
            foreach (Node node in Nodes)
            {
                //Node to the right
                if (Nodes.Contains(new Node(new Rectangle(node.Position.X + PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(Nodes[Nodes.IndexOf(new Node(new Rectangle(node.Position.X + PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height)))]);
                }
                //Node below
                if (Nodes.Contains(new Node(new Rectangle(node.Position.X, node.Position.Y + PacmanGame.gridSize, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(Nodes[Nodes.IndexOf(new Node(new Rectangle(node.Position.X, node.Position.Y + PacmanGame.gridSize, node.Position.Width, node.Position.Height)))]);
                }
                //Node to the left
                if (Nodes.Contains(new Node(new Rectangle(node.Position.X - PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(Nodes[Nodes.IndexOf(new Node(new Rectangle(node.Position.X - PacmanGame.gridSize, node.Position.Y, node.Position.Width, node.Position.Height)))]);
                }
                //Node above
                if (Nodes.Contains(new Node(new Rectangle(node.Position.X, node.Position.Y - PacmanGame.gridSize, node.Position.Width, node.Position.Height))))
                {
                    node.adjacentNodes.Add(Nodes[Nodes.IndexOf(new Node(new Rectangle(node.Position.X, node.Position.Y - PacmanGame.gridSize, node.Position.Width, node.Position.Height)))]);
                }
            }
        }
    }
}
