using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PHBC.DAO.Common;

namespace PHBC.DAO.Models
{
    public class DMToaSoanModel
    {
        public DMToaSoanModel()
        {
        }

        public DMToaSoanModel(DMToaSoan DMToaSoan)
        {
            this.Id = DMToaSoan.Id;
            this.MaToaSoan = DMToaSoan.MaToaSoan;
            this.TenToaSoan = DMToaSoan.TenToaSoan;
            this.DiaChi = DMToaSoan.DiaChi;
            this.SoDienThoai = DMToaSoan.SoDienThoai;
            this.Email = DMToaSoan.Email;
            this.Web = DMToaSoan.Web;
            this.MaSoThue = DMToaSoan.MaSoThue;
            this.TaiKhoan = DMToaSoan.TaiKhoan;
            this.TongBienTap = DMToaSoan.TongBienTap;
            this.NguoiDaiDien = DMToaSoan.NguoiDaiDien;
            this.CoQuanChuQuan = DMToaSoan.CoQuanChuQuan;
            this.Status = DMToaSoan.Status;
            this.KieuToaSoan = DMToaSoan.KieuToaSoan;
            this.NganHang = DMToaSoan.NganHang;
            if (this.KieuToaSoan == (int)Enums.KieuToaSoan.DiaPhuong)
            {
                this.NameKieuToaSoan = "Tòa soạn Địa Phương";
            }
            else if (this.KieuToaSoan == (int)Enums.KieuToaSoan.TrungUong)
            {
                this.NameKieuToaSoan = "Tòa soạn Trung Ương";
            }
            else this.NameKieuToaSoan = "";
            this.CreateBy = DMToaSoan.CreateBy;
            this.CreateDate = DMToaSoan.CreateDate;
            this.ModifyBy = DMToaSoan.ModifyBy;
            this.ModifyDate = DMToaSoan.ModifyDate;
        }

        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Mã Tòa Soạn")]
        public string MaToaSoan { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Tên tòa soạn")]
        public string TenToaSoan { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(15, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [RegularExpression(@"^[0-9 ]*$", ErrorMessage = Enums.ErrorMessage.WrongFormat)]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }
        [StringLength(256)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = Enums.ErrorMessage.WrongFormat)]
        public string Email { get; set; }
        [StringLength(256)]
        [Display(Name = "Web")]
        public string Web { get; set; }
        [StringLength(256)]
        [RegularExpression(@"^[0-9 -]*$", ErrorMessage = Enums.ErrorMessage.WrongFormatMST)]
        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }
        [StringLength(256)]
        [RegularExpression(@"^[0-9 -]*$", ErrorMessage = Enums.ErrorMessage.WrongFormatMST)]
        [Display(Name = "Tài khoản Ngân Hàng")]
        public string TaiKhoan { get; set; }
        [StringLength(256)]
        [Display(Name = "Tổng Biên Tập")]
        public string TongBienTap { get; set; }
        [StringLength(256)]
        [Display(Name = "Người Đại Diện")]
        public string NguoiDaiDien { get; set; }
        [StringLength(256)]
        [Display(Name = "Cơ quan chủ quản")]
        public string CoQuanChuQuan { get; set; }
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
        [Display(Name = "Kiểu Tòa Soạn")]
        public Nullable<int> KieuToaSoan { get; set; }
        [Display(Name = "Kiểu Tòa Soạn")]
        public string NameKieuToaSoan { get; set; }
        [StringLength(256)]
        [Display(Name = "Ngân Hàng")]
        public string NganHang { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public DMToaSoan toDMToaSoan()
        {
            DMToaSoan result = new DMToaSoan();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.MaToaSoan = this.MaToaSoan;
            result.TenToaSoan = this.TenToaSoan;
            result.DiaChi = this.DiaChi;
            result.SoDienThoai = this.SoDienThoai;
            result.Email = this.Email;
            result.Web = this.Web;
            result.MaSoThue = this.MaSoThue;
            result.TaiKhoan = this.TaiKhoan;
            result.TongBienTap = this.TongBienTap;
            result.NguoiDaiDien = this.NguoiDaiDien;
            result.CoQuanChuQuan = this.CoQuanChuQuan;
            result.Status = this.Status;
            result.KieuToaSoan = this.KieuToaSoan;
            result.NganHang = this.NganHang;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            return result;
        }
    }

    public class DMToaSoanSearchModel
    {
        public DMToaSoanSearchModel()
        {
        }  
        [StringLength(50)]
        [Display(Name = "Mã Tòa Soạn")]
        public string MaToaSoan { get; set; }     
        [StringLength(256)]
        [Display(Name = "Tên tòa soạn")]
        public string TenToaSoan { get; set; }  
        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }  
    }
}
