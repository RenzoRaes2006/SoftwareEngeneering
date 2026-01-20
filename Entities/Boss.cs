using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SofEngeneering_project.Behaviors;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Entities
{
    public class Boss : Enemy
    {
        private int _maxHP;
        public int CurrentHP { get; private set; }
        private Texture2D _pixel;

        public Boss(Texture2D texture, Vector2 position, List<Rectangle> frames, IMovementEnemy strategy, float scale, List<IGameObject> levelObjects, int hp, GraphicsDevice graphicsDevice)
            : base(texture, position, frames, strategy, scale, levelObjects)
        {
            _maxHP = hp;
            CurrentHP = hp;
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void TakeDamage(int damage)
        {
            if (IsDead) return;
            CurrentHP -= damage;
            if (CurrentHP <= 0) Die();

            if (MovementStrategy is PatrolEnemyBehavior patrol)
                patrol.UpdateStats(CurrentHP, _maxHP);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsDead) return;

            // Teken de Boss zelf (Enemy.Draw gebruikt de schaal 10f)
            base.Draw(spriteBatch);

            // HP Balk: 200 pixels breed, boven de Boss
            Rectangle bg = new Rectangle((int)Position.X, (int)Position.Y - 40, 200, 20);
            float hpPercent = (float)CurrentHP / _maxHP;
            Rectangle fg = new Rectangle((int)Position.X, (int)Position.Y - 40, (int)(200 * hpPercent), 20);

            spriteBatch.Draw(_pixel, bg, Color.Black * 0.7f);
            spriteBatch.Draw(_pixel, fg, Color.Red);
        }
    }
}