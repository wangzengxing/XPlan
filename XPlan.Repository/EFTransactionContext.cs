using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XPlan.Repository.Abstracts;

namespace XPlan.Repository.EntityFrameworkCore
{
    public class EFTransactionContext : ITransactionContext
    {
        private IDbContextTransaction _transaction;

        public EFTransactionContext(IDbContextTransaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public Task CommitAsync()
        {
            return _transaction.CommitAsync();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public Task RollbackAsync()
        {
            return _transaction.RollbackAsync();
        }
    }
}
