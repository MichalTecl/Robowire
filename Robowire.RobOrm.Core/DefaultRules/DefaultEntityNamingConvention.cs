using System;
using System.Collections;
using System.Reflection;

namespace Robowire.RobOrm.Core.DefaultRules
{
    public class DefaultEntityNamingConvention : IEntityNamingConvention
    {
        public string GetColumnName(PropertyInfo property)
        {
            if (!IsColumn(property))
            {
                return null;
            }

            return property.Name;
        }

        public bool IsColumn(PropertyInfo property)
        {
            //if (!property.CanRead || !property.CanWrite)
            //{
            //    return false;
            //}

            var propType = property.PropertyType;

            var nType = Nullable.GetUnderlyingType(propType);
            if (nType != null)
            {
                propType = nType;
            }

            return propType.IsPrimitive || typeof(string).IsAssignableFrom(propType);
        }

        public Type TryGetRefEntityType(PropertyInfo property)
        {
            if (!property.CanRead)
            {
                return null;
            }

            var pType = property.PropertyType;

            if (pType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(pType))
            {
                pType = pType.GetGenericArguments()[0];
            }

            var result = Attribute.IsDefined(pType, typeof(EntityAttribute)) ? pType : null;

            if (result != null && property.CanWrite)
            {
                throw new InvalidOperationException($"Invalid definition of {property.DeclaringType?.Name}.{property.Name}: Entity relation property must be read-only.");
            }

            return result;
        }

        public PropertyInfo GetPrimaryKeyProperty(Type entityType)
        {
            var entityAttribute = Attribute.GetCustomAttribute(entityType, typeof(EntityAttribute)) as EntityAttribute;
            var propertyName = (entityAttribute?.PrimaryKeyProperty) ?? "Id";

            return entityType.GetProperty(propertyName);
        }
    }
}
