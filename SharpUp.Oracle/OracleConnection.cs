using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.Oracle
{
    public class OracleConnection : IDisposable
    {
        internal global::Oracle.ManagedDataAccess.Client.OracleConnection _connection;

        public OracleConnection(string connectionString)
        {
            _connection = new global::Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
        }

        public void Open() => _connection.Open();

        public Task OpenAsync(CancellationToken token = default) => Task.Run(Open, token);

        public void Close() => _connection.Close();

        public Task CloseAsync(CancellationToken token = default) => Task.Run(Close, token);

        public OracleCommand CreateCommand() => new OracleCommand(this);

        public Task<OracleCommand> CreateCommandAsync() => Task.Run(CreateCommand);

        public OracleCommand CreateSqlCommand(string sql)
        {
            var cmd = CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public Task<OracleCommand> CreateSqlCommandAsync(string sql) => Task.Run(() => CreateSqlCommand(sql));

        public OracleCommand CreateSqlCommand(string sql, params object[] variables)
        {
            var cmd = CreateSqlCommand(sql);
            cmd.Parameters.Add(variables.Select((v, i) => new OracleParameter(i.ToString(), v)).ToArray());
            return cmd;
        }

        public Task<OracleCommand> CreateSqlCommandAsync(string sql, params object[] variables) => Task.Run(() => CreateSqlCommand(sql, variables));

        public OracleCommand CreateProcCommand(string storedProcedure)
        {
            var cmd = CreateCommand();
            cmd.CommandText = storedProcedure;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public Task<OracleCommand> CreateProcCommandAsync(string storedProcedure) => Task.Run(() => CreateProcCommand(storedProcedure));

        public OracleCommand CreateProcCommand(string storedProcedure, params OracleParameter[] parameters)
        {
            var cmd = CreateProcCommand(storedProcedure);
            cmd.Parameters.Add(parameters);
            return cmd;
        }

        public Task<OracleCommand> CreateProcCommandAsync(string storedProcedure, params OracleParameter[] parameters) => Task.Run(() => CreateProcCommand(storedProcedure, parameters));

        public OracleCommand CreateProcCommand(string storedProcedure, params object[] variables)
        {
            return CreateProcCommand(storedProcedure, variables.Select((v, i) => new OracleParameter(i.ToString(), v)).ToArray());
        }

        public Task<OracleCommand> CreateProcCommandAsync(string storedProcedure, params object[] variables) => Task.Run(() => CreateProcCommand(storedProcedure, variables));

        public OracleTransaction BeginTransaction() => new OracleTransaction(_connection.BeginTransaction());

        public Task<OracleTransaction> BeginTransactionAsync(CancellationToken token = default) => Task.Run(BeginTransaction, token);

        public OracleTransaction BeginTransaction(IsolationLevel level) => new OracleTransaction(_connection.BeginTransaction(level));

        public Task<OracleTransaction> BeginTransactionAsync(IsolationLevel level, CancellationToken token = default) => Task.Run(() => BeginTransaction(level), token);

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}