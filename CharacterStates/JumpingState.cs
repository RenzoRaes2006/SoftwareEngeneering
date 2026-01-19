using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;

namespace SofEngeneering_project.CharacterStates
{
    public class JumpingState : IHeroState
    {
        public void Enter(IHeroInterface hero) { }

        public void HandleInput(ICommand command, IHeroInterface hero)
        {
            // Niet dubbel springen
            if (command is JumpCommand) return;

            // Bewegen in de lucht mag wel
            command.Execute(hero);
        }

        public void Update(IHeroInterface hero, GameTime gameTime)
        {
            // Zwaartekracht toepassen
            hero.Velocity += new Vector2(0, 0.5f);

            // Als we naar beneden gaan vallen (Velocity > 0)
            if (hero.Velocity.Y > 0)
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }
    }
}