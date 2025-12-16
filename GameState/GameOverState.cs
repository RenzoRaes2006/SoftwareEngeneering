using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.GameState
{
    public class GameOverState:IGameState
    {
        private Game1 _game;
        private GameOverScreen _screen;
        private int _levelToRestart;

        public GameOverState(Game1 game, int levelToRestart)
        {
            _game = game;
            _levelToRestart = levelToRestart;
            _screen = new GameOverScreen(_game.GameFont, _game.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            string action = _screen.Update();

            if (action == "Restart")
            {
                // Herstart hetzelfde level
                _game.ChangeState(new PlayingState(_game, _levelToRestart));
            }
            else if (action == "Exit")
            {
                _game.Exit();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _screen.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
