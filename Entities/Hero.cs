using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Animaties;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.States;
using System.Collections.Generic;
using System.Diagnostics; // Voor Debug.WriteLine

namespace SofEngeneering_project.Entities
{
    public class Hero : IGameObject, IMovable
    {
        // --- POSITIE & SNELHEID ---
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        // --- GAME DATA (Voor HUD) ---
        public int CoinsRemaining { get; set; } = 0;
        public float PowerUpTimer { get; private set; } = 0f; // Publiek voor HUD, private set
        public float CoinFeedbackTimer { get; private set; } = 0f;

        // --- INTERNE VARIABELEN ---
        private bool _hasSuperJump = false;
        private List<IGameObserver> _observers = new List<IGameObserver>();

        // --- ANIMATIE INSTILLINGEN ---
        // De exacte X-posities op de sprite sheet
        private int[] _walkXCoords = { 16, 82, 146, 210, 272, 338, 402, 466 };
        private int _spriteY = 148;
        private int _spriteWidth = 26;
        private int _spriteHeight = 36;

        // --- COLLISION SETTINGS ---
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

        // --- OBJECTEN & PATTERNS ---
        public Animatie Animatie { get; private set; }
        public IHeroState CurrentState { get; set; }
        public IJumpStrategy JumpStrategy { get; set; }
        public List<IGameObject> LevelObjects { get; set; }

        // Variabele voor states om te weten of er gesprongen moet worden
        public bool WantsToJump { get; set; }

        public Hero(Texture2D texture, List<IGameObject> levelObjects)
        {
            Animatie = new Animatie(texture);

            // Start animatie instellen
            SetIdleAnimation();

            Position = new Vector2(50, 200); // Startpositie
            LevelObjects = levelObjects;

            // Standaard strategie en state
            JumpStrategy = new NormalJumpStrategy();
            CurrentState = new FallingState();
        }

