//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PHBC.DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ContinentCode { get; set; }
        public Nullable<bool> isPrintedCN23 { get; set; }
        public Nullable<bool> isPostalParcels { get; set; }
    
        public virtual Continent Continent { get; set; }
    }
}
