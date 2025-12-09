using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Entities
{
    public class PowerUp : IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle CollisionBox { get; private set; }

        // Source rectangle voor het specifieke fruitje uit de sheet
        private Rectangle _sourceRect;

        // Zodat we weten of hij al opgepakt is
        public bool IsCollected { get; set; } = false;

        public PowerUp(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            Texture = texture;
            _sourceRect = sourceRect;
            Position = position;

            // We gebruiken de grootte van de sourceRect (bijv 20x20) voor de collisionBox
            CollisionBox = new Rectangle((int)position.X, (int)position.Y, sourceRect.Width, sourceRect.Height);
        }

        public void Update(GameTime gameTime)
        {
            // Optioneel: Zweef effectje
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                spriteBatch.Draw(Texture, Position, _sourceRect, Color.White);
            }
        }
    }
}