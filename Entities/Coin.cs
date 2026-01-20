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

        private Rectangle _sourceRect;

        private float _scale;

        // opgepakt of niet
        public bool IsCollected { get; set; } = false;

        // --- ANIMATIE VARIABELEN ---
        private List<Rectangle> _frames;
        private int _currentFrame = 0;
        private float _timer = 0f;
        private float _frameSpeed = 0.1f;

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

                // terug naar 0 als einde is
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
