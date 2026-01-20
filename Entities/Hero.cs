using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Animaties;
using SofEngeneering_project.CharacterStates;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Hero : IGameObject, IHeroInterface, ISubject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int CoinsRemaining { get; set; } = 0;

        public IHeroState CurrentState { get; set; }
        public IJumpStrategy JumpStrategy { get; set; }

        private List<IObserver> _observers = new List<IObserver>();
        public Animatie Animatie { get; private set; }
        public List<IGameObject> LevelObjects { get; set; }

        public bool WantsToJump { get; set; }
        public bool IsLethalJump { get; set; } = false;

        // PowerUp
        private float _powerUpTimer = 0f;
        private bool _hasPowerUp = false;

        // Hitbox & Animatie Data
        private int _hitboxWidth = 25;
        private int _hitboxHeight = 36;
        public Rectangle CollisionBox => new Rectangle((int)Position.X, (int)Position.Y, _hitboxWidth, _hitboxHeight);

        private int[] _idleXCoords = { 18, 82, 146, 210 };
        private int[] _runXCoords = { 16, 82, 146, 210, 272, 338, 402, 466 };


        public Hero(Texture2D texture, List<IGameObject> levelObjects)
        {
            Animatie = new Animatie(texture);
            LevelObjects = levelObjects;
            Position = new Vector2(50, 200);

            JumpStrategy = new NormalJumpStrategy();
            SetIdleAnimation();
            CurrentState = new IdleState();
            CurrentState.Enter(this);
        }

        public void Update(GameTime gameTime)
        {
            // 1. PowerUp Timer
            if (_hasPowerUp)
            {
                _powerUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                NotifyObservers(); // Update de HUD elke frame voor de timer!
                if (_powerUpTimer <= 0) RemoveSuperJump();
            }

            // 2. Update State
            CurrentState.Update(this, gameTime);
            Animatie.Update(gameTime);

            // 3. Physics Handmatig
            ApplyPhysics();

            // 4. Flip sprite
            if (Velocity.X > 0) Animatie.Flip = false;
            else if (Velocity.X < 0) Animatie.Flip = true;

            // 5. CRUCIAAL: Reset jump flag PAS AAN HET EINDE
            WantsToJump = false;
        }

        private void ApplyPhysics()
        {
            // X-as beweging
            Position += new Vector2(Velocity.X, 0);
            Rectangle myRect = CollisionBox;
            foreach (var obj in LevelObjects)
            {
                if (obj == this || obj is Coin || obj is PowerUp || obj is Enemy || obj is Trap) continue;
                if (obj is BigWall gate && !gate.IsActive) continue;

                if (myRect.Intersects(obj.CollisionBox))
                {
                    if (Velocity.X > 0) Position = new Vector2(obj.CollisionBox.Left - _hitboxWidth, Position.Y);
                    else if (Velocity.X < 0) Position = new Vector2(obj.CollisionBox.Right, Position.Y);
                }
            }

            // Y-as beweging
            Position += new Vector2(0, Velocity.Y);
            myRect = CollisionBox;
            bool onGround = false;

            foreach (var obj in LevelObjects)
            {
                if (obj == this || obj is Coin || obj is PowerUp || obj is Enemy || obj is Trap) continue;
                if (obj is BigWall gate && !gate.IsActive) continue;

                if (myRect.Intersects(obj.CollisionBox))
                {
                    // Alleen landen als we vallen (Velocity > 0)
                    if (Velocity.Y > 0)
                    {
                        Rectangle overlap = Rectangle.Intersect(myRect, obj.CollisionBox);
                        // Check of we echt op de bovenkant landen
                        if (myRect.Bottom - overlap.Height <= obj.CollisionBox.Top + 10)
                        {
                            Position = new Vector2(Position.X, obj.CollisionBox.Top - _hitboxHeight);
                            Velocity = new Vector2(Velocity.X, 0);
                            onGround = true;
                        }
                    }
                }
            }

            // State update na landing
            if (onGround)
            {
                if (IsLethalJump) IsLethalJump = false;

                if (CurrentState is FallingState || CurrentState is JumpingState)
                {
                    CurrentState = new GroundedState();
                    CurrentState.Enter(this);
                }
            }
            // Als we vallen (Velocity > 0) en we zijn lethaljump aan het doen
            else if (Velocity.Y > 0)
            {
                IsLethalJump = true;
            }
        }

        public void PerformJump()
        {
            float jumpForce = JumpStrategy.CalculateJump(Velocity.Y);
            Velocity = new Vector2(Velocity.X, jumpForce);
            if (_hasPowerUp) IsLethalJump = true;
        }

        public void EnableSuperJump()
        {
            if (!_hasPowerUp)
            {
                JumpStrategy = new SuperJump(JumpStrategy);
                _hasPowerUp = true;
            }
            _powerUpTimer = 2.0f; // 2 seconden
            NotifyObservers();
        }

        private void RemoveSuperJump()
        {
            if (_hasPowerUp)
            {
                JumpStrategy = new NormalJumpStrategy();
                _hasPowerUp = false;
                IsLethalJump = false;
                NotifyObservers();
            }
        }

        public void RegisterObserver(IObserver observer) => _observers.Add(observer);
        public void RemoveObserver(IObserver observer) => _observers.Remove(observer);

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
                observer.OnNotify(CoinsRemaining, _hasPowerUp, _powerUpTimer);
        }

        public void CollectCoin()
        {
            CoinsRemaining--;
            NotifyObservers();
        }

        public void Bounce()
        {
            Velocity = new Vector2(Velocity.X, -10f);
            IsLethalJump = true;
        }

        public void SetRunAnimation()
        {
            Animatie.SetAnimationCoordinates(_runXCoords, 148, 26, 36);
        }
        public void SetIdleAnimation()
        {
            Animatie.SetAnimationCoordinates(_idleXCoords, 18, 26, 38);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effect = Animatie.Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // VISUELE FIX: De Idle sprite is 38 hoog, Run is 36 hoog.
            int drawOffsetY = 0;
            if (CurrentState is IdleState) drawOffsetY = -2;

            Vector2 drawPos = new Vector2((int)Position.X, (int)Position.Y + drawOffsetY);

            spriteBatch.Draw(Animatie.Texture, drawPos, Animatie.SourceRect, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
        }
    }
}