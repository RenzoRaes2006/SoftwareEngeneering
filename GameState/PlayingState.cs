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

            // 1. Initialiseer systemen
            _background = new ScrollingBackground(_game.BgTex, new Rectangle(0, 0, 16, 32), 0.5f, 4.0f);
            _hud = new HUD(_game.GameFont, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
            _inputHandler = new InputHandler();

            // 2. Level Factory aanroepen
            // HIER IS DE AANPASSING: We geven nu ook de enemy-assets mee!
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

            // 3. Hero maken
            _hero = new Hero(_game.KnightTex, _gameObjects);

            // Coins tellen voor de HUD
            int totalCoins = 0;
            foreach (var obj in _gameObjects) if (obj is Coin) totalCoins++;
            _hero.CoinsRemaining = totalCoins;

            _gameObjects.Add(_hero);

            // Camera resetten
            _game.Camera = new Camera(_game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
        }

        public void Update(GameTime gameTime)
        {
            // 1. Input Handler
            var commands = _inputHandler.GetCommands();
            foreach (var cmd in commands) _hero.CurrentState.HandleInput(cmd, _hero);

            // 2. Update alle objecten (Hero, Enemies, Coins, etc)
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].Update(gameTime);
            }

            // 3. Camera volgt Hero
            _game.Camera.Follow(_hero);



            // --------------------------------------------------------
            // 4. NIEUW: BOTSING EN WIN/VERLIES LOGICA
            // --------------------------------------------------------

            IGameObject enemyToKill = null; // Opslag voor dode enemy

            foreach (var obj in _gameObjects)
            {
                // A. Check Enemy (Slijm)
                if (obj is Enemy enemy)
                {
                    if (_hero.CollisionBox.Intersects(enemy.CollisionBox))
                    {
                        if (_hero.HasSuperJump)
                        {
                            // HELD WINT: Enemy gaat dood
                            enemyToKill = enemy;

                            // Bounce effect (stuiter omhoog)
                            _hero.Velocity = new Vector2(_hero.Velocity.X, -10f);
                        }
                        else
                        {
                            // HELD VERLIEST: Game Over
                            _game.ChangeState(new GameOverState(_game, _levelIndex));
                            return;
                        }
                    }
                }

                // B. Check Trap (MovingBlock) -> ALTIJD DOOD
                else if (obj is Trap trap)
                {
                    if (_hero.CollisionBox.Intersects(trap.CollisionBox))
                    {
                        _game.ChangeState(new GameOverState(_game, _levelIndex));
                        return;
                    }
                }
            }

            // Verwijder de dode enemy veilig uit de lijst
            if (enemyToKill != null)
            {
                _gameObjects.Remove(enemyToKill);
                // Als je in Hero.cs ook een lijst 'LevelObjects' hebt, 
                // verwijdert hij hem daar automatisch ook uit als het dezelfde lijst is.
                // Zo niet, moet je _hero.LevelObjects.Remove(enemyToKill) ook doen.
            }

            // --------------------------------------------------------

            // B. GAME OVER (Vallen in de afgrond)
            if (_hero.Position.Y > 800)
            {
                _game.ChangeState(new GameOverState(_game, _levelIndex));
            }

            // C. LEVEL GEWONNEN (Coins op)
            if (_hero.CoinsRemaining == 0)
            {
                if (_levelIndex >= 2)
                {
                    _game.ChangeState(new GameFinishedState(_game));
                }
                else
                {
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

            // 3. HUD (Zonder Camera, staat vast op scherm)
            spriteBatch.Begin();
            _hud.Draw(spriteBatch, _hero);
            spriteBatch.End();
        }
    }
}