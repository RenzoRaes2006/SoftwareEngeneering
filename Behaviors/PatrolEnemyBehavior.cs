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
        private bool _movingRight = true;

        // Geen start/eind coördinaten meer nodig!
        public PatrolEnemyBehavior(float speed)
        {
            _speed = speed;
        }

        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            float x = currentPosition.X;

            // Gewoon oneindig doorlopen... tot we botsen
            if (_movingRight) x += _speed;
            else x -= _speed;

            return new Vector2(x, currentPosition.Y);
        }

        // HIER GEBEURT HET:
        public void OnHorizontalCollision()
        {
            _movingRight = !_movingRight; // Draai de richting om
        }
    }
}

