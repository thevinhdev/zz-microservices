using System;
using System.Data;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IDisposable BeginTransaction(IsolationLevel level);
        void CommitChanges();
        Task CommitChangesAsync();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
