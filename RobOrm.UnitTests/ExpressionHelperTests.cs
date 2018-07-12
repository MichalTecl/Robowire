using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.Internal;

using Xunit;

namespace RobOrm.UnitTests
{
    public class ExpressionHelperTests
    {
        [Fact]
        public void TestExpressionEvaluation()
        {
            Expression<Func<IEntity1, IEntity1>> exp = e => e.A.B.C;
            var strExp = ExpressionsHelper.GetPropertiesChainText<IEntity1>(exp);
            Assert.Equal("IEntity1.A.B.C", strExp);

            Expression<Func<IEntity1, object>> oexp = e => e.X.Each().Y;

            strExp = ExpressionsHelper.GetPropertiesChainText<IEntity1>(oexp);
            Assert.Equal("IEntity1.X.Y", strExp);

            oexp = e => e.X.OrderBy(i => i.Id).Each().Y;
            strExp = ExpressionsHelper.GetPropertiesChainText<IEntity1>(oexp);
            Assert.Null(strExp);

            var v = 123;
            Expression<Func<IEntity1, int>> iexp = e => v + 5;
            strExp = ExpressionsHelper.GetPropertiesChainText<IEntity1>(iexp);
            Assert.Null(strExp);
        }
        
        [Entity]
        public interface IEntity1
        {
            int Id { get; set; }

            IEntity1 A { get; }

            IEntity1 B { get; }

            IEntity1 C { get; }

            IEnumerable<IEntity1> X { get; }

            IEnumerable<IEntity1> Y { get; }
        }
    }
}
