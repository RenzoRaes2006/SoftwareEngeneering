using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.view;
using SofEngeneering_project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SofEngeneering_project.GameState
{
    public class MenuState:IGameState
    {
        private Game1 _game;

        // De knoppen declareren (Hier zijn ze nog NULL/leeg)
        private Button _startButton;
        private Button _levelSelectButton;
        private Button _quitButton;

        // Muis status bewaren
        private MouseState _oldMouseState;

        public MenuState(Game1 game)
        {
            _game = game;

            // 1. Ophalen van Texture en Font
            // Als hier een error komt, is Game1.LoadContent nog niet klaar geweest
            Texture2D btnTex = _game.PixelTexture;
            SpriteFont font = _game.GameFont;

            // 2. KNOPPEN AANMAKEN (Cruciaal! Anders crasht de Update)
            _startButton = new Button(btnTex, font, "Play", new Vector2(300, 200));
            _levelSelectButton = new Button(btnTex, font, "Choose level", new Vector2(300, 270));
            _quitButton = new Button(btnTex, font, "Exit", new Vector2(300, 340));

            // 3. Eerste muis status ophalen
            _oldMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            MouseState newMouseState = Mouse.GetState();

            // Check of _startButton bestaat voordat we klikken (extra veiligheid)
            if (_startButton != null && _startButton.IsClicked(newMouseState, _oldMouseState))
            {
                _game.ChangeState(new PlayingState(_game, 1));
            }

            if (_levelSelectButton != null && _levelSelectButton.IsClicked(newMouseState, _oldMouseState))
            {
                _game.ChangeState(new ChooseLevelState(_game));
            }

            if (_quitButton != null && _quitButton.IsClicked(newMouseState, _oldMouseState))
            {
                _game.Exit();
            }

            _oldMouseState = newMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _game.GraphicsDevice.Clear(Color.DarkBlue);

            spriteBatch.Begin();
            spriteBatch.DrawString(_game.GameFont, "Software engeneering game", new Vector2(230, 100), Color.White);

            // Teken de knoppen alleen als ze bestaan
            if (_startButton != null) _startButton.Draw(spriteBatch);
            if (_levelSelectButton != null) _levelSelectButton.Draw(spriteBatch);
            if (_quitButton != null) _quitButton.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
