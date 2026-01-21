using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Behaviors
{
    public class PatrolEnemyBehavior : IMovementEnemy
    {
        private float _normalSpeed, _currentSpeed, _fleeTimer;
        private bool _movingRight = true, _isFleeing = false;
        private int _width, _height, _sensorOffset;
        private List<IGameObject> _levelObjects;

        public PatrolEnemyBehavior(float speed, int width, int height, List<IGameObject> levelObjects, int sensorOffset = 15)
        {
            _normalSpeed = _currentSpeed = speed;
            _width = width;
            _height = height;
            _levelObjects = levelObjects;
            _sensorOffset = sensorOffset;
        }

        public void PanicAndRun(bool runToRight)
        {
            _isFleeing = true;
            _fleeTimer = 1.2f;
            _currentSpeed = _normalSpeed * 2.5f;
            _movingRight = runToRight;
        }

        public void UpdateStats(int currentHP, int maxHP)
        {
            if (currentHP < maxHP * 0.4f) 
                _normalSpeed = 3.0f;
        }

        public Vector2 Move(Vector2 currentPosition, GameTime gameTime)
        {
            if (_isFleeing)
            {
                _fleeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_fleeTimer <= 0) 
                { 
                    _isFleeing = false;
                    _currentSpeed = _normalSpeed; 
                }
            }

            int sensorX = _movingRight ? (int)currentPosition.X + _width + _sensorOffset : (int)currentPosition.X - _sensorOffset;

            Rectangle groundSensor = new Rectangle(sensorX, (int)currentPosition.Y + _height - 25, 10, 45);

            bool groundAhead = false;
            foreach (var obj in _levelObjects)
            {
                if (obj is Block || (obj is BigWall wall && wall.IsActive))
                {
                    if (groundSensor.Intersects(obj.CollisionBox)) { groundAhead = true; break; }
                }
            }

            if (!groundAhead) { _movingRight = !_movingRight; return currentPosition; }

            float nextX = _movingRight ? currentPosition.X + _currentSpeed : currentPosition.X - _currentSpeed;
            return new Vector2(nextX, currentPosition.Y);
        }

        public void OnHorizontalCollision() => _movingRight = !_movingRight;
    }
}