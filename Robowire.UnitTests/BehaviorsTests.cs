using System;

using Robowire.Behavior;

using Xunit;

namespace Robowire.UnitTests
{
    public class BehaviorsTests
    {
        [Fact]
        public void DisposeCallCouldBeDisabled()
        {
            var container = new Container();
            container.Setup(s => s.For<BasicScenarios.DisposableTest>().Use<BasicScenarios.DisposableTest>().WithBehavior<DisposeBehavior>(db => db.Dispose = false));

            var locator = container.GetLocator();

            var disposable = locator.Get<BasicScenarios.DisposableTest>();

            Assert.False(disposable.Disposed);

            locator.Dispose();

            Assert.False(disposable.Disposed);
        }

        [Fact]
        public void ImportedExistingInstancesCouldBeSetupToBeDisposed()
        {
            var disposable = new BasicScenarios.DisposableTest();

            var container = new Container();
            container.Setup(c => c.For<IDisposable>().Import.Existing(disposable).WithBehavior<DisposeBehavior>(db => db.Dispose = true));

            using (var locator = container.GetLocator())
            {
                var inst = locator.Get<IDisposable>();
                Assert.Equal(disposable, inst);
            }

            Assert.True(disposable.Disposed);
        }

        [Fact]
        public void ServicecouldBeSetupToReturnNewInstanceEveryTime()
        {
            var container = new Container();
            container.Setup(s => s.For<BasicScenarios.DisposableTest>().Use<BasicScenarios.DisposableTest>().WithBehavior<LifecycleBehavior>(sb => sb.AlwaysNewInstance = true));

            var locator = container.GetLocator();

            var inst1 = locator.Get<BasicScenarios.DisposableTest>();
            var inst2 = locator.Get<BasicScenarios.DisposableTest>();

            Assert.NotEqual(inst1, inst2);
        }
    }
}
