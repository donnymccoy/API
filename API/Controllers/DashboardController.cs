using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Timers;
using System.Web.Http;
using System.Web.Http.Description;

using LifeSpan.API.BLL.Dashboard;
using LifeSpan.DTO.Common;
using LifeSpan.DTO.Dashboard;
using Newtonsoft.Json;

namespace Lifespan.API.Controllers
{
    /// <summary>
    ///     This controller provides dashboard functionality and is called from core web project.
    /// </summary>

    /// <remarks> 
    /// The ASP.NET MVC framework supports several types of action results including:
    ///     ViewResult - Represents HTML and markup.
    ///     EmptyResult - Represents no result.
    ///     RedirectResult - Represents a redirection to a new URL.
    ///     JsonResult - Represents a JavaScript Object Notation result that can be used in an AJAX application.
    ///     JavaScriptResult - Represents a JavaScript script.
    ///     ContentResult - Represents a text result.
    ///     FileContentResult - Represents a downloadable file (with the binary content).
    ///     FilePathResult - Represents a downloadable file(with a path).
    ///     FileStreamResult - Represents a downloadable file(with a file stream).
    /// All of these action results inherit from the base ActionResult class.
    /// 
    /// 
    ///     Deserialize request JSON into request DTO and immediately log everything.  
    ///     We don't use [FromBody] in the method declaration because you can only read that from the request once.  
    ///     This method allows us to log the absolute raw JSON from the consumer.
    /// </remarks>
    public class DashboardController : ApiController
    {

        #region "Class-level"

        
        /// <summary>Default controller constructor</summary>
        /// <remarks>Sample controller remarks</remarks>
        public DashboardController()
        {
        }

        /*
        // Logging and Exception handlers //
        private Utility.ExceptionHandler errorHandler;
        private StringBuilder log = new StringBuilder();
        private BLL.DTO.Logging.EventLog eventLog;

        // Logging and exception handling variables.  MethodName is built in each method. //
        private string className = MethodInfo.GetCurrentMethod().DeclaringType.Name;
        private string applicationName = string.Empty;
        private string environmentName = string.Empty;
        private string clientIP = string.Empty;
        private string hostName = string.Empty;
        private string apiVersion = string.Empty;

        // Unique transaction identifier for this execution.  Useful when chaining log events together. //
        private string trxGUID = string.Empty;


        // APIKey and Method authentication //
        private BLL.Core.Authentication auth;


        // Application control class //
        private DTO.Core.ApplicationControl appControl;


        // References to other namespace objects //        
        private BLL.Account bll;
        private Digital.API.Utilities.Utility util;

        /// <summary>
        /// Default constructor method
        /// </summary>
        public DashboardController()
        {
            // Create application control class //
            appControl = new Digital.DTO.Core.ApplicationControl();

            // Get environmental settings //
            util = new Digital.API.Utilities.Utility();


            // We persist this through the API in order to consistently log who makes record changes //
            appControl.DatabaseUserName = System.Configuration.ConfigurationManager.AppSettings["DB_UserUpdated"];


            appControl.ApplicationName = util.GetApplicationName();
            appControl.AssemblyBuildDate = Utility.ApplicationInformation.CompileDate;
            appControl.AssemblyBuildVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();


            appControl.Environment = util.GetEnvironmentName();
            appControl.LogFaults = ConfigurationManager.AppSettings["LogFaults"] == "True";
            appControl.LogInputs = ConfigurationManager.AppSettings["LogInputs"] == "True";
            appControl.LogOutputs = ConfigurationManager.AppSettings["LogOutputs"] == "True";
            appControl.LogTransactions = ConfigurationManager.AppSettings["LogTransactions"] == "True";


            // Initialize our objects //
            auth = new BLL.Core.Authentication(appControl);
            bll = new BLL.Account(appControl);
            errorHandler = new Utility.ExceptionHandler(appControl.ApplicationName, appControl.Environment);


            // Initialize logging (which inits its internal StopWatch) //
            ApplicationLogger.Main.ApplicationName = appControl.ApplicationName;
            ApplicationLogger.Main.EnvironmentName = appControl.Environment;
            eventLog = new BLL.DTO.Logging.EventLog(appControl.ApplicationName, appControl.Environment, this.className);


        }
        */
        #endregion


