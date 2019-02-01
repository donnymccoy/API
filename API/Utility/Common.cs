using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LifeSpan.API.Utility
{
    public class Common
    {
        /// <summary>This determines the true logged-in user, which can differ based on how this program executes (service, interactive, RDC). </summary>
        /// <returns>User name</returns>
        //TODO:  internal static string GetTrueUserName(bool includeDomain)
        /*
        {
            try
            {
                string userName = string.Empty;

                // The call to InvokeMethod below will fail if the Handle property is not retrieved
                string[] propertiesToSelect = new[] { "Handle", "ProcessId" };
                SelectQuery processQuery = new SelectQuery("Win32_Process", "Name = 'explorer.exe'", propertiesToSelect);

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(processQuery))
                using (ManagementObjectCollection processes = searcher.Get())
                    foreach (ManagementObject process in processes)
                    {
                        object[] outParameters = new object[2];
                        uint result = (uint)process.InvokeMethod("GetOwner", outParameters);

                        if (result == 0)
                        {
                            string user = (string)outParameters[0];
                            string domain = (string)outParameters[1];
                            uint processId = (uint)process["ProcessId"];

                            userName = string.Concat(includeDomain ? string.Concat(domain, @"\") : string.Empty);
                            userName += user;

                            break;
                        }
                        else
                        {
                            // TODO:  Handle GetOwner() failure...
                        }
                    }

                return userName;
            }
            catch (Exception ex)
            { throw; }
        }
        */

        internal static string GetEnvironment()
        {
            string environment = "ASPDTA"; // The default value

            try
            {
                var environmentFromConfig = ConfigurationManager.AppSettings["EventEnvironment"];

                if (environmentFromConfig != null)
                {
                    switch (environmentFromConfig)
                    {
                        case "Production":
                            environment = "ASPDTA";
                            break;
                        case "Development":
                        case "QA":
                        default:
                            environment = "INSTS2";
                            break;
                        case "Training":
                            environment = "ASPTRN";
                            break;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return environment;
        }

        public static int QueryId(System.Data.SqlClient.SqlConnection sqlConnNew)
        {
            int id = -1;

            System.Data.SqlClient.SqlCommand sqlCmdNew = null;

            try
            {
                sqlCmdNew = new System.Data.SqlClient.SqlCommand(
                    "Select @@Identity ",
                    sqlConnNew
                );
                sqlCmdNew.CommandType = System.Data.CommandType.Text;

                object o = sqlCmdNew.ExecuteScalar();
                if (o != null)
                {
                    id = int.TryParse(o.ToString(), out id) ? id : -1;
                }
            }
            catch (Exception e)
            {
                //TODO - ADD ERROR HANDLING IF WE THINK IT IS NECESSARY
                System.Diagnostics.Debug.Print(e.ToString());
            }
            finally
            {
                if (sqlCmdNew != null)
                {
                    sqlCmdNew.Dispose();
                }
            }

            return id;
        }

        public static bool GetSafeBoolean<T>(T parm)
        {
            bool safeBoolean = false;

            if (null != parm)
            {
                bool boolVar = false;
                if (bool.TryParse(parm.ToString(), out boolVar))
                {
                    safeBoolean = boolVar;
                }
            }

            return safeBoolean;
        }

        public static string GetSafeString<T>(T parm)
        {
            String safeString = string.Empty;

            if (null != parm)
            {
                safeString = parm.ToString().Trim();
            }

            return safeString;
        }

        public static bool IsNumeric<T>(T parm)
        {
            bool isNumeric = false;

            if (null != parm)
            {
                Regex reNum = new Regex(@"^\d+$");
                isNumeric = reNum.Match(parm.ToString()).Success;
            }

            return isNumeric;
        }

        /// <summary>
        /// Safely parses a nullable integer from a passed parameter of any type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <returns>If parsing succeded, returns the integer value. If parsing did not succed, returns -1.</returns>
        /// <exception>No exceptions</exception>
        public static int GetSafeInt<T>(T parm)
        {
            int safeInt = -1;

            if (IsNumeric(parm))
            {
                int parsedInt = 0;
                if (int.TryParse(parm.ToString(), out parsedInt))
                {
                    safeInt = parsedInt;
                }
            }

            return safeInt;
        }

        /// <summary>
        /// Safely parses a nullable integer from a passed parameter of any type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <returns>If parsing succeded, returns the integer value. If parsing did not succed, returns null.</returns>
        /// <exception>No exceptions</exception>
        public static int? GetSafeNullableInt<T>(T parm)
        {
            int? safeInt = null;

            if (IsNumeric(parm))
            {
                int parsedInt = 0;
                if (int.TryParse(parm.ToString(), out parsedInt))
                {
                    safeInt = parsedInt;
                }
            }

            return safeInt;
        }

        /// <summary>
        /// Handles safely the parameters to be parsed as DateTime 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <returns>
        ///  - DateTime if the passed parm is valid
        ///  - DateTime.MinValue (1/1/0001 12:00AM) if the passed parm is invalid
        /// </returns>
        public static DateTime GetSafeDateTime<T>(T parm)
        {
            DateTime safeDateTime = DateTime.MinValue;

            if (null != parm)
            {
                DateTime parsedDateTime = DateTime.MinValue;
                if (DateTime.TryParse(parm.ToString(), out parsedDateTime))
                {
                    safeDateTime = parsedDateTime;
                }
            }

            return safeDateTime;
        }

        /// <summary>
        /// Handles safely the parameters to be parsed as DatTime 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parm"></param>
        /// <returns>
        ///  - DateTime if the passed parm is valid
        ///  - null if the passed parm is invalid
        /// </returns>
        public static DateTime? GetSafeNullableDateTime<T>(T parm)
        {
            DateTime? safeNullableDateTime = null;

            if (null != parm)
            {
                DateTime parsedDateTime;
                if (DateTime.TryParse(parm.ToString(), out parsedDateTime))
                {
                    safeNullableDateTime = parsedDateTime;
                }
            }

            return safeNullableDateTime;
        }

        public static DateTime? ConvertDateStringToDateTime(string strDate)
        {
            try
            {
                DateTime dateTime;
                if (DateTime.TryParse(strDate, out dateTime))
                {
                    if (dateTime == DateTime.Now.MinValueDbSafe())
                    {
                        return null;
                    }
                    return dateTime;
                }
                //DateTime.TryParse failed, check if date format is an Integer (XMddyy)
                int intDate;
                if (!int.TryParse(strDate, out intDate))
                {
                    return null;
                }
                //strDate is in Integer format, make sure it is 6 digits long, with a leading zero if necessary, then re-parse
                strDate = intDate.ToString("D6");
                dateTime = DateTime.ParseExact(strDate, "MMddyy", null);

                return dateTime;
            }
            catch (Exception)
            {   //strDate is in an unrecognized format
                return null;
            }

        }




        //public static object GetNullableParmValue(Query query, int parmKey)
        //{
        //    object nullableOdbcParmValue = DBNull.Value;
        //    try
        //    {
        //        if (query.Filter.ContainsKey(parmKey))
        //        {
        //            nullableOdbcParmValue = query.Filter[parmKey];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return nullableOdbcParmValue;
        //}

        //public static object GetNullableParmValue(object parmValue)
        //{
        //    object nullableOdbcParmValue = DBNull.Value;
        //    try
        //    {
        //        if (null != parmValue)
        //        {
        //            nullableOdbcParmValue = parmValue;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return nullableOdbcParmValue;
        //}

        //public static System.Data.Odbc.OdbcParameter BuildOdbcParameter(Query query, int parmIndex, string parmName, System.Data.Odbc.OdbcType parmType,
        //     int size = 50, ParameterDirection direction = default(ParameterDirection))
        //{
        //    System.Data.Odbc.OdbcParameter odbcParameter = new System.Data.Odbc.OdbcParameter();

        //    try
        //    {
        //        odbcParameter.ParameterName = parmName;
        //        odbcParameter.OdbcType = parmType;
        //        odbcParameter.Size = size;
        //        odbcParameter.Direction = direction;

        //        odbcParameter.Value = DBNull.Value;
        //        try
        //        {
        //            if (query.Filter.ContainsKey(parmIndex))
        //            {
        //                odbcParameter.Value = query.Filter[parmIndex];
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return odbcParameter;
        //}

        //public static System.Data.Odbc.OdbcParameter BuildOdbcParameter(string parmName, System.Data.Odbc.OdbcType parmType, object parmValue,
        //     int size = 50, ParameterDirection direction = default(ParameterDirection))
        //{
        //    System.Data.Odbc.OdbcParameter odbcParameter = new System.Data.Odbc.OdbcParameter();

        //    try
        //    {
        //        odbcParameter.ParameterName = parmName;
        //        odbcParameter.OdbcType = parmType;
        //        odbcParameter.Size = size;
        //        odbcParameter.Direction = direction;

        //        odbcParameter.Value = DBNull.Value;
        //        if (null != parmValue)
        //        {
        //            odbcParameter.Value = parmValue;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return odbcParameter;
        //}

        public static TimeSpan? ConvertTimeStringToTimeSpan(string strTime)
        {
            TimeSpan? time = null;
            if (!string.IsNullOrEmpty(strTime))
            {
                TimeSpan parsedTime;
                if (TimeSpan.TryParse(strTime, out parsedTime))
                    time = parsedTime;
            }
            return time;
        }

        public static Decimal GetSafeDecimal<T>(T parm)
        {
            Decimal safeDecimal = 0M;
            try
            {
                Decimal.TryParse(parm.ToString(), out safeDecimal);
            }
            catch (Exception)
            {
                //throw;
            }

            return safeDecimal;
        }

        public static Decimal? GetSafeNullableDecimal<T>(T parm)
        {
            Decimal? safeDecimal = null;
            try
            {
                if ((null != parm) && (IsNumeric(parm)))
                {
                    Decimal dec = 0M;
                    if (Decimal.TryParse(parm.ToString(), out dec))
                    {
                        safeDecimal = dec;
                    }
                }
            }
            catch (Exception)
            {
                //throw;
            }

            return safeDecimal;
        }

    }
}