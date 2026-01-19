using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Behaviors
{
    public class ConstantMoveBehavior : IMovementEnemy
    {
        private float _speed;

        public ConstantMoveBehavior(float speed)
        {
            _speed = speed;
        }

        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            // Altijd naar rechts bewegen
            return new Vector2(currentPosition.X + _speed, currentPosition.Y);
        }

        public void OnHorizontalCollision()
        {
            // Doe niks.

        }
    }
}
