using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robowire.RobOrm.Core
{
    public interface IDbTypeAttribute
    {
        string ColumnDeclarationTypeText { get; }

        bool IsNullable { get; }
    }
}
