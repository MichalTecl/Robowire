using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robowire.RoboApi;
using Robowire.RoboApi.Internal;

using Xunit;

namespace Robowire.UnitTests
{
    public class RoboApiBasicTests
    {
		[Fact]
        public void TestCollection()
        {
            var container = new Container();

            container.Setup(s => s.For<SomeDependency>().Use<SomeDependency>());

            var installer = new RoboApiInstaller();
            installer.Install(null, container, typeof(RoboApiBasicTests).Assembly);

            var locator = container.GetLocator();

            var index = locator.Get<ControllerIndex>();

            var cType = index.GetControllerType(nameof(Controller1));

            Assert.Equal(typeof(Controller1), cType);

            var controller = locator.Get(cType) as ILocatorBoundController;

            Assert.NotNull(controller);
            Assert.Equal(locator, controller.Locator);
        }

        public class SomeDependency { }

		[Controller("controller1")]
        public class Controller1
        {
            public Controller1(SomeDependency sd) { }

            public Controller1 Method1(int parameter1, string parameter2, Controller1 parameter3)
            {
                return this;
            }

            public Controller1 Method2(int parameter1, string parameter2, Controller1 parameter3)
            {
                return this;
            }

            public void SomeVoidMethod(Controller1 param) { }
        }
    }
}
