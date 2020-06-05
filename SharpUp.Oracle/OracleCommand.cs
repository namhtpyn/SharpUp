using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.Oracle
{
    public sealed class OracleCommand : IDisposable
    {
        internal global::Oracle.ManagedDataAccess.Client.OracleCommand _command;
        internal OracleParameterCollection _collection;

        public string CommandText
        {
            get => _command.CommandText;
            set => _command.CommandText = value;
        }

        public int CommandTimeout
        {
            get => _command.CommandTimeout;
            set => _command.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => _command.CommandType;
            set => _command.CommandType = value;
        }

        //internal bool CustomProc { get; set; } = false;
        public OracleParameterCollection Parameters => _collection;

        public OracleCommand()
        {
            _command = new global::Oracle.ManagedDataAccess.Client.OracleCommand();
            _collection = new OracleParameterCollection(_command.Parameters);
        }

        public OracleCommand(OracleConnection connection) : this()
        {
            _command.Connection = connection._connection;
        }

        public int ExecuteNonQuery() => _command.ExecuteNonQuery();

        public Task<int> ExecuteNonQueryAsync(CancellationToken token = default) => Task.Run(ExecuteNonQuery, token);

        public OracleDataReader ExecuteReader() => new OracleDataReader(_command.ExecuteReader());

        public Task<OracleDataReader> ExecuteReaderAsync(CancellationToken token = default) => Task.Run(ExecuteReader, token);

        public object ExecuteScalar() => _command.ExecuteScalar();

        public Task<object> ExecuteScalarAsync(CancellationToken token = default) => Task.Run(ExecuteScalar, token);

        public T ExecuteScalar<T>() => (T)Convert.ChangeType(ExecuteScalar(), typeof(T));

        public Task<T> ExecuteScalarAsync<T>(CancellationToken token = default) => Task.Run(ExecuteScalar<T>, token);

        public Stream ExecuteStream() => _command.ExecuteStream();

        public Task<Stream> ExecuteStreamAsync(CancellationToken token = default) => Task.Run(ExecuteStream, token);

        public DataTable ExecuteDataTable() => ExecuteReader().ToDataTable();

        public Task<DataTable> ExecuteDataTableAsync(CancellationToken token = default) => Task.Run(ExecuteDataTable, token);

        public List<Dictionary<string, dynamic>> ExecuteList() => ExecuteReader().ToList();

        public Task<List<Dictionary<string, dynamic>>> ExecuteListAsync(CancellationToken token = default) => Task.Run(ExecuteList, token);

        public List<T> ExecuteList<T>() where T : new() => ExecuteReader().ToList<T>();

        public Task<List<T>> ExecuteListAsync<T>(CancellationToken token = default) where T : new() => Task.Run(ExecuteList<T>, token);

        public void Dispose()
        {
            _command.Dispose();
        }
    }
}