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
    
    public partial class POSLevel
    {
        public POSLevel()
        {
            this.POS = new HashSet<POS>();
        }
    
        public string POSLevelCode { get; set; }
        public string POSLevelName { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<POS> POS { get; set; }
    }
}
