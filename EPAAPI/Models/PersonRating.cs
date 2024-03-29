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
    
    public partial class PersonRating
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int AircraftTypeId { get; set; }
        public Nullable<int> RatingId { get; set; }
        public System.DateTime DateIssue { get; set; }
        public Nullable<System.DateTime> DateExpire { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> OrganizationId { get; set; }
    
        public virtual AircraftType AircraftType { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual PersonRatingDocument PersonRatingDocument { get; set; }
        public virtual Person Person { get; set; }
    }
}
