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
    
    public partial class ViewCrewTime
    {
        public int Id { get; set; }
        public string PID { get; set; }
        public string Name { get; set; }
        public System.DateTime CDate { get; set; }
        public Nullable<int> BoxId { get; set; }
        public string DateStr { get; set; }
        public Nullable<double> Day1_Duty { get; set; }
        public Nullable<int> Day1_Flight { get; set; }
        public Nullable<double> Day7_Duty { get; set; }
        public Nullable<int> Day7_Flight { get; set; }
        public Nullable<double> Day14_Duty { get; set; }
        public Nullable<int> Day14_Flight { get; set; }
        public Nullable<double> Day28_Duty { get; set; }
        public Nullable<int> Day28_Flight { get; set; }
        public Nullable<double> Year_Duty { get; set; }
        public Nullable<int> Year_Flight { get; set; }
        public Nullable<int> CalendarStatusId { get; set; }
        public string CalendarStatus { get; set; }
        public Nullable<int> ECSplitedId { get; set; }
        public Nullable<int> ECId { get; set; }
        public Nullable<System.DateTime> ECDateStartLocal { get; set; }
        public Nullable<System.DateTime> ECDateEndLocal { get; set; }
        public Nullable<System.DateTime> ECDateStart { get; set; }
        public Nullable<System.DateTime> ECDateEnd { get; set; }
        public Nullable<decimal> ECDuty { get; set; }
        public Nullable<int> ECBoxId { get; set; }
        public decimal FDPReduction { get; set; }
    }
}