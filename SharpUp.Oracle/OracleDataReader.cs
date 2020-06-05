using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.Oracle
{
    public class OracleDataReader
    {
        private global::Oracle.ManagedDataAccess.Client.OracleDataReader _reader;

        public OracleDataReader(global::Oracle.ManagedDataAccess.Client.OracleDataReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Load(_reader);
            _reader.Close();
            return dt;
        }

        public Task<DataTable> ToDataTableAsync(CancellationToken token = default) => Task.Run(ToDataTable, token);

        public List<Dictionary<string, dynamic>> ToList()
        {
            if (_reader == null) return null;

            var listObject = new List<Dictionary<string, dynamic>>();
            while (_reader.Read())
            {
                var newObject = new Dictionary<string, dynamic>();
                for (int index = 0; index < _reader.FieldCount; index++)
                {
                    var fieldName = _reader.GetName(index).Replace("_", "").ToUpper();
                    var fieldValue = _reader.GetValue(index);

                    newObject.Add(fieldName, fieldValue);
                }
                listObject.Add(newObject);
            }
            return listObject;
        }

        public Task<List<Dictionary<string, dynamic>>> ToListAsync(CancellationToken token = default) => Task.Run(ToList, token);

        public List<T> ToList<T>() where T : new()
        {
            if (_reader == null) return null;

            Type ObjectType = typeof(T);
            List<T> listObject = new List<T>();
            Dictionary<string, PropertyInfo> objectProps = ObjectType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name.ToUpper(), p => p);

            while (_reader.Read())
            {
                T newObject = new T();
                for (int index = 0; index < _reader.FieldCount; index++)
                {
                    string fieldName = _reader.GetName(index).Replace("_", "").ToUpper();
                    if (!objectProps.ContainsKey(fieldName)) continue;

                    var propInfo = objectProps[fieldName];
                    if (propInfo == null || !propInfo.CanWrite) continue;

                    try
                    {
                        var fieldValue = _reader.GetValue(index);
                        if (fieldValue == DBNull.Value) propInfo.SetValue(newObject, null);
                        else if (propInfo.PropertyType.IsEnum)
                            propInfo.SetValue(newObject, Enum.Parse(propInfo.PropertyType, fieldValue.ToString()));
                        else
                        {
                            Type type = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                            propInfo.SetValue(newObject, Convert.ChangeType(fieldValue, type));
                        }
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

        public Task<List<T>> ToListAsync<T>(CancellationToken token = default) where T : new() => Task.Run(ToList<T>, token);
    }
}