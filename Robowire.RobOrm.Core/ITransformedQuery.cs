using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.Common.Expressions;
using Robowire.RobOrm.Core.Query.Filtering;

namespace Robowire.RobOrm.Core
{
    public interface ITransformedQuery<T> : ITransformedQuery
    {
        IEnumerable<T> Execute();
    }

    public interface ITransformedQuery
    {
        IQuerySegment GetQuery(ExpressionMapperBase<IQuerySegment> queryMapper, IHasParameters paramsTarget);
    }
}
