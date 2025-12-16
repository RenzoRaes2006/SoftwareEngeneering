using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Entities;

namespace SofEngeneering_project.View
{
    public class HUD
    {
        private SpriteFont _font;
        private int _screenWidth;
        private int _screenHeight;

        // Constructor: We hebben de font en schermgrootte nodig voor positie-berekeningen
        public HUD(SpriteFont font, int screenWidth, int screenHeight)
        {
            _font = font;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        public void Draw(SpriteBatch spriteBatch, Hero hero)
        {
            // ---------------------------------------------------
            // 1. SUPERJUMP TIMER (Rechtsboven)
            // ---------------------------------------------------
            if (hero.PowerUpTimer > 0)
            {
                // :0.0 zorgt voor 1 cijfer achter de komma (bijv. 4.3s)
                string timerText = $"SuperJump: {hero.PowerUpTimer:0.0}s";

                // Bereken breedte om rechts uit te lijnen
                Vector2 timerSize = _font.MeasureString(timerText);
                Vector2 timerPos = new Vector2(_screenWidth - timerSize.X - 20, 20);

                // Teken schaduw (zwart) + tekst (geel)
                spriteBatch.DrawString(_font, timerText, timerPos + new Vector2(2, 2), Color.Black);
                spriteBatch.DrawString(_font, timerText, timerPos, Color.Yellow);
            }

            // ---------------------------------------------------
            // 2. COINS TELLER (Rechtsboven, onder timer)
            // ---------------------------------------------------
            // --- COINS TEKST MET FLIKKER EFFECT ---
            string coinText = $"Coins over: {hero.CoinsRemaining}";

            // Standaard kleur is wit
            Color coinColor = Color.White;

            // ALS de timer loopt (we hebben net een muntje gepakt)...
            if (hero.CoinFeedbackTimer > 0)
            {
                // TRUCJE: Gebruik de timer om te 'knipperen'
                // We vermenigvuldigen met 20 voor de snelheid. 
                // De 'Modulo 2' (% 2) zorgt dat het resultaat 0 of 1 is.
                // 0 = Rood, 1 = Wit
                if ((int)(hero.CoinFeedbackTimer * 20) % 2 == 0)
                {
                    coinColor = Color.Red; // Of Color.Transparent als je tekst wil laten verdwijnen
                }
            }

            // Positie berekenen
            Vector2 coinSize = _font.MeasureString(coinText);
            Vector2 coinPos = new Vector2(_screenWidth - coinSize.X - 20, 50);

            // Teken met de dynamische kleur
            spriteBatch.DrawString(_font, coinText, coinPos + new Vector2(2, 2), Color.Black); // Schaduw blijft zwart
            spriteBatch.DrawString(_font, coinText, coinPos, coinColor); // Tekst flikkert

            // ---------------------------------------------------
            // 3. LEVEL COMPLETE (Midden van scherm)
            // ---------------------------------------------------
            if (hero.CoinsRemaining == 0)
            {
                string winText = "LEVEL COMPLETE!";

                // Bereken grootte om te centreren
                Vector2 winSize = _font.MeasureString(winText);

                // Formule voor centeren: (SchermBreedte - TekstBreedte) / 2
                Vector2 centerPos = new Vector2(
                    (_screenWidth - winSize.X) / 2,
                    (_screenHeight - winSize.Y) / 2
                );

                // Teken schaduw + tekst (Limoen groen)
                spriteBatch.DrawString(_font, winText, centerPos + new Vector2(3, 3), Color.Black);
                spriteBatch.DrawString(_font, winText, centerPos, Color.LimeGreen);
            }
        }
    }
}