        #region "Compliance"
        /// <summary>Get all non-compliant notes for the current month.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// ***** These views are not built yet *****
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/ReviewMeetingsList?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into ????? view.
        /// </remarks>
        /// <returns>List of NonCompliantNotes</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.NonCompliantNoteResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, need detail form
        public HttpResponseMessage NonCompliantNotes(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.NonCompliantNoteResponse result = new NonCompliantNoteResponse();

            try
            {
                Compliance bll = new Compliance();

                result.NonCompliantNotes = bll.NonCompliantNotes((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.NonCompliantNotes.Count;
                result.RecordCount = result.NonCompliantNotes.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.NonCompliantNotes.Clear();}

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }


        /// <summary>Get all UDO notes requiring co-signature.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// ***** These views are not built yet *****
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/ReviewMeetingsList?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into ????? view.
        /// </remarks>
        /// <returns>List of UnsignedUDONotes</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.UnsignedUDONoteResponse))]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage UnsignedUDONotes(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.UnsignedUDONoteResponse result = new UnsignedUDONoteResponse();

            try
            {
                Compliance bll = new Compliance();

                result.UnsignedUDONotes = bll.UnsignedUDONotes((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.UnsignedUDONotes.Count;
                result.RecordCount = result.UnsignedUDONotes.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.UnsignedUDONotes.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }

        #endregion  // Compliance //


        #region "Review Meetings"
        /// <summary>Get all outstanding review meeting reports.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// ***** These views are not built yet *****
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/ReviewMeetingsList?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into ????? view.
        /// </remarks>
        /// <returns>List of ReviewMeetings</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.ReviewMeetingResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, need detail form
        public HttpResponseMessage OverdueReviewMeetingReports(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.ReviewMeetingResponse result = new ReviewMeetingResponse();

            try
            {
                ReviewMeetings bll = new ReviewMeetings();

                result.ReviewMeetings = bll.OverdueReviewMeetingReports((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.ReviewMeetings.Count;
                result.RecordCount = result.ReviewMeetings.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.ReviewMeetings.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }


        /// <summary>Get all upcoming review meetings.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// CURRENT:    window.open('../Home/ReviewMeetingsList', '_self');
        /// NEW:        window.open('../Home/UpcomingMeetingsList?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into ????? view.
        /// </remarks>
        /// <returns>List of ReviewMeetings</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.ReviewMeetingResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, need detail form
        public HttpResponseMessage UpcomingReviewMeetings(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.ReviewMeetingResponse result = new ReviewMeetingResponse();

            try
            {
                ReviewMeetings bll = new ReviewMeetings();

                result.ReviewMeetings = bll.UpcomingReviewMeetings((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.ReviewMeetings.Count;
                result.RecordCount = result.ReviewMeetings.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.ReviewMeetings.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }

        #endregion  // Review Meetings //


        #region "Missing Client Documents"
        /// <summary>Get all outstanding ROI documents.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/OutstandingReleases?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into client view.
        /// </remarks>
        /// <returns>List of MissingDocuments</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.MissingClientDocumentResponse))]
        [HttpGet]
        // data done, poco done, detail form done, need to check filters
        public HttpResponseMessage MissingROIDocuments(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.MissingClientDocumentResponse result = new MissingClientDocumentResponse();

            try
            {
                ClientDocuments bll = new ClientDocuments();

                result.MissingClientDocuments = bll.MissingROIDocuments((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.MissingClientDocuments.Count;
                result.RecordCount = result.MissingClientDocuments.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.MissingClientDocuments.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }


        /// <summary>Get all outstanding RX documents.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/MissingRx?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into client view.
        /// </remarks>
        /// <returns>List of MissingDocuments</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.MissingClientDocumentResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, detail form done, need to check filters
        public HttpResponseMessage MissingRXDocuments(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.MissingClientDocumentResponse result = new MissingClientDocumentResponse();

            try
            {
                ClientDocuments bll = new ClientDocuments();

                result.MissingClientDocuments = bll.MissingRXDocuments((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.MissingClientDocuments.Count;
                result.RecordCount = result.MissingClientDocuments.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.MissingClientDocuments.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }


        /// <summary>Get all outstanding RX documents.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/MissingRx?yearid=' + ddlYear + '', '_self');
        /// 
        /// Detailed list drills down into client view.
        /// </remarks>
        /// <returns>List of MissingPhysicians</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.MissingPhysicianResponse))]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage MissingPhysicians(APIEnums.RollupLevel rollupLevel)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.MissingPhysicianResponse result = new MissingPhysicianResponse();

            try
            {
                ClientDocuments bll = new ClientDocuments();

                result.MissingPhysicians = bll.MissingPhysicians();


                // Prepare result output //
                result.DashboardValue = result.MissingPhysicians.Count;
                result.RecordCount = result.MissingPhysicians.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.MissingPhysicians.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }
        
        #endregion  // Missing Client Documents //


        #region "Missing Therapist Reports"
        /// <summary>Get all timelogs that have been entered for evals but no report is completed.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/OutstandingEvaluations', '_self');
        /// EDIT:   window.open('../Home/EditOutstandingEvaluations', '_self');
        /// 
        /// Detailed list does not drilldown into any entity.
        /// </remarks>
        /// <returns>List of timelogs and ancillary information</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.MissingReportResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, detail form done, need to check filters
        public HttpResponseMessage MissingTherapistEvals(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.MissingReportResponse result = new MissingReportResponse();

            try
            {
                TherapistReports bll = new TherapistReports();

                result.MissingReports = bll.MissingTherapistEvals((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId);


                // Prepare result output //
                result.DashboardValue = result.MissingReports.Count;
                result.RecordCount = result.MissingReports.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.MissingReports.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }


        /// <summary>Get all timelogs that have been entered for reports but no report is completed.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <remarks>
        /// *****These views are not built yet *****
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/OutstandingEvaluations', '_self');
        /// EDIT:   window.open('../Home/EditOutstandingEvaluations', '_self');
        /// 
        /// Detailed list does not drilldown into any entity.
        /// </remarks>
        /// <returns>List of timelogs and ancillary information</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.MissingReportResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, detail form done, need to check filters
        public HttpResponseMessage MissingTherapistReports(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.MissingReportResponse result = new MissingReportResponse();

            try
            {
                TherapistReports bll = new TherapistReports();

                result.MissingReports = bll.MissingTherapistReports((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId);


                // Prepare result output //
                result.DashboardValue = result.MissingReports.Count;
                result.RecordCount = result.MissingReports.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.MissingReports.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }
        #endregion  // Missing Therapist Reports //


        #region "Staff Reminders"
        /// <summary>Get all birthdays within 30 days of today's date.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/Birthdays', '_self');
        /// 
        /// Detailed list does not drilldown into any entity.
        /// </remarks>
        /// <returns>List of staff birthdays</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.BirthdayResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, detail form done, need to check filters
        public HttpResponseMessage StaffBirthdays(APIEnums.RollupLevel rollupLevel)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.BirthdayResponse result = new BirthdayResponse();

            try
            {
                StaffReminders bll = new StaffReminders();

                result.Birthdays = bll.StaffBirthdays();


                // Prepare result output //
                result.DashboardValue = result.Birthdays.Count;
                result.RecordCount = result.Birthdays.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.Birthdays.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error,string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;

        }


        /// <summary>Get all employee expirations within 30 days of today's date.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="schoolYearId">Indicates which school year to query</param>
        /// <param name="includeAssistant">Used to specify whether to include assistant therapists in the query</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <param name="startDate">If not NULL, use a date range query</param>
        /// <param name="endDate">If not NULL, use a date range query</param>
        /// <remarks>
        /// Summary navigation leads to detailed list.  
        /// 
        /// VIEW:   window.open('../Home/EmployeeTracking', '_self');
        /// 
        /// Detailed list drills down into Employee view.
        /// </remarks>
        /// <returns>List of employee expirations</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.ExpirationResponse))]
        [HttpGet]
        //[Authorize]
        // data done, poco done, detail form done, need to check filters
        public HttpResponseMessage ExpirationDates(APIEnums.RollupLevel rollupLevel, APIEnums.IncludeAssistant includeAssistant, int employeeId, int schoolYearId, DateTime? startDate, DateTime? endDate)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.ExpirationResponse result = new ExpirationResponse();

            try
            {
                StaffReminders bll = new StaffReminders();

                result.Expirations = bll.StaffExpirations((includeAssistant == APIEnums.IncludeAssistant.True ? true : false), employeeId, schoolYearId, startDate, endDate);


                // Prepare result output //
                result.DashboardValue = result.Expirations.Count;
                result.RecordCount = result.Expirations.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.Expirations.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;
        }


        /// <summary>Get all employee reimbursements.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <remarks>
        /// ***** This is not an R1 deliverable. *****
        /// ***** These views are built. *****
        /// 
        /// SUMMARY:    window.open('../Report/ReimbursementSummaryListReport?yearid=' + ddlYear + '', '_blank');
        /// DETAIL:     window.open('../Report/ReimbursementDetailListReport?yearid=' + ddlYear + '', '_blank');
        /// 
        /// </remarks>
        /// <returns>List of employee reimbursements</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.ReimbursementResponse))]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage Reimbursements(APIEnums.RollupLevel rollupLevel, int employeeId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.ReimbursementResponse result = new ReimbursementResponse();

            try
            {
                StaffReminders bll = new StaffReminders();

                //result.Reimbursements = bll.Reimbursements(employeeId);


                // Prepare result output //
                result.DashboardValue = result.Reimbursements.Count;
                result.RecordCount = result.Reimbursements.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.Reimbursements.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;
        }


        /// <summary>Get all PTO information.</summary>
        /// <param name="rollupLevel">Level of detail requested</param>
        /// <param name="employeeId">EmployeeId to query for.  If 0, include ALL records.</param>
        /// <remarks>
        /// ***** This is not an R1 deliverable. *****
        /// ***** These views are not built yet *****
        ///  
        /// SUMMARY:    window.open('../Report/PTOSummaryListReport?yearid=' + ddlYear + '', '_blank');
        /// DETAIL:     window.open('../Report/PTODetailListReport?yearid=' + ddlYear + '', '_blank');
        /// 
        /// </remarks>
        /// <returns>List of PTO events</returns>
        /// <response code="200">Success</response>
        [ResponseType(typeof(LifeSpan.DTO.Dashboard.PtoResponse))]
        [HttpGet]
        //[Authorize]
        public HttpResponseMessage Pto(APIEnums.RollupLevel rollupLevel, int employeeId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Work objects //
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            LifeSpan.DTO.Dashboard.PtoResponse result = new PtoResponse();

            try
            {
                StaffReminders bll = new StaffReminders();

                //result.Ptos = bll.Pto(employeeId);


                // Prepare result output //
                result.DashboardValue = result.Ptos.Count;
                result.RecordCount = result.Ptos.Count;

                // Return summary only if requested //
                if (rollupLevel == APIEnums.RollupLevel.Summary) { result.Ptos.Clear(); }

                result.Success = true;
            }

            catch (Exception ex)
            {
                result.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                result.Errors.Add(new Error(Error.ErrorClass.Error, string.Empty, ex.Message, ex));
            }

            stopwatch.Stop();
            result.ExecutionTimeSpan = stopwatch.Elapsed;
            result.TimeStamp = DateTime.Now;
            response.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");

            return response;
        }

        #endregion  // Staff Reminders //

    }
}
