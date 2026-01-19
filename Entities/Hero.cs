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
        public bool HasSuperJump { get; private set; } = false;
        public bool IsLethalJump { get; private set; } = false;

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
            if (HasSuperJump)
            {
                PowerUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (PowerUpTimer <= 0 && !IsLethalJump) RemoveSuperJump();
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
            // LOGICA: Als we NU de powerup hebben, is deze sprong dodelijk tot we landen.
            if (HasSuperJump)
            {
                IsLethalJump = true;
            }
            else
            {
                IsLethalJump = false;
            }
        }

        public void EnableSuperJump()
        {
            if (!HasSuperJump)
            {
                JumpStrategy = new SuperJump(JumpStrategy);
                HasSuperJump = true;
            }
            PowerUpTimer = 2.0f;
        }

        private void RemoveSuperJump()
        {
            if (HasSuperJump)
            {
                JumpStrategy = new NormalJumpStrategy();
                HasSuperJump = false;
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
                if (obj == this || obj is PowerUp || obj is Coin || obj is Enemy ||obj is Trap) continue;
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
                // 1. Negeer objecten waar je doorheen mag (inclusief PowerUps!)
                if (obj == this || obj is PowerUp || obj is Coin || obj is Enemy || obj is Trap) continue;

                if (myRect.Intersects(obj.CollisionBox))
                {
                    // 2. SPRINGEN DOOR BLOKKEN (Velocity < 0)
                    // Als we omhoog gaan, doen we... NIETS.
                    // De 'continue' zorgt dat we de botsing negeren en er dwars doorheen vliegen.
                    if (Velocity.Y < 0)
                    {
                        continue;
                    }

                    // 3. LANDEN OP BLOKKEN (Velocity > 0)
                    // We mogen alleen landen als we aan het vallen zijn.
                    if (Velocity.Y > 0)
                    {
                        Rectangle overlap = Rectangle.Intersect(myRect, obj.CollisionBox);

                        // --- DE ECHTE FIX ---
                        // We moeten checken: "Raak ik het blok met mijn voeten?"
                        // Als we midden in een sprong zitten (en dus 'in' het blok zweven), 
                        // is onze onderkant (Bottom) veel lager dan de bovenkant van het blok.

                        // Formule: Zit de onderkant van de Hero (minus de overlap) ongeveer op de bovenkant van het blok?
                        // We geven 8 pixels speling.
                        if (myRect.Bottom - overlap.Height <= obj.CollisionBox.Top + 8)
                        {
                            // JA! We zitten er bovenop. Landen maar.
                            Position = new Vector2(Position.X, obj.CollisionBox.Top - CollisionBox.Height);
                            Velocity = new Vector2(Velocity.X, 0);

                            IsLethalJump = false;
                            if (PowerUpTimer <= 0 && HasSuperJump)
                            {
                                RemoveSuperJump();
                            }
                            // Wissel naar GroundedState
                            CurrentState = new GroundedState();
                            CurrentState.Enter(this);
                        }
                        // NEE? Dan zitten we er waarschijnlijk middenin (tijdens het omhoog springen).
                        // Doe niets en laat de speler gewoon vallen tot hij er wel op staat.
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