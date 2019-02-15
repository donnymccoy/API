using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using LifeSpan.API.Utility;
using LifeSpan.DTO.Dashboard;

namespace LifeSpan.API.BLL.Dashboard
{
    public class ReviewMeetings
    {
        /// <summary>Get all outstanding review meeting reports.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of ReviewMeetings</returns>
        public List<ReviewMeeting> OverdueReviewMeetingReports(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            List<ReviewMeeting> result = new List<ReviewMeeting>();

            try
            {
                DAL.Dashboard.ReviewMeetings dal = new DAL.Dashboard.ReviewMeetings();

                System.Data.DataTable dt = dal.OverdueReviewMeetingReports(includeAssistant, employeeId, schoolYearId, startDate, endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {

                        DateTime reportDueDate = DateTime.TryParse(dataRow["ReportDueDate"].ToString().Trim(), out reportDueDate) ? reportDueDate : DateTime.MinValue;
                        DateTime reportReceivedDate = DateTime.TryParse(dataRow["ReportReceivedDate"].ToString().Trim(), out reportReceivedDate) ? reportReceivedDate : DateTime.MinValue;

                        DateTime meetingDate = DateTime.TryParse(dataRow["MeetingDate"].ToString().Trim(), out meetingDate) ? meetingDate : DateTime.MinValue;

                        ReviewMeeting rpt = new ReviewMeeting()
                        {
                            ClientName = dataRow["ClientName"].ToString().Trim(),
                            LocationName = dataRow["LocationName"].ToString().Trim(),
                            LocationType = dataRow["LocationType"].ToString().Trim(),
                            LocationDistrict = dataRow["LocationDistrict"].ToString().Trim(),
                            TherapistName = dataRow["TherapistName"].ToString().Trim(),
                            SupervisorName = dataRow["SupervisorName"].ToString().Trim(),
                            ReportReceivedFlag = (bool)dataRow["ReportReceivedFlag"],
                            ReportReceivedDate = reportReceivedDate,
                            ReportDueDate = reportDueDate,
                            MeetingDate = meetingDate,
                            MeetingTypeName = dataRow["MeetingTypeName"].ToString().Trim(),
                            MeetingReportStatus = dataRow["MeetingStatusText"].ToString().Trim(),
                            ReportStatus = (reportDueDate == DateTime.MinValue ? ReportStatusEnum.Unknown : (reportDueDate < DateTime.Today ? ReportStatusEnum.Past : reportDueDate > DateTime.Today ? ReportStatusEnum.Future : ReportStatusEnum.Today)),
                            MeetingStatus = (meetingDate == DateTime.MinValue ? MeetingStatusEnum.Unknown : (meetingDate < DateTime.Today ? MeetingStatusEnum.Past : meetingDate > DateTime.Today ? MeetingStatusEnum.Future : MeetingStatusEnum.Today)),
                            TimeLogId = (int)dataRow["TimelogId"],
                            Therapist1Id = (int)dataRow["TherapistId"],
                            SupervisorId = (int)dataRow["SupervisorId"],
                            ClientId = (int)dataRow["ClientId"],
                            ClinicalReportId = (int)dataRow["ClinicalReportId"],
                            ReviewMeetingId = (int)dataRow["ReviewMeetingId"]
                        };

                        // Convert enum value to Display attribute //
                        rpt.MeetingStatusText = rpt.MeetingStatus.Description();
                        rpt.ReportStatusText = rpt.ReportStatus.Description();

                        result.Add(rpt);
                    }

                    // Filter the result list to only include reports where MeetingDate is >= today AND ReportDone is false //
                    result =
                        result.Where(t => t.ReportReceivedFlag == false && t.MeetingDate >= DateTime.Today && t.ReportDueDate <= DateTime.Today)
                            .OrderByDescending(t => t.MeetingDate).ThenBy(t => t.ClientName)
                            .ToList();

                }
                dt = null;


            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }


        /// <summary>Get all upcoming review meetings.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of ReviewMeetings</returns>
        public List<ReviewMeeting> UpcomingReviewMeetings(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            List<ReviewMeeting> result = new List<ReviewMeeting>();

            try
            {
                DAL.Dashboard.ReviewMeetings dal = new DAL.Dashboard.ReviewMeetings();

                System.Data.DataTable dt = dal.UpcomingReviewMeetings(includeAssistant, employeeId, schoolYearId, startDate, endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {

                        DateTime reportDueDate = DateTime.TryParse(dataRow["ReportDueDate"].ToString().Trim(), out reportDueDate) ? reportDueDate : DateTime.MinValue;
                        DateTime reportReceivedDate = DateTime.TryParse(dataRow["ReportReceivedDate"].ToString().Trim(), out reportReceivedDate) ? reportReceivedDate : DateTime.MinValue;

                        DateTime meetingDate = DateTime.TryParse(dataRow["MeetingDate"].ToString().Trim(), out meetingDate) ? meetingDate : DateTime.MinValue;

                        ReviewMeeting rpt = new ReviewMeeting()
                        {
                            ClientName = dataRow["ClientName"].ToString().Trim(),
                            LocationName = dataRow["LocationName"].ToString().Trim(),
                            LocationType = dataRow["LocationType"].ToString().Trim(),
                            LocationDistrict = dataRow["LocationDistrict"].ToString().Trim(),
                            TherapistName = dataRow["TherapistName"].ToString().Trim(),
                            SupervisorName = dataRow["SupervisorName"].ToString().Trim(),
                            ReportReceivedFlag = (bool)dataRow["ReportReceivedFlag"],
                            ReportReceivedDate = reportReceivedDate,
                            ReportDueDate = reportDueDate,
                            MeetingDate = meetingDate,
                            MeetingTypeName = dataRow["MeetingTypeName"].ToString().Trim(),
                            MeetingReportStatus = dataRow["MeetingStatusText"].ToString().Trim(),
                            ReportStatus = (reportDueDate == DateTime.MinValue ? ReportStatusEnum.Unknown : (reportDueDate < DateTime.Today ? ReportStatusEnum.Past : reportDueDate > DateTime.Today ? ReportStatusEnum.Future : ReportStatusEnum.Today)),
                            MeetingStatus = (meetingDate == DateTime.MinValue ? MeetingStatusEnum.Unknown : (meetingDate < DateTime.Today ? MeetingStatusEnum.Past : meetingDate > DateTime.Today ? MeetingStatusEnum.Future : MeetingStatusEnum.Today)),
                            TimeLogId = (int)dataRow["TimelogId"],
                            Therapist1Id = (int)dataRow["TherapistId"],
                            SupervisorId = (int)dataRow["SupervisorId"],
                            ClientId = (int)dataRow["ClientId"],
                            ClinicalReportId = (int)dataRow["ClinicalReportId"],
                            ReviewMeetingId = (int)dataRow["ReviewMeetingId"]

                        };

                        // Convert enum value to Display attribute //
                        rpt.MeetingStatusText = rpt.MeetingStatus.Description();
                        rpt.ReportStatusText = rpt.ReportStatus.Description();

                        result.Add(rpt);
                    }

                    // Filter the result list to only include meetings where MeetingDate is >= today //
                    result =
                        result.Where(t => t.MeetingDate >= DateTime.Today)
                            .OrderByDescending(t => t.MeetingDate).ThenBy(t => t.ClientName)
                            .ToList();

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