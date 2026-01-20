using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.Patterns
{
    public class NormalJumpStrategy : IJumpStrategy
    {
        public float CalculateJump(float currentVelocityY)
        {

            return -10f;
        }
    }
}