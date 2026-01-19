using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using System;

namespace SofEngeneering_project.CharacterStates
{
    public class FallingState : IHeroState
    {
        public void Enter(IMovable _hero) { /* Optioneel: speel val geluid/animatie */ }

        public void HandleInput(ICommand command, IMovable movable)
        {
            var hero = movable as Hero;

            // In FallingState negeren we Jump (geen dubbele sprong)
            if (command is JumpCommand)
            {
                return;
            }

            // MoveLeft en MoveRight en Idle mogen wel (sturen in de lucht)
            command.Execute(hero);
        }

        public void Update(IMovable movable, GameTime gameTime)
        {
            var hero = movable as Hero;

            // ALLEEN ZWAARTEKRACHT
            // Geen botsingslogica hier! Dat doet Hero.cs
            hero.Velocity += new Vector2(0, 0.5f);
        }
    }
}