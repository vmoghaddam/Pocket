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
    
    public partial class FlightPlanDay
    {
        public int FlightPlanId { get; set; }
        public int Day { get; set; }
    
        public virtual FlightPlan FlightPlan { get; set; }
    }
}
