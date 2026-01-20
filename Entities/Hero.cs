using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Animaties;
using SofEngeneering_project.CharacterStates;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.Services;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Hero : IGameObject, IHeroInterface, ISubject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int CoinsRemaining { get; set; }
        public int Lives { get; private set; } = 3;

        public IHeroState CurrentState { get; set; }
        public IJumpStrategy JumpStrategy { get; set; }
        public Animatie Animatie { get; private set; }
        public List<IGameObject> LevelObjects { get; set; }
        public bool WantsToJump { get; set; }
        public bool IsLethalJump { get; set; }

        private List<IObserver> _observers = new List<IObserver>();
        private bool _isInvulnerable, _isVisible = true, _hasPowerUp;
        private float _invulnerableTimer, _blinkTimer, _powerUpTimer;

        public Rectangle CollisionBox => new Rectangle((int)Position.X, (int)Position.Y, 25, 36);

        public Hero(Texture2D texture, List<IGameObject> levelObjects)
        {
            Animatie = new Animatie(texture);
            LevelObjects = levelObjects;
            JumpStrategy = new NormalJumpStrategy();
            CurrentState = new IdleState();
            SetIdleAnimation();
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 1. Zwaartekracht
            Velocity += new Vector2(0, 0.45f);

            // 2. Input & States (Hier wordt WantsToJump verwerkt)
            CurrentState.Update(this, gameTime);
            Animatie.Update(gameTime);

            // 3. Physics
            PhysicsService.MoveX(this, LevelObjects);
            bool isGrounded = PhysicsService.MoveY(this, LevelObjects);

            // 4. State switching
            if (isGrounded)
            {
                if (CurrentState is JumpingState || CurrentState is FallingState)
                {
                    CurrentState = new GroundedState();
                    CurrentState.Enter(this);
                }
            }

            HandleTimers(delta);
            NotifyObservers();

            // CRUCIAAL: Reset de vlag elke frame
            WantsToJump = false;
        }

        private void ChangeState(IHeroState newState)
        {
            CurrentState = newState;
            CurrentState.Enter(this);
        }

        private void HandleTimers(float delta)
        {
            if (_isInvulnerable)
            {
                _invulnerableTimer -= delta;
                _blinkTimer += delta;
                if (_blinkTimer > 0.1f) { _isVisible = !_isVisible; _blinkTimer = 0; }
                if (_invulnerableTimer <= 0) { _isInvulnerable = false; _isVisible = true; }
            }
            if (_hasPowerUp)
            {
                _powerUpTimer -= delta;
                if (_powerUpTimer <= 0) { JumpStrategy = new NormalJumpStrategy(); _hasPowerUp = false; }
            }
        }

        public void SetRunAnimation() => Animatie.SetAnimationCoordinates(new[] { 16, 82, 146, 210, 272, 338, 402, 466 }, 148, 26, 36);
        public void SetIdleAnimation() => Animatie.SetAnimationCoordinates(new[] { 18, 82, 146, 210 }, 18, 26, 38);
        public void PerformJump() => Velocity = new Vector2(Velocity.X, JumpStrategy.CalculateJump(Velocity.Y));
        public bool TakeDamage() { if (_isInvulnerable) return false; Lives--; _isInvulnerable = true; _invulnerableTimer = 2.0f; Bounce(); return Lives <= 0; }
        public void Bounce() { Velocity = new Vector2(Velocity.X, -10f); }
        public void RegisterObserver(IObserver observer) => _observers.Add(observer);
        public void RemoveObserver(IObserver observer) => _observers.Remove(observer);
        public void NotifyObservers() { foreach (var o in _observers) o.OnNotify(CoinsRemaining, _hasPowerUp, _powerUpTimer, Lives); }
        public void CollectCoin() => CoinsRemaining--;
        public void EnableSuperJump() { JumpStrategy = new SuperJump(new NormalJumpStrategy()); _hasPowerUp = true; _powerUpTimer = 2.0f; }
        public void Draw(SpriteBatch sb) { if (_isVisible) sb.Draw(Animatie.Texture, Position, Animatie.SourceRect, Color.White, 0f, Vector2.Zero, 1f, Animatie.Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f); }
    }
}