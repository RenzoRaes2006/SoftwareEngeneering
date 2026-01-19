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

        // DE HUIDIGE STATE
        private IGameState _currentState;

        // PUBLIEKE ASSETS (Zodat alle States ze kunnen gebruiken)
        public Texture2D KnightTex;
        public Texture2D BlockTex;
        public Texture2D BgTex;
        public Texture2D PowerUpTex;
        public Texture2D CoinTex;
        public SpriteFont GameFont;
        public Texture2D GreenSlimeTex;
        public Texture2D PurpleSlimeTex;

        // Data voor de factory
        public List<Rectangle> CoinFrames, greenSlimeFrames, purpleSlimeFrames;
        public Rectangle BlockPart, PowerUpPart, CoinPart;

        // De Camera delen we publiek
        public Camera Camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Camera initialiseren
            Camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // 1. LAAD ALLE ASSETS ÉÉN KEER
            KnightTex = Content.Load<Texture2D>("knight");
            BlockTex = Content.Load<Texture2D>("Block");
            BgTex = Content.Load<Texture2D>("Background_2");
            PowerUpTex = Content.Load<Texture2D>("fruit");
            CoinTex = Content.Load<Texture2D>("coin");
            GameFont = Content.Load<SpriteFont>("GameFont");
            GreenSlimeTex = Content.Load<Texture2D>("slime_green");
            PurpleSlimeTex = Content.Load<Texture2D>("slime_purple");


            // 2. SETUP DATA RECTANGLES
            BlockPart = new Rectangle(0, 0, 434, 768); // Pas aan naar jouw uitsnede
            PowerUpPart = new Rectangle(0, 0, 16, 16);
            CoinPart = new Rectangle(3, 3, 10, 10);

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



            // 3. START HET SPEL IN HET MENU
            _currentState = new MenuState(this);
        }

        // METHODE OM TE WISSELEN VAN STATE
        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
        }

        protected override void Update(GameTime gameTime)
        {
            // Input voor afsluiten mag altijd werken (noodknop)
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // Delegeer update naar de huidige state
            _currentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Delegeer draw naar de huidige state
            _currentState.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}