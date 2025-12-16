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
    public class LevelCompleteState:IGameState
    {
        private Game1 _game;
        private LevelCompleteScreen _screen;
        private int _completedLevelIndex;

        public LevelCompleteState(Game1 game, int completedLevelIndex)
        {
            _game = game;
            _completedLevelIndex = completedLevelIndex;
            _screen = new LevelCompleteScreen(_game.GameFont, _game.GraphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            string action = _screen.Update();

            if (action == "Next")
            {
                // Start het volgende level
                _game.ChangeState(new PlayingState(_game, _completedLevelIndex + 1));
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
