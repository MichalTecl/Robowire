using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Robowire.Common.Expressions;
using Robowire.RobOrm.Core.Query.Filtering;

namespace Robowire.RobOrm.Core
{
    public interface IMethodMapper
    {
        IQuerySegment Map(MethodCallExpression expression, ExpressionMapperBase<IQuerySegment> queryMapper, Type resultingTableType, IHasParameters paramTarget);
    }
}
