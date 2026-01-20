using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SofEngeneering_project.Animaties
{
    public class Animatie
    {
        public Texture2D Texture { get; private set; }
        public int CurrentFrame { get; private set; }
        public bool Flip { get; set; }

        private float _frameTime = 0.15f;
        private float _timer;

        private int[] _xPositions;
        private int _fixedY;
        private int _frameWidth;
        private int _frameHeight;

        public Rectangle SourceRect
        {
            get
            {
                if (_xPositions != null && _xPositions.Length > 0)
                {
                    int x = _xPositions[CurrentFrame];
                    return new Rectangle(x, _fixedY, _frameWidth, _frameHeight);
                }
                return Rectangle.Empty;
            }
        }

        public Animatie(Texture2D texture)
        {
            Texture = texture;
        }

        public void SetAnimationCoordinates(int[] xPositions, int y, int w, int h)
        {
            if (_xPositions != xPositions)
            {
                _xPositions = xPositions;
                _fixedY = y;
                _frameWidth = w;
                _frameHeight = h;
                CurrentFrame = 0;
                _timer = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_xPositions == null || _xPositions.Length == 0) return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _timer += delta;

            if (_timer >= _frameTime)
            {
                _timer = 0f;
                CurrentFrame++;
                if (CurrentFrame >= _xPositions.Length) CurrentFrame = 0;
            }
        }
    }
}