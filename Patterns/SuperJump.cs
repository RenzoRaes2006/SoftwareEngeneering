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
        public override float CalculateJump(float velocity)
        {
            return base.CalculateJump(velocity) * 1.5f;
        }
    }
}
