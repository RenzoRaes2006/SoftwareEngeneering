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

        // Positie en Grootte
        public Vector2 Position { get; set; }

        public Button(Texture2D texture, SpriteFont font, string text, Vector2 position)
        {
            _texture = texture;
            _font = font;
            _text = text;
            Position = position;

            // We maken de knop zo groot als de texture (bijv. 200x50)
            // Als je een 1x1 pixel texture gebruikt, moet je hier handmatig breedte/hoogte opgeven.
            // Voor nu gokken we dat je texture een knop-vorm heeft, of we stretchen hem:
            _rectangle = new Rectangle((int)Position.X, (int)Position.Y, 200, 50);
        }

        public bool IsClicked(MouseState currentMouse, MouseState previousMouse)
        {
            // Check of muis op knop staat
            Rectangle mouseRect = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            IsHovering = _rectangle.Intersects(mouseRect);

            // Check of er geklikt is (Linkermuisknop ingedrukt)
            if (IsHovering && currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            if (IsHovering) color = Color.Gray; // Wordt grijs als je erover muist

            // Teken de knop achtergrond (We stretchen de texture naar 200x50)
            spriteBatch.Draw(_texture, _rectangle, color);

            // Teken de tekst in het midden
            if (!string.IsNullOrEmpty(_text))
            {
                // Bereken midden van de knop voor de tekst
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
