using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.States; // Zorg dat JumpingState/FallingState hierin staan
using System;

namespace SofEngeneering_project.States
{
    public class GroundedState : IHeroState
    {
        public void Enter(IMovable movable)
        {
            var hero = movable as Hero;

            // Stop verticale snelheid bij landen
            hero.Velocity = new Vector2(hero.Velocity.X, 0);

            // Zet animatie op stilstaan
            hero.SetIdleAnimation();
        }

        public void HandleInput(ICommand command, IMovable movable)
        {
            command.Execute(movable);
            var hero = movable as Hero;

            // KIES ANIMATIE OP BASIS VAN SNELHEID
            // We gebruiken hier de nieuwe hulpmethodes van Hero
            if (Math.Abs(hero.Velocity.X) > 0.1f)
            {
                hero.SetRunAnimation();
            }
            else
            {
                hero.SetIdleAnimation();
            }

            // SPRINGEN
            if (hero.WantsToJump)
            {
                hero.PerformJump();

                // Wissel naar JumpingState
                hero.CurrentState = new JumpingState();
                hero.CurrentState.Enter(hero);
            }
        }

        public void Update(IMovable movable, GameTime gameTime)
        {
            var hero = movable as Hero;

            // --- CHECK OF WE NOG OP DE GROND STAAN ---
            Rectangle currentHitbox = hero.CollisionBox;

            // We checken een dun lijntje (1px hoog) precies onder de voeten
            Rectangle footCheck = new Rectangle(
                currentHitbox.X + 5,
                currentHitbox.Bottom,
                currentHitbox.Width - 10,
                1
            );

            bool onGround = false;
            foreach (var obj in hero.LevelObjects)
            {
                if (obj == hero) continue;
                if (obj is PowerUp) continue;

                if (footCheck.Intersects(obj.CollisionBox))
                {
                    onGround = true;
                    break;
                }
            }

            // Als we geen grond meer voelen, vallen we
            if (!onGround)
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }
    }
}