//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPAAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ViewPersonActiveCourse
    {
        public int CourseId { get; set; }
        public int PersonId { get; set; }
        public string No { get; set; }
        public int CourseTypeId { get; set; }
        public System.DateTime DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string Instructor { get; set; }
        public string Location { get; set; }
        public string Department { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public Nullable<int> Duration { get; set; }
        public Nullable<int> DurationUnitId { get; set; }
        public string CourseRemark { get; set; }
        public Nullable<int> Capacity { get; set; }
        public Nullable<int> Tuition { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public Nullable<int> CourseStatusId { get; set; }
        public Nullable<System.DateTime> DateDeadlineRegistration { get; set; }
        public string TrainingDirector { get; set; }
        public string Title { get; set; }
        public Nullable<bool> Recurrent { get; set; }
        public Nullable<int> Interval { get; set; }
        public Nullable<int> CalanderTypeId { get; set; }
        public Nullable<bool> IsInside { get; set; }
        public Nullable<bool> Quarantine { get; set; }
        public Nullable<System.DateTime> DateStartPractical { get; set; }
        public Nullable<System.DateTime> DateEndPractical { get; set; }
        public Nullable<int> DurationPractical { get; set; }
        public Nullable<int> DurationPracticalUnitId { get; set; }
        public Nullable<int> CT_CalendarTypeId { get; set; }
        public string CT_Title { get; set; }
        public Nullable<int> CT_LicenseResultBasicId { get; set; }
        public string CT_Remark { get; set; }
        public Nullable<int> CT_CourseCategoryId { get; set; }
        public Nullable<int> CT_Interval { get; set; }
        public Nullable<bool> CT_IsGeneral { get; set; }
        public Nullable<bool> CT_Status { get; set; }
        public Nullable<int> CT_Id { get; set; }
        public string CC_Title { get; set; }
        public string CaoTypeTitle { get; set; }
        public string CaoTypeRemark { get; set; }
        public string Organization { get; set; }
        public string DurationUnit { get; set; }
        public string Duration2 { get; set; }
        public string Currency { get; set; }
        public string CalendarType { get; set; }
        public string DurationPracticalUnit { get; set; }
        public Nullable<int> Remain { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<bool> IsGeneral { get; set; }
        public Nullable<int> CourseCustomerId { get; set; }
        public string CerNumber { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> DateIssue { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> DateStatus { get; set; }
        public Nullable<bool> StatusConfirmed { get; set; }
        public string Status { get; set; }
        public string CourseStatus { get; set; }
    }
}
