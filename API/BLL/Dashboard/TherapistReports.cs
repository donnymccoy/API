using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LifeSpan.DTO.Dashboard;

namespace LifeSpan.API.BLL.Dashboard
{
    public class TherapistReports
    {
        /// <summary>Get all timelogs that have been entered for evals but no report is completed.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <returns>List of timelogs and ancillary information</returns>
        public List<MissingReport> MissingTherapistEvals(bool includeAssistant, int employeeId, int schoolYearId)
        {
            List<MissingReport> result = new List<MissingReport>();

            try
            {
                DAL.Dashboard.TherapistReports dal = new DAL.Dashboard.TherapistReports();

                System.Data.DataTable dt = dal.MissingTherapistEvals(includeAssistant, employeeId, schoolYearId);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        MissingReport mr = new MissingReport()
                        {
                            ServiceId = (int)dataRow["ServiceId"],
                            LogIdOne = (int)dataRow["LogIdOne"],
                            LogIdTwo = (int)dataRow["LogIdTwo"],
                            ClientId = (int)dataRow["ClientId"],
                            TherapistId = (int)dataRow["TherapistId"],
                            Therapist2Id = (int)dataRow["Therapist2Id"],
                            ClientYearServiceId = (int)dataRow["ClientYearServiceId"],
                            
                            ReceiveDate = DBNull.Value.Equals(dataRow["ReceiveDate"]) ? new DateTime(1901,1,1) : Convert.ToDateTime(dataRow["ReceiveDate"]),
                            StartDate = DBNull.Value.Equals(dataRow["StartDate"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["StartDate"]),
                            EvalDate = DBNull.Value.Equals(dataRow["EvalDate"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["EvalDate"]),
                            ReportDateOne = DBNull.Value.Equals(dataRow["ReportDateOne"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["ReportDateOne"]),
                            ReportDateTwo = DBNull.Value.Equals(dataRow["ReportDateTwo"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["ReportDateTwo"]),

                            Client = dataRow["Client"].ToString().Trim(),
                            SchoolYearName = dataRow["SchoolYearName"].ToString().Trim(),
                            Therapist1Name = dataRow["Therapist1Name"].ToString().Trim(),
                            Therapist2Name = dataRow["Therapist2Name"].ToString().Trim(),
                            ReferralSource = dataRow["ReferralSource"].ToString().Trim(),
                            SessionType = dataRow["SessionType"].ToString().Trim(),
                            Service = dataRow["Service"].ToString().Trim()
                        };
                        result.Add(mr);
                    }
                }
                dt = null;


            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        /// <summary>Get all timelogs that have been entered for reports but no report is completed.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <returns>List of timelogs and ancillary information</returns>
        public List<MissingReport> MissingTherapistReports(bool includeAssistant, int employeeId, int schoolYearId)
        {
            List<MissingReport> result = new List<MissingReport>();

            try
            {
                DAL.Dashboard.TherapistReports dal = new DAL.Dashboard.TherapistReports();

                System.Data.DataTable dt = dal.MissingTherapistReports(includeAssistant, employeeId, schoolYearId);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        MissingReport mr = new MissingReport()
                        {
                            ServiceId = (int)dataRow["ServiceId"],
                            LogIdOne = (int)dataRow["LogIdOne"],
                            LogIdTwo = (int)dataRow["LogIdTwo"],
                            ClientId = (int)dataRow["ClientId"],
                            TherapistId = (int)dataRow["TherapistId"],
                            Therapist2Id = (int)dataRow["Therapist2Id"],
                            ClientYearServiceId = (int)dataRow["ClientYearServiceId"],

                            ReceiveDate = DBNull.Value.Equals(dataRow["ReceiveDate"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["ReceiveDate"]),
                            StartDate = DBNull.Value.Equals(dataRow["StartDate"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["StartDate"]),
                            EvalDate = DBNull.Value.Equals(dataRow["EvalDate"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["EvalDate"]),
                            ReportDateOne = DBNull.Value.Equals(dataRow["ReportDateOne"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["ReportDateOne"]),
                            ReportDateTwo = DBNull.Value.Equals(dataRow["ReportDateTwo"]) ? new DateTime(1901, 1, 1) : Convert.ToDateTime(dataRow["ReportDateTwo"]),

                            Client = dataRow["Client"].ToString().Trim(),
                            SchoolYearName = dataRow["SchoolYearName"].ToString().Trim(),
                            Therapist1Name = dataRow["Therapist1Name"].ToString().Trim(),
                            Therapist2Name = dataRow["Therapist2Name"].ToString().Trim(),
                            ReferralSource = dataRow["ReferralSource"].ToString().Trim(),
                            SessionType = dataRow["SessionType"].ToString().Trim(),
                            Service = dataRow["Service"].ToString().Trim()
                        };
                        result.Add(mr);
                    }
                }
                dt = null;


            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}