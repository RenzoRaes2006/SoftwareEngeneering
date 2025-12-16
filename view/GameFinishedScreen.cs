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
    public class GameFinishedScreen
    {
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private int _screenWidth;
        private int _screenHeight;

        private Rectangle _restartButton;
        private Rectangle _exitButton;

        // Kleuren voor hover effect
        private Color _restartColor = Color.White;
        private Color _exitColor = Color.White;

        public GameFinishedScreen(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;

            // Maak witte texture (1x1 pixel)
            _buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });

            int buttonWidth = 250;
            int buttonHeight = 50;
            int centerX = (_screenWidth - buttonWidth) / 2;
            int centerY = _screenHeight / 2;

            // Knoppen posities
            _restartButton = new Rectangle(centerX, centerY - 20, buttonWidth, buttonHeight);
            _exitButton = new Rectangle(centerX, centerY + 50, buttonWidth, buttonHeight);
        }

        public string Update()
        {
            MouseState mouse = Mouse.GetState();
            Point mousePoint = new Point(mouse.X, mouse.Y);

            // 1. Check RESTART knop
            if (_restartButton.Contains(mousePoint))
            {
                _restartColor = Color.LightGreen;
                if (mouse.LeftButton == ButtonState.Pressed) return "Restart";
            }
            else _restartColor = Color.Gray;

            // 2. Check EXIT knop
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
            // Donkere achtergrond
            spriteBatch.Draw(_buttonTexture, new Rectangle(0, 0, _screenWidth, _screenHeight), Color.Black * 0.8f);

            // Tekst: "Proficiat..."
            string line1 = "PROFICIAT!";
            string line2 = "U heeft de game voltooid.";

            // Centreer de teksten
            Vector2 size1 = _font.MeasureString(line1);
            Vector2 size2 = _font.MeasureString(line2);

            spriteBatch.DrawString(_font, line1, new Vector2((_screenWidth - size1.X) / 2, 100), Color.Gold);
            spriteBatch.DrawString(_font, line2, new Vector2((_screenWidth - size2.X) / 2, 140), Color.White);

            // Teken knoppen
            spriteBatch.Draw(_buttonTexture, _restartButton, _restartColor);
            DrawCenteredText(spriteBatch, "HERSTARTEN", _restartButton);

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
