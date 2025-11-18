using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_softEngeneering.Interfaces
{
    internal interface IInputReader
    {
        Vector2 ReadInput();
        public bool IsDestinationalInput { get; }
    }
}
