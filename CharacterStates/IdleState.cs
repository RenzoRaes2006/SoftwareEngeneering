using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.CharacterStates
{
    public class IdleState : IHeroState
    {
        public void Enter(IMovable movable)
        {
            var hero = movable as Hero;
            // 1. Zet direct stil
            hero.Velocity = new Vector2(0, hero.Velocity.Y);
            // 2. Start de animatie
            hero.SetIdleAnimation();
        }

        public void HandleInput(ICommand command, IMovable movable)
        {
            var hero = movable as Hero;

            // Als we input krijgen om te bewegen -> Ga naar RunningState
            if (command is MoveLeftCommand || command is MoveRightCommand)
            {
                // Voer het alvast uit zodat we niet 1 frame vertraging hebben
                command.Execute(hero);

                // Wissel van state
                hero.CurrentState = new RunningState();
                hero.CurrentState.Enter(hero);
            }

            // Springen
            if (command is JumpCommand)
            {
                hero.PerformJump();
                hero.CurrentState = new JumpingState();
                hero.CurrentState.Enter(hero);
            }
        }

        public void Update(IMovable movable, GameTime gameTime)
        {
            var hero = movable as Hero;
            // Check of we nog op de grond staan (voor bewegende platformen of vallen)
            if (!CheckIfGrounded(hero))
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }

        // Jouw bestaande check methode (gekopieerd uit GroundedState)
        private bool CheckIfGrounded(Hero hero)
        {
            Rectangle footCheck = new Rectangle(hero.CollisionBox.X + 5, hero.CollisionBox.Bottom, hero.CollisionBox.Width - 10, 1);
            foreach (var obj in hero.LevelObjects)
            {
                if (obj == hero || obj is PowerUp || obj is Coin) continue;
                if (footCheck.Intersects(obj.CollisionBox)) return true;
            }
            return false;
        }
    }
}

