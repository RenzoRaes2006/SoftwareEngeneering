using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Entities
{
    public class GateKeeper : IGameObject
    {
        private List<BigWall> _wallBlocks;
        private int _enemiesToKillCount;

        // Timer logica
        private bool _isUnlocking = false;
        private float _timer = 2.0f;
        private float _flickerInterval = 0.1f;

        public Rectangle CollisionBox => Rectangle.Empty; // Heeft zelf geen hitbox

        public GateKeeper(List<BigWall> wallBlocks, List<Enemy> enemiesToWatch)
        {
            _wallBlocks = wallBlocks;
            _enemiesToKillCount = enemiesToWatch.Count;

            // Abonneer op de dood van elke specifieke slime
            foreach (var enemy in enemiesToWatch)
            {
                enemy.OnDeath += OnEnemyDied;
            }
        }

        private void OnEnemyDied()
        {
            _enemiesToKillCount--;
            if (_enemiesToKillCount <= 0)
            {
                // Allemaal dood! Start de timer.
                _isUnlocking = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_isUnlocking)
            {
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Knipper Effect
                if (_timer > 0)
                {
                    _flickerInterval -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_flickerInterval <= 0)
                    {
                        // Toggle zichtbaarheid van alle blokken
                        foreach (var block in _wallBlocks) block.IsVisible = !block.IsVisible;
                        _flickerInterval = 0.1f; // Reset flicker snelheid
                    }
                }
                else
                {
                    // Tijd is op: Weg met de muur!
                    foreach (var block in _wallBlocks)
                    {
                        block.IsVisible = false;
                        block.IsActive = false; // Hero kan er nu doorheen
                    }
                    _isUnlocking = false; // Stop update loop
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) { /* Onzichtbaar */ }
    }
}
