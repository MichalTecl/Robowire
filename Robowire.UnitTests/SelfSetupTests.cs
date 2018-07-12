using System;

using Xunit;

namespace Robowire.UnitTests
{
    public class SelfSetupTests
    {
        [Fact]
        public void TestSelfSetup()
        {
            var container = new Container();

            container.Setup(s => s.ScanAssembly(typeof(SelfSetupTests).Assembly));

            var locator = container.GetLocator();

            var inst = locator.Get<ISelfImplementedIface>();

            Assert.NotNull(inst);
            Assert.True(inst is X);
        }


        public class ImplByClassAttribute : Attribute, ISelfSetupAttribute
        {
            private readonly Type m_implementingType;

            public ImplByClassAttribute(Type implementingType)
            {
                m_implementingType = implementingType;
            }

            public void Setup(Type markedType, IContainerSetup setup)
            {
                setup.For(markedType).Use(m_implementingType);
            }
        }

        [ImplByClass(typeof(X))]
        public interface ISelfImplementedIface
        {
            int Ab { get; }
        }

        public class X : ISelfImplementedIface
        {
            public int Ab => 42;
        }
    }
}
