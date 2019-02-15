using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LifeSpan.DTO.Dashboard;

namespace LifeSpan.API.BLL.Dashboard
{
    public class ClientDocuments
    {
        /// <summary>Get all outstanding ROI documents.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of MissingDocuments</returns>
        public List<MissingClientDocument> MissingROIDocuments(bool includeAssistant, int employeeId, int schoolYearId,
            DateTime? startDate, DateTime? endDate)
        {
            List<MissingClientDocument> result = new List<MissingClientDocument>();

            try
            {
                DAL.Dashboard.ClientDocuments dal = new DAL.Dashboard.ClientDocuments();

                System.Data.DataTable dt = dal.MissingROIDocuments(includeAssistant, employeeId, schoolYearId, startDate,
                    endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        MissingClientDocument md = new MissingClientDocument()
                        {
                            SchoolYearId = (int) dataRow["SchoolYearId"],
                            ClientDocTrackId = (int) dataRow["ClientDocTrackId"],
                            ClientId = (int) dataRow["ClientId"],
                            PhysicianId = (int) dataRow["PhysicianId"],
                            DocumentTemplateId = (int) dataRow["DocumentTemplateId"],
                            DocStylesId = (int) dataRow["DocStylesId"],
                            TherapistId = (int) dataRow["TherapistId"],
                            SupervisorId = (int) dataRow["SupervisorId"],

                            ReceiveDate =
                                DBNull.Value.Equals(dataRow["ReceiveDate"])
                                    ? new DateTime(1901, 1, 1)
                                    : Convert.ToDateTime(dataRow["ReceiveDate"]),
                            SentDate =
                                DBNull.Value.Equals(dataRow["SentDate"])
                                    ? new DateTime(1901, 1, 1)
                                    : Convert.ToDateTime(dataRow["SentDate"]),

                            Client = dataRow["Client"].ToString().Trim(),
                            SchoolYearName = dataRow["SchoolYearName"].ToString().Trim(),
                            Physician = dataRow["Physician"].ToString().Trim(),
                            DocumentName = dataRow["DocumentName"].ToString().Trim(),
                            PhysicianPhone = dataRow["PhysicianPhone"].ToString().Trim(),
                            ParentName = dataRow["ParentName"].ToString().Trim(),
                            ParentPhoneNumber = dataRow["ParentPhone"].ToString().Trim()
                        };
                        result.Add(md);
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


        /// <summary>Get all outstanding RX and Physician documents.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of MissingDocuments</returns>
        public List<MissingClientDocument> MissingRXDocuments(bool includeAssistant, int employeeId, int schoolYearId,
            DateTime? startDate, DateTime? endDate)
        {
            List<MissingClientDocument> result = new List<MissingClientDocument>();

            try
            {
                DAL.Dashboard.ClientDocuments dal = new DAL.Dashboard.ClientDocuments();

                System.Data.DataTable dt = dal.MissingRXDocuments(includeAssistant, employeeId, schoolYearId, startDate,
                    endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        MissingClientDocument md = new MissingClientDocument()
                        {
                            SchoolYearId = (int) dataRow["SchoolYearId"],
                            ClientDocTrackId = (int) dataRow["ClientDocTrackId"],
                            ClientId = (int) dataRow["ClientId"],
                            PhysicianId = (int) dataRow["PhysicianId"],
                            DocumentTemplateId = (int) dataRow["DocumentTemplateId"],
                            DocStylesId = (int) dataRow["DocStylesId"],
                            TherapistId = (int) dataRow["TherapistId"],
                            SupervisorId = (int) dataRow["SupervisorId"],

                            ReceiveDate =
                                DBNull.Value.Equals(dataRow["ReceiveDate"])
                                    ? new DateTime(1901, 1, 1)
                                    : Convert.ToDateTime(dataRow["ReceiveDate"]),
                            SentDate =
                                DBNull.Value.Equals(dataRow["SentDate"])
                                    ? new DateTime(1901, 1, 1)
                                    : Convert.ToDateTime(dataRow["SentDate"]),

                            Client = dataRow["Client"].ToString().Trim(),
                            SchoolYearName = dataRow["SchoolYearName"].ToString().Trim(),
                            Physician = dataRow["Physician"].ToString().Trim(),
                            DocumentName = dataRow["DocumentName"].ToString().Trim(),
                            PhysicianPhone = dataRow["PhysicianPhone"].ToString().Trim(),
                            ParentName = dataRow["ParentName"].ToString().Trim(),
                            ParentPhoneNumber = dataRow["ParentPhone"].ToString().Trim()
                        };
                        result.Add(md);
                    }
                }
                dt = null;

                // TODO:  Must handle situation where client has multiple therapists under same CYS as SY //


            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }


        /// <summary>Get active clients not assigned a physician.</summary>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <returns>List of MissingPhysicians</returns>
        public List<MissingPhysician> MissingPhysicians(bool includeAssistant, int employeeId)
        {
            List<MissingPhysician> result = new List<MissingPhysician>();

            try
            {
                DAL.Dashboard.ClientDocuments dal = new DAL.Dashboard.ClientDocuments();

                System.Data.DataTable dt = dal.MissingPhysicians(includeAssistant, employeeId);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {
                        MissingPhysician md = new MissingPhysician()
                        {
                            ClientId = (int) dataRow["ClientId"],
                            Client = dataRow["Client"].ToString().Trim(),
                            ParentName = dataRow["ParentName"].ToString().Trim(),
                            ParentPhoneNumber = dataRow["ParentPhone"].ToString().Trim()
                        };
                        result.Add(md);
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