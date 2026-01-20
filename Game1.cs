using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.GameState;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;
using System.Collections.Generic;

namespace SofEngeneering_project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D PixelTexture { get; private set; } 

        private IGameState _currentState;

        public Texture2D KnightTex;
        public Texture2D BlockTex;
        public Texture2D BgTex;
        public Texture2D PowerUpTex;
        public Texture2D CoinTex;
        public SpriteFont GameFont;
        public Texture2D GreenSlimeTex;
        public Texture2D PurpleSlimeTex;
        public Texture2D SurikenTex;
        public Texture2D MenuBackgroundTex;
        public Texture2D ChooseLevelBackgroundTex;
        public Texture2D HeartTex;
        public Texture2D VictoryScreen;

        public List<Rectangle> CoinFrames, greenSlimeFrames, purpleSlimeFrames;
        public Rectangle BlockPart, PowerUpPart, CoinPart, SurikenPart, WallPart;

        public Camera Camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            IsMouseVisible = true;
            base.Initialize();  
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            KnightTex = Content.Load<Texture2D>("knight");
            BlockTex = Content.Load<Texture2D>("TX Tileset Ground");
            BgTex = Content.Load<Texture2D>("Summer2");
            PowerUpTex = Content.Load<Texture2D>("fruit");
            CoinTex = Content.Load<Texture2D>("coin");
            GameFont = Content.Load<SpriteFont>("GameFont");
            GreenSlimeTex = Content.Load<Texture2D>("slime_green");
            PurpleSlimeTex = Content.Load<Texture2D>("slime_purple");
            SurikenTex = Content.Load<Texture2D>("Suriken");
            MenuBackgroundTex = Content.Load<Texture2D>("Summer5");
            ChooseLevelBackgroundTex = Content.Load<Texture2D>("Summer6");
            HeartTex = Content.Load<Texture2D>("heart");
            VictoryScreen = Content.Load<Texture2D>("Summer8");

            // data rechthoeken
            BlockPart = new Rectangle(0, 384, 96, 12);
            PowerUpPart = new Rectangle(0, 0, 16, 16);
            CoinPart = new Rectangle(3, 3, 10, 10);
            SurikenPart = new Rectangle(0, 0, 32, 32);
            WallPart = new Rectangle(0, 255, 32, 96);

            CoinFrames = new List<Rectangle>
            {
                new Rectangle(3, 3, 10, 10), new Rectangle(20, 3, 8, 10),
                new Rectangle(37, 3, 6, 10), new Rectangle(54, 3, 4, 10),
                new Rectangle(69, 3, 6, 10), new Rectangle(84, 3, 8, 10),
                new Rectangle(99, 3, 10, 10), new Rectangle(116, 3, 8, 10),
                new Rectangle(133, 3, 6, 10), new Rectangle(150, 3, 4, 10),
                new Rectangle(165, 3, 6, 10), new Rectangle(180, 3, 8, 10)
            };

            greenSlimeFrames = new List<Rectangle>
            {
                new Rectangle(5, 36, 14, 12),   
                new Rectangle(29, 35, 14, 13),  
                new Rectangle(53, 33, 14, 15),  
                new Rectangle(77, 35, 14, 13)   
            };


            purpleSlimeFrames = new List<Rectangle>
            {
                new Rectangle(5, 36, 14, 12),
                new Rectangle(29, 35, 14, 13),
                new Rectangle(53, 33, 14, 15),
                new Rectangle(77, 35, 14, 13)
            };

            PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            PixelTexture.SetData(new[] { Color.White });

            _currentState = new MenuState(this);
        }

        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            _currentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _currentState.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}