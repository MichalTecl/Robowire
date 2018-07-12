using System;
using System.Collections.Generic;
using System.Reflection;

using CodeGeneration;
using CodeGeneration.Primitives;

using Robowire.Core;
using Robowire.Plugin;
using Robowire.Plugin.DefaultPlugins;

using Xunit;

namespace Robowire.UnitTests
{
    public class BasicScenarios
    {
        [Fact]
        public void SetupClassToBeImplementationOfIface()
        {
            var container = new Container();

            container.Setup(s => s.For<ITestInterface1>().Use<TestClass1>());

            var locator = container.GetLocator();

            var x = locator.Get<ITestInterface1>();

            Assert.NotNull(x as TestClass1);
        }

        [Fact]
        public void EverythingIsSignleton()
        {
            var container = new Container();

            container.Setup(s => s.For<ITestInterface1>().Use<TestClass1>());

            var locator = container.GetLocator();

            var x = locator.Get<ITestInterface1>();
            var y = locator.Get(typeof(ITestInterface1));

            Assert.NotNull(x as TestClass1);
            Assert.NotNull(y as TestClass1);

            Assert.Equal(x, y);
        }

        [Fact]
        public void GetLocatorReturnsAlwaysNewInstance()
        {
            var container = new Container();

            container.Setup(s => s.For<ITestInterface1>().Use<TestClass1>());

            var locator1 = container.GetLocator();
            var locator2 = container.GetLocator();

            Assert.NotNull(locator1);
            Assert.NotNull(locator2);

            Assert.NotEqual(locator1, locator2);
        }

        [Fact]
        public void EachLocatorHasItsOwnInstances()
        {
            var container = new Container();

            container.Setup(s => s.For<ITestInterface1>().Use<TestClass1>());

            var locator1 = container.GetLocator();
            var locator2 = container.GetLocator();

            var x = locator1.Get<ITestInterface1>();
            var y = locator2.Get(typeof(ITestInterface1));

            Assert.NotNull(x as TestClass1);
            Assert.NotNull(y as TestClass1);

            Assert.NotEqual(x, y);
        }

        [Fact]
        public void ChildLocatorSharesInstancesWithParent()
        {
            var container = new Container();

            container.Setup(s => s.For<ITestInterface1>().Use<TestClass1>());

            var childContainer = new Container(container);
            childContainer.Setup(s => s.For<ITestInterface2>().Use<TestClass2>());

            var childLocator = childContainer.GetLocator();
            var parentLocator = childLocator.Parent;

            var fromChild = childLocator.Get<ITestInterface1>();
            var fromParent = parentLocator.Get<ITestInterface1>();

            Assert.NotNull(fromChild as TestClass1);
            Assert.NotNull(fromParent as TestClass1);

            Assert.Equal(fromChild, fromParent);
        }

        [Fact]
        public void LocatorResolvesCtorDependencies()
        {
            var container = new Container();

            container.Setup(s => s.For<ITestInterface2>().Use<TestClass2>());
            container.Setup(s => s.For<ITestInterface1>().Use<TestClass1>());

            var locator = container.GetLocator();

            var inner = locator.Get<ITestInterface1>();
            var outer = locator.Get<ITestInterface2>();

            inner.Property1 = 1234;

            Assert.Equal(inner.Property1, outer.Property2);
        }

        [Fact]
        public void LocatorCanPassDirectValueToCtorParam()
        {
            var outerObject = new TestClass1();

            var container = new Container();
            container.Setup(s => s.For<ITestInterface2>().Use<TestClass2>().With(outerObject));

            var locator = container.GetLocator();

            var inst = locator.Get<ITestInterface2>();

            outerObject.Property1 = 987;
            Assert.Equal(outerObject.Property1, inst.Property2);
        }

        [Fact]
        public void LocatorCanPassValueFactoryToCtorParam()
        {
            var outerObject = new TestClass1();

            var container = new Container();
            container.Setup(s => s.For<TestClass2>().Use<TestClass2>().With(loc => outerObject));

            var locator = container.GetLocator();

            var inst = locator.Get<TestClass2>();

            outerObject.Property1 = 987;
            Assert.Equal(outerObject.Property1, inst.Property2);
        }

        [Fact]
        public void DependencyOfLocatorTypeIsResolvedAutomatically()
        {
            var container = new Container();
            container.Setup(s => s.For<LocatorDependent>().Use<LocatorDependent>());

            var locator = container.GetLocator();
            var inst = locator.Get<LocatorDependent>();

            Assert.Equal(locator, inst.InjectedLocator);
        }

        [Fact]
        public void ExistingInstancesCouldBeImportedToContainer()
        {
            var imported = new TestClass1();

            var container = new Container();
            container.Setup(s => s.For<ITestInterface1>().Import.Existing(imported));

            var locator = container.GetLocator();

            var located = locator.Get<ITestInterface1>();

            Assert.Equal(imported, located);
        }

