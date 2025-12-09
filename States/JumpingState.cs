using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.States
{
    public class JumpingState: IHeroState
    {
        public void Enter(IMovable hero) { }

        public void HandleInput(ICommand command, IMovable hero)
        {
            // STATE PATTERN: De State beslist wat er mag.

            // We mogen niet nog eens springen in de lucht
            if (command is JumpCommand) return;

            // Maar we mogen WEL bewegen en stoppen (IdleCommand)
            command.Execute(hero);
        }

        public void Update(IMovable movable, GameTime gameTime)
        {
            var hero = movable as Hero;

            // Zwaartekracht
            hero.Velocity += new Vector2(0, 0.5f);

            // Als we naar beneden gaan vallen
            if (hero.Velocity.Y > 0)
            {
                hero.CurrentState = new FallingState();
                hero.CurrentState.Enter(hero);
            }
        }
    }
}
