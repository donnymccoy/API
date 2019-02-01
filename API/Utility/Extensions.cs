using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LifeSpan.API.Utility
{
    public static class Extensions
    {

        #region ENUM
        public static string Description(this Enum value)
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
        #endregion

        public static IOrderedEnumerable<TSource> OrderByWithDirection<TSource, TKey>
        (this IEnumerable<TSource> source,
         Func<TSource, TKey> keySelector,
         bool descending)
            {
                return descending ? source.OrderByDescending(keySelector)
                                  : source.OrderBy(keySelector);
            }

        public static IOrderedQueryable<TSource> OrderByWithDirection<TSource, TKey>
                (this IQueryable<TSource> source,
                 Expression<Func<TSource, TKey>> keySelector,
                 bool descending)
            {
                return descending ? source.OrderByDescending(keySelector)
                                  : source.OrderBy(keySelector);
            }

        //#region AccountNumberPatientCode

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.AccountNumberPatientCode> accountNumberPatientCodes)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("AccountNumberPatientCodeList");

        //    try
        //    {
        //        dt.Columns.Add("AccountNumber");
        //        dt.Columns.Add("PatientCode");

        //        //fill the table with data
        //        foreach (Digital.DTO.Practice.AccountNumberPatientCode accountNumberPatientCode in accountNumberPatientCodes)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["AccountNumber"] = accountNumberPatientCode.AccountNumber;
        //            dr["PatientCode"] = accountNumberPatientCode.PatientCode;
        //            dt.Rows.Add(dr);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return dt;
        //}

        //#endregion

        //#region Content

        //public static DataTable ToDataTable(this List<Digital.DTO.Core.Content> contents)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("ContentList");
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("Name");
        //    dt.Columns.Add("Description");
        //    dt.Columns.Add("Html");
        //    dt.Columns.Add("ReadOnly");
        //    dt.Columns.Add("Priority");
        //    dt.Columns.Add("StatusId");

        //    //fill the table with data
        //    foreach (Digital.DTO.Core.Content content in contents)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = content.Id;
        //        dr["Name"] = content.Name;
        //        dr["Description"] = content.Description;
        //        dr["Html"] = content.Html;
        //        dr["ReadOnly"] = content.ReadOnly;
        //        dr["Priority"] = content.Priority;
        //        dr["StatusId"] = content.StatusId;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        //#region MedicalAlert

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.MedicalAlert> medicalAlerts)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("PatientList");
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("Name");
        //    dt.Columns.Add("StatusId");

        //    //fill the table with data
        //    foreach (Digital.DTO.Practice.MedicalAlert medicalAlert in medicalAlerts)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = medicalAlert.Id;
        //        dr["Name"] = medicalAlert.Name;
        //        dr["StatusId"] = medicalAlert.StatusId;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        //#region Patient

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.Patient> patients)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("PatientList");
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("PatientCode");
        //    dt.Columns.Add("PersonId");
        //    dt.Columns.Add("AccountId");
        //    dt.Columns.Add("AccountTypeId");
        //    dt.Columns.Add("AccountGroupId");
        //    dt.Columns.Add("FinancialClassId");
        //    dt.Columns.Add("RelationshipId");
        //    dt.Columns.Add("FacilityCode");
        //    dt.Columns.Add("StatusId");
        //    dt.Columns.Add("UserName");

        //    //fill the table with data
        //    foreach (Digital.DTO.Practice.Patient patient in patients)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = patient.Id;
        //        dr["PatientCode"] = patient.Code;
        //        dr["PersonId"] = patient.PersonId;
        //        dr["AccountId"] = patient.AccountId;
        //        dr["AccountTypeId"] = patient.AccountTypeId;
        //        dr["AccountGroupId"] = patient.AccountGroupId;
        //        dr["FinancialClassId"] = patient.FinancialClassId;
        //        dr["RelationshipId"] = patient.RelationshipId;
        //        dr["FacilityCode"] = patient.FacilityCode;
        //        dr["StatusId"] = patient.StatusId;
        //        dr["UserName"] = patient.UserName;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        //#region PatientMedicalAlert

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.PatientMedicalAlert> patientMedicalAlerts)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("PatientMedicalAlertList");
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("PatientId");
        //    dt.Columns.Add("MedicalAlertId");
        //    dt.Columns.Add("Priority");
        //    dt.Columns.Add("StatusId");
        //    dt.Columns.Add("UserName");

        //    //fill the table with data
        //    foreach (Digital.DTO.Practice.PatientMedicalAlert patientMedicalAlert in patientMedicalAlerts)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = patientMedicalAlert.Id;
        //        dr["PatientId"] = patientMedicalAlert.PatientId;
        //        dr["MedicalAlertId"] = patientMedicalAlert.MedicalAlertId;
        //        dr["Priority"] = patientMedicalAlert.Priority;
        //        dr["StatusId"] = patientMedicalAlert.StatusId;
        //        dr["UserName"] = patientMedicalAlert.UserName;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        //#region Person

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.Person> persons)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("PersonList");

        //    try
        //    {
        //        dt.Columns.Add("Id");
        //        dt.Columns.Add("FirstName");
        //        dt.Columns.Add("MiddleName");
        //        dt.Columns.Add("LastName");
        //        dt.Columns.Add("SuffixId");
        //        dt.Columns.Add("SocialSecurityNumber");
        //        dt.Columns.Add("DateOfBirth");
        //        dt.Columns.Add("GenderId");
        //        dt.Columns.Add("EmailAddress");
        //        dt.Columns.Add("LanguageId");
        //        dt.Columns.Add("StatusId");
        //        dt.Columns.Add("UserName");

        //        //fill the table with data
        //        foreach (Digital.DTO.Practice.Person person in persons)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["Id"] = person.Id;
        //            dr["FirstName"] = person.FirstName;
        //            dr["MiddleName"] = person.MiddleName;
        //            dr["LastName"] = person.LastName;
        //            dr["SuffixId"] = person.SuffixId;
        //            dr["SocialSecurityNumber"] = string.IsNullOrEmpty(person.SocialSecurityNumber) ? null : person.SocialSecurityNumber;
        //            dr["DateOfBirth"] = person.DateOfBirth;
        //            dr["GenderId"] = person.GenderId;
        //            dr["EmailAddress"] = person.Email;
        //            dr["LanguageId"] = person.LanguageId;
        //            dr["StatusId"] = person.StatusId;
        //            dr["UserName"] = person.UserName;
        //            dt.Rows.Add(dr);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return dt;
        //}

        //#endregion

        //#region PersonAddress

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.PersonAddress> personAddresses)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("PersonAddressList");
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("PersonId");
        //    dt.Columns.Add("PersonAddressTypeId");
        //    dt.Columns.Add("Line1");
        //    dt.Columns.Add("Line2");
        //    dt.Columns.Add("City");
        //    dt.Columns.Add("StateId");
        //    dt.Columns.Add("ZipCode");
        //    dt.Columns.Add("ZipExtension");
        //    dt.Columns.Add("Instructions");
        //    dt.Columns.Add("Comments");
        //    dt.Columns.Add("StatusId");
        //    dt.Columns.Add("UserName");

        //    //fill the table with data
        //    foreach (Digital.DTO.Practice.PersonAddress personAddress in personAddresses)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = personAddress.Id;
        //        dr["PersonId"] = personAddress.PersonId;
        //        dr["PersonAddressTypeId"] = personAddress.TypeId;
        //        dr["Line1"] = personAddress.Line1;
        //        dr["Line2"] = personAddress.Line2;
        //        dr["City"] = personAddress.City;
        //        dr["StateId"] = personAddress.StateId;
        //        dr["ZipCode"] = personAddress.ZipCode;
        //        dr["ZipExtension"] = personAddress.ZipExtension;
        //        dr["Instructions"] = personAddress.Instructions;
        //        dr["Comments"] = personAddress.Comments;
        //        dr["StatusId"] = personAddress.StatusId;
        //        dr["UserName"] = personAddress.UserName;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        //#region PersonPhone

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.PersonPhone> personPhones)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("PersonPhoneList");
        //    dt.Columns.Add("Id");
        //    dt.Columns.Add("PersonId");
        //    dt.Columns.Add("PersonPhoneTypeId");
        //    dt.Columns.Add("Number");
        //    dt.Columns.Add("Extension");
        //    dt.Columns.Add("IsTextCapable");
        //    dt.Columns.Add("UseToConfirm");
        //    dt.Columns.Add("StatusId");
        //    dt.Columns.Add("UserName");

        //    //fill the table with data
        //    foreach (Digital.DTO.Practice.PersonPhone personPhone in personPhones)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] = personPhone.Id;
        //        dr["PersonId"] = personPhone.PersonId;
        //        dr["PersonPhoneTypeId"] = personPhone.TypeId;
        //        dr["Number"] = personPhone.Number;
        //        dr["Extension"] = personPhone.Extension;
        //        dr["IsTextCapable"] = personPhone.IsTextCapable;
        //        dr["UseToConfirm"] = personPhone.UseToConfirm;
        //        dr["StatusId"] = personPhone.StatusId;
        //        dr["UserName"] = personPhone.UserName;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        //#region Prospect

        //public static DataTable ToDataTable(this List<Digital.DTO.Practice.Prospect> prospects)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("ProspectList");

        //    try
        //    {
        //        dt.Columns.Add("Id");
        //        dt.Columns.Add("CallerFirstName");
        //        dt.Columns.Add("CallerLastName");
        //        dt.Columns.Add("PatientFirstName");
        //        dt.Columns.Add("PatientLastName");
        //        dt.Columns.Add("GenderId");
        //        dt.Columns.Add("DateOfBirth");
        //        dt.Columns.Add("PhoneNumber");
        //        dt.Columns.Add("PhoneType");
        //        dt.Columns.Add("EmailAddress");
        //        dt.Columns.Add("FacilityCode");
        //        dt.Columns.Add("VisitReasonId");
        //        dt.Columns.Add("CallDate");

        //        //fill the table with data
        //        foreach (Digital.DTO.Practice.Prospect prospect in prospects)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["Id"] = prospect.Id;
        //            dr["CallerFirstName"] = prospect.CallerFirstName;
        //            dr["CallerLastName"] = prospect.CallerLastName;
        //            dr["PatientFirstName"] = prospect.PatientFirstName;
        //            dr["PatientLastName"] = prospect.PatientLastName;
        //            dr["GenderId"] = prospect.GenderId;
        //            dr["DateOfBirth"] = prospect.DateOfBirth;
        //            dr["PhoneNumber"] = prospect.PhoneNumber;
        //            dr["PhoneType"] = prospect.PhoneType;
        //            dr["EmailAddress"] = prospect.Email;
        //            dr["FacilityCode"] = prospect.FacilityCode;
        //            dr["VisitReasonId"] = prospect.VisitReasonId;
        //            dr["CallDate"] = prospect.CallDate;
        //            dt.Rows.Add(dr);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return dt;
        //}

        //#endregion

        //#region Int Ids

        //public static DataTable ToDataTable(this int[] ids)
        //{
        //    //create the table definition
        //    DataTable dt = new DataTable("IntIDList");
        //    dt.Columns.Add("Id");


        //    //fill the table with data
        //    foreach (int id in ids)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Id"] =id;

        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}

        //#endregion

        #region DateTime

        public static DateTime MinValueDbSafe(this DateTime date) // 01/01/1940 is 'null' in the as400
        {
            return DateTime.Parse("01/01/1940");
        }

        #endregion
    }
}
