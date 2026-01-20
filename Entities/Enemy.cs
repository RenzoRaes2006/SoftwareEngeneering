using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Enemy : IGameObject
    {
        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get; set; }
        protected List<Rectangle> _frames;
        protected int _currentFrame;
        protected float _elapsedTime;
        protected float _scale;
        public IMovementEnemy MovementStrategy { get; protected set; }
        public bool IsDead { get; set; } = false;

        public Enemy(Texture2D texture, Vector2 position, List<Rectangle> frames, IMovementEnemy strategy, float scale, List<IGameObject> levelObjects)
        {
            Texture = texture;
            Position = position;
            _frames = frames;
            MovementStrategy = strategy;
            _scale = scale;
        }

        // ESSENTIEEL: De hitbox moet 10x zo groot zijn!
        public virtual Rectangle CollisionBox => new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(_frames[_currentFrame].Width * _scale),
            (int)(_frames[_currentFrame].Height * _scale)
        );

        public virtual void Die() => IsDead = true;

        public virtual void Update(GameTime gameTime)
        {
            if (IsDead) return;
            Position = MovementStrategy.Move(Position, gameTime);

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTime > 0.1f)
            {
                _currentFrame = (_currentFrame + 1) % _frames.Count;
                _elapsedTime = 0;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead) return;
            spriteBatch.Draw(Texture, Position, _frames[_currentFrame], Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public void OnWallHit() => MovementStrategy.OnHorizontalCollision();
    }
}