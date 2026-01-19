using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface IMovementEnemy
    {
        Vector2 Move(Vector2 currentPosition, GameTime gameTime);

        void OnHorizontalCollision();
    }
}
