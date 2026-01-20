using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.Behaviors;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Factories;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.Services;
using SofEngeneering_project.view;
using SofEngeneering_project.View;
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
            _hud = new HUD(_game.GameFont, _game.HeartTex, _game.GraphicsDevice.Viewport.Width);
            _inputHandler = new InputHandler();

            _gameObjects = LevelFactory.CreateLevel(
                levelIndex,
                _game.BlockTex, _game.BlockPart,
                _game.WallPart,
                _game.PowerUpTex, _game.PowerUpPart,
                _game.CoinTex, _game.CoinPart,
                _game.CoinFrames,
                _game.GreenSlimeTex,
                _game.greenSlimeFrames,
                _game.PurpleSlimeTex,
                _game.purpleSlimeFrames,
                _game.SurikenTex,
                _game.SurikenPart,
                _game.GraphicsDevice
            );

            _hero = new Hero(_game.KnightTex, _gameObjects);
            _hero.RegisterObserver(_hud);

            int totalCoins = 0;
            foreach (var obj in _gameObjects) if (obj is Coin) totalCoins++;
            _hero.CoinsRemaining = totalCoins;
            _hero.NotifyObservers();

            _gameObjects.Add(_hero);
            _game.Camera = new Camera(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            var commands = _inputHandler.GetCommands();
            foreach (var cmd in commands) _hero.CurrentState.HandleInput(cmd, _hero);

            for (int i = 0; i < _gameObjects.Count; i++)
            {
                var obj = _gameObjects[i];
                obj.Update(gameTime);

                if (obj is Enemy enemy)
                {
                    PhysicsService.HandleEnemyPhysics(enemy, _gameObjects);
                }
            }

            _game.Camera.Follow(_hero);

            IGameObject objectToRemove = null;

            foreach (var obj in _gameObjects)
            {
                if (obj == _hero) continue;

                if (_hero.CollisionBox.Intersects(obj.CollisionBox))
                {
                    if (obj is Coin coin && !coin.IsCollected) { coin.IsCollected = true; _hero.CollectCoin(); }
                    else if (obj is PowerUp power && !power.IsCollected) { power.IsCollected = true; _hero.EnableSuperJump(); }
                    else if (obj is Trap)
                    {
                        if (_hero.TakeDamage()) { _game.ChangeState(new GameOverState(_game, _levelIndex)); return; }
                    }
                    else if (obj is Enemy enemy && !enemy.IsDead)
                    {
                        // 1. Check eerst of we van bovenaf komen (Schade aan Enemy)
                        bool isFallingOnTop = _hero.Velocity.Y > 0 && _hero.CollisionBox.Bottom < enemy.CollisionBox.Top + 30;

                        if (isFallingOnTop)
                        {
                            _hero.Bounce(); // Speler springt weer omhoog

                            if (enemy is Boss boss)
                            {
                                boss.TakeDamage(20);
                                if (boss.CurrentHP <= 0) objectToRemove = boss;
                            }
                            else
                            {
                                enemy.Die();
                                objectToRemove = enemy;
                            }
                        }
                        // 2. GEBRUIK 'ELSE IF': Alleen als we NIET van bovenaf kwamen, krijgt de player schade
                        else
                        {
                            bool isHeroDead = _hero.TakeDamage();

                            if (isHeroDead)
                            {
                                _game.ChangeState(new GameOverState(_game, _levelIndex));
                                return;
                            }
                            else
                            {
                                // Enemy sprint weg na het raken van de speler
                                if (enemy.MovementStrategy is PatrolEnemyBehavior patrol)
                                {
                                    bool shouldRunRight = _hero.Position.X < enemy.Position.X;
                                    patrol.PanicAndRun(shouldRunRight);
                                }
                            }
                        }
                    }
                }
            }

            if (objectToRemove != null) _gameObjects.Remove(objectToRemove);
            if (_hero.Position.Y > 800) _game.ChangeState(new GameOverState(_game, _levelIndex));

            if (_hero.CoinsRemaining == 0)
            {
                if (_levelIndex >= 2) _game.ChangeState(new GameFinishedState(_game));
                else _game.ChangeState(new LevelCompleteState(_game, _levelIndex));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            // 1. Achtergrond (zonder camera)
            sb.Begin(samplerState: SamplerState.PointClamp);
            _background.Draw(sb, _game.Camera);
            sb.End();

            // 2. Game Objecten (MET camera transformatie)
            // De Boss MOET hierin zitten om mee te bewegen met de wereld
            sb.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _game.Camera.Transform);
            foreach (var obj in _gameObjects)
            {
                obj.Draw(sb);
            }
            sb.End();

            // 3. HUD (zonder camera)
            sb.Begin();
            _hud.Draw(sb);
            sb.End();
        }
    }
}