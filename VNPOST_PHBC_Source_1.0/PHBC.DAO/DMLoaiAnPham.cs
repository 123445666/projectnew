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
    
    public partial class DMLoaiAnPham
    {
        public DMLoaiAnPham()
        {
            this.BThongTinBaos = new HashSet<BThongTinBao>();
        }
    
        public string Id { get; set; }
        public string TenLoaiAnPham { get; set; }
        public bool CoKyXuatBan { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    
        public virtual ICollection<BThongTinBao> BThongTinBaos { get; set; }
    }
}