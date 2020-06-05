using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data
{
    public static class IDbConnectionExtension
    {
        public async static Task OpenTaskAsync(this IDbConnection connection, CancellationToken token = default)
        {
            await Task.Run(connection.Open, token);
        }

        public async static Task CloseTaskAsync(this IDbConnection connection, CancellationToken token = default)
        {
            await Task.Run(connection.Close, token);
        }

        public static IDbCommand CreateSqlCommand(this IDbConnection connection, string sql)
        {
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        public static async Task<IDbCommand> CreateSqlCommandAsync(this IDbConnection connection, string sql)
        {
            return await Task.Run(() => connection.CreateSqlCommand(sql));
        }

        public static IDbCommand CreateProcCommand(this IDbConnection connection, string storedProcedure)
        {
            IDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"BEGIN {storedProcedure}(); END;";
            return cmd;
        }

        public static async Task<IDbCommand> CreateProcCommandAsync(this IDbConnection connection, string storedProcedure)
        {
            return await Task.Run(() => connection.CreateProcCommand(storedProcedure));
        }

        public static IDbCommand CreateProcCommand(this IDbConnection connection, string storedProcedure, params object[] variables)
        {
            IDbCommand cmd = connection.CreateCommand();
            List<string> oraParams = new List<string>();
            foreach (var variable in variables)
            {
                var type = variable.GetType();
                if (type == typeof(string) || type == typeof(char))
                    oraParams.Add("'{" + variable.ToString() + "}'");
                else if (type.IsEnum)
                    oraParams.Add(((int)variable).ToString());
                else if (type == typeof(DateTime))
                    oraParams.Add($"TO_DATE('{(DateTime)variable:yyyy/MM/dd HH:mm:ss}', 'yyyy/mm/dd hh24:mi:ss')");
                else
                    oraParams.Add(variable.ToString());
            }
            cmd.CommandText = $"BEGIN {storedProcedure}({string.Join(",", oraParams)}); END;";
            return cmd;
        }

        public static async Task<IDbCommand> CreateProcCommandAsync(this IDbConnection connection, string storedProcedure, params object[] variables)
        {
            return await Task.Run(() => connection.CreateProcCommand(storedProcedure, variables));
        }
    }
}