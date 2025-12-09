using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface IMovable
    {
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
    }
}
