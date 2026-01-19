using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Behaviors
{
    public class PatrolEnemyBehavior : IMovementEnemy
    {
        private float _speed;
        private bool _movingRight = true; // Houdt de richting bij

        public PatrolEnemyBehavior(float speed)
        {
            _speed = speed;
        }

        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Beweeg links of rechts op basis van de boolean
            if (_movingRight)
                return new Vector2(currentPosition.X + _speed, currentPosition.Y);
            else
                return new Vector2(currentPosition.X - _speed, currentPosition.Y);
        }

        // Deze wordt aangeroepen door Enemy.cs als er een MUUR geraakt wordt
        public void OnHorizontalCollision()
        {
            _movingRight = !_movingRight; // Draai om
        }
    }
}

