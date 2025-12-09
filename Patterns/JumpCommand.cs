using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public class JumpCommand: ICommand
    {
        public void Execute(IMovable hero)
        {
            // We geven alleen het signaal door aan de Hero class
            if (hero is Hero h) h.WantsToJump = true;
        }

    }
}
