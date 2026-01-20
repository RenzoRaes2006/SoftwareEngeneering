using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SofEngeneering_project.view
{
    public class GenericMenuScreen
    {
        private SpriteFont _font;
        private Texture2D _pixelTexture;
        private int _screenWidth;
        private int _screenHeight;

        // Data voor dit specifieke scherm
        private string _titleText;
        private string _button1Text;
        private Color _overlayColor;

        // Knoppen
        private Rectangle _btn1Rect;
        private Rectangle _btn2Rect;

        // Hover kleuren
        private Color _btn1Color = Color.White;
        private Color _btn2Color = Color.White;

        public GenericMenuScreen(SpriteFont font, GraphicsDevice graphicsDevice, string title, string button1Text, Color overlayColor)
        {
            _font = font;
            _screenWidth = graphicsDevice.Viewport.Width;
            _screenHeight = graphicsDevice.Viewport.Height;
            _titleText = title;
            _button1Text = button1Text;
            _overlayColor = overlayColor;

            // Maak 1x1 witte pixel voor achtergronden
            _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            _pixelTexture.SetData(new[] { Color.White });

            // Knoppen posities berekenen
            int btnW = 250;
            int btnH = 50;
            int centerX = (_screenWidth - btnW) / 2;
            int centerY = _screenHeight / 2;

            _btn1Rect = new Rectangle(centerX, centerY - 30, btnW, btnH); // Actie knop (Retry/Next)
            _btn2Rect = new Rectangle(centerX, centerY + 40, btnW, btnH); // Exit knop
        }

        public string Update()
        {
            MouseState ms = Mouse.GetState();
            Point mousePos = new Point(ms.X, ms.Y);

            // Button 1 (De actie knop: Restart of Next)
            if (_btn1Rect.Contains(mousePos))
            {
                _btn1Color = Color.LightGreen;
                if (ms.LeftButton == ButtonState.Pressed) return "Action";
            }
            else _btn1Color = Color.Gray;

            // Button 2 (Altijd Exit)
            if (_btn2Rect.Contains(mousePos))
            {
                _btn2Color = Color.Red;
                if (ms.LeftButton == ButtonState.Pressed) return "Exit";
            }
            else _btn2Color = Color.Gray;

            return "";
        }

        public void Draw(SpriteBatch sb)
        {
            // 1. Achtergrond overlay
            sb.Draw(_pixelTexture, new Rectangle(0, 0, _screenWidth, _screenHeight), _overlayColor);

            // 2. Titel
            Vector2 titleSize = _font.MeasureString(_titleText);
            sb.DrawString(_font, _titleText, new Vector2((_screenWidth - titleSize.X) / 2, 100), Color.Gold);

            // 3. Knop 1
            sb.Draw(_pixelTexture, _btn1Rect, _btn1Color);
            DrawCenteredText(sb, _button1Text, _btn1Rect);

            // 4. Knop 2
            sb.Draw(_pixelTexture, _btn2Rect, _btn2Color);
            DrawCenteredText(sb, "EXIT", _btn2Rect);
        }

        private void DrawCenteredText(SpriteBatch sb, string text, Rectangle rect)
        {
            Vector2 size = _font.MeasureString(text);
            Vector2 pos = new Vector2(
                rect.X + (rect.Width - size.X) / 2,
                rect.Y + (rect.Height - size.Y) / 2
            );
            sb.DrawString(_font, text, pos, Color.Black);
        }
    }
}