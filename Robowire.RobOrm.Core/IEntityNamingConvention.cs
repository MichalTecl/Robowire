using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core
{
    public interface IEntityNamingConvention
    {
        string GetColumnName(PropertyInfo property);

        bool IsColumn(PropertyInfo property);

        Type TryGetRefEntityType(PropertyInfo property);

        PropertyInfo GetPrimaryKeyProperty(Type entityType);
    }
}
