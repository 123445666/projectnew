using PHBC.DAO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;

namespace PHBC.DAO.Models
{
    public class ThongTinGiaBaoModel
    {
        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        public string Id { get; set; }

        [Display(Name = "Mã Thông Tin Báo ")]
        public string ThongTinBaoId { get; set; }
        public string ThongTinGiaBaoMua { get; set; }
        public string ThongTinGiaBan { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Ngày Hiệu Lực")]
        [DataType(DataType.Date)]
        public string _NgayHieuLuc
        {
            set { this.NgayHieuLuc = Convert.ToDateTime(value); }
            get { return this.NgayHieuLuc == DateTime.MinValue ? "" : this.NgayHieuLuc.ToString(Enums.FormatType.ForMatDateVN); }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày Hết Hiệu Lực")]
        [DataType(DataType.Date)]
        public string _NgayHetHieuLuc
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    this.NgayHetHieuLuc = null;
                else
                    this.NgayHetHieuLuc = Convert.ToDateTime(value);
            }
            get
            {
                if (this.NgayHetHieuLuc.HasValue)
                    return this.NgayHetHieuLuc.Value.ToString(Enums.FormatType.ForMatDateVN);
                else
                    return "";
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày Hiệu Lực")]
        public DateTime NgayHieuLuc { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày Hết Hiệu Lực")]
        public Nullable<DateTime> NgayHetHieuLuc { set; get; }
        [Display(Name = "Tên tỉnh")]
        public string ProvinceCode { get; set; }

        [Display(Name = "Quyết Định")]
        public string QuyetDinh { get; set; }

        [Display(Name = "Loại Giá")]
        public short ValueType { get; set; }

        [Display(Name = "Loại Giá")]
        public string ValueTypeDesc { get; set; }

        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Giá")]
        [RegularExpression(Enums.RegexDefine.Interger,ErrorMessage= Enums.RegexMessage.Interger)]
        [Range(0, 1000000, ErrorMessage = Enums.ErrorMessage.RangeMinMax)]
        public string Value { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Ghi Chú")]
        public string GhiChu { get; set; }
        [Display(Name = "Báo Địa Phương")]
        public bool BaoTrungUongDiaPhuong { get; set; }
        [Display(Name = "Báo Ngoài Danh Mục")]
        public bool BaoTrongMucLuc { get; set; }
        public string userId { get; set; }
        [Display(Name = "Chọn file")]
        public string ChonFileUpload { get; set; }
        public BThongTinGiaBao toCreate()
        {
            BThongTinGiaBao result = new BThongTinGiaBao();
            result.Id = Guid.NewGuid().ToString();
            result.ThongTinBaoId = this.ThongTinBaoId;
            result.NgayHieuLuc = this.NgayHieuLuc;
            result.NgayHetHieuLuc = this.NgayHetHieuLuc;
            result.ProvinceCode = this.ProvinceCode;
            result.QuyetDinh = this.QuyetDinh;
            result.ValueType = this.ValueType;
            result.Value = Convert.ToInt32(this.Value);
            result.CreateBy = this.userId;
            result.CreateDate = DateTime.Now;
            result.Status = (int)Enums.RecordStatusCode.active;
            return result;
        }
        public void changeEdit(BThongTinGiaBao bThongTinGiaBao)
        {
            bThongTinGiaBao.NgayHieuLuc = this.NgayHieuLuc;
            bThongTinGiaBao.NgayHetHieuLuc = this.NgayHetHieuLuc;
            bThongTinGiaBao.QuyetDinh = this.QuyetDinh;
            bThongTinGiaBao.Value = Convert.ToInt32(this.Value);
            bThongTinGiaBao.ModifyBy = this.userId;
            bThongTinGiaBao.ModifyDate = DateTime.Now;
        }   
    }

    public class DanhSachGiaBaoModel
    {
        public DanhSachGiaBaoModel()
        {
            LstGiaMua = new List<ThongTinGiaBaoModel>();
            LstGiaBan = new List<ThongTinGiaBaoModel>();
        }
        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        public string ThongTinBaoId { get; set; }
        [Display(Name = "Báo Địa Phương")]
        public bool BaoTrungUongDiaPhuong { get; set; }
        [Display(Name = "Báo Ngoài Danh Mục")]
        public bool BaoTrongMucLuc { get; set; }
        public List<ThongTinGiaBaoModel> LstGiaMua { get; set; }
        public List<ThongTinGiaBaoModel> LstGiaBan { get; set; }    
    }

    public class ThongTinGiaBaoSearchModel
    {
        public ThongTinGiaBaoSearchModel()
        {
        }
        public string Search { get; set; }
        [Display(Name = "Mã Giá Báo")]
        public string Id { get; set; }
        [Display(Name = "Mã thông Tin Báo ")]
        public string ThongTinBaoId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Ngày Hiệu Lực")]
        [DataType(DataType.Date)]
        public string _NgayHieuLuc
        {
            set { this.NgayHieuLuc = Convert.ToDateTime(value); }
            get { return this.NgayHieuLuc == DateTime.MinValue ? "" : this.NgayHieuLuc.ToString(Enums.FormatType.ForMatDateVN); }
        }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày Hết Hiệu Lực")]
        [DataType(DataType.Date)]
        public string _NgayHetHieuLuc
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    this.NgayHetHieuLuc = null;
                else
                    this.NgayHetHieuLuc = Convert.ToDateTime(value);
            }
            get
            {
                if (this.NgayHetHieuLuc.HasValue)
                    return this.NgayHetHieuLuc.Value.ToString(Enums.FormatType.ForMatDateVN);
                else
                    return "";
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày Hiệu Lực")]
        public DateTime NgayHieuLuc { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Ngày Hết Hiệu Lực")]
        public Nullable<DateTime> NgayHetHieuLuc { set; get; }
        [Display(Name = "Tên Tỉnh")]
        public string ProvinceCode { get; set; }
        [Display(Name = "Quyết Định")]
        public string QuyetDinh { get; set; }
        [Display(Name = "Loại Giá")]
        public short ValueType { get; set; }
        [Display(Name = "Giá")]
        public string Value { get; set; }
        public Nullable<int> Status { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Ghi Chú")]
        public string GhiChu { get; set; }
        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
    }   
}