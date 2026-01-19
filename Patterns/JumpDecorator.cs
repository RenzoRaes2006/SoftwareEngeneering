using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public abstract class JumpDecorator : IJumpStrategy
    {
        protected IJumpStrategy _decoratedStrategy;

        public JumpDecorator(IJumpStrategy strategy)
        {
            _decoratedStrategy = strategy;
        }

        public virtual float CalculateJump(float currentVelocityY)
        {
            return _decoratedStrategy.CalculateJump(currentVelocityY);
        }
    }
}
