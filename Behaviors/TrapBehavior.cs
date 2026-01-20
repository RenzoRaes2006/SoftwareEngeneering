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
        private float _speed;
        private float _minX;
        private float _maxX; 
        private bool _movingRight = true;

        public TrapBehavior(float minX, float maxX, float speed)
        {
            _minX = minX;
            _maxX = maxX;
            _speed = speed;
        }

        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            float x = currentPosition.X;

            // Beweging
            if (_movingRight)
            {
                x += _speed;
                if (x >= _maxX)
                {
                    x = _maxX;
                    _movingRight = false; // omdraaien
                }
            }
            else
            {
                x -= _speed;
                if (x <= _minX)
                {
                    x = _minX;
                    _movingRight = true;  // omdraaien
                }
            }

            return new Vector2(x, currentPosition.Y);
        }

        public void OnHorizontalCollision()
        {
        }
    }
}
