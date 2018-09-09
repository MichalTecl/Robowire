using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core;

namespace RobOrm.UnitTests
{
    public class DbStringAttribute : Attribute, IDbTypeAttribute
    {
        public DbStringAttribute()
        {
            ColumnDeclarationTypeText = "string";
            IsNullable = true;
        }

        public string ColumnDeclarationTypeText { get; }

        public bool IsNullable { get; }
    }
}
