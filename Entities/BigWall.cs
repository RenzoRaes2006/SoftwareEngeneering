using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Entities
{
    public class BigWall : IGameObject
    {
        private Rectangle _bounds;
        private Texture2D _texture;
        private Rectangle _sourceRect;

        // Nieuwe properties voor het effect
        public bool IsActive { get; set; } = true;  // Voor botsingen
        public bool IsVisible { get; set; } = true; // Voor tekenen

        public Rectangle CollisionBox
        {
            get
            {
                if (IsActive)
                {
                    return _bounds;
                }
                else
                {
                    return Rectangle.Empty; // 0x0 pixels groot, dus onmogelijk om te raken
                }
            }
        }

        public BigWall(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            _texture = texture;
            _sourceRect = sourceRect;

            // We slaan de afmetingen op in de private variabele '_bounds'
            _bounds = new Rectangle((int)position.X, (int)position.Y, sourceRect.Width, sourceRect.Height);
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(_texture, new Vector2(CollisionBox.X, CollisionBox.Y), _sourceRect, Color.White);
            }
        }
    }
}
