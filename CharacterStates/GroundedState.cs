using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System;

namespace SofEngeneering_project.CharacterStates
{
    public class GroundedState : IHeroState
    {
        public void Enter(IHeroInterface hero)
        {
            // Stop verticale snelheid
            hero.Velocity = new Vector2(hero.Velocity.X, 0);
            hero.SetIdleAnimation();
        }

        public void HandleInput(ICommand command, IHeroInterface hero)
        {
            command.Execute(hero);

            // Animatie wissel
            if (Math.Abs(hero.Velocity.X) > 0.1f)
            {
                hero.SetRunAnimation();
            }
            else
            {
                hero.SetIdleAnimation();
            }

            // Springen
            if (hero.WantsToJump)
            {
                hero.PerformJump();
                hero.CurrentState = new JumpingState();
                hero.CurrentState.Enter(hero);
            }
        }

        public void Update(IHeroInterface hero, GameTime gameTime)
        {
            // Ground check
            Rectangle currentHitbox = hero.CollisionBox;
            Rectangle footCheck = new Rectangle(currentHitbox.X + 5, currentHitbox.Bottom, currentHitbox.Width - 10, 1);

            bool onGround = false;
            foreach (var obj in hero.LevelObjects)
            {
                if (obj == hero || obj is PowerUp || obj is Coin || obj is Enemy || obj is Trap) continue;
                if (obj is BigWall gate && !gate.IsActive) continue;

                if (footCheck.Intersects(obj.CollisionBox))
                {
                    onGround = true;
                    break;
                }
            }

            if (!onGround)
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }
    }
}