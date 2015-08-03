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
    
    public partial class SysAction
    {
        public SysAction()
        {
            this.SysMenus = new HashSet<SysMenu>();
            this.AspNetRoles = new HashSet<AspNetRole>();
        }
    
        public string Code { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public string Params { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public string Component { get; set; }
        public string Icon { get; set; }
        public string ControllerDesc { get; set; }
        public string ComponentDesc { get; set; }
        public string AreaDesc { get; set; }
    
        public virtual ICollection<SysMenu> SysMenus { get; set; }
        public virtual ICollection<AspNetRole> AspNetRoles { get; set; }
    }
}