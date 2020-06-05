using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;

namespace SharpUp.Oracle
{
    public class OracleParameterCollection
    {
        private global::Oracle.ManagedDataAccess.Client.OracleParameterCollection _collection;
        private List<OracleParameter> _params = new List<OracleParameter>();

        public OracleParameterCollection(global::Oracle.ManagedDataAccess.Client.OracleParameterCollection collection)
        {
            _collection = collection;
        }

        public void Add(OracleParameter param)
        {
            _params.Add(param);
            _collection.Add(param._param);
        }

        public void Add(object value)
        {
            Add(new OracleParameter(value));
        }

        public void Add(params object[] values)
        {
            foreach (var value in values) Add(value);
        }

        public void Add(params OracleParameter[] parameters)
        {
            foreach (var param in parameters) Add(param);
        }
    }
}