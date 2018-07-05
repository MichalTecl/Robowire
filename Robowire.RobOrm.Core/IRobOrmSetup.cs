using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core
{
    public interface IRobOrmSetup
    {
        IEntityNamingConvention EntityNamingConvention { get; }
    }
}
