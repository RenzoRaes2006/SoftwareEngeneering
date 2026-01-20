using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;

namespace SofEngeneering_project.GameState
{
    public class GameOverState : IGameState
    {
        private Game1 _game;
        private GenericMenuScreen _screen;
        private int _levelToRestart;

        public GameOverState(Game1 game, int levelToRestart)
        {
            _game = game;
            _levelToRestart = levelToRestart;

            _screen = new GenericMenuScreen(
                _game.GameFont,
                _game.GraphicsDevice,
                "YOU DIED",
                "TRY AGAIN",
                Color.DarkRed * 0.8f
            );
        }

        public void Update(GameTime gameTime)
        {
            string action = _screen.Update();

            if (action == "Action")
            {
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