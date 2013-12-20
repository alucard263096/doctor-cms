using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using log4net;
using log4net.Config;

namespace SunStar_CMS.admin.Classes.Utils 
{
    public class DBUtil: IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(
           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DbConnection _connection;
        private DbProviderFactory _factory;

        public DbConnection connection
        {
            get { return _connection; }
        }

        #region Constructor
        
        public DBUtil()
        {
            string DBServer = ConfigurationManager.AppSettings["DB_server"];
            string provider = "System.Data.SqlClient";

            string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Doctor;Integrated Security=True";

            _factory = DbProviderFactories.GetFactory(provider);
            _connection = _factory.CreateConnection();
            _connection.ConnectionString = connectionString;

            _connection.Open();
        }

        public DBUtil(string connectionString, string provider)
        {
            _factory = DbProviderFactories.GetFactory(provider);
            _connection = _factory.CreateConnection();
            _connection.ConnectionString = connectionString;
            _connection.Open();
        }
        #endregion

        #region Get Transaction
        public DbTransaction getTransaction()
        {
            return _connection.BeginTransaction(IsolationLevel.Serializable);
        }

        public DbTransaction getTransaction(IsolationLevel level)
        {
            return _connection.BeginTransaction(level);
        }
        #endregion

        public void Dispose()
        {
            if (null != _connection)
                _connection.Dispose();
        }

        #region GetDateSet Interface
        /// <summary>
        /// To get DataSet from the database
        /// </summary>
        /// <param name="sql">The sql string</param>
        /// <param name="table">The table name</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSet(string sql, string table)
        {
            return this.getDataSet(sql, table, null, null, null, CommandType.Text);
        }

        /// <summary>
        /// Get DataSet from the database with a prepared statement
        /// </summary>
        /// <param name="sql">The sql string</param>
        /// <param name="table">The table name</param>
        /// <param name="parameterName">The parameter names of the prepared statement</param>
        /// <param name="parameters">The parameters of the prepared statement</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSet(string sql, string table, string[] parameterName, Object[] parameters)
        {
            return this.getDataSet(sql, table, parameterName, parameters, null, CommandType.Text);
        }

        /// <summary>
        /// To get DataSet from the database with a transaction
        /// </summary>
        /// <param name="sql">The sql string</param>
        /// <param name="table">The table name</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSet(string sql, string table, DbTransaction transaction)
        {
            return this.getDataSet(sql, table, null, null, transaction, CommandType.Text);
        }

        /// <summary>
        /// Get DataSet from the database with a transaction and prepared statement
        /// </summary>
        /// <param name="sql">The sql string</param>
        /// <param name="table">The table name</param>
        /// <param name="parameterName">The parameter names of the prepared statement</param>
        /// <param name="parameters">The parameters of the prepared statement</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSet(string sql, string table, string[] parameterName, Object[] parameters, DbTransaction transaction)
        {
            return this.getDataSet(sql, table, parameterName, parameters, transaction, CommandType.Text);
        }

        /// <summary>
        /// To get DataSet from the database by store procedure
        /// </summary>
        /// <param name="storeProc">The store procedure</param>
        /// <param name="table">The table name</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSetByStoreProc(string storeProc, string table)
        {
            return this.getDataSet(storeProc, table, null, null, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get DataSet from the database by store procedure with parameters
        /// </summary>
        /// <param name="storeProc">The store procedure</param>
        /// <param name="table">The table name</param>
        /// <param name="parameterName">The parameter names of the store procedure</param>
        /// <param name="parameters">The parameters of the store procedure</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSetByStoreProc(string storeProc, string table, string[] parameterName, Object[] parameters)
        {
            return this.getDataSet(storeProc, table, parameterName, parameters, null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// To get DataSet from the database by store procedure with a transaction
        /// </summary>
        /// <param name="storeProc">The store procedure</param>
        /// <param name="table">The table name</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSetByStoreProc(string storeProc, string table, DbTransaction transaction)
        {
            return this.getDataSet(storeProc, table, null, null, transaction, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Get DataSet from the database with a transaction and store procedure with parameters
        /// </summary>
        /// <param name="storeProc">The store procedure</param>
        /// <param name="table">The table name</param>
        /// <param name="parameterName">The parameter names of the store procedure</param>
        /// <param name="parameters">The parameters of the store procedure</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSetByStoreProc(string storeProc, string table, string[] parameterName, Object[] parameters, DbTransaction transaction)
        {
            return this.getDataSet(storeProc, table, parameterName, parameters, transaction, CommandType.StoredProcedure);
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// To execute a sql
        /// </summary>
        /// <param name="sql">The sql statement for execution</param>
        /// <returns>The number of row affected, the same as DbCommand.ExecuteNonQuery</returns>
        public int executeNonQuery(string sql)
        {
            return this.executeNonQuery(sql, null, null);
        }

        /// <summary>
        /// To execute a prepared statement
        /// </summary>
        /// <param name="sql">The prepared statement</param>
        /// <param name="parameterName">The string array of parameter names</param>
        /// <param name="parameters">The object array of the parameters</param>
        /// <returns>The number of row affected, the same as DbCommand.ExecuteNonQuery</returns>
        public int executeNonQuery(string sql, string[] parameterName, Object[] parameters)
        {
            using (DbCommand command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                this.addParameters(command, parameterName, parameters, null);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// To execute a sql with a transaction
        /// </summary>
        /// <param name="sql">The sql statement</param>
        /// <param name="transaction">The transaction used</param>
        /// <returns>The number of row affected, the same as DbCommand.ExecuteNonQuery</returns>
        public int executeNonQuery(string sql, DbTransaction transaction)
        {
            return this.executeNonQuery(sql, null, null, transaction);
        }

        /// <summary>
        /// To execute a prepared statement with a transaction
        /// </summary>
        /// <param name="sql">The prepared statement</param>
        /// <param name="parameterName">The string array of the parameter names</param>
        /// <param name="parameters">The object array of the parameters</param>
        /// <param name="transaction">The transaction used</param>
        /// <returns>The number of row affected, the same as DbCommand.ExecuteNonQuery</returns>
        public int executeNonQuery(string sql, string[] parameterName, Object[] parameters, DbTransaction transaction)
        {
            using (DbCommand command = transaction.Connection.CreateCommand())
            {
                command.CommandText = sql;
                this.addParameters(command, parameterName, parameters, transaction);
                return command.ExecuteNonQuery();
            }
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// To execute scalar
        /// </summary>
        /// <param name="sql">The sql statement</param>
        /// <returns>The result object of the statement</returns>
        public object executeScalar(string sql)
        {
            return this.executeScalar(sql, null, null);
        }

        /// <summary>
        /// To execute scalar with a prepared statement
        /// </summary>
        /// <param name="sql">The prepared statement</param>
        /// <param name="parameterName">The string array of parameter names</param>
        /// <param name="parameters">The object array of the parameters</param>
        /// <returns>The result object of the prepared statement</returns>
        public object executeScalar(string sql, string[] parameterName, Object[] parameters)
        {
            using (DbCommand command = _connection.CreateCommand())
            {
                command.CommandText = sql;
                this.addParameters(command, parameterName, parameters, null);
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// To execute scalar with a transaction
        /// </summary>
        /// <param name="sql">The sql statement</param>
        /// <param name="transaction">The transaction used</param>
        /// <returns>The result object of the sql statement</returns>
        public object executeScalar(string sql, DbTransaction transaction)
        {
            return this.executeScalar(sql, null, null, transaction);
        }

        /// <summary>
        /// To execute scalar with a transaction and parameters
        /// </summary>
        /// <param name="sql">The sql statement</param>
        /// <param name="parameterName">String array of parameters' name</param>
        /// <param name="parameters">Object array of parameters</param>
        /// <param name="transaction">The transaction used</param>
        /// <returns>The result object of the sql statement</returns>
        public object executeScalar(string sql, string[] parameterName, Object[] parameters, DbTransaction transaction)
        {
            using (DbCommand command = transaction.Connection.CreateCommand())
            {
                command.CommandText = sql;
                this.addParameters(command, parameterName, parameters, transaction);
                return command.ExecuteScalar();
            }
        }
        #endregion

        #region ExecuteStoreProcedure
        /// <summary>
        /// To execute store procedure
        /// </summary>
        /// <param name="storeProc">Store procedure</param>
        /// <param name="parameterName">String array of input parameter name</param>
        /// <param name="parameters">String array of input parameters value</param>
        /// <param name="outputParameterName">String array of output parameter name</param>
        /// <param name="outputParameterSize">Int array of output parameter size, null if no need to specify</param>
        /// <param name="outputParameterType">DbType array of output parameter type</param>
        /// <param name="outputParameterDirection">ParameterDirection array of parameter direction</param>
        /// <returns>Object array of those output parameters value with the sequence in the outputParameterName</returns>
        public object[] executeStoreProc(string storeProc, string[] parameterName, Object[] parameters,
            string[] outputParameterName, int?[] outputParameterSize, DbType[] outputParameterType,
            ParameterDirection[] outputParameterDirection)
        {
            return executeStoreProc(storeProc, parameterName, parameters, outputParameterName,
                outputParameterSize, outputParameterType, outputParameterDirection, null);
        }

        /// <summary>
        /// To execute store procedure with transaction
        /// </summary>
        /// <param name="storeProc">Store procedure</param>
        /// <param name="parameterName">String array of input parameter name</param>
        /// <param name="parameters">String array of input parameters value</param>
        /// <param name="outputParameterName">String array of output parameter name</param>
        /// <param name="outputParameterSize">Int array of output parameter size, null if no need to specify</param>
        /// <param name="outputParameterType">DbType array of output parameter type</param>
        /// <param name="outputParameterDirection">ParameterDirection array of parameter direction</param>
        /// <param name="transaction">Transaction used</param>
        /// <returns>Object array of those output parameters value with the sequence in the outputParameterName</returns>
        public object[] executeStoreProc(string storeProc, string[] parameterName, Object[] parameters,
            string[] outputParameterName, int?[] outputParameterSize, DbType[] outputParameterType,
            ParameterDirection[] outputParameterDirection, DbTransaction transaction)
        {
            int index = 0;
            object[] result = new object[outputParameterName.Length];
            DbConnection conn = _connection;
            if (transaction != null)
                conn = transaction.Connection;
            using (DbCommand command = conn.CreateCommand())
            {
                command.CommandText = storeProc;
                command.CommandType = CommandType.StoredProcedure;
                this.addParameters(command, parameterName, parameters, transaction);
                DbParameter[] output = this.addOutputReturnParameters(command, outputParameterName,
                    outputParameterSize, outputParameterType, outputParameterDirection, transaction);

                command.ExecuteNonQuery();
                for (int i = 0; i < output.Length; i++)
                {
                    result[index] = output[i].Value;
                    index++;
                }
            }
            return result;
        }
        #endregion

        #region PrivateFunction
        /// <summary>
        /// To get DataSet from database
        /// </summary>
        /// <param name="sql">Command want to run</param>
        /// <param name="table">Table name in the DataSet</param>
        /// <param name="parameterName">String array of parameters' name, null for no parameters</param>
        /// <param name="parameters">Object array of parameters, null for no parameters</param>
        /// <param name="transaction">Transaction used, null for no transaction</param>
        /// <param name="commType">CommandType of the command</param>
        /// <returns>DataSet of result</returns>
        private DataSet getDataSet(string sql, string table, string[] parameterName, Object[] parameters, DbTransaction transaction, CommandType commType)
        {
            DbConnection conn = _connection;
            if (transaction != null)
                conn = transaction.Connection;
            using (DbCommand command = conn.CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = commType;
                this.addParameters(command, parameterName, parameters, transaction);
                DbDataAdapter adapter = _factory.CreateDataAdapter();
                adapter.SelectCommand = command;
                DataSet set = new DataSet();
                adapter.Fill(set, table);
                return set;
            }
        }

        /// <summary>
        /// To add parameters and transaction to the Dbcommand
        /// </summary>
        /// <param name="command">Dbcommand</param>
        /// <param name="parameterName">String array of the parameters name</param>
        /// <param name="parameters">Object array of the parameters</param>
        /// <param name="transaction">The transaction you want to use</param>
        private void addParameters(DbCommand command, string[] parameterName, Object[] parameters, DbTransaction transaction)
        {
            #region Set Transaction
            if (transaction != null)
            {
                Log.Debug("With Transaction: \n" + command.CommandText);
                command.Transaction = transaction;
            }
            else
            {
                Log.Debug("Without Transaction: \n" + command.CommandText);
            }
            #endregion

            if (parameters != null)
            {
                Log.Debug("Parameters : ");
                for (int i = 0; i < parameters.Length; i++)
                {
                    DbParameter param = command.CreateParameter();
                    param.ParameterName = parameterName[i];

                    param.Value = parameters[i];
                    command.Parameters.Add(param);
                    Log.Debug(parameterName[i] + "\t\t" + parameters[i]);
                }
            }
        }

        /// <summary>
        /// To add output parameters for a store procedure
        /// </summary>
        /// <param name="command">Store procedure</param>
        /// <param name="outputParameterName">String array of the output parameters' name</param>
        /// <param name="transaction">Transaction used, null for no transaction</param>
        /// <returns>Array of the output paramters</returns>
        private DbParameter[] addOutputReturnParameters(DbCommand command, string[] outputParameterName,
            int?[] outputParameterSize, DbType[] outputParameterType, ParameterDirection[] outputParameterDirection,
            DbTransaction transaction)
        {
            #region Set Transaction
            if (transaction != null)
            {
                Log.Debug("With Transaction: \n" + command.CommandText);
                command.Transaction = transaction;
            }
            else
            {
                Log.Debug("Without Transaction: \n" + command.CommandText);
            }
            #endregion

            if (outputParameterName != null)
            {
                DbParameter[] output = new DbParameter[outputParameterName.Length];
                Log.Debug("Parameters : ");
                for (int i = 0; i < outputParameterName.Length; i++)
                {
                    DbParameter param = command.CreateParameter();
                    param.ParameterName = outputParameterName[i];
                    if (outputParameterSize[i] != null)
                        param.Size = (int)outputParameterSize[i];
                    param.DbType = outputParameterType[i];
                    param.Direction = outputParameterDirection[i];
                    command.Parameters.Add(param);
                    output[i] = param;
                    Log.Debug(outputParameterDirection[i] + "\t\t" + outputParameterName[i] + "\t\t" + ((outputParameterSize[i] != null) ? outputParameterSize[i].ToString() : "null"));
                }
                return output;
            }
            return new DbParameter[0];
        }


        #endregion


        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <returns></returns>
        public int getMasterId(string table_name)
        {
            string sql = "sp_get_master_id";
            return Convert.ToInt32(this.getDataSetByStoreProc(sql, table_name, new string[] { "@table_name" }, new object[] { table_name }).Tables[0].Rows[0][0]);

        }
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <returns></returns>
        public int getMasterId(string table_name, DbTransaction transaction)
        {
            string sql = "sp_get_master_id";
            return Convert.ToInt32(this.getDataSetByStoreProc(sql, table_name, new string[] { "@table_name" }, new object[] { table_name }, transaction).Tables[0].Rows[0][0]);

        }
    }
}
