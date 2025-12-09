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

        // NIEUW: We moeten weten waar de camera is om de achtergrond te berekenen
        Vector2 Position { get; }

        void Follow(IGameObject target);
    }
}
