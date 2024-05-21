using IOIT.Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        #region Fields

        //private DbTransaction _transaction;        
        private bool _disposed;
        private readonly DbContext _dbContext;
        #endregion

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        #region Private methods
        private DbConnection DbConnection
        {
            get
            {
                return _dbContext.Database.GetDbConnection();//https://stackoverflow.com/questions/41935752/entity-framework-core-how-to-get-the-connection-from-the-dbcontext
            }
        }

        private DbTransaction DbTransaction { get; set; }
        private void OpenConnection()
        {
            if (DbConnection.State != ConnectionState.Open)
            {
                DbConnection.Open();
            }
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseTransaction(DbTransaction transaction)
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        #endregion

        #region Methods

        public virtual IDisposable BeginTransaction(IsolationLevel isolationLevel)
        {
            var transaction = DbTransaction;
            if (transaction != null)
            {
                throw new ApplicationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                "Please commit or rollback the existing transaction before starting a new one.");
            }

            OpenConnection();
            transaction = DbConnection.BeginTransaction(isolationLevel);
            _dbContext.Database.UseTransaction(transaction);
            DbTransaction = transaction;
            return transaction;
        }

        public virtual void CommitTransaction()
        {
            var transaction = DbTransaction;
            if (transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _dbContext.SaveChanges();
                transaction.Commit();
                ReleaseTransaction(transaction);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public virtual void RollbackTransaction()
        {
            var transaction = DbTransaction;
            if (transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            transaction.Rollback();
            ReleaseTransaction(transaction);
        }

        public virtual void CommitChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.FirstOrDefault().GetDatabaseValues().Properties.ToDictionary(x => x.Name, x => x.PropertyInfo);
                throw new Application.Common.Exceptions.DbUpdateConcurrencyException(databaseValues);
            }
        }

        public virtual async Task CommitChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var databaseValues = ex.Entries.FirstOrDefault().GetDatabaseValues().Properties.ToDictionary(x => x.Name, x => x.PropertyInfo);
                throw new Application.Common.Exceptions.DbUpdateConcurrencyException(databaseValues);
            }

        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }

        #endregion
    }
}
