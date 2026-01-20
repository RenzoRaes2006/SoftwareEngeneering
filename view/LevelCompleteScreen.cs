using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.view
{
    public class LevelCompleteScreen
    {
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private int _screenWidth;
        private int _screenHeight;

        private Rectangle _nextButton;
        private Rectangle _exitButton;
        private Color _nextColor = Color.White;
        private Color _exitColor = Color.White;

        public LevelCompleteScreen(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;

            _buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });

            int buttonWidth = 250;
            int buttonHeight = 50;
            int centerX = (_screenWidth - buttonWidth) / 2;

            // Knoppen posities
            _nextButton = new Rectangle(centerX, _screenHeight / 2 - 30, buttonWidth, buttonHeight);
            _exitButton = new Rectangle(centerX, _screenHeight / 2 + 40, buttonWidth, buttonHeight);
        }

        public string Update()
        {
            MouseState mouse = Mouse.GetState();
            Point mousePoint = new Point(mouse.X, mouse.Y);

            // Check Next Level knop
            if (_nextButton.Contains(mousePoint))
            {
                _nextColor = Color.LightGreen;
                if (mouse.LeftButton == ButtonState.Pressed) return "Next";
            }
            else _nextColor = Color.Gray;

            // Check Exit knop
            if (_exitButton.Contains(mousePoint))
            {
                _exitColor = Color.Red;
                if (mouse.LeftButton == ButtonState.Pressed) return "Exit";
            }
            else _exitColor = Color.Gray;

            return "";
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Donkere achtergrond (optioneel, voor sfeer)
            spriteBatch.Draw(_buttonTexture, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.Black * 0.5f);

            string title = "LEVEL COMPLETE!";
            Vector2 titleSize = _font.MeasureString(title);
            spriteBatch.DrawString(_font, title, new Vector2((_screenWidth - titleSize.X) / 2, 150), Color.Gold);

            // Knoppen tekenen
            spriteBatch.Draw(_buttonTexture, _nextButton, _nextColor);
            DrawCenteredText(spriteBatch, "NEXT LEVEL", _nextButton);

            spriteBatch.Draw(_buttonTexture, _exitButton, _exitColor);
            DrawCenteredText(spriteBatch, "EXIT", _exitButton);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle rect)
        {
            Vector2 size = _font.MeasureString(text);
            Vector2 pos = new Vector2(rect.X + (rect.Width - size.X) / 2, rect.Y + (rect.Height - size.Y) / 2);
            spriteBatch.DrawString(_font, text, pos, Color.Black);
        }
    }
}

