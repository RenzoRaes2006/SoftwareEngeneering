using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Entities
{
    public class Block : IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle CollisionBox { get; private set; }

        private Rectangle _sourceRect;
        private const int TILE_SIZE = 64;

        public Block(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            Texture = texture;
            _sourceRect = sourceRect;
            Position = position;

            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, TILE_SIZE, 12);
        }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, TILE_SIZE, 12);

            spriteBatch.Draw(Texture,destRect, _sourceRect, Color.White);
        }
    }
}