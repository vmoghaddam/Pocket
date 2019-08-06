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
    
    public partial class ViewFlightPlanCalander
    {
        public int Id { get; set; }
        public int CalendarId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> FlightPlanRegisterId { get; set; }
        public string Register { get; set; }
        public Nullable<int> RegisterId { get; set; }
        public Nullable<int> IsLocked { get; set; }
        public Nullable<int> IsOpen { get; set; }
        public string Title { get; set; }
        public Nullable<int> BaseId { get; set; }
        public string BaseName { get; set; }
        public string BaseIATA { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DateActive { get; set; }
        public string Customer { get; set; }
        public Nullable<int> Interval { get; set; }
        public string IntervalType { get; set; }
        public string Types { get; set; }
        public Nullable<int> TotalFlights { get; set; }
        public Nullable<int> VirtualRegisterId { get; set; }
        public string VirtualRegister { get; set; }
        public Nullable<int> VirtualTypeId { get; set; }
        public Nullable<decimal> RegisterAssignProgress { get; set; }
        public int IsApproved100 { get; set; }
        public int IsApproved50 { get; set; }
        public Nullable<System.DateTime> DateApproved100 { get; set; }
        public Nullable<System.DateTime> DateApproved50 { get; set; }
        public int IsApproved60 { get; set; }
        public Nullable<System.DateTime> DateApproved60 { get; set; }
        public int IsApproved70 { get; set; }
        public Nullable<System.DateTime> DateApproved70 { get; set; }
        public Nullable<System.DateTime> DateApproved80 { get; set; }
        public int IsApproved80 { get; set; }
        public int IsApproved90 { get; set; }
        public Nullable<System.DateTime> DateApproved90 { get; set; }
        public Nullable<bool> IsApplied { get; set; }
        public Nullable<System.DateTime> DateApplied { get; set; }
    }
}