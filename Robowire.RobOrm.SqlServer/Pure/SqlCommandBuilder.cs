using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Robowire.RobOrm.Core.NonOrm;

namespace Robowire.RobOrm.SqlServer.Pure
{
    public class SqlCommandBuilder : ISqlBuilder, ISqlExecutor
    {
        
        private readonly List<SqlParameter> m_parameters = new List<SqlParameter>();

        private readonly List<Action<SqlParameterCollection>> m_paramsCallbacks = new List<Action<SqlParameterCollection>>();
        private string m_commandText;
        private CommandType m_commandType;

        private readonly IExecutor m_executor;

        public SqlCommandBuilder(IExecutor executor)
        {
            m_executor = executor;
        }

        public ISqlExecutor Call(string storedProcedureName)
        {
            m_commandText = storedProcedureName;
            m_commandType = CommandType.StoredProcedure;

            return this;
        }

        public ISqlExecutor Execute(string sql)
        {
            m_commandText = sql;
            m_commandType = CommandType.Text;

            return this;
        }

        public ISqlExecutor ExecuteWithParams(string sql, params object[] parameter)
        {
            if (parameter == null)
            {
                return Execute(sql);
            }

            for (var i = 0; i < parameter.Length; i++)
            {
                var paramName = $"@p{i}";
                WithParam(paramName, parameter[i]);

                var placeHolder = $"{{{i}}}";

                if (!sql.Contains(placeHolder))
                {
                    throw new InvalidOperationException($"Placeholder \"{placeHolder}\" expected");
                }

                sql = sql.Replace(placeHolder, paramName);
            }

            return Execute(sql);
        }

        public ISqlExecutor WithParam(string paramName, object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }

            m_parameters.Add(new SqlParameter(paramName, value));
            return this;
        }

        public ISqlExecutor WithParam(SqlParameter parameter)
        {
            m_parameters.Add(parameter);
            return this;
        }

        public ISqlExecutor WithParams(Action<SqlParameterCollection> paramsCollection)
        {
            m_paramsCallbacks.Add(paramsCollection);
            return this;
        }

        public ISqlExecutor WithOptionalParam(Func<bool> includeParameter, string name, object value)
        {
            return WithOptionalParam(includeParameter, name, () => value);
        }

        public ISqlExecutor WithOptionalParam(Func<bool> includeParameter, string name, Func<object> value)
        {
            return includeParameter() ? WithParam(name, value()) : this;
        }

        public ISqlExecutor WithStructuredParam(string parameterName, string dataTypeName, DataTable table)
        {
            var parameter = new SqlParameter
                                {
                                    ParameterName = parameterName,
                                    TypeName = dataTypeName,
                                    SqlDbType = SqlDbType.Structured,
                                    Value = table
                                };
            m_parameters.Add(parameter);

            return this;
        }

        public ISqlExecutor WithStructuredParam(string parameterName, DataTable table)
        {
            return WithStructuredParam(parameterName, table.TableName, table);
        }

        public ISqlExecutor WithStructuredParam<T>(
            string parameterName,
            string dataTypeName,
            IEnumerable<T> collection,
            IEnumerable<string> dataTypeAttributes,
            Func<T, object[]> rowGenerator)
        {
            var table = new DataTable(dataTypeName);

            foreach (var attribute in dataTypeAttributes)
            {
                table.Columns.Add(attribute);
            }

            foreach (var row in collection)
            {
                table.Rows.Add(rowGenerator(row));
            }

            return WithStructuredParam(parameterName, dataTypeName, table);
        }

        public void Read(Action<DbDataReader> readerAction)
        {
            Execute<object>(
                cmd =>
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            readerAction(reader);
                        }

                        return null;
                    });
        }
        
        public T Read<T>(Func<DbDataReader, T> readerAction)
        {
            return Execute<T>(
                cmd =>
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        return readerAction(reader);
                    }
                });
        }
        
        public void ReadRows(Action<DbDataReader> rowAction)
        {
            Read(
                reader =>
                    {
                        while (reader.Read())
                        {
                            rowAction(reader);
                        }
                    });
        }
        
        public IList<T> MapRows<T>(Func<DbDataReader, T> rowMapper)
        {
            var result = new List<T>();

            ReadRows(row => result.Add(rowMapper(row)));

            return result;
        }
        
        public object Scalar()
        {
            return Execute(c => c.ExecuteScalar());
        }

        public T Scalar<T>()
        {
            var res = Scalar();
            if (res == null || DBNull.Value.Equals(res) && (Nullable.GetUnderlyingType(typeof(T)) != null || typeof(T).IsClass))
            {
                return default(T);
            }

            return (T)res;
        }

        public int NonQuery()
        {
            return Execute(c => c.ExecuteNonQuery());
        }
        
        public DataTable Table()
        {
            return Execute(
                c =>
                    {
                        using (var adapter = new SqlDataAdapter(c))
                        {
                            var table = new DataTable();
                            adapter.Fill(table);
                            return table;
                        }
                    });
        }

        public DataSet DataSet()
        {
            return Execute(
                c =>
                {
                    using (var adapter = new SqlDataAdapter(c))
                    {
                        var set = new DataSet();
                        adapter.Fill(set);
                        return set;
                    }
                });
        }
        
        private void SetupCommand(SqlCommand command)
        {
            if (string.IsNullOrWhiteSpace(m_commandText))
            {
                throw new InvalidOperationException("No command specified. Use Execute, Call or ExecuteWithParams method.");
            }

            command.CommandType = m_commandType;
            command.CommandText = m_commandText;

            command.Parameters.AddRange(m_parameters.ToArray());

            foreach (var paramsCallback in m_paramsCallbacks)
            {
                paramsCallback(command.Parameters);
            }
        }

        private T Execute<T>(Func<SqlCommand, T> action)
        {
            return m_executor.Execute(SetupCommand, action);
        }
        
    }
}
