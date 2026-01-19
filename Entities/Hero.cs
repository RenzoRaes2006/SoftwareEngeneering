using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Animaties;
using SofEngeneering_project.CharacterStates;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SofEngeneering_project.Entities
{
    public class Hero : IGameObject, IMovable
    {
        // =============================================================
        // 1. DATA & PROPERTIES
        // =============================================================

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        // HUD Data
        public int CoinsRemaining { get; set; } = 0;
        public float PowerUpTimer { get; private set; } = 0f;
        public float CoinFeedbackTimer { get; private set; } = 0f;

        // Interne flags
        private bool _hasSuperJump = false;
        private List<IGameObserver> _observers = new List<IGameObserver>();
        public bool WantsToJump { get; set; }

        // =============================================================
        // 2. ANIMATIE INSTELLINGEN (AANGEPAST OP JOUW NIEUWE METINGEN)
        // =============================================================

        // --- IDLE (Stilstaan) ---
        // We pakken de HOOGSTE Y-waarde (18) en de GROOTSTE hoogte (38).
        // Hierdoor passen alle frames erin.
        private int _idleSpriteY = 18;
        private int _idleWidth = 26;
        private int _idleHeight = 38;

        // Jouw gemeten X-coördinaten:
        private int[] _idleXCoords = { 18, 82, 146, 210 };

        // --- RUN (Rennen) ---
        // Standaard waarden voor de rennende ridder
        private int _runSpriteY = 148;
        private int _runWidth = 26;
        private int _runHeight = 36;
        private int[] _runXCoords = { 16, 82, 146, 210, 272, 338, 402, 466 };

        // =============================================================
        // 3. COLLISION BOX
        // =============================================================

        // We houden de hitbox consistent op de Run-hoogte (36) of Idle-hoogte (38).
        // 36 is veiliger om nergens in vast te komen zitten tijdens het rennen.
        private int _hitboxWidth = 25;
        private int _hitboxHeight = 36;
        private int _offsetX = 0;
        private int _offsetY = 0;

        public Rectangle CollisionBox => new Rectangle(
            (int)Position.X + _offsetX,
            (int)Position.Y + _offsetY,
            _hitboxWidth,
            _hitboxHeight
        );

        // =============================================================
        // 4. DEPENDENCIES
        // =============================================================
        public Animatie Animatie { get; private set; }
        public IHeroState CurrentState { get; set; }
        public IJumpStrategy JumpStrategy { get; set; }
        public List<IGameObject> LevelObjects { get; set; }

        // =============================================================
        // 5. CONSTRUCTOR
        // =============================================================
        public Hero(Texture2D texture, List<IGameObject> levelObjects)
        {
            Animatie = new Animatie(texture);

            Position = new Vector2(50, 200);
            LevelObjects = levelObjects;

            JumpStrategy = new NormalJumpStrategy();

            // Start in Idle
            CurrentState = new IdleState();
            CurrentState.Enter(this);
        }

        // =============================================================
        // 6. UPDATE
        // =============================================================
        public void Update(GameTime gameTime)
        {
            WantsToJump = false;

            // PowerUp
            if (_hasSuperJump)
            {
                PowerUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (PowerUpTimer <= 0) RemoveSuperJump();
            }

            // Animatie & State
            Animatie.Update(gameTime);
            CurrentState.Update(this, gameTime);

            // Fysica (Eerst X, dan Y)
            Position += new Vector2(Velocity.X, 0);
            HandleHorizontalCollision();

            Position += new Vector2(0, Velocity.Y);
            HandleVerticalCollision();

            // Objecten
            HandleObjectCollisions();

            // Flip sprite
            if (Velocity.X > 0) Animatie.Flip = false;
            else if (Velocity.X < 0) Animatie.Flip = true;

            // Feedback Timer
            if (CoinFeedbackTimer > 0)
                CoinFeedbackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        // =============================================================
        // 7. DRAW
        // =============================================================
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = Animatie.Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // --- CORRECTIE VOOR VERSCHILLENDE HOOGTES ---
            int drawOffsetY = 0;

            if (CurrentState is IdleState)
            {
                // Run is 36 hoog, Idle is 38 hoog.
                // 36 - 38 = -2.
                // Dit betekent dat we 2 pixels hoger moeten tekenen,
                // zodat de onderkant (de voeten) op dezelfde plek blijft.
                drawOffsetY = (_runHeight - _idleHeight);
            }

            Vector2 drawPos = new Vector2((int)Position.X, (int)Position.Y + drawOffsetY);

            spriteBatch.Draw(
                Animatie.Texture,
                drawPos,
                Animatie.SourceRect,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                effect,
                0f
            );
        }

        // =============================================================
        // 8. ANIMATIE SETTERS
        // =============================================================
        public void SetRunAnimation()
        {
            Animatie.SetAnimationCoordinates(_runXCoords, _runSpriteY, _runWidth, _runHeight);
        }

        public void SetIdleAnimation()
        {
            Animatie.SetAnimationCoordinates(_idleXCoords, _idleSpriteY, _idleWidth, _idleHeight);
        }

        // =============================================================
        // 9. GAMEPLAY (Jump & Powerups)
        // =============================================================
        public void PerformJump()
        {
            float jumpForce = JumpStrategy.CalculateJump(Velocity.Y);
            Velocity = new Vector2(Velocity.X, jumpForce);
        }

        public void EnableSuperJump()
        {
            if (!_hasSuperJump)
            {
                JumpStrategy = new SuperJump(JumpStrategy);
                _hasSuperJump = true;
            }
            PowerUpTimer = 5.0f;
        }

        private void RemoveSuperJump()
        {
            if (_hasSuperJump)
            {
                JumpStrategy = new NormalJumpStrategy();
                _hasSuperJump = false;
                PowerUpTimer = 0;
            }
        }

        public void AddObserver(IGameObserver observer) => _observers.Add(observer);

        // =============================================================
        // 10. COLLISION HANDLING
        // =============================================================
        private void HandleHorizontalCollision()
        {
            Rectangle myRect = CollisionBox;
            foreach (var obj in LevelObjects)
            {
                if (obj == this || obj is PowerUp || obj is Coin || obj is Enemy) continue;
                if (Velocity.Y < 0) continue; // One-way platform

                if (myRect.Intersects(obj.CollisionBox))
                {
                    if (Velocity.X > 0)
                        Position = new Vector2(obj.CollisionBox.Left - _hitboxWidth - _offsetX, Position.Y);
                    else if (Velocity.X < 0)
                        Position = new Vector2(obj.CollisionBox.Right - _offsetX, Position.Y);

                    Velocity = new Vector2(0, Velocity.Y);
                }
            }
        }

        private void HandleVerticalCollision()
        {
            Rectangle myRect = CollisionBox;
            foreach (var obj in LevelObjects)
            {
                if (obj == this || obj is PowerUp || obj is Coin || obj is Enemy) continue;
                if (Velocity.Y < 0) continue; // Spring door platformen heen

                if (myRect.Intersects(obj.CollisionBox))
                {
                    // LANDEN
                    if (Velocity.Y > 0)
                    {
                        // Zet positie op het blok
                        float newY = obj.CollisionBox.Top - _hitboxHeight - _offsetY;
                        Position = new Vector2(Position.X, newY);

                        Velocity = new Vector2(Velocity.X, 0);

                        // State switch: Als we vielen, ga nu naar Idle of Running
                        if (CurrentState is FallingState)
                        {
                            if (Math.Abs(Velocity.X) > 0.1f)
                            {
                                CurrentState = new RunningState();
                                CurrentState.Enter(this);
                            }
                            else
                            {
                                CurrentState = new IdleState();
                                CurrentState.Enter(this);
                            }
                        }
                    }
                }
            }
        }

        private void HandleObjectCollisions()
        {
            foreach (var obj in LevelObjects)
            {
                if (obj is PowerUp powerUp && !powerUp.IsCollected)
                {
                    if (CollisionBox.Intersects(powerUp.CollisionBox))
                    {
                        powerUp.IsCollected = true;
                        EnableSuperJump();
                    }
                }
                if (obj is Coin coin && !coin.IsCollected)
                {
                    if (CollisionBox.Intersects(coin.CollisionBox))
                    {
                        coin.IsCollected = true;
                        CoinsRemaining--;
                        CoinFeedbackTimer = 1f;
                    }
                }
            }
        }
    }
}