using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ICommand = SofEngeneering_project.Interfaces.ICommand;


namespace SofEngeneering_project.Patterns
{
    public class MoveLeftCommand : ICommand
    {
        public void Execute(IMovable movable)
        {
            movable.Velocity = new Vector2(-5, movable.Velocity.Y);
            if (movable is Hero hero) hero.Animatie.Flip = true;
        }
    }
}
