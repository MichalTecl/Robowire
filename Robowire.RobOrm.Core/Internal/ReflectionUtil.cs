using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core.Internal
{
    public static class ReflectionUtil
    {
        public static IEnumerable<PropertyInfo> GetAllProperties(Type t)
        {
            foreach (var prop in t.GetProperties())
            {
                yield return prop;
            }

            foreach (var iface in t.GetInterfaces())
            {
                foreach (var prop in iface.GetProperties())
                {
                    yield return prop;
                }
            }
        }

        public static PropertyInfo GetProperty(Type entityType, string propertyName)
        {
            return GetAllProperties(entityType).FirstOrDefault(p => p.Name == propertyName);
        }
    }
}
