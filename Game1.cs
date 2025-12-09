using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Factories;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.view;
using SofEngeneering_project.View;
using System.Collections.Generic;

namespace SofEngeneering_project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // --- VISUALS ---
        private ICamera _camera;
        private ScrollingBackground _background;
        private HUD _hud; // <--- DEZE ONTBRAK

        // --- GAME OBJECTS ---
        private List<IGameObject> _gameObjects;
        private Hero _hero;


        // --- INPUT ---
        private InputHandler _inputHandler;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Initialiseer de camera met de huidige schermgrootte
            _camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // 1. LAAD TEXTURES
            Texture2D knightTex = Content.Load<Texture2D>("knight");
            Texture2D blockTex = Content.Load<Texture2D>("Block");
            Texture2D backgroundTex = Content.Load<Texture2D>("Background_2");
            Texture2D PowerUpTex = Content.Load<Texture2D>("fruit");
            Texture2D Coin = Content.Load<Texture2D>("coin");
            SpriteFont font = Content.Load<SpriteFont>("GameFont");


            // 2. SETUP ACHTERGROND
            // We pakken een stukje van 16x32 uit de texture (linksboven: 0,0)
            Rectangle bgPart = new Rectangle(0, 0, 16, 32);

            //Hud aanmaken
            _hud = new HUD(font, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Texture, SourceRect, ScrollSpeed (0.5f), Scale (4.0f = groter maken)
            _background = new ScrollingBackground(backgroundTex, bgPart, 0.5f, 4.0f);

            //powerup rectangle
            Rectangle applePart = new Rectangle(0, 0, 16, 16);

            Rectangle blockPart = new Rectangle(0, 0, 434, 768);

            Rectangle CoinPart = new Rectangle(3, 3, 10, 10);

            List<Rectangle> coinFrames = new List<Rectangle>();
            coinFrames.Add(new Rectangle(3, 3, 10, 10));
            coinFrames.Add(new Rectangle(20, 3, 8, 10));
            coinFrames.Add(new Rectangle(37, 3, 6, 10));
            coinFrames.Add(new Rectangle(54, 3, 4, 10));
            coinFrames.Add(new Rectangle(69, 3, 6, 10));
            coinFrames.Add(new Rectangle(84, 3, 8, 10));
            coinFrames.Add(new Rectangle(99, 3, 10, 10));
            coinFrames.Add(new Rectangle(116, 3, 8, 10));
            coinFrames.Add(new Rectangle(133, 3, 6, 10));
            coinFrames.Add(new Rectangle(150, 3, 4, 10));
            coinFrames.Add(new Rectangle(165, 3, 6, 10));
            coinFrames.Add(new Rectangle(180, 3, 8, 10));



            // 3. SETUP LEVEL (FACTORY)
            _gameObjects = LevelFactory.CreateLevel(blockTex, blockPart, PowerUpTex, applePart, Coin, CoinPart, coinFrames);

            // 4. SETUP HERO
            // We geven de lijst met gameObjects mee zodat de Hero kan checken op botsingen
            _hero = new Hero(knightTex, _gameObjects);


            //aantal coins tellen
            int totalCoins = 0;
            foreach (var obj in _gameObjects)
            {
                if (obj is Coin)
                {
                    totalCoins++;
                }
            }

            _hero.CoinsRemaining = totalCoins;

            // Voeg de hero ook toe aan de algemene lijst zodat hij getekend en geüpdatet wordt
            _gameObjects.Add(_hero);

            // 5. SETUP INPUT
            _inputHandler = new InputHandler();

            
        }

        protected override void Update(GameTime gameTime)
        {
            // Sluit de game met Esc of Back button
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // --- INPUT VERWERKING (COMMAND PATTERN) ---
            var commands = _inputHandler.GetCommands();
            foreach (var cmd in commands)
            {
                // De huidige State van de Hero bepaalt wat er gebeurt met de input
                _hero.CurrentState.HandleInput(cmd, _hero);
            }

            // --- UPDATE ALLE OBJECTEN ---
            // We gebruiken een for-loop omdat dit veiliger is als de lijst verandert tijdens update
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].Update(gameTime);
            }


            // --- CAMERA UPDATE ---
            // Laat de camera de speler volgen NA de physics update
            _camera.Follow(_hero);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // ================================================================
            // BLOK 1: ACHTERGROND (Zonder Camera Matrix)
            // ================================================================
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Teken achtergrond
            if (_background != null)
                _background.Draw(_spriteBatch, _camera);

            _spriteBatch.End(); // <--- DEZE MOET HIER STAAN!


            // ================================================================
            // BLOK 2: DE SPEELWERELD (MET Camera Matrix)
            // ================================================================
            _spriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                transformMatrix: _camera.Transform // Hier gebruiken we de camera
            );

            // Teken alle game objecten (Hero, blokken, coins, powerups)
            foreach (var obj in _gameObjects)
            {
                obj.Draw(_spriteBatch);
            }

            _spriteBatch.End(); // <--- DEZE BEN JE WAARSCHIJNLIJK VERGETEN!


            // ================================================================
            // BLOK 3: DE HUD / UI (Zonder Camera Matrix)
            // ================================================================
            // We starten weer vers, want tekst moet stilstaan op het scherm
            _spriteBatch.Begin();

            if (_hud != null)
            {
                _hud.Draw(_spriteBatch, _hero);
            }

            _spriteBatch.End(); // <--- EN DEZE MOET HET AFSLUITEN


            base.Draw(gameTime);
        }
    }
}