using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace LifeSpan.API.DAL.Dashboard
{
    public class StaffReminders
    {
        /// <summary>Get all birthdays within 30 days of today's date.</summary>
        /// <returns>List of Birthdays</returns>
        public DataTable StaffBirthdays()
        {
            DataTable dt = new DataTable();
            
            try
            {
                string command = "GetStaffBirthdays";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();

                dt = Utility.Data.ExecuteQueryDataTable(command, ref parameters, Utility.DBConnections.dataDb);

            }
            catch (Exception)
            {

                throw;
            }

            return dt;
        }


        /// <summary>Get all employee tracking documents that are either expired or will expire within 30 days of today's date.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of Expirations</returns>
        public DataTable StaffExpirations(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string command = "GetStaffExpirations";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();
                parameters.Add(new System.Data.OleDb.OleDbParameter("@IncludeAssistant", includeAssistant));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EmployeeId", employeeId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@SchoolYearId", schoolYearId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@StartDate", (startDate)));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EndDate", (endDate)));

                dt = Utility.Data.ExecuteQueryDataTable(command, ref parameters, Utility.DBConnections.dataDb);

            }
            catch (Exception)
            {

                throw;
            }

            return dt;
        }


    }
}