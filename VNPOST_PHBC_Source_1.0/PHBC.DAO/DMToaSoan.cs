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
    
    public partial class DMToaSoan
    {
        public DMToaSoan()
        {
            this.BThongTinBaos = new HashSet<BThongTinBao>();
        }
    
        public string Id { get; set; }
        public string MaToaSoan { get; set; }
        public string TenToaSoan { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public string MaSoThue { get; set; }
        public string TaiKhoan { get; set; }
        public string NganHang { get; set; }
        public string TongBienTap { get; set; }
        public string NguoiDaiDien { get; set; }
        public string CoQuanChuQuan { get; set; }
        public int Status { get; set; }
        public Nullable<int> KieuToaSoan { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    
        public virtual ICollection<BThongTinBao> BThongTinBaos { get; set; }
    }
}
