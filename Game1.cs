using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Enums;
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

        // --- GAME STATE ---
        private GameState _currentState;
        private StartScreen _startScreen;
        private LevelCompleteScreen _levelCompleteScreen;
        private int _currentLevelIndex = 1; // Houdt bij welk level we zijn
        private GameFinishedScreen _gameFinishedScreen;

        // --- VISUALS ---
        private ICamera _camera;
        private ScrollingBackground _background;
        private HUD _hud;

        // --- GAME OBJECTS ---
        private List<IGameObject> _gameObjects;
        private Hero _hero;
        private InputHandler _inputHandler;

        // --- ASSETS (Opslaan zodat we levels kunnen herladen) ---
        private Texture2D _knightTex, _blockTex, _bgTex, _powerUpTex, _coinTex;
        private SpriteFont _gameFont;
        private List<Rectangle> _coinFrames;
        private Rectangle _blockPart, _powerUpPart, _coinPart;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _currentState = GameState.Menu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // 1. LAAD ALLE ASSETS IN DE VARIABELEN
            _knightTex = Content.Load<Texture2D>("knight");
            _blockTex = Content.Load<Texture2D>("Block");
            _bgTex = Content.Load<Texture2D>("Background_2");
            _powerUpTex = Content.Load<Texture2D>("fruit");
            _coinTex = Content.Load<Texture2D>("coin");
            _gameFont = Content.Load<SpriteFont>("GameFont");

            // 2. SETUP DATA
            Rectangle bgPart = new Rectangle(0, 0, 16, 32);
            _background = new ScrollingBackground(_bgTex, bgPart, 0.5f, 4.0f);

            _blockPart = new Rectangle(0, 0, 434, 768); // Check jouw values!
            _powerUpPart = new Rectangle(0, 0, 16, 16);
            _coinPart = new Rectangle(3, 3, 10, 10);

            _coinFrames = new List<Rectangle>
            {
                new Rectangle(3, 3, 10, 10), new Rectangle(20, 3, 8, 10),
                new Rectangle(37, 3, 6, 10), new Rectangle(54, 3, 4, 10),
                new Rectangle(69, 3, 6, 10), new Rectangle(84, 3, 8, 10),
                new Rectangle(99, 3, 10, 10), new Rectangle(116, 3, 8, 10),
                new Rectangle(133, 3, 6, 10), new Rectangle(150, 3, 4, 10),
                new Rectangle(165, 3, 6, 10), new Rectangle(180, 3, 8, 10)
            };

            // 3. MAAK SCHERMEN
            _startScreen = new StartScreen(_gameFont, GraphicsDevice);
            _levelCompleteScreen = new LevelCompleteScreen(_gameFont, GraphicsDevice);
            _hud = new HUD(_gameFont, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _inputHandler = new InputHandler();
            _gameFinishedScreen = new GameFinishedScreen(_gameFont, GraphicsDevice);

            // LET OP: We laden het level pas als we op 'Play' drukken, 
            // of we laden hier alvast level 1 in de achtergrond.
            LoadLevel(1);
        }

        // --- NIEUWE METHODE: LEVEL LADEN ---
        private void LoadLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;

            // Maak level via factory met het level nummer
            _gameObjects = LevelFactory.CreateLevel(levelIndex, _blockTex, _blockPart, _powerUpTex, _powerUpPart, _coinTex, _coinPart, _coinFrames);

            // Maak nieuwe Hero (reset positie en stats)
            _hero = new Hero(_knightTex, _gameObjects);

            // Tel coins opnieuw
            int totalCoins = 0;
            foreach (var obj in _gameObjects)
            {
                if (obj is Coin) totalCoins++;
            }
            _hero.CoinsRemaining = totalCoins;

            _gameObjects.Add(_hero);

            // Reset camera (optioneel)
            _camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            switch (_currentState)
            {
                case GameState.Menu:
                    string menuAction = _startScreen.Update();
                    if (menuAction == "Play")
                    {
                        LoadLevel(1); // Start bij Level 1
                        _currentState = GameState.Playing;
                    }
                    else if (menuAction == "Exit") Exit();
                    break;

                case GameState.Playing:
                    // Input & Update
                    var commands = _inputHandler.GetCommands();
                    foreach (var cmd in commands) _hero.CurrentState.HandleInput(cmd, _hero);

                    for (int i = 0; i < _gameObjects.Count; i++) _gameObjects[i].Update(gameTime);
                    _camera.Follow(_hero);

                    // --- CHECK: ZIJN ALLE COINS OP? ---
                    if (_hero.CoinsRemaining == 0)
                    {
                        // IS DIT HET LAATSTE LEVEL? (Bijvoorbeeld level 2)
                        if (_currentLevelIndex >= 2)
                        {
                            // JA: Ga direct naar het eindscherm ("Proficiat!")
                            _currentState = GameState.GameFinished;
                        }
                        else
                        {
                            // NEE: Ga naar het tussen-scherm ("Volgend Level")
                            _currentState = GameState.LevelComplete;
                        }
                    }
                    break;

                case GameState.LevelComplete:
                    string levelAction = _levelCompleteScreen.Update();

                    if (levelAction == "Next")
                    {
                        int nextLevel = _currentLevelIndex + 1;

                        // AANNAME: Je hebt 2 levels. Dus als nextLevel 3 is, zijn we klaar.
                        if (nextLevel > 2)
                        {
                            // GA NAAR HET EINDSCHERM
                            _currentState = GameState.GameFinished;
                        }
                        else
                        {
                            // Laad volgende level
                            LoadLevel(nextLevel);
                            _currentState = GameState.Playing;
                        }
                    }
                    else if (levelAction == "Exit") Exit();
                    break;

                // NIEUWE CASE VOOR HET EINDSCHERM
                case GameState.GameFinished:
                    string endAction = _gameFinishedScreen.Update();

                    if (endAction == "Restart")
                    {
                        // Terug naar hoofdmenu
                        _currentState = GameState.Menu;
                    }
                    else if (endAction == "Exit")
                    {
                        Exit();
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TEKEN ACHTERGROND (Altijd zichtbaar behalve misschien in menu)
            if (_currentState == GameState.Playing || _currentState == GameState.LevelComplete)
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _background.Draw(_spriteBatch, _camera);
                _spriteBatch.End();
            }

            switch (_currentState)
            {
                case GameState.Menu:
                    _spriteBatch.Begin();
                    _startScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;

                case GameState.Playing:
                    // Wereld
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);
                    foreach (var obj in _gameObjects) obj.Draw(_spriteBatch);
                    _spriteBatch.End();
                    // HUD
                    _spriteBatch.Begin();
                    _hud.Draw(_spriteBatch, _hero);
                    _spriteBatch.End();
                    break;

                case GameState.LevelComplete:
                    // Teken eerst de wereld nog op de achtergrond (bevroren)
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);
                    foreach (var obj in _gameObjects) obj.Draw(_spriteBatch);
                    _spriteBatch.End();

                    // Teken daaroverheen het scherm
                    _spriteBatch.Begin();
                    _levelCompleteScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;

                case GameState.GameFinished:
                    // Je mag kiezen: wil je de game wereld op de achtergrond zien?
                    // Zo ja, laat het blok hieronder staan. Zo nee, haal het weg.
                    _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);
                    foreach (var obj in _gameObjects) obj.Draw(_spriteBatch);
                    _spriteBatch.End();

                    // Teken het eindscherm eroverheen
                    _spriteBatch.Begin();
                    _gameFinishedScreen.Draw(_spriteBatch);
                    _spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}