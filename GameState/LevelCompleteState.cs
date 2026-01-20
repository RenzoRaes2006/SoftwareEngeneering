using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;

namespace SofEngeneering_project.GameState
{
    public class LevelCompleteState : IGameState
    {
        private Game1 _game;
        private GenericMenuScreen _screen;
        private int _completedLevelIndex;

        public LevelCompleteState(Game1 game, int completedLevelIndex)
        {
            _game = game;
            _completedLevelIndex = completedLevelIndex;

            _screen = new GenericMenuScreen(
                _game.GameFont,
                _game.GraphicsDevice,
                "LEVEL COMPLETE!",
                "NEXT LEVEL",
                Color.Black * 0.7f
            );
        }

        public void Update(GameTime gameTime)
        {
            string action = _screen.Update();

            if (action == "Action")
            {
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