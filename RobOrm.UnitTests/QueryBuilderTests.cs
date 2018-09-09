using System.Collections.Generic;
using System.Linq;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.Query.Building;
using Robowire.RobOrm.Core.Query.Model;

using Xunit;

namespace RobOrm.UnitTests
{
    public class QueryBuilderTests
    {
        [Fact]
        public void TestRootEntityNameAndAlias()
        {
            var model = new SlowDataModelHelper();
            var qb = new QueryBuilder<IEntity1>(model, null);

            var query = qb.Build();

            Assert.Equal("Entity1", query.RootTableName);
            Assert.Equal(nameof(IEntity1), query.RootTableAlias);
        }

        [Fact]
        public void TestSelectedColumns()
        {
            var model = new SlowDataModelHelper();
            var qb = new QueryBuilder<IEntity1>(model, null);
            
            var query = qb.Build();

            Assert.Equal(0, query.Joins.Count());

            var colsList = query.SelectedColumns.ToList();

            Assert.Equal(5, colsList.Count);
            Assert.NotNull(colsList.Single(i => i.ColumnName == nameof(IEntity1.Id)));
            Assert.NotNull(colsList.Single(i => i.ColumnName == nameof(IEntity1.ChildId)));
            Assert.NotNull(colsList.Single(i => i.ColumnName == nameof(IEntity1.SomeText)));
            Assert.NotNull(colsList.Single(i => i.ColumnName == nameof(IEntity1.SomeBool)));
            Assert.NotNull(colsList.Single(i => i.ColumnName == nameof(IEntity1.ParentId)));

            Assert.True(colsList.All(i => i.EntityAlias == nameof(IEntity1)));
        }

        [Fact]
        public void TestJoins()
        {
            var model = new SlowDataModelHelper();
            var qb = new QueryBuilder<IEntity1>(model, null);


            var minId = 123;

            qb.Join(e => e.Child.Child.Child)
              .Where(e => e.Children.Each().SomeBool && e.Id != minId)
              .OrderBy(e => e.Id)
              .OrderByDesc(e => e.ChildId)
              .Take(100)
              .Skip(10);

            var query = qb.Build();

            var joins = query.Joins.ToList();
            Assert.Equal(3, joins.Count);

            var qstr = query.ToString();

            Assert.NotNull(query);
        }

        [Entity]
        public interface IEntity1
        {
            int Id { get; }

            [DbString]
            string SomeText { get; set; }

            int? ChildId { get; set; }

            [ForeignKey(nameof(ParentId))]
            IEnumerable<IEntity1> Children { get; }
            
            [NotFk]
            int? ParentId { get; set; }

            bool SomeBool { get; set; }

            IEntity1 Child { get; }
        }
    }
}
