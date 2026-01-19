using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SofEngeneering_project.Interfaces
{
    public interface IHeroInterface : IMovable
    {
        // State Machine
        IHeroState CurrentState { get; set; }
        IJumpStrategy JumpStrategy { get; set; }

        // Input & Physics flags
        bool WantsToJump { get; set; }

        // Data voor checks
        List<IGameObject> LevelObjects { get; }
        Rectangle CollisionBox { get; }

        // Acties
        void PerformJump();
        void SetRunAnimation();
        void SetIdleAnimation();
    }
}