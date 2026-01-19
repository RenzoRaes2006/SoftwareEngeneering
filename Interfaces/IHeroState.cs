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
        void HandleInput(ICommand command, IHeroInterface hero);
        void Update(IHeroInterface hero, GameTime gameTime);
        void Enter(IHeroInterface hero);
    }
}
