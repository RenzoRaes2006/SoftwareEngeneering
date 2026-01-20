using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.CharacterStates
{
    public class FallingState : IHeroState
    {
        public void Enter(IHeroInterface hero) { }

        public void HandleInput(ICommand command, IHeroInterface hero)
        {
            if (command is Patterns.JumpCommand) return;
            command.Execute(hero);
        }

        public void Update(IHeroInterface hero, GameTime gameTime) { }
    }
}