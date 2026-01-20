using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.GameState
{
    public class ChooseLevelState : IGameState
    {
        private Game1 _game;

        // De knoppen variabelen
        private Button _lvl1Btn;
        private Button _lvl2Btn;
        private Button _backBtn;

        // Muis status voor de "Ghost Click" fix
        private MouseState _oldMouseState;

        public ChooseLevelState(Game1 game)
        {
            _game = game;

            // Assets ophalen
            Texture2D btnTex = _game.PixelTexture;
            SpriteFont font = _game.GameFont;

            // --- DEZE REGELS MISTE JE WAARSCHIJNLIJK ---
            // Hier worden de knoppen echt aangemaakt (geïnstantieerd).
            // Zonder dit zijn ze 'null' en crasht de game.
            _lvl1Btn = new Button(btnTex, font, "LEVEL 1", new Vector2(300, 200));
            _lvl2Btn = new Button(btnTex, font, "LEVEL 2", new Vector2(300, 270));
            _backBtn = new Button(btnTex, font, "BACK", new Vector2(300, 400));
            // ------------------------------------------

            // Muis status initialiseren
            _oldMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            // Check veiligheidshalve of de knoppen bestaan (!= null)
            if (_lvl1Btn != null && _lvl1Btn.IsClicked(ms, _oldMouseState))
            {
                _game.ChangeState(new PlayingState(_game, 1));
            }

            if (_lvl2Btn != null && _lvl2Btn.IsClicked(ms, _oldMouseState))
            {
                _game.ChangeState(new PlayingState(_game, 2));
            }

            if (_backBtn != null && _backBtn.IsClicked(ms, _oldMouseState))
            {
                _game.ChangeState(new MenuState(_game));
            }

            // Onthoud muis status
            _oldMouseState = ms;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            Rectangle screenRect = new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(_game.ChooseLevelBackgroundTex, screenRect, Color.White);

            spriteBatch.DrawString(_game.GameFont, "CHOOSE LEVEL", new Vector2(320, 100), Color.Black);

            if (_lvl1Btn != null) _lvl1Btn.Draw(spriteBatch);
            if (_lvl2Btn != null) _lvl2Btn.Draw(spriteBatch);
            if (_backBtn != null) _backBtn.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
