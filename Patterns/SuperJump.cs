using SofEngeneering_project.Decorator;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public class SuperJump : JumpDecorator
    {
        public SuperJump(IJumpStrategy strategy) : base(strategy)
        {
        }

        public override float CalculateJump(float currentVelocityY)
        {
            // Decorator patroon: Voer origineel uit en doe er iets extra's mee (x 1.5)
            return base.CalculateJump(currentVelocityY) * 1.5f;
        }
    }
}
