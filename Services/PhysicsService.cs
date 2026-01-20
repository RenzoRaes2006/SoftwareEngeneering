using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Services
{
    public static class PhysicsService
    {
        public static void MoveX(Hero hero, List<IGameObject> objects)
        {
            hero.Position += new Vector2(hero.Velocity.X, 0);
            Rectangle hb = hero.CollisionBox;
            foreach (var obj in objects)
            {
                if (obj is Block || (obj is BigWall gate && gate.IsActive))
                {
                    if (hb.Intersects(obj.CollisionBox))
                    {
                        if (hero.Velocity.X > 0) hero.Position = new Vector2(obj.CollisionBox.Left - hb.Width, hero.Position.Y);
                        else if (hero.Velocity.X < 0) hero.Position = new Vector2(obj.CollisionBox.Right, hero.Position.Y);
                    }
                }
            }
        }

        public static bool MoveY(Hero hero, List<IGameObject> objects)
        {
            hero.Position += new Vector2(0, hero.Velocity.Y);
            Rectangle hb = hero.CollisionBox;
            bool grounded = false;
            foreach (var obj in objects)
            {
                if (obj is Block || (obj is BigWall gate && gate.IsActive))
                {
                    if (hb.Intersects(obj.CollisionBox))
                    {
                        if (hero.Velocity.Y > 0 && hb.Bottom <= obj.CollisionBox.Top + hero.Velocity.Y + 2)
                        {
                            hero.Position = new Vector2(hero.Position.X, obj.CollisionBox.Top - hb.Height);
                            hero.Velocity = new Vector2(hero.Velocity.X, 0);
                            grounded = true;
                        }
                    }
                }
            }
            return grounded;
        }

        public static void HandleEnemyPhysics(Enemy enemy, List<IGameObject> objects)
        {
            if (enemy == null || enemy.IsDead) return;

            Rectangle eb = enemy.CollisionBox;
            bool onGround = false;

            foreach (var obj in objects)
            {
                if (obj is Block || (obj is BigWall gate && gate.IsActive))
                {
                    if (eb.Intersects(obj.CollisionBox))
                    {
                        // SOLID fix: De Boss heeft door zijn 10x schaal een grotere marge nodig (45px) 
                        // om trillen op dunne blokken te voorkomen.
                        int margin = (enemy is Boss) ? 45 : 15;

                        if (eb.Bottom >= obj.CollisionBox.Top && eb.Bottom <= obj.CollisionBox.Top + margin)
                        {
                            enemy.Position = new Vector2(enemy.Position.X, obj.CollisionBox.Top - eb.Height);
                            onGround = true;
                            break;
                        }
                    }
                }
            }

            if (!onGround)
            {
                float gravity = (enemy is Boss) ? 2.5f : 4f;
                enemy.Position = new Vector2(enemy.Position.X, enemy.Position.Y + gravity);
            }
        }
    }
}