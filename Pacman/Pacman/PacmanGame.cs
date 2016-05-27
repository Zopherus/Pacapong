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
    public enum GameState { Menu, Maze, GameEnd};
    public enum Direction { Up = 1, Right, Down, Left};

    public class PacmanGame : Game
    {
        public const int horizontalSpace = 8 * gridSize;
        public const int verticalSpace = gridSize;
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
        public static List<Texture2D> GhostTextures = new List<Texture2D>();
        public static Texture2D GhostPowerupTexture;
        public static Texture2D BlackTexture;
        public static Texture2D InvaderTexture;
        public static SpriteFont spriteFont;
        public static Song MenuSong;
        public static Song GameSong;

        public static Texture2D boxGreen;
        public static Texture2D boxPink;
        public static Texture2D boxPurple;
        public static Texture2D boxYellow;
        public static Texture2D paddleBox;

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
            pacman = new Pacman(new Rectangle(0, 0, gridSize, gridSize));
            pacman.PlayerCaught = Player.Left;
            Map.InitializeMap();
        }

        protected override void Initialize()
        {
            MediaPlayer.IsRepeating = true;
            PacmanGame.gameState = GameState.Menu;
            Start();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PacmanTexture = Content.Load<Texture2D>("Sprites/PacmanSpritesheet");
            WallTexture = Content.Load<Texture2D>("Sprites/Wall");
            DotTexture = Content.Load<Texture2D>("Sprites/Dot");
            PowerupTexture = Content.Load<Texture2D>("Sprites/Powerup");
            GhostTextures.Add(Content.Load<Texture2D>("Sprites/Ghost1"));
            GhostTextures.Add(Content.Load<Texture2D>("Sprites/Ghost2"));
            GhostTextures.Add(Content.Load<Texture2D>("Sprites/Ghost3"));
            GhostTextures.Add(Content.Load<Texture2D>("Sprites/Ghost4"));
            GhostPowerupTexture = Content.Load<Texture2D>("Sprites/GhostPowerup");
            BlackTexture = Content.Load<Texture2D>("Sprites/Black");
            InvaderTexture = Content.Load<Texture2D>("Sprites/Invader");
            spriteFont = Content.Load<SpriteFont>("SpriteFonts/SpriteFont");

            boxGreen = Content.Load<Texture2D>("Sprites/Menu Green");
            boxPink = Content.Load<Texture2D>("Sprites/Menu Pink");
            boxPurple = Content.Load<Texture2D>("Sprites/Menu Purple");
            boxYellow = Content.Load<Texture2D>("Sprites/Menu Yellow");
            paddleBox = Content.Load<Texture2D>("Sprites/Paddles");

            //MenuSong.MenuSong = Content.Load<Song>("Music/LisztPaganiniChiptude.wav");
            //GameSong = Content.Load<Song>("Music/DebussyArabesqueMeme-ix.wav");
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
            if (gameState == GameState.GameEnd)
                UpdateStates.UpdateGameEnd(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);
            spriteBatch.Begin();
            if (gameState == GameState.Menu)
                DrawStates.DrawMenu();
            if (gameState == GameState.Maze)
                DrawStates.DrawMaze();
            if (gameState == GameState.GameEnd)
                DrawStates.DrawGameEnd();
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
