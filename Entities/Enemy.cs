using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Enemy : IGameObject, IMovable
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Rectangle CollisionBox { get; private set; }

        public event Action OnDeath;

        private List<Rectangle> _frames;
        private int _currentFrame = 0;
        private float _timer = 0f;
        private float _frameSpeed = 0.2f;
        private int _maxHeight;
        private float _scale;

        public List<IGameObject> LevelObjects { get; private set; }
        public IMovementEnemy MovementStrategy { get; private set; }

        public Enemy(Texture2D texture, Vector2 startPosition, List<Rectangle> frames, IMovementEnemy moveBehavior, float scale, List<IGameObject> levelObjects)
        {
            Texture = texture;
            Position = startPosition;
            _frames = frames;
            MovementStrategy = moveBehavior;
            _scale = scale;
            LevelObjects = levelObjects;

            _maxHeight = 0;
            foreach (var f in frames) if (f.Height > _maxHeight) _maxHeight = f.Height;
            UpdateCollisionBox();
        }

        public void Update(GameTime gameTime)
        {
            Vector2 oldPos = Position;

            // 1. Move
            Position = MovementStrategy.Move(Position, gameTime);
            Velocity = Position - oldPos;
            UpdateCollisionBox();

            // 2. Physics (Interne logica om afhankelijkheid van PhysicsService te vermijden als die niet bestaat)
            foreach (var obj in LevelObjects)
            {
                if (obj == this) continue;

                // CRUCIAAL: Negeer Hero, Coins, Traps, etc.
                if (obj is Hero || obj is Coin || obj is PowerUp || obj is Trap || obj is Enemy) continue;
                // Gate check
                if (obj is BigWall gate && !gate.IsActive) continue;

                if (CollisionBox.Intersects(obj.CollisionBox))
                {
                    // Botsing met muur -> Zet terug en draai om
                    if (Velocity.X > 0) Position = new Vector2(obj.CollisionBox.Left - CollisionBox.Width, Position.Y);
                    else if (Velocity.X < 0) Position = new Vector2(obj.CollisionBox.Right, Position.Y);

                    UpdateCollisionBox();
                    MovementStrategy.OnHorizontalCollision();
                }
            }

            // 3. Animatie
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _frameSpeed)
            {
                _timer = 0f;
                _currentFrame++;
                if (_currentFrame >= _frames.Count) _currentFrame = 0;
            }
        }

        private void UpdateCollisionBox()
        {
            int scaledWidth = (int)(_frames[0].Width * _scale);
            int scaledHeight = (int)(_maxHeight * _scale);
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, scaledWidth, scaledHeight);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle currentRect = _frames[_currentFrame];
            float originalOffset = _maxHeight - currentRect.Height;
            float scaledOffset = originalOffset * _scale;
            Vector2 drawPos = new Vector2(Position.X, Position.Y + scaledOffset);

            spriteBatch.Draw(Texture, drawPos, currentRect, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }

        public void Die() => OnDeath?.Invoke();
    }
}