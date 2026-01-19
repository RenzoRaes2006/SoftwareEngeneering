using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Patterns
{
    public class JumpCommand : ICommand
    {
        public void Execute(IMovable movable)
        {
            // We checken of het object een Hero is (via de interface)
            if (movable is IHeroInterface hero)
            {
                hero.WantsToJump = true;
            }
        }
    }
}