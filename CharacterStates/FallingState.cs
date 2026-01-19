using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;

namespace SofEngeneering_project.CharacterStates
{
    public class FallingState : IHeroState
    {
        public void Enter(IHeroInterface hero) { }

        public void HandleInput(ICommand command, IHeroInterface hero)
        {
            // Niet springen tijdens vallen
            if (command is JumpCommand) return;

            // Sturen in de lucht mag
            command.Execute(hero);
        }

        public void Update(IHeroInterface hero, GameTime gameTime)
        {
            // Zwaartekracht
            hero.Velocity += new Vector2(0, 0.5f);

            // De landing check gebeurt in Hero.Update (via PhysicsService) 
            // of hier als je logic strikt wilt scheiden. 
            // In jouw architectuur regelt PhysicsService de positie correctie 
            // en Hero.Update() zet de state naar GroundedState als Velocity.Y 0 wordt.
        }
    }
}