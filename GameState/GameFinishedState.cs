using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;

namespace SofEngeneering_project.GameState
{
    public class GameFinishedState : IGameState
    {
        private Game1 _game;
        private GenericMenuScreen _screen;

        public GameFinishedState(Game1 game)
        {
            _game = game;

            // Configureer voor Einde Spel (Goud/Groen overlay)
            _screen = new GenericMenuScreen(
                _game.GameFont,
                _game.GraphicsDevice,
                "VICTORY! YOU COMPLETED THE GAME",
                "MAIN MENU",
                Color.DarkGreen * 0.9f
            );
        }

        public void Update(GameTime gameTime)
        {
            string action = _screen.Update();

            if (action == "Action") // Action betekent hier naar Menu
            {
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