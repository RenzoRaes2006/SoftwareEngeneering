using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SofEngeneering_project.View
{
    public class StartScreen
    {
        private SpriteFont _font;
        private Texture2D _buttonTexture; // Voor een gekleurd blokje achter de tekst (optioneel)
        private int _screenWidth;
        private int _screenHeight;

        // De knoppen (Rechthoeken waar je op kan klikken)
        private Rectangle _playButton;
        private Rectangle _exitButton;

        // Kleuren
        private Color _playColor = Color.White;
        private Color _exitColor = Color.White;

        public StartScreen(SpriteFont font, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;

            // Maak een simpele witte texture voor de knop-achtergrond (1x1 pixel)
            _buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White });

            // CENTREER DE KNOPPEN
            int buttonWidth = 200;
            int buttonHeight = 50;
            int centerX = (_screenWidth - buttonWidth) / 2;

            // Play knop iets boven het midden
            _playButton = new Rectangle(centerX, _screenHeight / 2 - 60, buttonWidth, buttonHeight);

            // Exit knop iets onder het midden
            _exitButton = new Rectangle(centerX, _screenHeight / 2 + 20, buttonWidth, buttonHeight);
        }

        // Deze methode geeft terug wat de gebruiker wil doen: "Play", "Exit", of "" (niets)
        public string Update()
        {
            MouseState mouse = Mouse.GetState();
            Point mousePoint = new Point(mouse.X, mouse.Y);

            // 1. CHECK PLAY BUTTON
            if (_playButton.Contains(mousePoint))
            {
                _playColor = Color.LightGreen; // Hover effect
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    return "Play";
                }
            }
            else
            {
                _playColor = Color.Gray; // Normale kleur
            }

            // 2. CHECK EXIT BUTTON
            if (_exitButton.Contains(mousePoint))
            {
                _exitColor = Color.Red; // Hover effect
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    return "Exit";
                }
            }
            else
            {
                _exitColor = Color.Gray; // Normale kleur
            }

            return ""; // Niets aangeklikt
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Teken Titel
            string title = "Software Engeneering Game";
            Vector2 titleSize = _font.MeasureString(title);
            spriteBatch.DrawString(_font, title, new Vector2((_screenWidth - titleSize.X) / 2, 100), Color.Gold);

            // Teken Play Knop (Achtergrond + Tekst)
            spriteBatch.Draw(_buttonTexture, _playButton, _playColor);
            DrawCenteredText(spriteBatch, "PLAY", _playButton);

            // Teken Exit Knop (Achtergrond + Tekst)
            spriteBatch.Draw(_buttonTexture, _exitButton, _exitColor);
            DrawCenteredText(spriteBatch, "EXIT", _exitButton);
        }

        // Hulpmethode om tekst in het midden van een knop te zetten
        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle rect)
        {
            Vector2 size = _font.MeasureString(text);
            Vector2 pos = new Vector2(
                rect.X + (rect.Width - size.X) / 2,
                rect.Y + (rect.Height - size.Y) / 2
            );
            spriteBatch.DrawString(_font, text, pos, Color.Black);
        }
    }
}