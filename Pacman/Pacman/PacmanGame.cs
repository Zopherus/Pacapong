using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pacman
{
    public enum Player { Left, Right };
    public enum GameState { Menu, Maze};
    public enum Direction { Up = 1, Right, Down, Left};

    public class PacmanGame : Game
    {
        public const int horizontalSpace = 8 * gridSize;
        public const int verticalSpace = 3 * gridSize;
        public const int mapWidth = 21 * gridSize;
        public const int mapHeight = 21 * gridSize;
        public const int screenWidth = horizontalSpace * 2 + mapWidth;
        public const int screenHeight = verticalSpace * 2 + mapHeight;
        public const int gridSize = 30;
        
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static KeyboardState keyboard;
        public static KeyboardState oldKeyboard;
        public static MouseState mouse;
        public static MouseState oldMouse;

        public static Pacman pacman;

        public static Texture2D PacmanTexture;
        public static Texture2D WallTexture;
        public static Texture2D DotTexture;
        public static Texture2D PowerupTexture;
        public static Texture2D GhostTexture;
        public static Texture2D GhostPowerupTexture;
        public static Texture2D BlackTexture;
        public static SpriteFont spriteFont;

        public static GameState gameState;

        public PacmanGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = screenHeight,
                PreferredBackBufferWidth = screenWidth
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        //Used to initialize the program
        public void Start()
        {
            UpdateStates.TimerMaze.reset();
            pacman = new Pacman(new Rectangle(gridSize + horizontalSpace, gridSize + verticalSpace, gridSize, gridSize));
            Map.InitializeMap();
        }

        protected override void Initialize()
        {
            PacmanGame.gameState = GameState.Menu;
            Start();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PacmanTexture = Content.Load<Texture2D>("Sprites/Pacman");
            WallTexture = Content.Load<Texture2D>("Sprites/Wall");
            DotTexture = Content.Load<Texture2D>("Sprites/Dot");
            PowerupTexture = Content.Load<Texture2D>("Sprites/Powerup");
            GhostTexture = Content.Load<Texture2D>("Sprites/Ghost");
            GhostPowerupTexture = Content.Load<Texture2D>("Sprites/GhostPowerup");
            BlackTexture = Content.Load<Texture2D>("Sprites/Black");
            spriteFont = Content.Load<SpriteFont>("SpriteFonts/SpriteFont");
        }

        protected override void Update(GameTime gameTime)
        {
            oldKeyboard = keyboard;
            oldMouse = mouse;
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (gameState == GameState.Menu)
                UpdateStates.UpdateMenu();
            if (gameState == GameState.Maze)
                UpdateStates.UpdateMaze(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            if (gameState == GameState.Menu)
                DrawStates.DrawMenu();
            if (gameState == GameState.Maze)
                DrawStates.DrawMaze();
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