        // --- UPDATE LOOP ---
        public void Update(GameTime gameTime)
        {
            WantsToJump = false;

            // 1. POWERUP TIMER LOGICA
            if (_hasSuperJump)
            {
                PowerUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (PowerUpTimer <= 0)
                {
                    RemoveSuperJump();
                }
            }

            // 2. UPDATES
            Animatie.Update(gameTime);
            CurrentState.Update(this, gameTime);

            // 3. BEWEGING & PHYSICS
            // Horizontaal
            Position += new Vector2(Velocity.X, 0);
            HandleHorizontalCollision();

            // Verticaal
            Position += new Vector2(0, Velocity.Y);
            HandleVerticalCollision();

            // 4. OBJECT INTERACTIES (Coins & Powerups)
            HandleObjectCollisions();

            // 5. VISUELE RICHTING (Flip)
            if (Velocity.X > 0) Animatie.Flip = false;
            else if (Velocity.X < 0) Animatie.Flip = true;

            if (CoinFeedbackTimer > 0)
            {
                CoinFeedbackTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        // --- DRAW ---
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = Animatie.Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawPos = new Vector2((int)Position.X, (int)Position.Y);

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

        // --- METHODES VOOR ANIMATIES (Gebruikt door States) ---
        public void SetRunAnimation()
        {
            Animatie.SetAnimationCoordinates(_walkXCoords, _spriteY, _spriteWidth, _spriteHeight);
        }

        public void SetIdleAnimation()
        {
            int[] idleFrame = { _walkXCoords[0] };
            Animatie.SetAnimationCoordinates(idleFrame, _spriteY, _spriteWidth, _spriteHeight);
        }

        // --- JUMP LOGICA (Strategy Pattern) ---
        public void PerformJump()
        {
            // Gebruik de huidige strategie (Normal of Super)
            float jumpForce = JumpStrategy.CalculateJump(Velocity.Y);
            Velocity = new Vector2(Velocity.X, jumpForce);

            Notify("JUMP");
        }

        // --- POWERUP LOGICA (Decorator Pattern) ---
        public void EnableSuperJump()
        {
            if (!_hasSuperJump)
            {
                JumpStrategy = new SuperJump(JumpStrategy);
                _hasSuperJump = true;
                Debug.WriteLine("POWERUP: SuperJump Active!");
            }
            // Reset timer naar 5 seconden
            PowerUpTimer = 5.0f;
        }

        private void RemoveSuperJump()
        {
            if (_hasSuperJump)
            {
                JumpStrategy = new NormalJumpStrategy();
                _hasSuperJump = false;
                PowerUpTimer = 0;
                Debug.WriteLine("POWERUP: SuperJump Expired.");
            }
        }

        // --- OBSERVER PATTERN ---
        public void AddObserver(IGameObserver observer) => _observers.Add(observer);
        public void Notify(string eventName) { foreach (var obs in _observers) obs.OnNotify(eventName); }

        // --- COLLISION LOGICA ---

        // 1. Horizontale muren
        private void HandleHorizontalCollision()
        {
            Rectangle myRect = CollisionBox;
            foreach (var obj in LevelObjects)
            {
                if (obj == this) continue;
                if (obj is PowerUp || obj is Coin) continue;

                // --- DE OPLOSSING ---
                // 1. Verwijder/negeer de 'block.IsPlatform' check die je eerder had.

                // 2. Voeg deze check toe:
                // Als we OMHOOG springen (Velocity Y is negatief), negeren we muren.
                // Dit zorgt dat we soepel door de onderkant van blokken kunnen vliegen.
                if (Velocity.Y < 0)
                {
                    continue;
                }
                // --------------------

                if (myRect.Intersects(obj.CollisionBox))
                {
                    // Jouw bestaande botsing code (duw naar links/rechts)
                    if (Velocity.X > 0)
                    {
                        float newX = obj.CollisionBox.Left - _hitboxWidth - _offsetX;
                        Position = new Vector2(newX, Position.Y);
                    }
                    else if (Velocity.X < 0)
                    {
                        float newX = obj.CollisionBox.Right - _offsetX;
                        Position = new Vector2(newX, Position.Y);
                    }
                    Velocity = new Vector2(0, Velocity.Y);
                }
            }
        }
       

        // 2. Verticale botsing (Plafond check)
        // Let op: Landen op de grond wordt geregeld in de FallingState/GroundedState 'HasLanded' check.
        private void HandleVerticalCollision()
        {
            Rectangle myRect = CollisionBox;
            foreach (var obj in LevelObjects)
            {
                if (obj == this) continue;
                if (obj is PowerUp || obj is Coin) continue;

                // --- AANPASSING ---
                // Als we omhoog springen, willen we GEEN botsing met blokken (we vliegen erdoor).
                // Dus we skippen de rest van de loop als Y < 0.
                if (Velocity.Y < 0)
                {
                    continue;
                }
                // ------------------

                if (myRect.Intersects(obj.CollisionBox))
                {
                    // Omdat we hierboven al 'continue' doen bij Y < 0, 
                    // zal de code voor "hoofd stoten" nooit meer uitgevoerd worden op blokken.
                    // Dit is precies wat je wilt: door het plafond heen springen.

                    // Je kunt hier eventueel nog wel logica voor landen (Y > 0) laten staan
                    // als je dat hier regelt (maar meestal zit dat in je States).
                }
            }
        }

        // 3. Interacties met Items (Oppakken)
        private void HandleObjectCollisions()
        {
            foreach (var obj in LevelObjects)
            {
                // -- POWERUP --
                if (obj is PowerUp powerUp && !powerUp.IsCollected)
                {
                    if (CollisionBox.Intersects(powerUp.CollisionBox))
                    {
                        powerUp.IsCollected = true;
                        EnableSuperJump();
                        Notify("POWERUP_COLLECTED");
                    }
                }

                // -- COIN --
                if (obj is Coin coin && !coin.IsCollected)
                {
                    if (CollisionBox.Intersects(coin.CollisionBox))
                    {
                        coin.IsCollected = true;
                        CoinsRemaining--;
                        // NIEUW: Zet de timer op 1 seconde (1 seconde flikkeren)
                        CoinFeedbackTimer = 1f;
                    }
                }
            }
        }
    }
}