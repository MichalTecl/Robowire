using System.Collections.Generic;
using System.Linq;

using Robowire;
using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.Query.Reader;

using Xunit;

namespace RobOrm.UnitTests
{
    public class ResultMappingTests
    {
        [Fact]
        public void PkMappingTest()
        {
            var container = new Container();
            container.Setup(c => c.ScanType<ISimpleEntity>());

            var locator = container.GetLocator();

            var reader = new DataReaderMock("ISimpleEntity.Id", "ISimpleEntity.Text", "ISimpleEntity.Number");
            reader.Add(1, "Text1", 42);
            reader.Add(2, "Text2", 84);

            var entities = ResultSetReader.Read<ISimpleEntity>(reader, locator).ToList();

            Assert.Equal(2, entities.Count);

            for (var i = 1; i < 3; i++)
            {
                var entity = entities[i - 1];

                Assert.Equal(i, entity.Id);
                Assert.Equal($"Text{i}", entity.Text);
                Assert.Equal(42 * i, entity.Number);
            }
        }
        
        [Fact]
        public void TestHierarchicaMapping()
        {
            var container = new Container();
            container.Setup(c => c.ScanType<IHierarchy>());

            var locator = container.GetLocator();

            var reader = new DataReaderMock("IHierarchy.Id", "IHierarchy.Friend.Id", "IHierarchy.Children.Id", "IHierarchy.Children.Children.Id");
            reader.Add("A", "Friend_Of_A", "ChildOfA_1", "ChildOfChildOfA1_1");
            reader.Add("A", "Friend_Of_A", "ChildOfA_1", "ChildOfChildOfA1_2");
            reader.Add("A", "Friend_Of_A", "ChildOfA_2", "ChildOfChildOfA2_1");

            var entities = ResultSetReader.Read<IHierarchy>(reader, locator).ToList();

            Assert.Equal(1, entities.Count);

            Assert.Equal("A", entities[0].Id);
            Assert.Equal(2, entities[0].Children.Count());

            Assert.Equal(2, entities[0].Children.First().Children.Count());
            Assert.Equal(1, entities[0].Children.Last().Children.Count());

            Assert.Equal("ChildOfChildOfA2_1", entities[0].Children.Last().Children.Single().Id);

            Assert.NotNull(entities[0].Friend);
            Assert.Equal("Friend_Of_A", entities[0].Friend.Id);
        }

        [Entity]
        public interface IHierarchy
        {
            string Id { get; set; }
            IHierarchy Friend { get; }
            IEnumerable<IHierarchy> Children { get; }
        }

        [Entity]
        public interface ISimpleEntity
        {
            int? Id { get; }

            string Text{ get; set; }

            int Number { get; set; }
        }
    }
}
