using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace LifeSpan.API.DAL.Dashboard
{
    public class TherapistReports
    {
        private enum ReportSpecifier
        {
            Evals = 0,
            AllOthers = 1
        }

        /// <summary>Get all timelogs that have been entered for evals but no report is completed.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <returns>List of timelogs and ancillary information</returns>
        public DataTable MissingTherapistEvals(bool includeAssistant, int employeeId, int schoolYearId)
        {
            DataTable dt = new DataTable();

            try
            {
                string command = "GetMissingTherapistReports";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();
                parameters.Add(new System.Data.OleDb.OleDbParameter("@IncludeAssistant", includeAssistant));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EmployeeId", employeeId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@SchoolYearId", schoolYearId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@ReportSpecifier", ReportSpecifier.Evals));

                dt = Utility.Data.ExecuteQueryDataTable(command, ref parameters, Utility.DBConnections.dataDb);

            }
            catch (Exception)
            {

                throw;
            }

            return dt;
        }


        /// <summary>Get all timelogs that have been entered for reports but no report is completed.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <returns>List of timelogs and ancillary information</returns>
        public DataTable MissingTherapistReports(bool includeAssistant, int employeeId, int schoolYearId)
        {
            DataTable dt = new DataTable();

            try
            {
                string command = "GetMissingTherapistReports";
                List<System.Data.OleDb.OleDbParameter> parameters = new List<System.Data.OleDb.OleDbParameter>();
                parameters.Add(new System.Data.OleDb.OleDbParameter("@IncludeAssistant", includeAssistant));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@EmployeeId", employeeId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@SchoolYearId", schoolYearId));
                parameters.Add(new System.Data.OleDb.OleDbParameter("@ReportSpecifier", ReportSpecifier.AllOthers));

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