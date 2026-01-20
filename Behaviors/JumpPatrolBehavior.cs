using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Behaviors
{
    public class JumpPatrolBehavior : IMovementEnemy
    {
        private float _speed;
        private bool _movingRight = true;
        private int _width, _height;
        private List<IGameObject> _levelObjects;
        private float _jumpTimer = 2.0f;

        public JumpPatrolBehavior(float speed, int width, int height, List<IGameObject> levelObjects)
        {
            _speed = speed; _width = width; _height = height; _levelObjects = levelObjects;
        }

        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            _jumpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            float jumpImpulse = 0;

            if (_jumpTimer <= 0)
            {
                jumpImpulse = -12f;
                _jumpTimer = 2.0f;
            }

            int sensorX = _movingRight ? (int)currentPosition.X + _width : (int)currentPosition.X - 10;
            Rectangle wallSensor = new Rectangle(sensorX, (int)currentPosition.Y, 10, _height);

            foreach (var obj in _levelObjects)
            {
                if (obj is Block || (obj is BigWall wall && wall.IsActive))
                {
                    if (wallSensor.Intersects(obj.CollisionBox))
                    {
                        _movingRight = !_movingRight;
                        break;
                    }
                }
            }

            float nextX = _movingRight ? currentPosition.X + _speed : currentPosition.X - _speed;

            return new Vector2(nextX, jumpImpulse);
        }

        public void OnHorizontalCollision() => _movingRight = !_movingRight;
    }
}