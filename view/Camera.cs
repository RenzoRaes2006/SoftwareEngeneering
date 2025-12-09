using Microsoft.Xna.Framework;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.view
{
    public class Camera : ICamera
    {
        public Matrix Transform { get; private set; }

        // NIEUW: De publieke positie
        public Vector2 Position { get; private set; }

        private int _screenWidth;
        private int _screenHeight;

        public Camera(int width, int height)
        {
            _screenWidth = width;
            _screenHeight = height;
        }

        public void Follow(IGameObject target)
        {
            var bounds = target.CollisionBox;
            float centerX = bounds.X + bounds.Width / 2f;
            float centerY = bounds.Y + bounds.Height / 2f;

            // NIEUW: Sla de huidige positie op zodat de achtergrond die kan lezen
            Position = new Vector2(centerX, centerY);

            var position = Matrix.CreateTranslation(-centerX, -centerY, 0);
            var offset = Matrix.CreateTranslation(_screenWidth / 2f, _screenHeight / 2f, 0);

            Transform = position * offset;
        }
    }
}
