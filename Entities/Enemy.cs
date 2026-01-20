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
        public Vector2 Velocity;
        protected List<Rectangle> _frames;
        protected int _currentFrame;
        protected float _elapsedTime;
        protected float _scale;
        public IMovementEnemy MovementStrategy { get; protected set; }
        public bool IsDead { get; set; } = false;
        protected List<IGameObject> _levelObjects;

        public Enemy(Texture2D texture, Vector2 position, List<Rectangle> frames, IMovementEnemy strategy, float scale, List<IGameObject> levelObjects)
        {
            Texture = texture;
            Position = position;
            _frames = frames;
            MovementStrategy = strategy;
            _scale = scale;
            _levelObjects = levelObjects;
            Velocity = Vector2.Zero;
        }

        public virtual Rectangle CollisionBox => new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            (int)(_frames[0].Width * _scale),
            (int)(_frames[0].Height * _scale)
        );

        public virtual void Die() => IsDead = true;

        public virtual void Update(GameTime gameTime)
        {
            if (IsDead) return;


            Vector2 movement = MovementStrategy.Move(Position, gameTime);

            Position = new Vector2(movement.X, Position.Y);

            if (movement.Y < 0) Velocity.Y = movement.Y;

            ApplyPhysics();

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTime > 0.1f)
            {
                _currentFrame = (_currentFrame + 1) % _frames.Count;
                _elapsedTime = 0;
            }
        }

        protected void ApplyPhysics()
        {
            Velocity.Y += 0.45f;
            Position += new Vector2(0, Velocity.Y);

            bool onGround = false;
            Rectangle hb = CollisionBox;

            foreach (var obj in _levelObjects)
            {
                if (obj is Block || (obj is BigWall wall && wall.IsActive))
                {
                    if (hb.Intersects(obj.CollisionBox))
                    {
                        // Collision van bovenaf
                        if (Velocity.Y > 0 && hb.Bottom >= obj.CollisionBox.Top && hb.Bottom <= obj.CollisionBox.Top + 20)
                        {
                            Position = new Vector2(Position.X, obj.CollisionBox.Top - hb.Height);
                            Velocity.Y = 0;
                            onGround = true;
                            break;
                        }
                    }
                }
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