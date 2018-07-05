using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core.Migration
{
    public interface ISchemaMigrator
    {
        void MigrateStructure(IServiceLocator locator, IMigrationScriptBuilder script);
    }
}
