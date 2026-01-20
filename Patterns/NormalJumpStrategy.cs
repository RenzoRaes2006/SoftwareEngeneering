using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Patterns
{
    public class NormalJumpStrategy : IJumpStrategy
    {
        public float CalculateJump(float currentVelocityY)
        {
            // Verhoogd naar -15 (was -12). 
            // Hoe negatiever, hoe harder je omhoog gaat.
            return -10f;
        }
    }
}