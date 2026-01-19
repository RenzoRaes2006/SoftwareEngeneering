using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Enemy : IGameObject
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public Rectangle CollisionBox { get; private set; }

        public event Action OnDeath;

        // --- ANIMATIE ---
        private List<Rectangle> _frames;
        private int _currentFrame = 0;
        private float _timer = 0f;
        private float _frameSpeed = 0.2f;
        private int _maxHeight;

        // --- SCHAAL & COLLISION ---
        private float _scale;

        // We maken deze public (property), handig voor debugging of behaviors
        public List<IGameObject> LevelObjects { get; private set; }

        // --- GEDRAG ---
        private IMovementEnemy _moveBehavior;

        // Constructor
        public Enemy(Texture2D texture, Vector2 startPosition, List<Rectangle> frames, IMovementEnemy moveBehavior, float scale, List<IGameObject> levelObjects)
        {
            Texture = texture;
            Position = startPosition;
            _frames = frames;
            _moveBehavior = moveBehavior;
            _scale = scale;
            LevelObjects = levelObjects;

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
            foreach (var obj in LevelObjects)
            {
                // A. Negeer jezelf
                if (obj == this) continue;

                // B. FILTER: Negeer alles waar we doorheen mogen lopen.
                // HIER ZIT DE FIX: We hebben 'Enemy' en 'Trap' toegevoegd.
                // Nu botst hij NIET meer tegen de Grote Paarse Slime.
                if (obj is Coin || obj is PowerUp || obj is Hero || obj is Enemy || obj is Trap)
                {
                    continue;
                }

                // C. Check botsing met wat overblijft (dus Block en GateBlock)
                if (CollisionBox.Intersects(obj.CollisionBox))
                {
                    // -- BOTSING MET MUUR --

                    // Correctie: Zet de enemy strak tegen de muur aan
                    if (Position.X > oldPosition.X) // We bewogen naar rechts
                    {
                        Position = new Vector2(obj.CollisionBox.Left - CollisionBox.Width, Position.Y);
                    }
                    else if (Position.X < oldPosition.X) // We bewogen naar links
                    {
                        Position = new Vector2(obj.CollisionBox.Right, Position.Y);
                    }

                    // Update hitbox na de correctie
                    UpdateCollisionBox();

                    // Vertel de Behavior dat we botsten (zodat hij kan omdraaien)
                    _moveBehavior.OnHorizontalCollision();

                    break; // Klaar, we hebben de muur gevonden
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

        public void Die()
        {
            // Roep iedereen aan die luistert (de GateKeeper)
            OnDeath?.Invoke();
        }
    }
}