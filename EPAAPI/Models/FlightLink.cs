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
    
    public partial class FlightLink
    {
        public int Flight1Id { get; set; }
        public int Flight2Id { get; set; }
        public int ReasonId { get; set; }
        public string Remark { get; set; }
    
        public virtual FlightInformation FlightInformation { get; set; }
        public virtual FlightInformation FlightInformation1 { get; set; }
    }
}
