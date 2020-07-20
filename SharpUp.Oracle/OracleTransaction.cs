using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.Oracle
{
    public class OracleTransaction : IDisposable
    {
        private global::Oracle.ManagedDataAccess.Client.OracleTransaction _transaction;

        public OracleTransaction(global::Oracle.ManagedDataAccess.Client.OracleTransaction transaction)
        {
            _transaction = transaction;
        }

        public void Commit() => _transaction.Commit();

        public Task CommitAsync(CancellationToken token = default) => Task.Run(Commit, token);

        public void Rollback() => _transaction.Rollback();

        public Task RollbackAsync(CancellationToken token = default) => Task.Run(Rollback, token);

        public void Rollback(string savepointName) => _transaction.Rollback(savepointName);

        public Task RollbackAsync(string savepointName, CancellationToken token = default) => Task.Run(() => Rollback(savepointName), token);

        public void Save(string savepointName) => _transaction.Save(savepointName);

        public Task SaveAsync(string savepointName, CancellationToken token = default) => Task.Run(() => _transaction.Save(savepointName), token);

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}