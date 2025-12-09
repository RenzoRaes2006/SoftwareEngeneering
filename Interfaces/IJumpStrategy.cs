using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface IJumpStrategy
    {
        float CalculateJump(float currentVelocityY);
    }
}
