using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System;
using System.Collections.Generic;

namespace SofEngeneering_project.Services
{
    public static class PhysicsService
    {
        public static void MoveX(Hero hero, List<IGameObject> objects)
        {
            if (Math.Abs(hero.Velocity.X) < 0.01f) return;

            hero.Position += new Vector2(hero.Velocity.X, 0);
            Rectangle hb = hero.CollisionBox;

            foreach (var obj in objects)
            {

                if (obj is BigWall gate && gate.IsActive)
                {
                    if (hb.Intersects(obj.CollisionBox))
                    {
                        if (hero.Velocity.X > 0)
                            hero.Position = new Vector2(obj.CollisionBox.Left - hb.Width, hero.Position.Y);
                        else if (hero.Velocity.X < 0)
                            hero.Position = new Vector2(obj.CollisionBox.Right, hero.Position.Y);

                        hero.Velocity = new Vector2(0, hero.Velocity.Y);
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

                        if (hero.Velocity.Y > 0)
                        {
                            float feetPosition = hb.Bottom;
                            float platformTop = obj.CollisionBox.Top;

                            if (feetPosition >= platformTop && feetPosition <= platformTop + hero.Velocity.Y + 10)
                            {
                                hero.Position = new Vector2(hero.Position.X, platformTop - hb.Height);
                                hero.Velocity = new Vector2(hero.Velocity.X, 0);
                                grounded = true;
                                break;
                            }
                        }

                    }
                }
            }
            return grounded;
        }
    }
}