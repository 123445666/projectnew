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
    
    public partial class BDieuChinhPhanHuongUnit
    {
        public string Id { get; set; }
        public string ThongTinBaoId { get; set; }
        public string SoBao { get; set; }
        public string DieuChinhKHXBDetailId { get; set; }
        public string DiemTiepNhanId { get; set; }
        public string UnitCode { get; set; }
        public int Nam { get; set; }
        public int Quy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan1 { get; set; }
        public virtual BDieuChinhKHXBDetail BDieuChinhKHXBDetail { get; set; }
    }
}
