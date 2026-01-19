using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;

namespace SofEngeneering_project.CharacterStates
{
    public class RunningState : IHeroState
    {
        public void Enter(IHeroInterface hero)
        {
            hero.SetRunAnimation();
        }

        public void HandleInput(ICommand command, IHeroInterface hero)
        {
            // Blijf bewegen
            if (command is MoveLeftCommand || command is MoveRightCommand)
            {
                command.Execute(hero);
            }

            // Als we 'IdleCommand' krijgen (geen toetsen) -> Ga naar Idle
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

        public void Update(IHeroInterface hero, GameTime gameTime)
        {
            if (!CheckIfGrounded(hero))
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }

        private bool CheckIfGrounded(IHeroInterface hero)
        {
            Rectangle footCheck = new Rectangle(hero.CollisionBox.X + 5, hero.CollisionBox.Bottom, hero.CollisionBox.Width - 10, 1);
            foreach (var obj in hero.LevelObjects)
            {
                if (obj == hero || obj is PowerUp || obj is Coin || obj is Enemy || obj is Trap) continue;
                if (obj is BigWall gate && !gate.IsActive) continue;

                if (footCheck.Intersects(obj.CollisionBox)) return true;
            }
            return false;
        }
    }
}