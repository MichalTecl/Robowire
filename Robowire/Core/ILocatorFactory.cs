using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.Core
{
    public interface ILocatorFactory
    {
        IServiceLocator CreateLocatorInstance(IServiceLocator parentLocator, Dictionary<string, Func<IServiceLocator, object>> factories);
    }
}
