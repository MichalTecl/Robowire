using System.Transactions;

namespace Robowire.RobOrm.Core
{
    public interface ITransactionManager<TConnection>
    {
        ITransaction<TConnection> Open();
    }
}
