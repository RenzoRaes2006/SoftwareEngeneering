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
    public class RunningState : IHeroState
    {
        public void Enter(IMovable movable)
        {
            var hero = movable as Hero;
            hero.SetRunAnimation();
        }

        public void HandleInput(ICommand command, IMovable movable)
        {
            var hero = movable as Hero;

            // Blijf bewegen
            if (command is MoveLeftCommand || command is MoveRightCommand)
            {
                command.Execute(hero);
            }

            // BELANGRIJK: Als we 'IdleCommand' krijgen (geen toetsen ingedrukt) -> Ga naar Idle
            if (command is IdleCommand)
            {
                hero.CurrentState = new IdleState();
                hero.CurrentState.Enter(hero);
            }

            // Springen tijdens rennen
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
            // Val check
            if (!CheckIfGrounded(hero))
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }

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
