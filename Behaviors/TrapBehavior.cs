using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Behaviors
{
    public class TrapBehavior : IMovementEnemy
    {
        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            // Doe niks, blijf waar je bent
            return currentPosition;
        }

        public void OnHorizontalCollision()
        {
            throw new NotImplementedException();
        }
    }
}
