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
    public enum GameState { Menu, Maze, EnterName, HighScore };
    public enum Direction { Up = 1, Right, Down, Left};
    /// <summary>
    /// This is the main type for your game
    /// </summary>
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
            Highscore.ReadFromFile();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            PacmanGame.gameState = GameState.Menu;
            Start();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
            if (gameState == GameState.EnterName)
                UpdateStates.UpdateEnterName();
            if (gameState == GameState.HighScore)
                UpdateStates.UpdateHighScore();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.Menu)
                DrawStates.DrawMenu();
            if (gameState == GameState.Maze)
                DrawStates.DrawMaze();
            if (gameState == GameState.EnterName)
                DrawStates.DrawEnterName();
            if (gameState == GameState.HighScore)
                DrawStates.DrawHighScore();
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
