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
        private float _minX; // Linkergrens
        private float _maxX; // Rechtergrens
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
                // Hebben we de rechtergrens bereikt?
                if (x >= _maxX)
                {
                    x = _maxX;           // Zet hem strak op de grens
                    _movingRight = false; // Draai om
                }
            }
            else
            {
                x -= _speed;
                // Hebben we de linkergrens bereikt?
                if (x <= _minX)
                {
                    x = _minX;           // Zet hem strak op de grens
                    _movingRight = true;  // Draai om
                }
            }

            // De Y-positie blijft hetzelfde, die verandert niet
            return new Vector2(x, currentPosition.Y);
        }

        public void OnHorizontalCollision()
        {
        }
    }
}
