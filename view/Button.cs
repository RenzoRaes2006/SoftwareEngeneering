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
    public class Button
    {
        private Texture2D _texture;
        private SpriteFont _font;
        private Rectangle _rectangle;
        private string _text;

        public bool IsHovering { get; private set; }

        public Vector2 Position { get; set; }

        public Button(Texture2D texture, SpriteFont font, string text, Vector2 position)
        {
            _texture = texture;
            _font = font;
            _text = text;
            Position = position;

            _rectangle = new Rectangle((int)Position.X, (int)Position.Y, 200, 50);
        }

        public bool IsClicked(MouseState currentMouse, MouseState previousMouse)
        {
            Rectangle mouseRect = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            IsHovering = _rectangle.Intersects(mouseRect);

            if (IsHovering && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (IsHovering) color = Color.Gray;

            spriteBatch.Draw(_texture, _rectangle, color);

            if (!string.IsNullOrEmpty(_text))
            {
                Vector2 textSize = _font.MeasureString(_text);
                Vector2 textPos = new Vector2(
                    _rectangle.X + (_rectangle.Width / 2) - (textSize.X / 2),
                    _rectangle.Y + (_rectangle.Height / 2) - (textSize.Y / 2)
                );

                spriteBatch.DrawString(_font, _text, textPos, Color.Black);
            }
        }
    }
}
