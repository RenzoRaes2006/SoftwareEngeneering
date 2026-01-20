using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface ICamera
    {
        Matrix Transform { get; }
        Vector2 Position { get; }
        void Follow(IGameObject target);
    }
}
