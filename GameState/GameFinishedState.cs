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
    public class GameFinishedState:IGameState
    {
        private Game1 _game;
        private GameFinishedScreen _screen;

        public GameFinishedState(Game1 game)
        {
            _game = game;
            _screen = new GameFinishedScreen(_game.GameFont, _game.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            string action = _screen.Update();

            if (action == "Restart")
            {
                // Ga terug naar het hoofdmenu
                _game.ChangeState(new MenuState(_game));
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
