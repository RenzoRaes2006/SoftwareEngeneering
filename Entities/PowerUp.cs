using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Entities
{
    public class PowerUp : IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public bool IsCollected { get; set; } = false;

        private Rectangle _sourceRect;
        private float _scale = 2.0f;

        private bool _respawns;         
        private float _respawnTimer;    
        private const float RESPAWN_DELAY = 5.0f;


        public PowerUp(Texture2D texture, Rectangle sourceRect, Vector2 position, bool respawns = false)
        {
            Texture = texture;
            _sourceRect = sourceRect;
            Position = position;

            _respawns = respawns;
            _respawnTimer = RESPAWN_DELAY;
        }

        public Rectangle CollisionBox => new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(_sourceRect.Width * _scale),
            (int)(_sourceRect.Height * _scale)
        );

        public void Update(GameTime gameTime)
        {

            if (IsCollected && _respawns)
            {
                _respawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_respawnTimer <= 0)
                {
                    IsCollected = false;
                    _respawnTimer = RESPAWN_DELAY;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                spriteBatch.Draw(Texture, Position, _sourceRect, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
        }
    }
}