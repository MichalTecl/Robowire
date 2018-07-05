using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.Core
{
    public class NamedFactory
    {
        public NamedFactory(Func<IServiceLocator, object> factory)
        {
            Name = $"f{Guid.NewGuid():N}";
            Factory = factory;
        }

        public string Name { get; }

        public Func<IServiceLocator, object> Factory { get; }
    }
}
