using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Factories;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.View;
using SofEngeneering_project.view;
using System.Collections.Generic;

namespace SofEngeneering_project.GameState
{
    public class PlayingState : IGameState
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

            _background = new ScrollingBackground(_game.BgTex, new Rectangle(0, 0, 2304, 1288), 0.5f, 0.5f);
            _hud = new HUD(_game.GameFont, _game.GraphicsDevice.Viewport.Width);
            _inputHandler = new InputHandler();

            // Factory aanroep
            _gameObjects = LevelFactory.CreateLevel(
                levelIndex,
                _game.BlockTex, _game.BlockPart,
                _game.PowerUpTex, _game.PowerUpPart,
                _game.CoinTex, _game.CoinPart,
                _game.CoinFrames,
                _game.GreenSlimeTex,
                _game.greenSlimeFrames,
                _game.PurpleSlimeTex,
                _game.purpleSlimeFrames,
                _game.SurikenTex,
                _game.SurikenPart
            );

            _hero = new Hero(_game.KnightTex, _gameObjects);

            // --- FIX: Observer registreren ---
            _hero.RegisterObserver(_hud);

            // --- FIX: Coins tellen en instellen ---
            int totalCoins = 0;
            foreach (var obj in _gameObjects) if (obj is Coin) totalCoins++;
            _hero.CoinsRemaining = totalCoins;
            _hero.NotifyObservers(); // Update de HUD direct

            _gameObjects.Add(_hero);
            _game.Camera = new Camera(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            // 1. Input
            var commands = _inputHandler.GetCommands();
            foreach (var cmd in commands) _hero.CurrentState.HandleInput(cmd, _hero);

            // 2. Update Alles
            for (int i = 0; i < _gameObjects.Count; i++) _gameObjects[i].Update(gameTime);

            // 3. Camera
            _game.Camera.Follow(_hero);

            // 4. COMBAT & LOGICA
            IGameObject objectToRemove = null;

            foreach (var obj in _gameObjects)
            {
                if (obj == _hero) continue;

                if (_hero.CollisionBox.Intersects(obj.CollisionBox))
                {
                    // A. Coins
                    if (obj is Coin coin && !coin.IsCollected)
                    {
                        coin.IsCollected = true;
                        _hero.CollectCoin();
                    }
                    // B. PowerUps
                    else if (obj is PowerUp power && !power.IsCollected)
                    {
                        power.IsCollected = true;
                        _hero.EnableSuperJump();
                    }
                    // C. Trap -> DOOD
                    else if (obj is Trap)
                    {
                        _game.ChangeState(new GameOverState(_game, _levelIndex));
                        return;
                    }
                    // D. Enemy -> KILL OF DOOD
                    else if (obj is Enemy enemy)
                    {
                        // HERO KILLS ENEMY: Als Hero valt (Velocity Y > 0) EN boven de vijand zit
                        // Marge van 20 pixels
                        bool isFallingOnTop = _hero.Velocity.Y > 0 && _hero.CollisionBox.Bottom < enemy.CollisionBox.Top + 20;

                        if (isFallingOnTop)
                        {
                            enemy.Die();
                            _hero.Bounce(); // Stuiter
                            objectToRemove = enemy;
                        }
                        else
                        {
                            // Aangeraakt aan zijkant of onderkant -> Hero Dood
                            _game.ChangeState(new GameOverState(_game, _levelIndex));
                            return;
                        }
                    }
                }
            }

            if (objectToRemove != null) _gameObjects.Remove(objectToRemove);

            // 5. Game Over (Vallen)
            if (_hero.Position.Y > 800) _game.ChangeState(new GameOverState(_game, _levelIndex));

            // 6. Win Condition (Munten op)
            if (_hero.CoinsRemaining == 0)
            {
                if (_levelIndex >= 2) _game.ChangeState(new GameFinishedState(_game));
                else _game.ChangeState(new LevelCompleteState(_game, _levelIndex));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _background.Draw(spriteBatch, _game.Camera);
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _game.Camera.Transform);
            foreach (var obj in _gameObjects) obj.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            _hud.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}