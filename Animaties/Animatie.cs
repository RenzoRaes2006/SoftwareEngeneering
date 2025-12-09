using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Animaties
{
    public class Animatie
    {
        public Texture2D Texture { get; private set; }
        public int CurrentFrame { get; private set; }
        public int FrameCount { get; private set; }

        public bool Flip { get; set; }

        private float _frameTime = 0.15f;
        private float _timer;

        // AANGEPAST: We gebruiken nu exacte breedte/hoogte van de uitsnede
        private int _frameWidth;
        private int _frameHeight;

        // NIEUW: Lijst met X-posities en de vaste Y-positie
        private int[] _xPositions;
        private int _fixedY;
        private bool _useCoordinates = false; // Om te checken of we de nieuwe methode gebruiken

        public Animatie(Texture2D texture)
        {
            Texture = texture;
            // Standaard waarden om crashes te voorkomen
            _frameWidth = 64;
            _frameHeight = 64;
        }

        // NIEUWE METHODE: Gebruik deze voor jouw loop-animatie
        public void SetAnimationCoordinates(int[] xPositions, int y, int w, int h)
        {
            // Alleen resetten als we van animatie veranderen
            if (_xPositions != xPositions)
            {
                _xPositions = xPositions;
                _fixedY = y;
                _frameWidth = w;
                _frameHeight = h;

                FrameCount = xPositions.Length;
                CurrentFrame = 0;
                _timer = 0;
                _useCoordinates = true;
            }
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _frameTime)
            {
                _timer = 0f;
                CurrentFrame++;
                if (CurrentFrame >= FrameCount) CurrentFrame = 0;
            }
        }

        public Rectangle SourceRect
        {
            get
            {
                // Als we de nieuwe methode met exacte coördinaten gebruiken:
                if (_useCoordinates && _xPositions != null && _xPositions.Length > 0)
                {
                    int x = _xPositions[CurrentFrame];
                    return new Rectangle(x, _fixedY, _frameWidth, _frameHeight);
                }

                // Fallback naar de oude methode (als je die nog ergens anders voor gebruikt)
                return new Rectangle(0, 0, 64, 64);
            }
        }
    }
}
