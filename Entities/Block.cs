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

        // NIEUW: Welk stukje van de 1024x1024 afbeelding gebruiken we?
        private Rectangle _sourceRect;

        // CONSTANT: Hoe groot is een blok in de spelwereld?
        // Ook al is je bron 434x768, we willen het waarschijnlijk tekenen als een 64x64 tegel.
        private const int GameTileSize = 64;

        // Constructor aangepast: accepteert nu ook de sourceRectangle
        public Block(Texture2D texture, Rectangle sourceRect, Vector2 position)
        {
            Texture = texture;
            _sourceRect = sourceRect;
            Position = position;

            // De collision box blijft 64x64, passend bij de spelwereld
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, GameTileSize, GameTileSize);
        }

        public void Update(GameTime gameTime)
        {
            // Blokken doen voorlopig niets
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // We moeten nu een 'Destination Rectangle' maken.
            // Dit zegt: "Teken op deze positie, met deze afmeting (64x64)"
            Rectangle destRect = new Rectangle((int)Position.X, (int)Position.Y, GameTileSize, GameTileSize);

            spriteBatch.Draw(
                Texture,
                destRect,    // Waar en hoe groot op het scherm? (64x64)
                _sourceRect, // Welk stukje uit de texture? (434x768)
                Color.White
            );
        }
    }
}