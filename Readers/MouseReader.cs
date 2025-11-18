using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_softEngeneering.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_softEngeneering.Readers
{
    internal class MouseReader : IInputReader
    {

        public bool IsDestinationalInput => true;

        public Vector2 ReadInput()
        {
            MouseState state = Mouse.GetState();
            Vector2 directionMouse = new Vector2(state.X, state.Y);
            //if (directionMouse != Vector2.Zero)
            //{
            //    directionMouse.Normalize();
            //}
            return directionMouse;
        }
    }
}
