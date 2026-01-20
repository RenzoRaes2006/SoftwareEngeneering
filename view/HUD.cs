using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Interfaces;

namespace SofEngeneering_project.View
{
    public class HUD : IObserver
    {
        private SpriteFont _font;
        private Texture2D _heartTex; // Het plaatje voor het hartje
        private int _screenWidth;

        private int _coins;
        private bool _showPowerUpTimer;
        private float _timeRemaining;
        private int _lives;

        // Constructor accepteert nu de Heart Texture
        public HUD(SpriteFont font, Texture2D heartTex, int screenWidth)
        {
            _font = font;
            _heartTex = heartTex;
            _screenWidth = screenWidth;
        }

        public void OnNotify(int coinsRemaining, bool hasPowerUp, float powerUpTimeLeft, int lives)
        {
            _coins = coinsRemaining;
            _showPowerUpTimer = hasPowerUp;
            _timeRemaining = powerUpTimeLeft;
            _lives = lives;
        }

        public void Draw(SpriteBatch sb)
        {
            // 1. Coins (Rechtsboven)
            string coinText = $"Coins: {_coins}";
            Vector2 coinSize = _font.MeasureString(coinText);
            sb.DrawString(_font, coinText, new Vector2(_screenWidth - coinSize.X - 20, 20), Color.Yellow);

            // 2. Levens (Linksboven) - Teken een hartje voor elk leven
            for (int i = 0; i < _lives; i++)
            {
                // Elk hartje 40 pixels opschuiven. Schaal 1.5f voor duidelijkheid.
                sb.Draw(_heartTex, new Vector2(20 + (i * 40), 20), null, Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }

            // 3. Timer
            if (_showPowerUpTimer)
            {
                string timeText = $"Super Jump: {_timeRemaining:0.0}s";
                Vector2 timeSize = _font.MeasureString(timeText);
                sb.DrawString(_font, timeText, new Vector2(_screenWidth - timeSize.X - 20, 50), Color.Red);
            }
        }
    }
}