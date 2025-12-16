using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Factories;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.view;
using SofEngeneering_project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.GameState
{
    public class PlayingState:IGameState
    {
        private Game1 _game;
        private List<IGameObject> _gameObjects;
        private Hero _hero;
        private InputHandler _inputHandler;
        private ScrollingBackground _background;
        private HUD _hud;

        private int _levelIndex;

        public PlayingState(Game1 game, int levelIndex)
        {
            _game = game;
            _levelIndex = levelIndex;

            // 1. Initialiseer systemen
            _background = new ScrollingBackground(_game.BgTex, new Rectangle(0, 0, 16, 32), 0.5f, 4.0f);
            _hud = new HUD(_game.GameFont, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
            _inputHandler = new InputHandler();

            // 2. Level Factory aanroepen met de assets uit Game1
            _gameObjects = LevelFactory.CreateLevel(
                levelIndex,
                _game.BlockTex, _game.BlockPart,
                _game.PowerUpTex, _game.PowerUpPart,
                _game.CoinTex, _game.CoinPart,
                _game.CoinFrames
            );

            // 3. Hero maken
            _hero = new Hero(_game.KnightTex, _gameObjects);

            // Coins tellen
            int totalCoins = 0;
            foreach (var obj in _gameObjects) if (obj is Coin) totalCoins++;
            _hero.CoinsRemaining = totalCoins;

            _gameObjects.Add(_hero);

            // Camera resetten voor het nieuwe level
            _game.Camera = new Camera(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            // 1. Input Handler
            var commands = _inputHandler.GetCommands();
            foreach (var cmd in commands) _hero.CurrentState.HandleInput(cmd, _hero);

            // 2. Update alle objecten
            for (int i = 0; i < _gameObjects.Count; i++) _gameObjects[i].Update(gameTime);

            // 3. Camera volgt Hero
            _game.Camera.Follow(_hero);

            // --- CONTROLEER SPELSTATUS ---

            // A. GAME OVER (Vallen)
            if (_hero.Position.Y > 800)
            {
                // Ga naar GameOverState en onthoud welk level we speelden
                _game.ChangeState(new GameOverState(_game, _levelIndex));
            }

            // B. LEVEL GEWONNEN (Coins op)
            if (_hero.CoinsRemaining == 0)
            {
                // Is dit het laatste level? (Hier hardcoded op 2)
                if (_levelIndex >= 2)
                {
                    _game.ChangeState(new GameFinishedState(_game));
                }
                else
                {
                    // Ga naar volgend level scherm
                    _game.ChangeState(new LevelCompleteState(_game, _levelIndex));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // 1. Achtergrond
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _background.Draw(spriteBatch, _game.Camera);
            spriteBatch.End();

            // 2. Wereld (Met Camera)
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _game.Camera.Transform);
            foreach (var obj in _gameObjects) obj.Draw(spriteBatch);
            spriteBatch.End();

            // 3. HUD (Zonder Camera)
            spriteBatch.Begin();
            _hud.Draw(spriteBatch, _hero);
            spriteBatch.End();
        }
    }
}
