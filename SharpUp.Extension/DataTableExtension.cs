using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data
{
    public static class DataTableExtension
    {
        public static async Task LoadAsync(this DataTable table, IDataReader reader, CancellationToken token = default)
        {
            await Task.Run(() => table.Load(reader), token);
        }

        public static List<Dictionary<string, dynamic>> ToList(this DataTable table)
        {
            if (table == null) return null;

            var listObject = new List<Dictionary<string, dynamic>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, dynamic> newObject = new Dictionary<string, dynamic>();
                for (int index = 0; index < table.Columns.Count; index++)
                {
                    string fieldName = table.Columns[index].ColumnName;
                    var fieldValue = row[fieldName];
                    newObject.Add(fieldName.Replace("_", ""), fieldValue);
                }
                listObject.Add(newObject);
            }
            return listObject;
        }

        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            if (table == null) return null;

            Type ObjectType = typeof(T);
            List<T> listObject = new List<T>();
            Dictionary<string, PropertyInfo> objectProps = ObjectType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(p => p.Name.ToUpper(), p => p);

            foreach (DataRow row in table.Rows)
            {
                T newObject = new T();
                for (int index = 0; index < table.Columns.Count; index++)
                {
                    string fieldName = table.Columns[index].ColumnName;
                    if (!objectProps.ContainsKey(fieldName.Replace("_", "").ToUpper())) continue;

                    var propInfo = objectProps[fieldName.Replace("_", "").ToUpper()];
                    if (propInfo == null || !propInfo.CanWrite) continue;

                    try
                    {
                        var fieldValue = row[fieldName];
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

        public static async Task<List<T>> ToListAsync<T>(this DataTable table, CancellationToken token = default) where T : new()
        {
            return await Task.Run(table.ToList<T>, token);
        }
    }
}