        [Fact]
        public void ContainerCouldBeSetupToCallFactory()
        {
            var imported = new TestClass1();

            var container = new Container();
            container.Setup(s => s.For<ITestInterface1>().Import.FromFactory(l => imported));

            var locator = container.GetLocator();

            var located = locator.Get<ITestInterface1>();

            Assert.Equal(imported, located);
        }

        [Fact]
        public void LocatorDisposesAllCreatedDisposables()
        {
            var container = new Container();
            container.Setup(s => s.For<DisposableTest>().Use<DisposableTest>());

            var locator = container.GetLocator();

            var disposable = locator.Get<DisposableTest>();

            Assert.False(disposable.Disposed);

            locator.Dispose();

            Assert.True(disposable.Disposed);
        }
        
        [Fact]
        public void GeneratorListenerIsCalledWithModuleCode()
        {
            var container = new Container();

            var listener = new TestListener();

            container.Setup(s => s.SubscribeCodeGeneratorListener(listener));

            container.GetLocator();

            Assert.False(string.IsNullOrWhiteSpace(listener.Code));
            Assert.False(listener.HasErrors);
            Assert.Null(listener.Errors);
            Assert.Equal(1, listener.NumberOfCalls);
        }

        [Fact]
        public void GeneratorListenerIsCalledOnlyOnce()
        {
            var container = new Container();

            var listener = new TestListener();

            container.Setup(s => s.SubscribeCodeGeneratorListener(listener));
            container.Setup(s => s.SubscribeCodeGeneratorListener(listener));

            container.GetLocator();

            Assert.Equal(1, listener.NumberOfCalls);
        }

        [Fact]
        public void GeneratorListenerCapturesCompilationExceptions()
        {
            var container = new Container();
            var listener = new TestListener();

            container.Setup(c => c.RegisterPlugin(s => s.DefaultInstanceCreators.Add(new CompilationErrorPlugin())));
            container.Setup(c => c.For<ITestInterface2>().Use<TestClass2>());
            container.Setup(c => c.SubscribeCodeGeneratorListener(listener));

            try
            {
                container.GetLocator();
            }
            catch
            {
                ;
            }

            Assert.True(listener.HasErrors);
            Assert.NotNull(listener.Errors);
        }

        [Fact]
        public void TestInstantiateNow()
        {
            var container = new Container();
            container.Setup(c => c.For<ITestInterface1>().Use<TestClass1>());

            var locator = container.GetLocator();

            var dependency = locator.Get<ITestInterface1>();
            dependency.Property1 = 42;

            var constructedOnFly = locator.InstantiateNow<TestClass2>();

            Assert.Equal(dependency.Property1, constructedOnFly.Property2);
        }

        [Fact]
        public void ImportedExistingInstancesAreNotDisposedByDefault()
        {
            var disposable = new DisposableTest();

            var container = new Container();
            container.Setup(c => c.For<IDisposable>().Import.Existing(disposable));

            using (var locator = container.GetLocator())
            {
                var inst = locator.Get<IDisposable>();
                Assert.Equal(disposable, inst);
            }

            Assert.False(disposable.Disposed);
        }

        public interface ITestInterface1
        {
            int Property1 { get; set; }
        }

        public class TestClass1 : ITestInterface1
        {
            public int Property1 { get; set; }
        }

        public interface ITestInterface2 { int Property2 { get; } }

        public class TestClass2 : ITestInterface2
        {
            private readonly ITestInterface1 m_dependency;

            public TestClass2(ITestInterface1 dependency)
            {
                m_dependency = dependency;
            }

            public int Property2
            {
                get
                {
                    return m_dependency.Property1;
                }
            }
        }

        public class LocatorDependent
        {
            public LocatorDependent(IServiceLocator injectedLocator)
            {
                InjectedLocator = injectedLocator;
            }

            public IServiceLocator InjectedLocator { get; }
        }

        public class DisposableTest : IDisposable
        {
            public bool Disposed { get; private set; }
            public void Dispose()
            {
                Disposed = true;
            }
        }

        private class TestListener : IGeneratedCodeListener
        {
            public string Code { get; private set; }

            public bool HasErrors { get; private set; }

            public Exception Errors { get; private set; }

            public int NumberOfCalls { get; private set; }

            public void OnContainerGenerated(string containerCode, bool hasErrors, Exception errors)
            {
                NumberOfCalls++;
                Code = containerCode;
                HasErrors = hasErrors;
                Errors = errors;
            }
        }

        private class CompilationErrorPlugin : IPlugin
        {
            public bool IsApplicable(IServiceSetupRecord setup)
            {
                return true;
            }
            
            public INamedReference GenerateFactoryMethod(
                IServiceSetupRecord setup,
                Dictionary<string, INamedReference> ctorParamValueFactoryFields,
                INamedReference valueFactoryField,
                IClassBuilder locatorBuilder,
                INamedReference previousPluginMethod)
            {
                var method = locatorBuilder.HasMethod("error");
                method.Body.Write("cannot compile this");

                return method;
            }

            public IPlugin InheritToChildContainer()
            {
                throw new NotImplementedException();
            }
        }
    }
}
