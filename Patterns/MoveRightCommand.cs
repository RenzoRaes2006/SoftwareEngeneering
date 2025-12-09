using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public class MoveRightCommand: ICommand
    {
        public void Execute(IMovable hero) => hero.Velocity = new Vector2(5, hero.Velocity.Y);
    }
}
