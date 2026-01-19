using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using System.Collections.Generic;

namespace SofEngeneering_project.Services
{
    public static class PhysicsService
    {
        public static void CheckHorizontalCollision(IMovable entity, Rectangle entityBox, List<IGameObject> objects)
        {
            foreach (var obj in objects)
            {
                if (obj == entity) continue;

                // --- FIX: Negeer collision tussen Hero en Enemy ---
                // Hierdoor draaien enemies niet meer om als ze je raken, 
                // maar lopen ze 'in' je zodat de Game Over check kan werken.
                if (obj is Coin || obj is PowerUp || obj is Trap) continue;
                if (entity is Hero && obj is Enemy) continue;
                if (entity is Enemy && obj is Enemy) continue;
                if (entity is Enemy && obj is Hero) continue; // <--- DEZE ONTBREKTE!

                // Gate logica
                if (obj is BigWall gate && !gate.IsActive) continue;

                if (entityBox.Intersects(obj.CollisionBox))
                {
                    if (entity.Velocity.X > 0)
                        entity.Position = new Vector2(obj.CollisionBox.Left - entityBox.Width, entity.Position.Y);
                    else if (entity.Velocity.X < 0)
                        entity.Position = new Vector2(obj.CollisionBox.Right, entity.Position.Y);

                    entity.Velocity = new Vector2(0, entity.Velocity.Y);

                    // Enemy draait om bij muur
                    if (entity is Enemy enemy) enemy.MovementStrategy.OnHorizontalCollision();
                }
            }
        }

        public static bool CheckVerticalCollision(IMovable entity, Rectangle entityBox, List<IGameObject> objects)
        {
            bool isGrounded = false;

            foreach (var obj in objects)
            {
                if (obj == entity) continue;
                if (obj is Coin || obj is PowerUp || obj is Trap) continue;
                if (entity is Hero && obj is Enemy) continue;
                if (entity is Enemy && obj is Hero) continue; // <--- DEZE OOK TOEVOEGEN

                if (obj is BigWall gate && !gate.IsActive) continue;

                if (entityBox.Intersects(obj.CollisionBox))
                {
                    if (entity.Velocity.Y < 0) continue;

                    Rectangle overlap = Rectangle.Intersect(entityBox, obj.CollisionBox);
                    if (entity.Velocity.Y >= 0 && (entityBox.Bottom - overlap.Height) <= obj.CollisionBox.Top + 10)
                    {
                        entity.Position = new Vector2(entity.Position.X, obj.CollisionBox.Top - entityBox.Height);
                        entity.Velocity = new Vector2(entity.Velocity.X, 0);
                        isGrounded = true;
                    }
                }
            }
            return isGrounded;
        }
    }
}