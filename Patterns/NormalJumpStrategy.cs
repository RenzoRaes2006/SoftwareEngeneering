using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Patterns
{
    public class NormalJumpStrategy : IJumpStrategy
    {
        public float CalculateJump(float currentVelocityY)
        {
            return -12f; // Springkracht
        }
    }
}
