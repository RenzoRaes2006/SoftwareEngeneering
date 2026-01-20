using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using System;

namespace SofEngeneering_project.CharacterStates
{
    public class GroundedState : IHeroState
    {
        public void Enter(IHeroInterface hero)
        {
            hero.Velocity = new Vector2(hero.Velocity.X, 0);
            hero.SetIdleAnimation();
        }

        public void HandleInput(ICommand command, IHeroInterface hero)
        {
            command.Execute(hero);

            if (hero.WantsToJump)
            {
                hero.PerformJump();
                hero.WantsToJump = false;
                hero.CurrentState = new JumpingState();
                hero.CurrentState.Enter(hero);
            }
        }

        public void Update(IHeroInterface hero, GameTime gameTime) { }
    }
}