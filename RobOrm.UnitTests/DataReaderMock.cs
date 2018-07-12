using System.Collections.Generic;
using System.Linq;

using Robowire.RobOrm.Core;
using Robowire.RobOrm.Core.Query.Reader;

namespace RobOrm.UnitTests
{
    public class DataReaderMock : DataReaderBase
    {
        private readonly string[] m_columns;
        private List<List<object>> m_data = new List<List<object>>();
        private int m_position = -1;

        public DataReaderMock(params string[] columnNames)
            : base()
        {
            m_columns = columnNames;
        }

        private DataReaderMock(string path, List<string> columnIndex, int position, List<List<object>> data)
            : base(path, columnIndex)
        {
            m_position = position;
            m_data = data;
            m_columns = columnIndex.ToArray();
        }

        public void Add(params object[] o)
        {
            m_data.Add(o.ToList());
        }

        protected override IDataReader CreateChildReader(string childPath, List<string> columnIndex)
        {
            var child = new DataReaderMock(childPath, m_columns.ToList(), m_position, m_data);
            return child;
        }

        protected override bool GetIsNull(int column)
        {
            return m_data[m_position][column] == null;
        }

        protected override T GetValue<T>(int column)
        {
            return (T)m_data[m_position][column];
        }

        protected override bool NextRecord()
        {
            m_position++;
            return m_position < m_data.Count;
        }

        protected override IEnumerable<string> GetColumnsOrder()
        {
            return m_columns;
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
