using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core.DefaultRules
{
    internal class DefaultRobOrmSetup : IRobOrmSetup
    {
        public IEntityNamingConvention EntityNamingConvention { get; } = new DefaultEntityNamingConvention();
    }
}
