using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SofEngeneering_project.Behaviors
{
    public class GateKeeper : IGameObject
    {
        private List<BigWall> _gates;
        private List<Enemy> _enemiesToWatch;
        private bool _alreadyOpened = false;

        public Rectangle CollisionBox => Rectangle.Empty;

        public GateKeeper(List<BigWall> gates, List<Enemy> enemiesToWatch)
        {
            _gates = gates;
            _enemiesToWatch = enemiesToWatch;
        }

        public void Update(GameTime gameTime)
        {
            if (_alreadyOpened) return;

            if (_enemiesToWatch.Count > 0 && _enemiesToWatch.All(e => e.IsDead))
            {
                foreach (var gate in _gates)
                {
                    gate.IsActive = false;
                }
                _alreadyOpened = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) { }
    }
}