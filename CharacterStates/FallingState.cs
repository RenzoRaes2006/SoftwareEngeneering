using Microsoft.Xna.Framework;
using SofEngeneering_project.Entities;
using SofEngeneering_project.Interfaces;
using SofEngeneering_project.Patterns;
using SofEngeneering_project.CharacterStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.CharacterStates
{
    public class FallingState: IHeroState
    {
        public void Enter(IMovable _hero) { var hero = _hero as Hero; }

        public void HandleInput(ICommand command, IMovable hero)
        {
            // STATE PATTERN: 
            // In de FallingState negeren we spring-commando's (tenzij je dubbele sprong wilt).
            if (command is JumpCommand)
            {
                return;
            }

            // We voeren WEL MoveLeft, MoveRight en IdleCommand uit.
            // Dit lost je probleem op dat hij blijft zweven naar links/rechts.
            command.Execute(hero);
        }

        public void Update(IMovable movable, GameTime gameTime)
        {
            var hero = movable as Hero;
            hero.Velocity += new Vector2(0, 0.5f); // Zwaartekracht

            // --- DE FIX ---
            // We pakken de HUIDIGE, echte hitbox (met jouw offset en height instellingen)
            Rectangle currentHitbox = hero.CollisionBox;

            // We voorspellen waar die hitbox straks is (alleen verticaal bewegen)
            Rectangle futureBox = new Rectangle(
                currentHitbox.X,
                currentHitbox.Y + (int)hero.Velocity.Y,
                currentHitbox.Width,
                currentHitbox.Height
            );

            foreach (var obj in hero.LevelObjects)
            {
                if (obj == hero) continue;
                if (obj is PowerUp || obj is Coin) continue;

                if (futureBox.Intersects(obj.CollisionBox))
                {
                    // Checken of we echt vallen (snelheid omlaag) én of we bovenop het blok zitten
                    // We checken of de onderkant van onze voeten (Bottom) boven het blok zat
                    if (hero.Velocity.Y > 0 && currentHitbox.Bottom <= obj.CollisionBox.Top + 10)
                    {
                        // Landen!
                        // We moeten uitrekenen waar de Hero Position moet komen.
                        // Positie = Blok Bovenkant - Hitbox Hoogte - Hitbox Offset Y

                        // Omdat CollisionBox.Bottom = Position.Y + OffsetY + Height
                        // Moet Position.Y = BlokTop - OffsetY - Height

                        // Een makkelijkere manier: we duwen gewoon de CollisionBox omhoog
                        int offsetFromPos = currentHitbox.Bottom - (int)hero.Position.Y;

                        hero.Position = new Vector2(hero.Position.X, obj.CollisionBox.Top - offsetFromPos);

                        hero.CurrentState = new IdleState();
                        hero.CurrentState.Enter(hero);
                        return;
                    }
                }
            }
        }
    }
}
