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
    
    public partial class BThongTinGiaBao
    {
        public string Id { get; set; }
        public string ThongTinBaoId { get; set; }
        public System.DateTime NgayHieuLuc { get; set; }
        public Nullable<System.DateTime> NgayHetHieuLuc { get; set; }
        public string ProvinceCode { get; set; }
        public string QuyetDinh { get; set; }
        public short ValueType { get; set; }
        public int Value { get; set; }
        public Nullable<int> Status { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
    
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual Province Province { get; set; }
    }
}
