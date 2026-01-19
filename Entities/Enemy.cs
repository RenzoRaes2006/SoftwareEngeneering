using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Enemy : IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle CollisionBox { get; private set; }

        // --- ANIMATIE ---
        private List<Rectangle> _frames;
        private int _currentFrame = 0;
        private float _timer = 0f;
        private float _frameSpeed = 0.2f;
        private int _maxHeight;

        // --- SCHAAL & COLLISION ---
        private float _scale;
        private List<IGameObject> _levelObjects; // Referentie naar de wereld

        // --- GEDRAG ---
        private IMovementEnemy _moveBehavior;

        // Constructor aangepast: accepteert nu 'levelObjects'
        public Enemy(Texture2D texture, Vector2 startPosition, List<Rectangle> frames, IMovementEnemy moveBehavior, float scale, List<IGameObject> levelObjects)
        {
            Texture = texture;
            Position = startPosition;
            _frames = frames;
            _moveBehavior = moveBehavior;
            _scale = scale;
            _levelObjects = levelObjects;

            // 1. Bereken max hoogte voor hitbox
            _maxHeight = 0;
            foreach (var f in frames)
            {
                if (f.Height > _maxHeight) _maxHeight = f.Height;
            }

            // 2. Initialiseer hitbox
            UpdateCollisionBox();
        }

        public void Update(GameTime gameTime)
        {
            Vector2 oldPosition = Position;

            // 1. BEWEGEN (Vraag nieuwe positie aan Behavior)
            Position = _moveBehavior.Move(Position, gameTime);

            // Update hitbox tijdelijk om te checken op nieuwe plek
            UpdateCollisionBox();

            // 2. BOTSING CHECKEN MET MUREN
            foreach (var obj in _levelObjects)
            {
                // Negeer zichzelf, coins en powerups (daar lopen we doorheen)
                if (obj == this || obj is Coin || obj is PowerUp || obj is Hero) continue;

                if (CollisionBox.Intersects(obj.CollisionBox))
                {
                    // -- BOTSING GEVONDEN --

                    // A. Correctie: Zet de enemy strak tegen de muur aan
                    if (Position.X > oldPosition.X) // We bewogen naar rechts
                    {
                        // Zet rechtskant van enemy tegen linkerkant van muur
                        Position = new Vector2(obj.CollisionBox.Left - CollisionBox.Width, Position.Y);
                    }
                    else if (Position.X < oldPosition.X) // We bewogen naar links
                    {
                        // Zet linkerkant van enemy tegen rechterkant van muur
                        Position = new Vector2(obj.CollisionBox.Right, Position.Y);
                    }

                    // B. Update hitbox op de gecorrigeerde positie
                    UpdateCollisionBox();

                    // C. Strategie Triggeren: "Draai om!"
                    _moveBehavior.OnHorizontalCollision();

                    break; // Stop loop na eerste botsing
                }
            }

            // 3. ANIMEREN
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

            // Anti-stuiter correctie met schaal
            float originalOffset = _maxHeight - currentRect.Height;
            float scaledOffset = originalOffset * _scale;

            Vector2 drawPos = new Vector2(Position.X, Position.Y + scaledOffset);

            spriteBatch.Draw(Texture, drawPos, currentRect, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}