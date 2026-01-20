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
    public class Trap : IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle CollisionBox { get; private set; }

        private Rectangle _sourceRect;
        private IMovementEnemy _moveBehavior;

        public Trap(Texture2D texture, Rectangle sourceRect, Vector2 startPosition, IMovementEnemy moveBehavior)
        {
            Texture = texture;
            _sourceRect = sourceRect;
            Position = startPosition;
            _moveBehavior = moveBehavior;

            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, sourceRect.Width, sourceRect.Height);
        }

        public void Update(GameTime gameTime)
        {
            Position = _moveBehavior.Move(Position, gameTime);

            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, _sourceRect.Width, _sourceRect.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, _sourceRect, Color.White);
        }
    }
}
