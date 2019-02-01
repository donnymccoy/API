using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using LifeSpan.API.Utility;

namespace LifeSpan.API.DAL.Dashboard
{
    public class ClientDocuments
    {
        private enum DocTypeSpecifier
        {
            [Description("1")]
            ROI = 0,

            [Description("0")]
            RX = 1
        }
        /// <summary>Get all outstanding ROI documents.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of MissingDocuments</returns>
        public DataTable MissingROIDocuments(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string command = "GetMissingClientDocuments";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();
                parameters.Add(new System.Data.OleDb.OleDbParameter("@IncludeAssistant", includeAssistant));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EmployeeId", employeeId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@SchoolYearId", schoolYearId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@StartDate", (startDate)));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EndDate", (endDate)));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@DocTypeSpecifier", DocTypeSpecifier.ROI.Description()));

                dt = Utility.Data.ExecuteQueryDataTable(command, ref parameters, Utility.DBConnections.dataDb);

            }
            catch (Exception)
            {

                throw;
            }

            return dt;
        }


        /// <summary>Get all outstanding RX and Physician documents.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of MissingDocuments</returns>
        public DataTable MissingRXDocuments(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                string command = "GetMissingClientDocuments";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();
                parameters.Add(new System.Data.OleDb.OleDbParameter("@IncludeAssistant", includeAssistant));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EmployeeId", employeeId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@SchoolYearId", schoolYearId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@StartDate", (startDate)));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EndDate", (endDate)));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@DocTypeSpecifier", DocTypeSpecifier.RX.Description()));

                dt = Utility.Data.ExecuteQueryDataTable(command, ref parameters, Utility.DBConnections.dataDb);

            }
            catch (Exception)
            {

                throw;
            }

            return dt;
        }


        /// <summary>Get active clients not assigned a physician.</summary>
        /// <returns>List of MissingPhysicians</returns>
        public DataTable MissingPhysicians()
        {
            DataTable dt = new DataTable();

            try
            {
                string command = "GetMissingPhysicians";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();

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
