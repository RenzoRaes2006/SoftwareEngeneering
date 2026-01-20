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
        private Rectangle _sourceRect;
        private float _scrollSpeed;
        private float _scale;

        public ScrollingBackground(Texture2D texture, Rectangle sourceRect, float scrollSpeed, float scale)
        {
            _texture = texture;
            _sourceRect = sourceRect;
            _scrollSpeed = scrollSpeed;
            _scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            int tileWidth = (int)(_sourceRect.Width * _scale);
            int tileHeight = (int)(_sourceRect.Height * _scale);

            float xOffset = (camera.Position.X * _scrollSpeed) % tileWidth;

            int tilesX = (800 / tileWidth) + 2;
            int tilesY = (480 / tileHeight) + 2;

            for (int x = -1; x < tilesX; x++)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    Vector2 position = new Vector2(
                        (x * tileWidth) - xOffset,
                        y * tileHeight
                    );

                    spriteBatch.Draw(
                        _texture,
                        position,
                        _sourceRect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        _scale, 
                        SpriteEffects.None,
                        0f
                    );
                }
            }
        }
    }
}

