using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface ICommand
    {
        void Execute(IMovable movable);
    }
}
