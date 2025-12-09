using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface IHeroState
    {
        void HandleInput(ICommand command, IMovable hero);
        void Update(IMovable hero, GameTime gameTime);
        void Enter(IMovable hero); // Wordt aangeroepen als de state start
    }
}
