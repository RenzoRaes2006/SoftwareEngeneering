using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_softEngeneering.Animations;
using Project_softEngeneering.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project_softEngeneering
{
    internal class Character : IGameObject, IMovable
    {
        private Texture2D texture;
        private Texture2D blockTexture; 
        Animation animation;
        private Vector2 positie;
        private Vector2 snelheid;
        Vector2 versnelling = new Vector2(0.1f, 0.1f);
        Vector2 v1 = new Vector2(1, 2);
        Vector2 v2 = new Vector2(3, 4);
        private IInputReader inputReader;
        private MovementManager movementManager;

        public Vector2 Position { get => positie; set => positie = value; }
        public Vector2 Speed { get => snelheid; set => snelheid = value; }
        public IInputReader InputReader { get => inputReader; set => inputReader = value; }

        public Character(Texture2D texture, IInputReader inputReader)
        {
            this.texture = texture;
            this.inputReader = inputReader;
            var afstandTussenv1v2 = (v2 - v1).Length();
            snelheid = new Vector2(1,1);
            movementManager = new MovementManager();

            animation = new Animation();
            animation.AddFrame(new AnimationFrame(new Rectangle(45, 50, 30, 78)));
            animation.AddFrame(new AnimationFrame(new Rectangle(173, 50, 30, 78)));
            animation.AddFrame(new AnimationFrame(new Rectangle(302, 50, 30, 78)));
            animation.AddFrame(new AnimationFrame(new Rectangle(430, 50, 30, 78)));
            animation.AddFrame(new AnimationFrame(new Rectangle(558, 50, 30, 78)));
            animation.AddFrame(new AnimationFrame(new Rectangle(681, 50, 30, 78)));


        }

        public void Draw(SpriteBatch spriteBatch)
        {

        spriteBatch.Draw(texture, positie, animation.CurrentFrame.SourceRectangle, Color.White);

        }

        public void Update(GameTime gameTime)
        {

            Move();
         //   MoveMouse();
            animation.Update(gameTime);
        }

        public void ChangeInput(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        private void Move()
        {
            movementManager.Move(this);

            //var toekomstigePositie = positie + direction;
            //if (
            //  (toekomstigePositie.X < (800 - 180)
            //   && toekomstigePositie.X > 0) &&
            //   (toekomstigePositie.Y < 480 - 247
            //   && toekomstigePositie.Y > 0)
            //)
            //{
            //    positie = toekomstigePositie;
            //}







            //mannetje laten bouncen op muren
            //positie += snelheid;
            //snelheid += versnelling;
            //float maximaleSnelheid = 10;
            //snelheid = Limit(snelheid, maximaleSnelheid);
            //if (positie.X + snelheid.X  > 800-30 )
            //{
            //    snelheid = new Vector2(snelheid.X < 0 ? 1 : -1, snelheid.Y);
            //    versnelling.X *= -1;
            //}
            // if (positie.X + snelheid.X < 0)
            //{
            //    snelheid = new Vector2(snelheid.X < 0 ? 1 : -1, snelheid.Y);
            //    versnelling.X *= -1;
            //}
            // if (positie.Y + snelheid.Y > 480-78)
            //{
            //    snelheid = new Vector2(snelheid.X, snelheid.Y < 0 ? 1 : -1);
            //    versnelling.Y *= -1;
            //}
            // if (positie.Y + snelheid.Y < 0)
            //{
            //    snelheid = new Vector2(snelheid.X, snelheid.Y < 0 ? 1 : -1);
            //    versnelling.Y *= -1;
            //}

        }

        private Vector2 Limit(Vector2 v, float max)
        {
            if(v.Length() > max)
            {
                var ratio = max / v.Length();
                v.X *= ratio;
                v.Y *= ratio;
            }
            return v;
        }

        private void MoveMouse()
        {
            MouseState state = Mouse.GetState();
            Vector2 mouseVector = new Vector2(state.X, state.Y);

            var afstand = mouseVector - positie;
            afstand.Normalize();
            afstand = Vector2.Multiply(afstand, 0.1f);
            snelheid += afstand;
            snelheid = Limit(snelheid, 10);
            
            positie += snelheid;
        }
    }
}
