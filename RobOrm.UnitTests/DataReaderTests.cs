using Xunit;

namespace RobOrm.UnitTests
{
    public class DataReaderTests
    {
        [Fact]
        public void TestReader()
        {
            var reader = new DataReaderMock("A", "A.B", "A.C", "A.B.A");
            reader.Add("A1", "A.B1", "A.C1", "A.B.A1");

            Assert.True(reader.Read());

            Assert.False(reader.IsNull("A"));
            Assert.Equal("A1", reader.Get<string>("A"));
            Assert.True(reader.IsNull("B"));

            var child = reader.GetDeeperReader("A");
            Assert.True(child.IsNull("A"));
            Assert.Equal("A.B1", child.Get<string>("B"));
            Assert.Equal("A.C1", child.Get<string>("C"));

            child = child.GetDeeperReader("B");
            Assert.True(child.IsNull("B"));
            Assert.True(child.IsNull("C"));
            Assert.Equal("A.B.A1", child.Get<string>("A"));
        }
    }
}
