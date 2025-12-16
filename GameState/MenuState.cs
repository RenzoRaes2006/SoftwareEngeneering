using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.GameState
{
    public class MenuState:IGameState
    {
        private Game1 _game;
        private StartScreen _startScreen;

        public MenuState(Game1 game)
        {
            _game = game;
            _startScreen = new StartScreen(_game.GameFont, _game.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            string action = _startScreen.Update();

            if (action == "Play")
            {
                // Start Level 1
                _game.ChangeState(new PlayingState(_game, 1));
            }
            else if (action == "Exit")
            {
                _game.Exit();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _startScreen.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
