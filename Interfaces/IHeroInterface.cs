using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SofEngeneering_project.Interfaces
{
    public interface IHeroInterface : IMovable
    {
        IHeroState CurrentState { get; set; }
        IJumpStrategy JumpStrategy { get; set; }

        bool WantsToJump { get; set; }

        List<IGameObject> LevelObjects { get; }
        Rectangle CollisionBox { get; }

        void PerformJump();
        void SetRunAnimation();
        void SetIdleAnimation();
    }
}