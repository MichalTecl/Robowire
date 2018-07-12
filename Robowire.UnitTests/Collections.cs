using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Robowire.UnitTests
{
    public class Collections
    {
        [Fact]
        public void LocatorCollectsRequiredInstances()
        {
            var container = new Container();
            container.Setup(
                s =>
                    {
                        s.For<C1>().Use<C1>();
                        s.For<C2>().Use<C2>();

                        s.Collect<IDisposable>();
                    });

            var locator = container.GetLocator();

            var collection = locator.GetCollection<IDisposable>().ToList();
            
            Assert.Equal(2, collection.Count);
        }

        [Fact]
        public void CollectionItemsAreSignleton()
        {
            var container = new Container();
            container.Setup(
                s =>
                {
                    s.For<C1>().Use<C1>();
                    s.For<C2>().Use<C2>();

                    s.Collect<IDisposable>();
                });

            var locator = container.GetLocator();

            var collection = locator.GetCollection<IDisposable>().ToList();

            Assert.Equal(2, collection.Count);

            Assert.Contains(locator.Get<C1>(), collection);
            Assert.Contains(locator.Get<C2>(), collection);
        }

        [Fact]
        public void EachTypeIsCollectedOnlyOnce()
        {
            var container = new Container();
            container.Setup(
                s =>
                {
                    s.For<C1>().Use<C1>();
                    s.For<C2>().Use<C2>();

                    s.Collect<IDisposable>();
                    s.Collect<IDisposable>();
                });

            var locator = container.GetLocator();

            var collection = locator.GetCollection<IDisposable>().ToList();

            Assert.Equal(2, collection.Count);
        }

        [Fact]
        public void CollectionInheritsCollectionFromParentContainer()
        {
            var parent = new Container();
            parent.Setup(
                s =>
                {
                    s.For<C1>().Use<C1>();
                    
                    s.Collect<IDisposable>();
                });

            var child = new Container(parent);
            child.Setup(s => s.For<C2>().Use<C2>());

            var locator = child.GetLocator();

            var collection = locator.GetCollection<IDisposable>().ToList();

            Assert.Equal(2, collection.Count);
        }

        public class C1 : IDisposable {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }

        public class C2 : C1
        {
        }


    }
}
