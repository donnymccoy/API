using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSpan.API.Utility
{
    public class DBConnections
    {
        public const string dataDb = "LifespanData";
        public const string apiDb = "LifespanAPI";
        public const string elmahDb = "ELMAH";
    }

    /// <summary>
    /// Provides decoupled db connectivity for DAL
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Executes a stored procedure without returning data
        /// </summary>
        /// <param name="command">Name of stored procedure</param>
        /// <param name="parameters">List of parameters needed to execue the stored procedure</param>
        /// <param name="connectionStringName">Name, from DBConnections class, to use from web.config</param>
        /// <returns>Returns the number of affected rows</returns>
        public static int ExecuteNonQuery(string command, ref List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        {
            int rowsAffected = 0;

            System.Data.SqlClient.SqlConnection sqlConnNew = null;
            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    command,
                    sqlConnNew
                );
                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
                    {
                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
                        sqlParameter.Value = parameter.Value;
                        sqlParameter.Direction = parameter.Direction;
                        sqlCmdNew.Parameters.Add(sqlParameter);
                    }
                }

                sqlConnNew.Open();
                sqlCmdNew.CommandTimeout = 0;
                rowsAffected = sqlCmdNew.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //we want to bubble this up
                throw e;
            }
            finally
            {
                if (sqlConnNew != null)
                {
                    if (sqlConnNew.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnNew.Close();
                    }
                    sqlConnNew.Dispose();
                }

                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return rowsAffected;
        }


        /// <summary>
        /// Executes a stored procedure, returning a single piece of data
        /// </summary>
        /// <param name="command">Name of stored procedure</param>
        /// <param name="parameters">List of parameters needed to execue the stored procedure</param>
        /// <param name="connectionStringName">Name, from DBConnections class, to use from web.config</param>
        /// <returns>Returns the object retrieved from the database</returns>
        public static object ExecuteQueryScalar(string command, ref List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        {
            object result = new object();

            System.Data.SqlClient.SqlConnection sqlConnNew = null;
            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    command,
                    sqlConnNew
                );
                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

                if (parameters != null)
                {
                    if (parameters != null)
                    {
                        foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
                        {
                            System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
                            sqlParameter.Value = parameter.Value;
                            sqlParameter.Direction = parameter.Direction;
                            sqlCmdNew.Parameters.Add(sqlParameter);
                        }
                    }
                }

                sqlConnNew.Open();
                sqlCmdNew.CommandTimeout = 0;
                result = sqlCmdNew.ExecuteScalar();
            }
            catch (Exception e)
            {
                //we want to bubble this up
                throw e;
            }
            finally
            {
                if (sqlConnNew != null)
                {
                    if (sqlConnNew.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnNew.Close();
                    }
                    sqlConnNew.Dispose();
                }

                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return result;
        }


        /// <summary>
        /// Executes a stored procedure, returning a single table of data
        /// </summary>
        /// <param name="command">Name of stored procedure</param>
        /// <param name="parameters">List of parameters needed to execue the stored procedure</param>
        /// <param name="connectionStringName">Name, from DBConnections class, to use from web.config</param>
        /// <returns>Returns a datatable full of data retrieved from the database</returns>
        public static System.Data.DataTable ExecuteQueryDataTable(string command, ref List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        {
            System.Data.DataTable result = new System.Data.DataTable();

            System.Data.SqlClient.SqlConnection sqlConnNew = null;
            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    command,
                    sqlConnNew
                );
                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
                    {
                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
                        sqlParameter.Value = parameter.Value;
                        sqlParameter.Direction = parameter.Direction;
                        sqlParameter.Size = parameter.Size;
                        sqlCmdNew.Parameters.Add(sqlParameter);
                    }
                }

                sqlConnNew.Open();
                sqlCmdNew.CommandTimeout = 0;
                System.Data.SqlClient.SqlDataReader sqlDataReader = sqlCmdNew.ExecuteReader();
                if (sqlDataReader != null)
                {
                    result.Load(sqlDataReader);
                }

                //assign all the out parameter values
                foreach (SqlParameter sParam in sqlCmdNew.Parameters.Cast<SqlParameter>().Where(p => p.Direction == ParameterDirection.Output))
                {
                    OleDbParameter oParam = parameters.Cast<OleDbParameter>().Where(p => p.ParameterName == sParam.ParameterName).FirstOrDefault();
                    oParam.Value = sParam.Value;
                }
            }
            catch (Exception e)
            {
                //we want to bubble this up
                throw e;
            }
            finally
            {
                if (sqlConnNew != null)
                {
                    if (sqlConnNew.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnNew.Close();
                    }
                    sqlConnNew.Dispose();
                }

                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return result;
        }


        /// <summary>
        /// Executes a stored procedure, returning multiple tables of data
        /// </summary>
        /// <param name="command">Name of stored procedure</param>
        /// <param name="parameters">List of parameters needed to execue the stored procedure</param>
        /// <param name="connectionStringName">Name, from DBConnections class, to use from web.config</param>
        /// <returns>Returns a dataset full of data retrieved from the database</returns>
        public static System.Data.DataSet ExecuteQueryDataSet(string command, ref List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        {
            System.Data.DataSet result = new System.Data.DataSet();

            System.Data.SqlClient.SqlConnection sqlConnNew = null;
            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    command,
                    sqlConnNew
                );
                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

                if (parameters != null)
                {
                    foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
                    {
                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
                        sqlParameter.Value = parameter.Value;
                        sqlParameter.Direction = parameter.Direction;
                        sqlCmdNew.Parameters.Add(sqlParameter);
                    }
                }

                sqlConnNew.Open();
                sqlCmdNew.CommandTimeout = 0;
                System.Data.SqlClient.SqlDataAdapter sqlDataAdapter = new System.Data.SqlClient.SqlDataAdapter(sqlCmdNew);
                if (sqlDataAdapter != null)
                {
                    sqlDataAdapter.Fill(result);
                }
            }
            catch (Exception e)
            {
                //we want to bubble this up
                throw e;
            }
            finally
            {
                if (sqlConnNew != null)
                {
                    if (sqlConnNew.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnNew.Close();
                    }
                    sqlConnNew.Dispose();
                }

                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return result;
        }


        /// <summary>
        /// Executes an inline command, returning a single piece of data
        /// </summary>
        /// <param name="command">Name of stored procedure</param>
        /// <param name="parameters">List of parameters needed to execue the stored procedure</param>
        /// <param name="connectionStringName">Name, from DBConnections class, to use from web.config</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteQueryScalar(string command, ref List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName, System.Data.CommandType commandType)
        {
            object result = new object();

            System.Data.SqlClient.SqlConnection sqlConnNew = null;
            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    command,
                    sqlConnNew
                );
                sqlCmdNew.CommandType = commandType;

                if (parameters != null)
                {
                    if (parameters != null)
                    {
                        foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
                        {
                            System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
                            sqlParameter.Value = parameter.Value;
                            sqlParameter.Direction = parameter.Direction;
                            sqlCmdNew.Parameters.Add(sqlParameter);
                        }
                    }
                }

                sqlConnNew.Open();
                sqlCmdNew.CommandTimeout = 0;
                result = sqlCmdNew.ExecuteScalar();
            }
            catch (Exception e)
            {
                //we want to bubble this up
                throw e;
            }
            finally
            {
                if (sqlConnNew != null)
                {
                    if (sqlConnNew.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnNew.Close();
                    }
                    sqlConnNew.Dispose();
                }

                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return result;
        }


        /// <summary>
        /// Executes an inline command, returning a single table of data
        /// </summary>
        /// <param name="command">Name of stored procedure</param>
        /// <param name="parameters">List of parameters needed to execue the stored procedure</param>
        /// <param name="connectionStringName">Name, from DBConnections class, to use from web.config</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static System.Data.DataTable ExecuteQueryDataTable(string command, ref List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName, System.Data.CommandType commandType)
        {
            System.Data.DataTable result = new System.Data.DataTable();

            System.Data.SqlClient.SqlConnection sqlConnNew = null;
            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    command,
                    sqlConnNew
                );
                sqlCmdNew.CommandType = commandType;

                if (parameters != null)
                {
                    foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
                    {
                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
                        sqlParameter.Value = parameter.Value;
                        sqlParameter.Direction = parameter.Direction;
                        sqlParameter.Size = parameter.Size;
                        sqlCmdNew.Parameters.Add(sqlParameter);
                    }
                }

                sqlConnNew.Open();
                sqlCmdNew.CommandTimeout = 0;
                System.Data.SqlClient.SqlDataReader sqlDataReader = sqlCmdNew.ExecuteReader();
                if (sqlDataReader != null)
                {
                    result.Load(sqlDataReader);
                }

                //assign all the out parameter values
                foreach (SqlParameter sParam in sqlCmdNew.Parameters.Cast<SqlParameter>().Where(p => p.Direction == ParameterDirection.Output))
                {
                    OleDbParameter oParam = parameters.Cast<OleDbParameter>().Where(p => p.ParameterName == sParam.ParameterName).FirstOrDefault();
                    oParam.Value = sParam.Value;
                }
            }
            catch (Exception e)
            {
                //we want to bubble this up
                throw e;
            }
            finally
            {
                if (sqlConnNew != null)
                {
                    if (sqlConnNew.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnNew.Close();
                    }
                    sqlConnNew.Dispose();
                }

                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return result;
        }
        

        /// <summary>
        /// Create a SqlParameter of the detected type, assign the passed in value, and add the parameter to the parameter collection passed in by reference.
        /// Replaces a code block like
        ///       System.Data.SqlClient.SqlParameter parmName = new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar);
        ///       parmName.Value = name;
        ///       sqlCommand.Parameters.Add(parmName);
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parmName"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="parmValue"></param>
        public static void AddSqlParameter(ref SqlCommand sqlCommand, string parmName, SqlDbType sqlDbType, object parmValue, bool isNullable)
        {
            try
            {
                System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parmName, sqlDbType);
                sqlParameter.IsNullable = isNullable;

                if (null != parmValue)
                {
                    // To switch on built-in types, use the TypeCode enumeration:
                    switch (Type.GetTypeCode(parmValue.GetType()))
                    {
                        case TypeCode.Boolean:
                            sqlParameter.Value = Common.GetSafeBoolean(parmValue);
                            break;
                        case TypeCode.Int32:
                            sqlParameter.Value = Common.GetSafeInt(parmValue);
                            break;
                        case TypeCode.Decimal:
                            // 
                            break;
                        case TypeCode.String:
                            sqlParameter.Value = Common.GetSafeString(parmValue);
                            break;
                        case TypeCode.DateTime:
                            sqlParameter.Value = Common.GetSafeDateTime(parmValue);
                            break;
                        default:
                            break;
                    }
                }

                #region //
                /* The following approach needs to be adjusted to handle the null parmValue cases better - MVS
		                TypeSwitch.Do(parmValue,
                     TypeSwitch.Case<string>(() => sqlParameter.Value = Common.GetSafeString(parmValue)),
                     TypeSwitch.Case<int>(() => sqlParameter.Value = Common.GetSafeInt(parmValue)),
                     TypeSwitch.Case<int?>(() => sqlParameter.Value = Common.GetSafeNullableInt(parmValue)),
                     TypeSwitch.Case<DateTime>(() => sqlParameter.Value = Common.GetSafeDateTime(parmValue)),
                     TypeSwitch.Case<DateTime?>(() => sqlParameter.Value = Common.GetSafeNullableDateTime(parmValue)),
                     TypeSwitch.Case<bool>(() => sqlParameter.Value = Common.GetSafeBoolean(parmValue)),
                     TypeSwitch.Default(() => sqlParameter.Value = parmValue.ToString())
                    );
                    */
                #endregion

                sqlCommand.Parameters.Add(sqlParameter);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Add a parameter to the referred OleDbParameter list
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parmName"></param>
        /// <param name="sqlDbType"></param>
        /// <param name="parmValue"></param>
        /// <param name="isNullable"></param>
        public static void AddOleDbParameter(ref List<System.Data.OleDb.OleDbParameter> parameters, string parmName, SqlDbType sqlDbType, object parmValue, bool isNullable)
        {
            //List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();

            try
            {
                System.Data.OleDb.OleDbParameter parameter = new System.Data.OleDb.OleDbParameter(parmName, sqlDbType);
                if (isNullable)
                {
                    parameter.IsNullable = true;
                    parameter.Value = null;
                }
                if (sqlDbType == SqlDbType.DateTime2)
                {
                    parameter.OleDbType = OleDbType.DBTimeStamp;
                }

                if (null != parmValue)
                {
                    // To switch on built-in types, use the TypeCode enumeration:
                    switch (Type.GetTypeCode(parmValue.GetType()))
                    {
                        case TypeCode.Boolean:
                            parameter.Value = Common.GetSafeBoolean(parmValue);
                            break;
                        case TypeCode.Int32:
                            parameter.Value = Common.GetSafeInt(parmValue);
                            break;
                        case TypeCode.Decimal:
                            // 
                            break;
                        case TypeCode.String:
                            parameter.Value = Common.GetSafeString(parmValue);
                            break;
                        case TypeCode.DateTime:
                            parameter.Value = Common.GetSafeDateTime(parmValue);
                            break;
                        default:
                            parameter.Value = Common.GetSafeString(parmValue);
                            break;
                    }
                }

                parameters.Add(parameter);
            }
            catch (Exception e)
            {
                throw e;
            }

            //return parameters;
        }

        //public static List<System.Data.OleDb.OleDbParameter> QueryToOleDbParameters<T>(Digital.DTO.Common.Query query)
        //{
        //    List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();

        //    if (query != null)
        //    {
        //        try
        //        {
        //            if (query.Filter != null)
        //            {
        //                foreach (int key in query.Filter.Keys)
        //                {
        //                    string parmName = "@" + Enum.GetName(typeof(T), key);
        //                    Utilities.Data.AddOleDbParameter(ref parameters, parmName, System.Data.SqlDbType.Int, query.Filter[key], true);
        //                }
        //            }

        //            Utilities.Data.AddOleDbParameter(ref parameters, "@Offset", System.Data.SqlDbType.Int, query.Offset, false);
        //            Utilities.Data.AddOleDbParameter(ref parameters, "@Fetch", System.Data.SqlDbType.Int, query.Fetch, false);
        //            Utilities.Data.AddOleDbParameter(ref parameters, "@SortColumn", System.Data.SqlDbType.VarChar, query.SortColumn, false);
        //            Utilities.Data.AddOleDbParameter(ref parameters, "@SortDirection", System.Data.SqlDbType.VarChar, query.SortDirection, false);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }

        //    return parameters;
        //}



        #region Async Helper Methods
        //public async static Task<object> ExecuteQueryScalarAsync(string command, List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        //{
        //    object result = new object();

        //    try
        //    {
        //        using (SqlConnection sqlConnNew = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
        //        {
        //            using (SqlCommand sqlCmdNew = new System.Data.SqlClient.SqlCommand(command, sqlConnNew))
        //            {
        //                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

        //                if (parameters != null)
        //                {
        //                    if (parameters != null)
        //                    {
        //                        foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
        //                        {
        //                            System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
        //                            sqlParameter.Value = parameter.Value;
        //                            sqlParameter.Direction = parameter.Direction;
        //                            sqlCmdNew.Parameters.Add(sqlParameter);
        //                        }
        //                    }
        //                }

        //                sqlConnNew.Open();
        //                sqlCmdNew.CommandTimeout = 0;
        //                result = await sqlCmdNew.ExecuteScalarAsync();
        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //we want to bubble this up
        //        throw e;
        //    }
        //}

        //public async static Task<System.Data.DataTable> ExecuteQueryDataTableAsync(string command, List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        //{
        //    System.Data.DataTable result = new System.Data.DataTable();

        //    try
        //    {
        //        using (SqlConnection sqlConnNew = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
        //        {
        //            using (SqlCommand sqlCmdNew = new SqlCommand(command, sqlConnNew))
        //            {
        //                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

        //                if (parameters != null)
        //                {
        //                    foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
        //                    {
        //                        //System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
        //                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);

        //                        sqlParameter.Direction = parameter.Direction;
        //                        if (parameter.Direction == ParameterDirection.Input)
        //                        {
        //                            sqlParameter.Value = parameter.Value;
        //                        }
        //                        sqlCmdNew.Parameters.Add(sqlParameter);
        //                    }
        //                }

        //                sqlConnNew.Open();
        //                sqlCmdNew.CommandTimeout = 0;
        //                System.Data.SqlClient.SqlDataReader sqlDataReader = await sqlCmdNew.ExecuteReaderAsync();
        //                if (sqlDataReader != null)
        //                {
        //                    result.Load(sqlDataReader);
        //                }
        //                sqlDataReader.Close();

        //                //assign all the out parameter values
        //                foreach (SqlParameter sParam in sqlCmdNew.Parameters.Cast<SqlParameter>().Where(p => p.Direction == ParameterDirection.Output))
        //                {
        //                    OleDbParameter oParam = parameters.Cast<OleDbParameter>().Where(p => p.ParameterName == sParam.ParameterName).FirstOrDefault();
        //                    oParam.Value = sParam.Value;
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        //we want to bubble this up
        //        throw e;
        //    }
        //}

        //public async static Task<int> ExecuteNonQueryAsync(string command, List<System.Data.OleDb.OleDbParameter> parameters, string connectionStringName)
        //{
        //    int rowsAffected = 0;

        //    try
        //    {
        //        using (SqlConnection sqlConnNew = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
        //        {
        //            using (SqlCommand sqlCmdNew = new SqlCommand(command, sqlConnNew))
        //            {
        //                sqlCmdNew.CommandType = System.Data.CommandType.StoredProcedure;

        //                if (parameters != null)
        //                {
        //                    foreach (System.Data.OleDb.OleDbParameter parameter in parameters)
        //                    {
        //                        System.Data.SqlClient.SqlParameter sqlParameter = new System.Data.SqlClient.SqlParameter(parameter.ParameterName, parameter.OleDbType);
        //                        sqlParameter.Value = parameter.Value;
        //                        sqlParameter.Direction = parameter.Direction;
        //                        sqlCmdNew.Parameters.Add(sqlParameter);
        //                    }
        //                }

        //                sqlConnNew.Open();
        //                sqlCmdNew.CommandTimeout = 0;
        //                rowsAffected = await sqlCmdNew.ExecuteNonQueryAsync();

        //                //assign all the out parameter values
        //                foreach (SqlParameter sParam in sqlCmdNew.Parameters.Cast<SqlParameter>().Where(p => p.Direction == ParameterDirection.Output))
        //                {
        //                    OleDbParameter oParam = parameters.Cast<OleDbParameter>().Where(p => p.ParameterName == sParam.ParameterName).FirstOrDefault();
        //                    oParam.Value = sParam.Value;
        //                }

        //                return rowsAffected;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        //we want to bubble this up
        //        throw e;
        //    }
        //}


        #endregion

    }

}
