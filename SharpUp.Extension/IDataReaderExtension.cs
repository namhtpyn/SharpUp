using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data
{
    public static class IDataReaderExtension
    {
        public static async Task CloseAsync(this IDataReader reader, CancellationToken token = default)
        {
            await Task.Run(reader.Close, token);
        }

        public static async Task<bool> ReadAsync(this IDataReader reader, CancellationToken token = default)
        {
            return await Task.Run(reader.Read, token);
        }

        public static DataTable ToDataTable(this IDataReader reader)
        {
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            return dt;
        }

        public static async Task<DataTable> ToDataTableAsync(this IDataReader reader, CancellationToken token = default)
        {
            return await Task.Run(reader.ToDataTable, token);
        }

        public static List<T> ToList<T>(this IDataReader reader) where T : new()
        {
            if (reader == null) return null;

            Type ObjectType = typeof(T);
            List<T> listObject = new List<T>();
            Dictionary<string, PropertyInfo> objectProps = ObjectType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name.ToUpper(), p => p);

            while (reader.Read())
            {
                T newObject = new T();
                for (int index = 0; index < reader.FieldCount; index++)
                {
                    string fieldName = reader.GetName(index).Replace("_", "").ToUpper();
                    if (!objectProps.ContainsKey(fieldName)) continue;

                    var propInfo = objectProps[fieldName];
                    if (propInfo == null || !propInfo.CanWrite) continue;

                    try
                    {
                        var fieldValue = reader.GetValue(index);
                        if (fieldValue == DBNull.Value) propInfo.SetValue(newObject, default);
                        else if (propInfo.PropertyType.IsEnum)
                            propInfo.SetValue(newObject, Enum.Parse(propInfo.PropertyType, fieldValue.ToString()));
                        else
                            propInfo.SetValue(newObject, Convert.ChangeType(fieldValue, propInfo.PropertyType));
                    }
                    catch
                    {
                        propInfo.SetValue(newObject, default);
                    }
                }
                listObject.Add(newObject);
            }
            return listObject;
        }

        public static async Task<List<T>> ToListAsync<T>(this IDataReader reader, CancellationToken token = default) where T : new()
        {
            return await Task.Run(reader.ToList<T>, token);
        }
    }
}