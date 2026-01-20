using SofEngeneering_project.Interfaces;
using System;

namespace SofEngeneering_project.Patterns
{
    public class JumpCommand : ICommand
    {
        public void Execute(IMovable movable)
        {
            if (movable is IHeroInterface hero)
            {

                if (Math.Abs(hero.Velocity.Y) < 0.1f)
                {
                    hero.WantsToJump = true;
                }
            }
        }
    }
}