using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.view
{
    public class ScrollingBackground
    {
        private Texture2D _texture;
        private Rectangle _sourceRect; // Welk stukje van 16x32 knippen we uit?
        private float _scrollSpeed;
        private float _scale;          // Hoeveel keer vergroten we het?

        public ScrollingBackground(Texture2D texture, Rectangle sourceRect, float scrollSpeed, float scale)
        {
            _texture = texture;
            _sourceRect = sourceRect;
            _scrollSpeed = scrollSpeed;
            _scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            // 1. Bereken hoe groot 1 tegel is op het scherm (bijv 16 * 4 = 64 pixels breed)
            int tileWidth = (int)(_sourceRect.Width * _scale);
            int tileHeight = (int)(_sourceRect.Height * _scale);

            // 2. Bereken de verschuiving (parallax effect)
            float xOffset = (camera.Position.X * _scrollSpeed) % tileWidth;

            // 3. Hoeveel tegels hebben we nodig om het scherm te vullen?
            // We doen +2 om zeker te weten dat we geen gaten zien aan de randen
            int tilesX = (800 / tileWidth) + 2;
            int tilesY = (480 / tileHeight) + 2;

            // 4. Dubbele loop: Teken rijen en kolommen
            for (int x = -1; x < tilesX; x++)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    // De positie op het scherm berekenen
                    Vector2 position = new Vector2(
                        (x * tileWidth) - xOffset, // Schuif mee met camera
                        y * tileHeight             // Y blijft vast staan (of je kan ook Y-parallax doen)
                    );

                    // Teken de uitsnede vergroot
                    spriteBatch.Draw(
                        _texture,
                        position,
                        _sourceRect, // HIER pakken we alleen dat stukje van 16x32
                        Color.White,
                        0f,
                        Vector2.Zero,
                        _scale,      // HIER vergroten we het
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
    }
}

