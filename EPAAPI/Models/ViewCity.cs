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
    
    public partial class ViewCity
    {
        public int Id { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public string SortName { get; set; }
        public string Country { get; set; }
        public Nullable<int> PhoneCode { get; set; }
        public string FullName { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
    }
}
