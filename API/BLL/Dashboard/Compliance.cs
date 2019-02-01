using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using LifeSpan.DTO.Dashboard;

namespace LifeSpan.API.BLL.Dashboard
{
    public class Compliance
    {
        /// <summary>Get all non-compliant notes for the current month.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of NonCompliantNotes</returns>
        public List<NonCompliantNote> NonCompliantNotes(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            List<NonCompliantNote> result = new List<NonCompliantNote>();

            try
            {
                DAL.Dashboard.Compliance dal = new DAL.Dashboard.Compliance();

                System.Data.DataTable dt = dal.NonCompliantNotes(includeAssistant, employeeId, schoolYearId, startDate, endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {

                        DateTime sessionStartDateTime = DateTime.TryParse(dataRow["SessionStartDateTime"].ToString().Trim(), out sessionStartDateTime) ? sessionStartDateTime : DateTime.MinValue;
                        DateTime sessionEndDateTime = DateTime.TryParse(dataRow["SessionEndDateTime"].ToString().Trim(), out sessionEndDateTime) ? sessionEndDateTime : DateTime.MinValue;
                        DateTime timelogSignedDate = DateTime.TryParse(dataRow["TimelogSignedDate"].ToString().Trim(), out timelogSignedDate) ? timelogSignedDate : DateTime.MinValue;
                        DateTime createdDate = DateTime.TryParse(dataRow["CreatedDate"].ToString().Trim(), out createdDate) ? createdDate : DateTime.MinValue;


                        NonCompliantNote rpt = new NonCompliantNote()
                        {
                            TimeLogId = (int)dataRow["TimelogId"],
                            Therapist1Id = (int)dataRow["TherapistId"],
                            HoursNonCompliant = (int)dataRow["HoursNonCompliant"],
                            Therapist1Name = dataRow["TherapistName"].ToString().Trim(),
                            ClientName = dataRow["ClientName"].ToString().Trim(),
                            CreatedDate = createdDate,
                            SessionEndDateTime = sessionEndDateTime,
                            SessionStartDateTime = sessionStartDateTime,
                            TimelogSignedDate = timelogSignedDate
                        };
                        result.Add(rpt);
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


        /// <summary>Get all UDO notes requiring co-signature.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of UnsignedUDONotes</returns>
        public List<UnsignedUDONote> UnsignedUDONotes(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            List<UnsignedUDONote> result = new List<UnsignedUDONote>();

            try
            {
                int month = DateTime.Today.Month;
                int year = DateTime.Today.Year;

                DAL.Dashboard.Compliance dal = new DAL.Dashboard.Compliance();

                System.Data.DataTable dt = dal.UnsignedUDONotes(includeAssistant, employeeId, schoolYearId, startDate, endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {

                        DateTime sessionStartDateTime = DateTime.TryParse(dataRow["SessionStartDateTime"].ToString().Trim(), out sessionStartDateTime) ? sessionStartDateTime : DateTime.MinValue;
                        DateTime sessionEndDateTime = DateTime.TryParse(dataRow["SessionEndDateTime"].ToString().Trim(), out sessionEndDateTime) ? sessionEndDateTime : DateTime.MinValue;
                        DateTime soapPNoteSignedDate = DateTime.TryParse(dataRow["SOAPNoteSignedDate"].ToString().Trim(), out soapPNoteSignedDate) ? soapPNoteSignedDate : DateTime.MinValue;
                        DateTime soapNoteSupSignedDate = DateTime.TryParse(dataRow["SOAPNoteSupSignedDate"].ToString().Trim(), out soapNoteSupSignedDate) ? soapNoteSupSignedDate : DateTime.MinValue;
                        DateTime timelogSignedDate = DateTime.TryParse(dataRow["TimelogSignedDate"].ToString().Trim(), out timelogSignedDate) ? timelogSignedDate : DateTime.MinValue;
                        DateTime createdDate = DateTime.TryParse(dataRow["CreatedDate"].ToString().Trim(), out createdDate) ? createdDate : DateTime.MinValue;


                        UnsignedUDONote rpt = new UnsignedUDONote()
                        {
                            TimeLogId = (int)dataRow["TimelogId"],
                            SOAPNoteId = (int)dataRow["SOAPNoteId"],
                            ClientId = (int)dataRow["ClientId"],
                            Therapist1Id = (int)dataRow["TherapistId"],
                            SupervisorId = (int)dataRow["SupervisorId"],
                            SOAPNoteSignedById = (int)dataRow["SOAPNoteSignedById"],
                            SOAPNoteSupSignedById = (int)dataRow["SOAPNoteSupSignedById"],

                            Therapist1Name = dataRow["TherapistName"].ToString().Trim(),
                            ClientName = dataRow["ClientName"].ToString().Trim(),
                            SupervisorName = dataRow["SupervisorName"].ToString().Trim(),
                            SOAPNoteSignedBy = dataRow["SOAPNoteSignedBy"].ToString().Trim(),
                            SOAPNoteSupSignedBy = dataRow["SOAPNoteSupSignedBy"].ToString().Trim(),

                            CreatedDate = createdDate,
                            SessionEndDateTime = sessionEndDateTime,
                            SessionStartDateTime = sessionStartDateTime,
                            TimelogSignedDate = soapPNoteSignedDate,
                            SOAPNoteSignedDate = soapNoteSupSignedDate,
                            SOAPNoteSupSignedDate = timelogSignedDate
                        };
                        result.Add(rpt);
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