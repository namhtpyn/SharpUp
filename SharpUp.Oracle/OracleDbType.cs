using System;
using System.Collections.Generic;
using System.Text;

namespace SharpUp.Oracle
{
    public enum OracleDbType
    {
        BFile = global::Oracle.ManagedDataAccess.Client.OracleDbType.BFile,
        BinaryDouble = global::Oracle.ManagedDataAccess.Client.OracleDbType.BinaryDouble,
        BinaryFloat = global::Oracle.ManagedDataAccess.Client.OracleDbType.BinaryFloat,
        Blob = global::Oracle.ManagedDataAccess.Client.OracleDbType.Blob,
        Boolean = global::Oracle.ManagedDataAccess.Client.OracleDbType.Boolean,
        Byte = global::Oracle.ManagedDataAccess.Client.OracleDbType.Byte,
        Char = global::Oracle.ManagedDataAccess.Client.OracleDbType.Char,
        Clob = global::Oracle.ManagedDataAccess.Client.OracleDbType.Clob,
        Date = global::Oracle.ManagedDataAccess.Client.OracleDbType.Date,
        Decimal = global::Oracle.ManagedDataAccess.Client.OracleDbType.Decimal,
        Double = global::Oracle.ManagedDataAccess.Client.OracleDbType.Double,
        Int16 = global::Oracle.ManagedDataAccess.Client.OracleDbType.Int16,
        Int32 = global::Oracle.ManagedDataAccess.Client.OracleDbType.Int32,
        Int64 = global::Oracle.ManagedDataAccess.Client.OracleDbType.Int64,
        IntervalDS = global::Oracle.ManagedDataAccess.Client.OracleDbType.IntervalDS,
        IntervalYM = global::Oracle.ManagedDataAccess.Client.OracleDbType.IntervalYM,
        Long = global::Oracle.ManagedDataAccess.Client.OracleDbType.Long,
        LongRaw = global::Oracle.ManagedDataAccess.Client.OracleDbType.LongRaw,
        NChar = global::Oracle.ManagedDataAccess.Client.OracleDbType.NChar,
        NClob = global::Oracle.ManagedDataAccess.Client.OracleDbType.NClob,
        NVarchar2 = global::Oracle.ManagedDataAccess.Client.OracleDbType.NVarchar2,
        Raw = global::Oracle.ManagedDataAccess.Client.OracleDbType.Raw,
        RefCursor = global::Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor,
        Single = global::Oracle.ManagedDataAccess.Client.OracleDbType.Single,
        TimeStamp = global::Oracle.ManagedDataAccess.Client.OracleDbType.TimeStamp,
        TimeStampLTZ = global::Oracle.ManagedDataAccess.Client.OracleDbType.TimeStampLTZ,
        TimeStampTZ = global::Oracle.ManagedDataAccess.Client.OracleDbType.TimeStampTZ,
        Varchar2 = global::Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2,
        XmlType = global::Oracle.ManagedDataAccess.Client.OracleDbType.XmlType,
    }

    public static class TypeOracleExtension
    {
        public static OracleDbType ToOracleDbType(this Type type)
        {
            if (type == typeof(string)) return OracleDbType.Varchar2;
            else if (type == typeof(DateTime)) return OracleDbType.Date;
            else if (type == typeof(Int64)) return OracleDbType.Int64;
            else if (type == typeof(Int32)) return OracleDbType.Int32;
            else if (type == typeof(Int16)) return OracleDbType.Int16;
            else if (type == typeof(long)) return OracleDbType.Long;
            else if (type == typeof(sbyte)) return OracleDbType.Byte;
            else if (type == typeof(byte)) return OracleDbType.Int16;
            else if (type == typeof(decimal)) return OracleDbType.Decimal;
            else if (type == typeof(float)) return OracleDbType.Single;
            else if (type == typeof(double)) return OracleDbType.Double;
            else if (type == typeof(bool)) return OracleDbType.Boolean;
            else if (type == typeof(char)) return OracleDbType.Char;
            else if (type == typeof(TimeSpan)) return OracleDbType.IntervalDS;
            else return OracleDbType.Raw;
        }
    }
}