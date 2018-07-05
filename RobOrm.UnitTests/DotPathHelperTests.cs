using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RobOrm.Core.Internal;

using Xunit;

namespace RobOrm.UnitTests
{
    public class DotPathHelperTests
    {
        [Theory]
        [InlineData(".a", "a")]
        [InlineData("a.", "a")]
        [InlineData(".a.", "a")]
        [InlineData(".a.b", "a.b")]
        [InlineData("a.b.", "a.b")]
        public void TestTrimDots(string inp, string exp)
        {
            Assert.Equal(exp, DotPathHelper.TrimDots(inp));
        }

        [Theory]
        [InlineData("a", "b", "a.b")]
        [InlineData("a.b", "c", "a.b.c")]
        [InlineData("a.", "b.", "a.b")]
        public void TestCombine(string a, string b, string exp)
        {
            Assert.Equal(exp, DotPathHelper.Combine(a, b));
        }

        [Theory]
        [InlineData("a.b", "a")]
        [InlineData("root.child1.child2.child3", "root.child1.child2")]
        [InlineData("root.child1.child2", "root.child1")]
        [InlineData("root.child1", "root")]
        [InlineData("root", null)]
        public void TestGetParent(string inp, string exp)
        {
            Assert.Equal(exp, DotPathHelper.GetParent(inp));
        }
    }
}
