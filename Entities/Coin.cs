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
    public class Coin: IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle CollisionBox { get; private set; }

        // Source rectangle voor Coin
        private Rectangle _sourceRect;

        private float _scale; // NIEUW: Hoeveel keer groter?

        // Zodat we weten of hij al opgepakt is
        public bool IsCollected { get; set; } = false;

        // --- ANIMATIE VARIABELEN ---
        private List<Rectangle> _frames; // De lijst met alle uitsnedes
        private int _currentFrame = 0;   // Welk plaatje laten we zien?
        private float _timer = 0f;       // Timer voor snelheid
        private float _frameSpeed = 0.1f; // Snelheid animatie (0.1s per frame)

        public Coin(Texture2D texture, Rectangle sourceRect, Vector2 position, float scale, List<Rectangle> frames)
        {
            Texture = texture;
            _sourceRect = sourceRect;
            Position = position;
            _scale = scale;
            _frames = frames;

            int width = (int)(sourceRect.Width * scale);
            int height = (int)(sourceRect.Height * scale);

            CollisionBox = new Rectangle((int)position.X, (int)position.Y, width, height);
            Rectangle firstFrame = _frames[0];
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _frameSpeed)
            {
                _timer = 0f;
                _currentFrame++;

                // Loop terug naar 0 als we aan het einde zijn
                if (_currentFrame >= _frames.Count)
                {
                    _currentFrame = 0;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                Rectangle sourceRect = _frames[_currentFrame];
                spriteBatch.Draw(Texture, Position, sourceRect, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
        }
    }
}
