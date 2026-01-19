using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.View
{
    public class HUD : IObserver
    {
        private SpriteFont _font;
        private int _screenWidth;

        // Data om te tekenen
        private int _coins;
        private bool _showPowerUpTimer;
        private float _timeRemaining;

        public HUD(SpriteFont font, int screenWidth)
        {
            _font = font;
            _screenWidth = screenWidth;
        }

        // Hier komt de data binnen van de Hero
        public void OnNotify(int coinsRemaining, bool hasPowerUp, float powerUpTimeLeft)
        {
            _coins = coinsRemaining;
            _showPowerUpTimer = hasPowerUp;
            _timeRemaining = powerUpTimeLeft;
        }

        public void Draw(SpriteBatch sb)
        {
            // 1. Teken Coins (Rechtsboven)
            string coinText = $"Coins needed: {_coins}";
            Vector2 coinSize = _font.MeasureString(coinText);
            Vector2 coinPos = new Vector2(_screenWidth - coinSize.X - 20, 20);

            sb.DrawString(_font, coinText, coinPos, Color.Yellow);

            // 2. Teken Timer (Alleen als actief)
            if (_showPowerUpTimer)
            {
                // :0.0 zorgt voor 1 cijfer na de komma (bijv 1.4s)
                string timeText = $"Super Jump: {_timeRemaining:0.0}s";

                Vector2 timeSize = _font.MeasureString(timeText);
                // Plaats hem 30 pixels ONDER de coins
                Vector2 timePos = new Vector2(_screenWidth - timeSize.X - 20, 20 + coinSize.Y + 5);

                sb.DrawString(_font, timeText, timePos, Color.Red); // Rood voor urgentie
            }
        }
    }
}