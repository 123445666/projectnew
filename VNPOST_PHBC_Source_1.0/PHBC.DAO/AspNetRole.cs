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
    
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.SysActions = new HashSet<SysAction>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Discriminator { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<SysAction> SysActions { get; set; }
    }
}
