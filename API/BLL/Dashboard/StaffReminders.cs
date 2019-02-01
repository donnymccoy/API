using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using LifeSpan.DTO;
using LifeSpan.DTO.Dashboard;

namespace LifeSpan.API.BLL.Dashboard
{
    public class StaffReminders
    {
        /// <summary>Get all birthdays within 30 days of today's date.</summary>
        /// <returns>List of Birthdays</returns>
        public List<Birthday> StaffBirthdays()
        {
            List<Birthday> result = new List<Birthday>();

            try
            {
                DAL.Dashboard.StaffReminders dal = new DAL.Dashboard.StaffReminders();

                System.Data.DataTable dt = dal.StaffBirthdays();
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {

                        DateTime dob = DateTime.TryParse(dataRow["BirthDate"].ToString().Trim(), out dob) ? dob : DateTime.MinValue;
                        string displayDate = dob.ToString("MM/dd");

                        Birthday bd = new Birthday()
                        {
                            EmployeeId = (int)dataRow["EmployeeId"],
                            EmployeeName = dataRow["EmployeeName"].ToString().Trim(),
                            StreetAddress = dataRow["StreetAddress"].ToString().Trim(),
                            CSZ = dataRow["CSZ"].ToString().Trim(),
                            DOB = dob,
                            DisplayDate = displayDate
                        };
                        result.Add(bd);
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


        /// <summary>Get all employee tracking documents that are either expired or will expire within 30 days of today's date.</summary>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <returns>List of Expirations</returns>
        public List<Expiration> StaffExpirations(bool includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            List<Expiration> result = new List<Expiration>();

            try
            {
                DAL.Dashboard.StaffReminders dal = new DAL.Dashboard.StaffReminders();

                System.Data.DataTable dt = dal.StaffExpirations(includeAssistant, employeeId, schoolYearId, startDate, endDate);
                if (dt != null && dt.Rows.Count != 0)
                {
                    foreach (System.Data.DataRow dataRow in dt.Rows)
                    {

                        Expiration exp = new Expiration();
                        exp.EmployeeId = (int)dataRow["EmployeeId"];
                        exp.EmployeeName = dataRow["EmployeeName"].ToString().Trim();
                        exp.SupervisorId = (int)dataRow["SupervisorId"];
                        exp.SupervisorName = dataRow["SupervisorName"].ToString().Trim();
                        exp.ItemName = dataRow["ItemName"].ToString().Trim();

                        DateTime itemDueDate = DateTime.TryParse(dataRow["ItemDueDate"].ToString().Trim(), out itemDueDate) ? itemDueDate : DateTime.MinValue;
                        exp.ExpirationDate = itemDueDate;

                        TimeSpan difference = itemDueDate - DateTime.Today;
                        exp.DaysToExpiration = difference.Days;
                        
                        result.Add(exp);
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