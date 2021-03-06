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
    
    public partial class Unit
    {
        public Unit()
        {
            this.POS = new HashSet<POS>();
            this.Unit1 = new HashSet<Unit>();
            this.UserInfoes = new HashSet<UserInfo>();
            this.BDiemTiepNhans = new HashSet<BDiemTiepNhan>();
            this.BPhanHuongNhuCaus = new HashSet<BPhanHuongNhuCau>();
            this.BThongTinBaos = new HashSet<BThongTinBao>();
            this.BDieuChinhPhanHuongUnits = new HashSet<BDieuChinhPhanHuongUnit>();
            this.BPhanHuongNhuCauUnits = new HashSet<BPhanHuongNhuCauUnit>();
        }
    
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string ParentUnitCode { get; set; }
        public string CommuneCode { get; set; }
        public string UnitTypeCode { get; set; }
    
        public virtual Commune Commune { get; set; }
        public virtual ICollection<POS> POS { get; set; }
        public virtual ICollection<Unit> Unit1 { get; set; }
        public virtual Unit Unit2 { get; set; }
        public virtual UnitType UnitType { get; set; }
        public virtual ICollection<UserInfo> UserInfoes { get; set; }
        public virtual ICollection<BDiemTiepNhan> BDiemTiepNhans { get; set; }
        public virtual ICollection<BPhanHuongNhuCau> BPhanHuongNhuCaus { get; set; }
        public virtual ICollection<BThongTinBao> BThongTinBaos { get; set; }
        public virtual ICollection<BDieuChinhPhanHuongUnit> BDieuChinhPhanHuongUnits { get; set; }
        public virtual ICollection<BPhanHuongNhuCauUnit> BPhanHuongNhuCauUnits { get; set; }
    }
}
