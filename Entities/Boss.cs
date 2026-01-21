using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Boss : Enemy
    {
        public int CurrentHP { get; private set; }
        private int _maxHP;
        private Texture2D _pixel;

        public Boss(Texture2D texture, Vector2 position, List<Rectangle> frames, Interfaces.IMovementEnemy strategy, float scale, List<Interfaces.IGameObject> levelObjects, int hp, GraphicsDevice graphicsDevice)
            : base(texture, position, frames, strategy, scale, levelObjects)
        {
            _maxHP = hp;
            CurrentHP = hp;
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void TakeDamage(int damage)
        {
            if (IsDead) 
                return;

            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0;
                Die();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead) 
                return;

            base.Draw(spriteBatch);

            // HP Balk tekenen
            int barWidth = (int)(_frames[0].Width * _scale);
            float hpPercent = (float)CurrentHP / _maxHP;

            Rectangle bg = new Rectangle((int)Position.X, (int)Position.Y - 20, barWidth, 10);
            Rectangle fg = new Rectangle((int)Position.X, (int)Position.Y - 20, (int)(barWidth * hpPercent), 10);

            spriteBatch.Draw(_pixel, bg, Color.Black * 0.5f);
            spriteBatch.Draw(_pixel, fg, Color.Red);
        }
    }
}