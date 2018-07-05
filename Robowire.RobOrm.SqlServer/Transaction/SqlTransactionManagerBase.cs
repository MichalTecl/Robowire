using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Robowire.RobOrm.Core;

namespace Robowire.RobOrm.SqlServer.Transaction
{
    public abstract class SqlTransactionManagerBase : ITransactionManager<SqlConnection>
    {
        private readonly ThreadLocal<ISqlTransaction> m_threadTransaction= new ThreadLocal<ISqlTransaction>();

        protected abstract Func<SqlConnection> ConnectionFactory { get; }

        public ITransaction<SqlConnection> Open()
        {
            var parent = m_threadTransaction.Value;

            var child = parent == null ? (ISqlTransaction)new SqlTransaction(ConnectionFactory, this) : new ChildSqlTransaction(parent, this);

            m_threadTransaction.Value = child;

            return child;
        }

        public void RemoveCurrentTransaction()
        {
            var child = m_threadTransaction.Value;
            if (child == null)
            {
                return;
            }
            
            m_threadTransaction.Value = child.Parent;
        }
    }
}
