using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofEngeneering_project.Interfaces
{
    public interface IGameObserver
    {
        void OnNotify(string eventName);
    }
}
