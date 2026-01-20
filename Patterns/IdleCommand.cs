using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public class IdleCommand : ICommand
    {
        public void Execute(IMovable movable)
        {
            movable.Velocity = new Vector2(0, movable.Velocity.Y);
        }
    }
}
