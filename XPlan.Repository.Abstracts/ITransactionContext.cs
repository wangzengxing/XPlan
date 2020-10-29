using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Repository.Abstracts
{
    public interface ITransactionContext
    {
        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
