namespace PHBC.DAO.Models
{
    using PHBC.DAO.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class ThongTinBaoModel
    {
        public ThongTinBaoModel()
        {
            if (!string.IsNullOrEmpty(this.MaBao))
            {
                this.MaBao = this.MaBao.ToUpper();
            }                       
        }
        public string Id { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]        
        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [StringLength(256, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Mã Tòa Soạn")]
        public string MaToaSoan { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Loại Ấn Phẩm")]
        public string LoaiAnPham { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Giá Bìa")]
        [RegularExpression(Enums.RegexDefine.Numeric, ErrorMessage = Enums.RegexMessage.Numeric)]
        [Range(1, 1000000, ErrorMessage = Enums.ErrorMessage.RangeMinMax)]
        public string GiaBia { get; set; }
        [Display(Name = "Số Trang")]
        [RegularExpression(Enums.RegexDefine.Interger, ErrorMessage = Enums.RegexMessage.Interger)]
        public string SoTrang { get; set; }
        [Display(Name = "Trọng Lượng")]
        [RegularExpression(Enums.RegexDefine.Numeric, ErrorMessage = Enums.RegexMessage.Numeric)]
        public string TrongLuong { get; set; }
        [Display(Name = "Kích Thước")]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string KichThuoc { get; set; }
        [Display(Name = "Giấy Phép Xuất Bản")]
        [StringLength(50, ErrorMessage = Enums.ErrorMessage.StringLengthMax)]
        public string GiayPhep { get; set; }
        [Display(Name = "Báo Nhập Khẩu")]
        public bool BaoNgoaiVan { get; set; }
        [Display(Name = "Báo Ngoài Danh Mục")]
        public bool BaoTrongMucLuc { get; set; }
        [Display(Name = "Báo Có Thuế")]
        public bool CoThue { get; set; }
        [Display(Name = "Mức Thuế")]
        [RegularExpression(Enums.RegexDefine.Numeric, ErrorMessage = Enums.RegexMessage.Numeric)]
        public string MucThue { get; set; }
        [Display(Name = "Báo Địa Phương")]
        public bool BaoTrungUongDiaPhuong { get; set; }        
        [Display(Name = "Báo Công Ích")]
        public bool BaoCongIchNgoaiCongIch { get; set; }
        [Display(Name = "Giá Vốn")]
        public Nullable<double> GiaVon { get; set; }
        [Display(Name = "Giá Bán")]
        public Nullable<double> GiaBan { get; set; }
        [Display(Name = "Trạng Thái")]
        public int Status { get; set; }
        [Display(Name = "Đơn vị")]
        public string UnitCode { get; set; }
        [Display(Name = "Ngày Tạo")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Người Tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày Sửa")]
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Người Sửa")]
        public string ModifyBy { get; set; }        
        public string TenLoaiAnPham { get; set; }
        [Display(Name = "Tên Tòa Soạn")]
        public string TenToaSoan { get; set; }
        [Display(Name = "Mã Báo Cha")]
        public string ParentId { get; set; }

        [Display(Name = "Tên Báo Cha")]
        public string ParentName { get; set; }
        [Display(Name = "Miêu tả báo")]
        [DataType(DataType.MultilineText)]
        public string GhiChu { get; set; }            
        public string BaoNgoaiVanDesc { get; set; }
        public string BaoTrongMucLucDesc { get; set; }
        public string CoThueDesc { get; set; }
        public string BaoCongIchNgoaiCongIchDesc { get; set; }
        public string BaoTrungUongDiaPhuongDesc { get; set; }
        public string userId { get; set; }        
        public BThongTinBao toBThongtinbao()
        {
            BThongTinBao result = new BThongTinBao();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.MaBao = this.MaBao;
            result.TenBao = this.TenBao;
            result.MaToaSoan = this.MaToaSoan;
            result.LoaiAnPham = this.LoaiAnPham;
            result.ParentId = this.ParentId;
            result.GiaBia = Convert.ToDouble(this.GiaBia);
            result.GhiChu = this.GhiChu;
            result.SoTrang = Convert.ToInt32(this.SoTrang);
            result.TrongLuong = Convert.ToDouble(this.TrongLuong);
            result.KichThuoc = this.KichThuoc;
            result.GiayPhep = this.GiayPhep;
            result.BaoNgoaiVan = this.BaoNgoaiVan;
            result.BaoCongIch = this.BaoCongIchNgoaiCongIch;
            result.BaoTrongMucLuc = this.BaoTrongMucLuc;
            result.BaoTrungUongDiaPhuong = this.BaoTrungUongDiaPhuong;
            result.CoThue = this.CoThue;
            result.MucThue = Convert.ToDouble(this.MucThue);
            result.CreateBy = this.userId;
            result.CreateDate = this.CreateDate;
            result.UnitCode = this.UnitCode;
            result.Status = (int)Enums.RecordStatusCode.active;

            return result;
        }
        public BThongTinBao toCreate()
        {
            BThongTinBao result = new BThongTinBao();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.MaBao = this.MaBao;
            result.TenBao = this.TenBao;
            result.MaToaSoan = this.MaToaSoan;
            result.LoaiAnPham = this.LoaiAnPham;
            result.ParentId = this.ParentId;
            result.GiaBia = Convert.ToDouble(this.GiaBia);
            result.GhiChu = this.GhiChu;
            result.SoTrang = Convert.ToInt32(this.SoTrang);
            result.TrongLuong = Convert.ToDouble(this.TrongLuong);
            result.KichThuoc = this.KichThuoc;
            result.GiayPhep = this.GiayPhep;
            result.BaoNgoaiVan = this.BaoNgoaiVan;
            result.BaoCongIch = this.BaoCongIchNgoaiCongIch;
            result.BaoTrongMucLuc = this.BaoTrongMucLuc;
            result.BaoTrungUongDiaPhuong = this.BaoTrungUongDiaPhuong;
            result.CoThue = this.CoThue;
            result.MucThue = Convert.ToDouble(this.MucThue);
            result.CreateBy = this.userId;
            result.CreateDate = this.CreateDate;
            result.UnitCode = this.UnitCode;
            result.Status = (int)Enums.RecordStatusCode.active;            

            return result;
        }
        public void changeEdit(BThongTinBao bThongTinBao)
        {
            bThongTinBao.MaBao = this.MaBao;
            bThongTinBao.TenBao = this.TenBao;
            bThongTinBao.MaToaSoan = this.MaToaSoan;
            bThongTinBao.LoaiAnPham = this.LoaiAnPham;
            bThongTinBao.ParentId = this.ParentId;
            bThongTinBao.GiaBia = Convert.ToDouble(this.GiaBia);
            bThongTinBao.GhiChu = this.GhiChu;
            bThongTinBao.SoTrang = Convert.ToInt32(this.SoTrang);
            bThongTinBao.TrongLuong = Convert.ToDouble(this.TrongLuong);
            bThongTinBao.KichThuoc = this.KichThuoc;
            bThongTinBao.GiayPhep = this.GiayPhep;
            bThongTinBao.BaoNgoaiVan = this.BaoNgoaiVan;
            bThongTinBao.BaoCongIch = this.BaoCongIchNgoaiCongIch;
            bThongTinBao.BaoTrongMucLuc = this.BaoTrongMucLuc;
            bThongTinBao.BaoTrungUongDiaPhuong = this.BaoTrungUongDiaPhuong;
            bThongTinBao.CoThue = this.CoThue;
            bThongTinBao.MucThue = Convert.ToDouble(this.MucThue);
            bThongTinBao.UnitCode = this.UnitCode;
            bThongTinBao.ModifyBy = this.userId;
            bThongTinBao.ModifyDate = DateTime.Now;
        }        
    }

    public class BaoDiemInModel
    {
        public BaoDiemInModel()
        {
            DiemIn_Current = new List<DMDiemIn>();
            DiemIn_NoMap = new List<DMDiemIn>();
        }

        [Key]
        public string Id { get; set; }

        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }

        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        public string lstDiemIn { get; set; }
        public List<DMDiemIn> DiemIn_Current { get; set; }
        public List<DMDiemIn> DiemIn_NoMap { get; set; }        
    }

    public class BaoKyXuatBanModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }

        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        public string KyConfig { get; set; }
        public DateTime ModifyDate { get; set; }
        public int LoaiKy { get; set; }
        
        //public virtual IEnumerable<BaoDiemInModel> BaoDiemIns { get; set; } 
    }

    public class KeHoachXuatBanModel
    {

        [Key]
        public string Id { get; set; }
        public string IdKeHoachXuatBan { get; set; }
        public string ThongTinBaoId { get; set; }

        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }

        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        public string KyConfig { get; set; }
        public int LoaiKy { get; set; }

        [Display(Name = "Năm")]
        public Nullable<int> Nam { get; set; }

        [Display(Name = "Quý")]
        public Nullable<int> Quy { get; set; }

        [Display(Name = "Số báo bắt đầu")]
        public Nullable<int> SoBatDau { get; set; }
        public List<BKeHoachXuatBanDetail> detailKHXB { get; set; }
        public List<BDieuChinhKHXBDetailModel> dieuchinhdetailKHXB { get; set; }

        public Dictionary<string, object> data = new Dictionary<string, object>();

        public List<dynamic> khxb = new List<dynamic>();
        
    }

    public class ThongTinBaoSearchModel
    {
        public ThongTinBaoSearchModel()
        {
        }
        public string Id { get; set; }
        public string Search { get; set; }
        [Display(Name = "Mã Báo")]
        public string MaBao { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        [Display(Name = "Mã Tòa Soạn")]
        public string MaToaSoan { get; set; }
        [Display(Name = "Loại Ấn Phẩm")]
        public string LoaiAnPham { get; set; }
        [Display(Name = "Tên Loại Ấn Phẩm")]
        public string TenLoaiAnPham { get; set; }

        [Display(Name = "Báo Ngoại Văn")]
        public int BaoNgoaiVan { get; set; }
        [Display(Name = "Báo Trong Danh Mục")]
        public Nullable<int> BaoTrongMucLuc { get; set; }
        [Display(Name = "Có Thuế")]
        public Nullable<int> CoThue { get; set; }
        [Display(Name = "Mức Thuế")]
        public Nullable<double> MucThue { get; set; }
        [Display(Name = "Báo TW/ĐP")]
        public Nullable<int> BaoTrungUongDiaPhuong { get; set; }

        [Display(Name = "Báo Công Ích")]
        public string BaoCongIchNgoaiCongIch { get; set; }
        [Display(Name = "Số Trang")]
        public string SoTrang { get; set; }
        [Display(Name = "Kích Thước")]
        public string KichThuoc { get; set; }
        [Display(Name = "Trọng Lượng")]
        public string TrongLuong { get; set; }
        [Display(Name = "Giấy Phép")]
        public string GiayPhep { get; set; }
        [Display(Name = "Giá Vốn")]
        public Nullable<double> GiaVon { get; set; }
        [Display(Name = "Giá Bía")]
        public string GiaBia { get; set; }
        [Display(Name = "Giá Bán")]
        public Nullable<double> GiaBan { get; set; }

        [Display(Name = "Miêu tả báo")]
        [DataType(DataType.MultilineText)]
        public string GhiChu { get; set; }        
    }

}