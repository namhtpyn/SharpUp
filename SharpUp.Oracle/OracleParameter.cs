using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SharpUp.Oracle
{
    public class OracleParameter
    {
        internal global::Oracle.ManagedDataAccess.Client.OracleParameter _param;

        public OracleDbType DbType
        {
            get => (OracleDbType)_param.OracleDbType;
            set => _param.OracleDbType = (global::Oracle.ManagedDataAccess.Client.OracleDbType)value;
        }

        public OracleCollectionType CollectionType
        {
            get => (OracleCollectionType)_param.CollectionType;
            set => _param.CollectionType = (global::Oracle.ManagedDataAccess.Client.OracleCollectionType)value;
        }

        public object Value
        {
            get => _param.Value;
            set
            {
                if (value is OracleDbType && (OracleDbType)value == OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output;
                    DbType = OracleDbType.RefCursor;
                    return;
                }

                if (value != null && value.GetType().IsArray)
                {
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    DbType = value.GetType().GetElementType().ToOracleDbType();
                }
                _param.Value = value;
            }
        }

        public string ParameterName
        {
            get => _param.ParameterName;
            set => _param.ParameterName = value;
        }

        public ParameterDirection Direction
        {
            get => _param.Direction;
            set => _param.Direction = value;
        }

        public OracleParameter()
        {
            _param = new global::Oracle.ManagedDataAccess.Client.OracleParameter();
        }

        public OracleParameter(object value) : this()
        {
            Value = value;
        }

        public OracleParameter(string paramName, object value) : this(value)
        {
            ParameterName = paramName;
        }

        public OracleParameter(string paramName, object value, OracleDbType type, ParameterDirection direction = ParameterDirection.Input) : this(paramName, value)
        {
            DbType = type;
            Direction = direction;
        }
    }
}