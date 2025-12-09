using Microsoft.Xna.Framework.Input;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public class InputHandler
    {

        public List<ICommand> GetCommands()
        {
            var commands = new List<ICommand>();
            var state = Keyboard.GetState();

            // Horizontale beweging
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Q))
                commands.Add(new MoveLeftCommand());
            else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                commands.Add(new MoveRightCommand());
            else
                commands.Add(new IdleCommand());

            // Springen
            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Z))
                commands.Add(new JumpCommand());

            return commands;
        }
    }
}
