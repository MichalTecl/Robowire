using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire
{
    public interface ISelfSetupAttribute
    {
        void Setup(Type markedType, IContainerSetup setup);
    }
}
