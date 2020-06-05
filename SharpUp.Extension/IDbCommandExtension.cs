using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data
{
    public static class IDbCommandExtension
    {
        public async static Task<int> ExecuteNonQueryAsync(this IDbCommand command, CancellationToken token = default)
        {
            return await Task.Run(command.ExecuteNonQuery, token);
        }

        public async static Task<IDataReader> ExecuteReaderAsync(this IDbCommand command, CancellationToken token = default)
        {
            return await Task.Run(command.ExecuteReader, token);
        }

        public static DataTable ExecuteDataTable(this IDbCommand command)
        {
            IDataReader reader = command.ExecuteReader();
            return reader.ToDataTable();
        }

        public async static Task<DataTable> ExecuteDataTableAsync(this IDbCommand command, CancellationToken token = default)
        {
            return await Task.Run(command.ExecuteDataTable, token);
        }

        public static List<T> ExecuteList<T>(this IDbCommand command) where T : new()
        {
            IDataReader reader = command.ExecuteReader();
            return reader.ToList<T>();
        }

        public async static Task<List<T>> ExecuteListAsync<T>(this IDbCommand command, CancellationToken token = default) where T : new()
        {
            return await Task.Run(command.ExecuteList<T>, token);
        }

        public static T ExecuteScalar<T>(this IDbCommand command)
        {
            return (T)Convert.ChangeType(command.ExecuteScalar(), typeof(T));
        }

        public async static Task<T> ExecuteScalarAsync<T>(this IDbCommand command, CancellationToken token = default)
        {
            return await Task.Run(command.ExecuteScalar<T>, token);
        }
    }
}