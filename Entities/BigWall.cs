using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Entities
{
    public class BigWall : IGameObject
    {
        private Rectangle _bounds;
        private Texture2D _texture;
        private Rectangle _sourceRect;

        public bool IsActive { get; set; } = true;
        public bool IsVisible { get; set; } = true;

        public Rectangle CollisionBox
        {
            get
            {
                if (IsActive) 
                    return _bounds;
                else 
                    return Rectangle.Empty;
            }
        }

        public BigWall(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            _texture = texture;
            _sourceRect = sourceRect;
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