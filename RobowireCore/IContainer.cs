using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.Plugin;
using Robowire.Plugin.Flow;

namespace Robowire
{
    public interface IContainer
    {
        IContainer Parent { get; }

        IReadOnlyPluginCollection Plugins { get; }

        IServiceLocator GetLocator();

        IContainer Setup(Action<IContainerSetup> setup);
    }
}
