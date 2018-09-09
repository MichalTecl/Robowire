using System;
using System.Collections.Generic;
using System.Linq;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.Query.Model;

using Xunit;

namespace RobOrm.UnitTests
{
    public class SlowDataModelHelperTests
    {
        [Fact]
        public void TestReferenceHandling()
        {
            var sut = new SlowDataModelHelper();

            var refs = sut.GetReferences(typeof(IUser)).ToList();

            Assert.Equal(2, refs.Count);

            var referenceInfo = refs.Single(i => i.LeftModelPropertyName == nameof(IUser.House));
            Assert.Equal(typeof(IUser), referenceInfo.LeftEntityType);
            Assert.Equal(nameof(IUser.HouseId), referenceInfo.LeftKeyColumnName);
            Assert.Equal(nameof(IHouse.Id), referenceInfo.RightKeyColumnName);
            Assert.Equal(typeof(IHouse), referenceInfo.RightEntityType);

            referenceInfo = refs.Single(i => i.LeftModelPropertyName == nameof(IUser.Cottage));
            Assert.Equal(typeof(IUser), referenceInfo.LeftEntityType);
            Assert.Equal(nameof(IUser.WeekendHouseId), referenceInfo.LeftKeyColumnName);
            Assert.Equal(nameof(IHouse.Id), referenceInfo.RightKeyColumnName);
            Assert.Equal(typeof(IHouse), referenceInfo.RightEntityType);
        }

        [Fact]
        public void TestBeingReferrenced()
        {
            var sut = new SlowDataModelHelper();

            var refs = sut.GetReferences(typeof(IHouse)).ToList();

            Assert.Equal(2, refs.Count);

            var referenceInfo = refs.Single(i => i.LeftModelPropertyName == nameof(IHouse.Owners));
            Assert.Equal(typeof(IHouse), referenceInfo.LeftEntityType);
            Assert.Equal(nameof(IHouse.Id), referenceInfo.LeftKeyColumnName);
            Assert.Equal(nameof(IUser.HouseId), referenceInfo.RightKeyColumnName);
            Assert.Equal(typeof(IUser), referenceInfo.RightEntityType);

            referenceInfo = refs.Single(i => i.LeftModelPropertyName == nameof(IHouse.WeekendOwners));
            Assert.Equal(typeof(IHouse), referenceInfo.LeftEntityType);
            Assert.Equal(nameof(IHouse.Id), referenceInfo.LeftKeyColumnName);
            Assert.Equal(nameof(IUser.WeekendHouseId), referenceInfo.RightKeyColumnName);
            Assert.Equal(typeof(IUser), referenceInfo.RightEntityType);
        }

        [Fact]
        public void TestColumnsMapping()
        {
            var sut = new SlowDataModelHelper();

            var cols = sut.GetTableDataColumns(typeof(IEttx)).ToList();

            Assert.Equal(5, cols.Count);
        }

        [Entity]
        public interface IEttx
        {
            int Id { get; }

            Guid GuidCol { get; set; }

            DateTime DtCol { get; set; }

            bool BoolCol { get; set; }

            decimal DecimalCol { get; set; }
        }


        [Entity]
        public interface IUser
        {
            int Id { get; }

            int HouseId { get; set; }

            IHouse House { get; }
            
            [NotFk]
            int WeekendHouseId { get; set; }

            [LocalKey(nameof(WeekendHouseId))]
            IHouse Cottage { get; }
        }

        [Entity]
        public interface IHouse
        {
            int Id { get; }

            IEnumerable<IUser> Owners { get; }

            [ForeignKey(nameof(IUser.WeekendHouseId))]
            IEnumerable<IUser> WeekendOwners { get; }
        }
    }
}
