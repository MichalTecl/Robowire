using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Robowire.Common.Expressions;

using Xunit;

namespace RobOrm.UnitTests
{
    public class ExpressionEvaluatorTests
    {
        public int TestProperty { get; set; }

        [Fact]
        public void TestIsEvaluable()
        {
            Expression<Func<ExpressionEvaluatorTests, int>> exp = t => t.TestProperty;
            Assert.False(ExpressionEvaluator.IsSelfEvaluable(exp));

            var v = 123;
            exp = t => v;
            Assert.True(ExpressionEvaluator.IsSelfEvaluable(exp));

            exp = t => "abc".ToUpper().LastIndexOf("c", StringComparison.Ordinal);
            Assert.True(ExpressionEvaluator.IsSelfEvaluable(exp));
        }

        [Fact]
        public void TestEvaluation()
        {
            var v = 123;
            Expression<Func<ExpressionEvaluatorTests, int>> exp = t => v;
            Assert.Equal(v, ExpressionEvaluator.Eval(exp));

            
            exp = t => "abc".ToUpper().LastIndexOf("x", StringComparison.Ordinal);
            Assert.Equal(-1, ExpressionEvaluator.Eval(exp));

            exp = t => Nothing(Nothing("abc".ToUpper().LastIndexOf("x", StringComparison.Ordinal)) + Nothing(TestProperty));
            Assert.Equal(-1, ExpressionEvaluator.Eval(exp));
        }

        private int Nothing(int x)
        {
            return x;
        }
    }
}
