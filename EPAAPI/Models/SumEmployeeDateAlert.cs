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
    
    public partial class SumEmployeeDateAlert
    {
        public int Id { get; set; }
        public Nullable<int> PassportExpired { get; set; }
        public Nullable<int> PassportExpiring { get; set; }
        public Nullable<int> NDTExpired { get; set; }
        public Nullable<int> NDTExpiring { get; set; }
        public Nullable<int> CAOExpired { get; set; }
        public Nullable<int> CAOExpiring { get; set; }
        public Nullable<int> MedicalExpired { get; set; }
        public Nullable<int> MedicalExpiring { get; set; }
    }
}