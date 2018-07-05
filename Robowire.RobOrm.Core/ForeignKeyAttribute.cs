using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(string foreignKeyColumnName)
        {
            ForeignKeyColumnName = foreignKeyColumnName;
        }

        public string ForeignKeyColumnName { get; private set; }

        public static string GetForeignKeyName(PropertyInfo property, string prefferedKeyName)
        {
            return (GetCustomAttribute(property, typeof(ForeignKeyAttribute)) as ForeignKeyAttribute)?.ForeignKeyColumnName ?? prefferedKeyName;
        }
    }
}
