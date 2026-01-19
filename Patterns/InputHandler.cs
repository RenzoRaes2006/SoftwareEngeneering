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

        // Caching van commands (Optimization)
        private ICommand _moveLeft = new MoveLeftCommand();
        private ICommand _moveRight = new MoveRightCommand();
        private ICommand _jump = new JumpCommand();
        private ICommand _idle = new IdleCommand();

        public List<ICommand> GetCommands()
        {
            var commands = new List<ICommand>();
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Q))
                commands.Add(_moveLeft);
            else if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                commands.Add(_moveRight);
            else
                commands.Add(_idle);

            if (state.IsKeyDown(Keys.Space) || state.IsKeyDown(Keys.Z))
                commands.Add(_jump);

            return commands;
        }
    }
}
