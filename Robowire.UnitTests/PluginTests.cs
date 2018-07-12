using System;
using System.Linq;
using System.Reflection;

using CodeGeneration;
using CodeGeneration.Primitives;

using Robowire.Plugin.DefaultPlugins;

using Xunit;

namespace Robowire.UnitTests
{
    public class AutoImplPluginTests
    {
        [Fact]
        public void TestAutoImpl()
        {
            var container = new Container();

            container.Setup(c => c.For<IAutoImplemented>());
            container.Setup(c => c.RegisterPlugin(s => s.CustomInstanceCreators.Add(new ReturnMemberNameImplementorPlugin())));

            var locator = container.GetLocator();

            var inst = locator.Get<IAutoImplemented>();

            Assert.NotNull(inst);

            Assert.Equal(nameof(IAutoImplemented.Property1), inst.Property1);
            Assert.Equal(nameof(IAutoImplemented.Method1), inst.Method1(1,true));
        }

        [Fact]
        public void ChildContainerInheritsParentsPlugins()
        {
            var container = new Container();

            container.Setup(c => c.RegisterPlugin(s => s.CustomInstanceCreators.Add(new ReturnMemberNameImplementorPlugin())));

            var childContainer = new Container(container);
            childContainer.Setup(c => c.For<IAutoImplemented>());

            var locator = childContainer.GetLocator();

            var inst = locator.Get<IAutoImplemented>();

            Assert.NotNull(inst);

            Assert.Equal(nameof(IAutoImplemented.Property1), inst.Property1);
            Assert.Equal(nameof(IAutoImplemented.Method1), inst.Method1(1, true));
        }

        [Fact]
        public void SetupCanAccessRegisteredPlugins()
        {
            var plugin = new ReturnMemberNameImplementorPlugin();

            var container = new Container();

            container.Setup(c => c.RegisterPlugin(s => s.CustomInstanceCreators.Add(plugin)));

            container.Setup(
                c =>
                    {
                        var p = c.GetRegisteredPlugins<ReturnMemberNameImplementorPlugin>().Single();
                        Assert.Equal(plugin, p);
                    });
        }

        [Fact]
        public void SetupCanAccessRegisteredPluginsInheritedFromParentContainer()
        {
            var plugin = new ReturnMemberNameImplementorPlugin();
            
            var container = new Container();

            container.Setup(c => c.RegisterPlugin(s => s.CustomInstanceCreators.Add(plugin)));

            var child = new Container(container);

            child.Setup(
                c =>
                {
                    var p = c.GetRegisteredPlugins<ReturnMemberNameImplementorPlugin>().Single();
                    Assert.Equal(plugin, p);
                });
        }

        public interface IAutoImplemented
        {
            string Property1 { get; }

            string Method1(int a, bool b);
        }

        public class ReturnMemberNameImplementorPlugin : InterfaceImplementorBase
        {
            protected override bool IsApplicable(Type interfaceType, Type implementingType)
            {
                return true;
            }

            protected override bool ImplementMethod(IClassBuilder impl, MethodInfo method, INamedReference serviceLocatorField)
            {
                impl.ImplementsMethod(method)
                    .WithModifier("public")
                    .Body.Write("return ")
                    .Write("\"")
                    .Write(method.Name)
                    .Write("\"")
                    .EndStatement();

                return true;
            }

            protected override InterfaceImplementorBase InheritForChildContainer()
            {
                return this;
            }

            protected override bool ImplementProperty(IClassBuilder impl, PropertyInfo propertyInfo, INamedReference serviceLocatorField)
            {
                impl.HasProperty(propertyInfo.Name, propertyInfo.PropertyType)
                    .WithModifier("public")
                    .HasGetter()
                    .Write("return \"")
                    .Write(propertyInfo.Name)
                    .Write("\"")
                    .EndStatement();

                return true;
            }
        }
    }
}
