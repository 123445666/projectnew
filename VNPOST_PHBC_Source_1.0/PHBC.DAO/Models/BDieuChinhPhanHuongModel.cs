using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PHBC.DAO.Common;
using System.Linq;

namespace PHBC.DAO.Models
{
    /// <summary>
    /// created : 23/07/2015
    /// Author : vietvb
    /// </summary>
    #region PHNC
    public class BPhanHuongNhuCauUnitModel
    {
        public BPhanHuongNhuCauUnitModel()
        {
        }

        public BPhanHuongNhuCauUnitModel(BPhanHuongNhuCauUnit BPhanHuongNhuCauUnit)
        {
            this.Id = BPhanHuongNhuCauUnit.Id;
            this.UnitCode = BPhanHuongNhuCauUnit.UnitCode;
            if (BPhanHuongNhuCauUnit.Unit != null)
            {
                this.UnitName = BPhanHuongNhuCauUnit.Unit.UnitName;
            }
            this.DiemTiepNhanId = BPhanHuongNhuCauUnit.DiemTiepNhanId;
            if (BPhanHuongNhuCauUnit.BDiemTiepNhan != null)
            {
                this.TenDiemTiepNhan = BPhanHuongNhuCauUnit.BDiemTiepNhan.Name;
            }
            this.ThongTinBaoId = BPhanHuongNhuCauUnit.ThongTinBaoId;
            if (BPhanHuongNhuCauUnit.BThongTinBao != null)
            {
                this.TenBao = BPhanHuongNhuCauUnit.BThongTinBao.TenBao;
            }
            this.BDiemTiepNhan = BPhanHuongNhuCauUnit.BDiemTiepNhan;
            this.BThongTinBao = BPhanHuongNhuCauUnit.BThongTinBao;
            this.Unit = BPhanHuongNhuCauUnit.Unit;
            this.CreateBy = BPhanHuongNhuCauUnit.CreateBy;
            this.CreateDate = BPhanHuongNhuCauUnit.CreateDate;
            this.ModifyBy = BPhanHuongNhuCauUnit.ModifyBy;
            this.ModifyDate = BPhanHuongNhuCauUnit.ModifyDate;
        }

        public BPhanHuongNhuCauUnit toBPhanHuongNhuCauUnit()
        {
            BPhanHuongNhuCauUnit result = new BPhanHuongNhuCauUnit();
            result.Id = string.IsNullOrEmpty(this.Id) ? this.UnitCode + this.ThongTinBaoId : this.Id;
            result.UnitCode = this.UnitCode;
            result.DiemTiepNhanId = this.DiemTiepNhanId;
            result.ThongTinBaoId = this.ThongTinBaoId;
            result.BDiemTiepNhan = this.BDiemTiepNhan;
            result.BThongTinBao = this.BThongTinBao;
            result.Unit = this.Unit;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            return result;
        }

        public string Id { get; set; }
        [Display(Name = "Mã Bưu Cục")]
        public string UnitCode { get; set; }
        [Display(Name = "Tên Bưu Cục")]
        public string UnitName { get; set; }
        [Display(Name = "Mã Báo")]
        public string ThongTinBaoId { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Điểm Đặt Nhu Cầu")]
        public string DiemTiepNhanId { get; set; }
        [Display(Name = "Tên Điểm Đặt Nhu Cầu")]
        public string TenDiemTiepNhan { get; set; }
        [Display(Name = "Ngày tạo")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Người sửa")]
        public string ModifyBy { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual IEnumerable<Unit> Units { get; set; }
    }
    public class BPhanHuongNhuCauDistrictModel
    {
        public BPhanHuongNhuCauDistrictModel()
        {
        }

        public BPhanHuongNhuCauDistrictModel(BPhanHuongNhuCauDistrict BPhanHuongNhuCauDistrict)
        {
            this.Id = BPhanHuongNhuCauDistrict.Id;
            this.DistrictCode = BPhanHuongNhuCauDistrict.DistrictCode;
            if (BPhanHuongNhuCauDistrict.District != null)
            {
                this.DistrictName = BPhanHuongNhuCauDistrict.District.DistrictName;
            }
            this.DiemTiepNhanId = BPhanHuongNhuCauDistrict.DiemTiepNhanId;
            if (BPhanHuongNhuCauDistrict.BDiemTiepNhan != null)
            {
                this.TenDiemTiepNhan = BPhanHuongNhuCauDistrict.BDiemTiepNhan.Name;
            }
            this.ThongTinBaoId = BPhanHuongNhuCauDistrict.ThongTinBaoId;
            if (BPhanHuongNhuCauDistrict.BThongTinBao != null)
            {
                this.TenBao = BPhanHuongNhuCauDistrict.BThongTinBao.TenBao;
            }
            this.BDiemTiepNhan = BPhanHuongNhuCauDistrict.BDiemTiepNhan;
            this.BThongTinBao = BPhanHuongNhuCauDistrict.BThongTinBao;
            this.District = BPhanHuongNhuCauDistrict.District;
            this.CreateBy = BPhanHuongNhuCauDistrict.CreateBy;
            this.CreateDate = BPhanHuongNhuCauDistrict.CreateDate;
            this.ModifyBy = BPhanHuongNhuCauDistrict.ModifyBy;
            this.ModifyDate = BPhanHuongNhuCauDistrict.ModifyDate;
        }

        public BPhanHuongNhuCauDistrict toBPhanHuongNhuCauDistrict()
        {
            BPhanHuongNhuCauDistrict result = new BPhanHuongNhuCauDistrict();
            result.Id = string.IsNullOrEmpty(this.Id) ? this.DistrictCode + this.ThongTinBaoId : this.Id;
            result.DistrictCode = this.DistrictCode;
            result.DiemTiepNhanId = this.DiemTiepNhanId;
            result.ThongTinBaoId = this.ThongTinBaoId;
            result.BDiemTiepNhan = this.BDiemTiepNhan;
            result.BThongTinBao = this.BThongTinBao;
            result.District = this.District;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            return result;
        }

        public string Id { get; set; }
        [Display(Name = "Mã Huyện")]
        public string DistrictCode { get; set; }
        [Display(Name = "Tên Huyện")]
        public string DistrictName { get; set; }
        [Display(Name = "Mã Báo")]
        public string ThongTinBaoId { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Điểm Đặt Nhu Cầu")]
        public string DiemTiepNhanId { get; set; }
        [Display(Name = "Tên Điểm Đặt Nhu Cầu")]
        public string TenDiemTiepNhan { get; set; }
        [Display(Name = "Ngày tạo")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Người sửa")]
        public string ModifyBy { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual District District { get; set; }
        public virtual IEnumerable<District> Districts { get; set; }
    }
    #endregion
    #region DieuChinhPHNC
    public class BDieuChinhPhanHuongUnitModel
    {
        public BDieuChinhPhanHuongUnitModel()
        {
        }

        public BDieuChinhPhanHuongUnitModel(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit)
        {
            this.Id = BDieuChinhPhanHuongUnit.Id;
            this.UnitCode = BDieuChinhPhanHuongUnit.UnitCode;
            if (BDieuChinhPhanHuongUnit.Unit != null)
            {
                this.UnitName = BDieuChinhPhanHuongUnit.Unit.UnitName;
            }
            this.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
            if (BDieuChinhPhanHuongUnit.BDiemTiepNhan != null)
            {
                this.TenDiemTiepNhan = BDieuChinhPhanHuongUnit.BDiemTiepNhan.Name;
            }
            this.ThongTinBaoId = BDieuChinhPhanHuongUnit.ThongTinBaoId;
            if (BDieuChinhPhanHuongUnit.BThongTinBao != null)
            {
                this.TenBao = BDieuChinhPhanHuongUnit.BThongTinBao.TenBao;
            }
            this.BDiemTiepNhan = BDieuChinhPhanHuongUnit.BDiemTiepNhan;
            this.BThongTinBao = BDieuChinhPhanHuongUnit.BThongTinBao;
            this.BDieuChinhKHXBDetail = BDieuChinhPhanHuongUnit.BDieuChinhKHXBDetail;
            this.DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
            this.Unit = BDieuChinhPhanHuongUnit.Unit;
            this.CreateBy = BDieuChinhPhanHuongUnit.CreateBy;
            this.CreateDate = BDieuChinhPhanHuongUnit.CreateDate;
            this.ModifyBy = BDieuChinhPhanHuongUnit.ModifyBy;
            this.ModifyDate = BDieuChinhPhanHuongUnit.ModifyDate;
            this.Quy = BDieuChinhPhanHuongUnit.Quy;
            this.Nam = BDieuChinhPhanHuongUnit.Nam;
            this.SoBao = BDieuChinhPhanHuongUnit.SoBao;
        }

        public BDieuChinhPhanHuongUnit toBDieuChinhPhanHuongUnit()
        {
            BDieuChinhPhanHuongUnit result = new BDieuChinhPhanHuongUnit();
            result.Id = string.IsNullOrEmpty(this.Id) ? this.UnitCode + this.ThongTinBaoId : this.Id;
            result.UnitCode = this.UnitCode;
            result.DieuChinhKHXBDetailId = this.DieuChinhKHXBDetailId;
            result.Quy = this.Quy;
            result.Nam = this.Nam;
            result.SoBao = this.SoBao;
            result.DiemTiepNhanId = this.DiemTiepNhanId;
            result.ThongTinBaoId = this.ThongTinBaoId;
            result.Unit = this.Unit;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            result.BDiemTiepNhan = this.BDiemTiepNhan;
            result.BThongTinBao = this.BThongTinBao;
            return result;
        }

        public string Id { get; set; }
        [Display(Name = "Mã Bưu Cục")]
        public string UnitCode { get; set; }
        [Display(Name = "Tên Bưu Cục")]
        public string UnitName { get; set; }
        [Display(Name = "Mã Báo")]
        public string ThongTinBaoId { get; set; }
        [Display(Name = "Số Báo")]
        public string SoBao { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        [Display(Name = "Mã điều chỉnh")]
        public string DieuChinhKHXBDetailId { get; set; }
        [Display(Name = "Năm điều chỉnh")]
        public int Nam { get; set; }
        [Display(Name = "Quý điều chỉnh")]
        public int Quy { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Điểm Đặt Nhu Cầu")]
        public string DiemTiepNhanId { get; set; }
        [Display(Name = "Tên Điểm Đặt Nhu Cầu")]
        public string TenDiemTiepNhan { get; set; }
        [Display(Name = "Ngày tạo")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Người sửa")]
        public string ModifyBy { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual IEnumerable<Unit> Units { get; set; }
        public virtual BDieuChinhKHXBDetail BDieuChinhKHXBDetail { get; set; }
    }
    public class BDieuChinhPhanHuongDistrictModel
    {
        public BDieuChinhPhanHuongDistrictModel()
        {
        }

        public BDieuChinhPhanHuongDistrictModel(BDieuChinhPhanHuongDistrict BDieuChinhPhanHuongDistrict)
        {
            this.Id = BDieuChinhPhanHuongDistrict.Id;
            this.DistrictCode = BDieuChinhPhanHuongDistrict.DistrictCode;
            if (BDieuChinhPhanHuongDistrict.District != null)
            {
                this.DistrictName = BDieuChinhPhanHuongDistrict.District.DistrictName;
            }
            this.DiemTiepNhanId = BDieuChinhPhanHuongDistrict.DiemTiepNhanId;
            if (BDieuChinhPhanHuongDistrict.BDiemTiepNhan != null)
            {
                this.TenDiemTiepNhan = BDieuChinhPhanHuongDistrict.BDiemTiepNhan.Name;
            }
            this.ThongTinBaoId = BDieuChinhPhanHuongDistrict.ThongTinBaoId;
            if (BDieuChinhPhanHuongDistrict.BThongTinBao != null)
            {
                this.TenBao = BDieuChinhPhanHuongDistrict.BThongTinBao.TenBao;
            }
            this.BDiemTiepNhan = BDieuChinhPhanHuongDistrict.BDiemTiepNhan;
            this.BThongTinBao = BDieuChinhPhanHuongDistrict.BThongTinBao;
            this.District = BDieuChinhPhanHuongDistrict.District;
            this.CreateBy = BDieuChinhPhanHuongDistrict.CreateBy;
            this.CreateDate = BDieuChinhPhanHuongDistrict.CreateDate;
            this.ModifyBy = BDieuChinhPhanHuongDistrict.ModifyBy;
            this.ModifyDate = BDieuChinhPhanHuongDistrict.ModifyDate;
            this.Quy = BDieuChinhPhanHuongDistrict.Quy;
            this.Nam = BDieuChinhPhanHuongDistrict.Nam;
            this.SoBao = BDieuChinhPhanHuongDistrict.SoBao;
        }

        public BDieuChinhPhanHuongDistrict toBDieuChinhPhanHuongDistrict()
        {
            BDieuChinhPhanHuongDistrict result = new BDieuChinhPhanHuongDistrict();
            result.Id = string.IsNullOrEmpty(this.Id) ? this.DistrictCode + this.DieuChinhKHXBDetailId : this.Id;
            result.DieuChinhKHXBDetailId = this.DieuChinhKHXBDetailId;
            result.DistrictCode = this.DistrictCode;
            result.DiemTiepNhanId = this.DiemTiepNhanId;
            result.ThongTinBaoId = this.ThongTinBaoId;
            result.Quy = this.Quy;
            result.Nam = this.Nam;
            result.SoBao = this.SoBao;
            result.District = this.District;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            result.BDiemTiepNhan = this.BDiemTiepNhan;
            result.BThongTinBao = this.BThongTinBao;
            return result;
        }
        public string Id { get; set; }
        [Display(Name = "Mã Bưu Cục")]
        public string UnitCode { get; set; }
        [Display(Name = "Tên Bưu Cục")]
        public string UnitName { get; set; }
        [Display(Name = "Mã Báo")]
        public string ThongTinBaoId { get; set; }
        [Display(Name = "Số Báo")]
        public string SoBao { get; set; }
        [Display(Name = "Tên Báo")]
        public string TenBao { get; set; }
        [Display(Name = "Mã điều chỉnh")]
        public string DieuChinhKHXBDetailId { get; set; }
        [Display(Name = "Năm điều chỉnh")]
        public int Nam { get; set; }
        [Display(Name = "Quý điều chỉnh")]
        public int Quy { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Điểm Đặt Nhu Cầu")]
        public string DiemTiepNhanId { get; set; }
        [Display(Name = "Tên Điểm Đặt Nhu Cầu")]
        public string TenDiemTiepNhan { get; set; }
        [Display(Name = "Ngày tạo")]
        public System.DateTime CreateDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Người sửa")]
        public string ModifyBy { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        [Display(Name = "Mã Quận")]
        public string DistrictCode { get; set; }
        [Display(Name = "Tên Quận")]
        public string DistrictName { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual District District { get; set; }
        public virtual IEnumerable<Unit> Units { get; set; }
        public virtual BDieuChinhKHXBDetail BDieuChinhKHXBDetail { get; set; }
    }
    public class BDieuChinhPhanHuongUnitSearchModel
    {
        public BDieuChinhPhanHuongUnitSearchModel()
        {
        }
        public string UnitCode { get; set; }
        public string ThongTinBaoId { get; set; }
        public string DieuChinhKHXBDetailId { get; set; }
        public string DiemTiepNhanId { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
    #endregion
    public class UnitModelDieuChinh
    {
        public string ThongTinBaoId;
        public string DieuChinhKHXBDetailId;
        private List<v_Unit> lstUnit;
        private List<Province> lstProvince;
        public List<TinhThanh> lstTinhThanh { get; set; }
        public List<QuanHuyen> lstQuanHuyen { get; set; }
        public List<BDieuChinhPhanHuongUnit> lstPHNC { get; set; }
        public List<BDiemTiepNhan> lstDTN { get; set; }
        public UnitModelDieuChinh()
        {

        }
        public UnitModelDieuChinh(List<Province> _lstProvince)
        {
            lstProvince = _lstProvince;
        }
        public UnitModelDieuChinh(List<v_Unit> _lstUnit, List<Province> _lstProvince, List<BDiemTiepNhan> _lstBDiemTiepNhan = null)
        {
            lstUnit = _lstUnit;
            lstProvince = _lstProvince;
            lstDTN = _lstBDiemTiepNhan;
            this.build();
        }
        public List<v_Unit> LstUnit
        {
            get { return this.lstUnit; }
        }

        public void setListUnitMapNew(List<v_Unit> _lstUnit, string DiemTiepNhanId)
        {
            this.AddMapUnit(_lstUnit, DiemTiepNhanId);
        }
        public void setListDistrictMapNew(List<QuanHuyen> _lstDistrict, string DiemTiepNhanId)
        {
            this.AddMapDistrict(_lstDistrict, DiemTiepNhanId);
        }

        public void UpdateListUnitMap(List<v_Unit> _lstUnit, string DiemTiepNhanId)
        {
            this.UpdateMapUnit(_lstUnit, DiemTiepNhanId);
        }
        public void UpdateListDistrictMap(List<QuanHuyen> _lstQuanHuyen, string DiemTiepNhanId)
        {
            this.UpdateMapDistrict(_lstQuanHuyen, DiemTiepNhanId);
        }

        public void setListDieuChinhPHNC(List<BDieuChinhPhanHuongUnit> _lstPHNC, List<BDieuChinhPhanHuongDistrict> _lstPHNCDistrict)
        {
            List<QuanHuyen> _lstDistrict = (from sc in lstQuanHuyen
                                            join soc in _lstPHNCDistrict
                                            on sc.DistrictCode equals soc.DistrictCode
                                            select new QuanHuyen(sc, soc.DiemTiepNhanId, soc.BDiemTiepNhan.Name)
                         ).ToList();
            this.AddMapDistrict(_lstDistrict);
            //List<v_Unit> _lstUnit = lstUnit.Where(a => _lstPHNC.Any(p => p.UnitCode.Equals(a.UnitCode))).ToList();
            List<v_Unit> _lstUnit = (from sc in lstUnit
                                     join soc in _lstPHNC
                                     on sc.UnitCode equals soc.UnitCode
                                     select new v_Unit(sc, soc.DiemTiepNhanId, soc.BDiemTiepNhan.Name)
                         ).ToList();

            this.AddMapUnit(_lstUnit);            

            lstPHNC = _lstPHNC;
        }
        public void setListPHNC(List<BPhanHuongNhuCauUnit> _lstPHNC, List<BPhanHuongNhuCauDistrict> _lstPHNCDistrict)
        {
            List<QuanHuyen> _lstDistrict = (from sc in lstQuanHuyen
                                            join soc in _lstPHNCDistrict
                                            on sc.DistrictCode equals soc.DistrictCode
                                            select new QuanHuyen(sc, soc.DiemTiepNhanId, soc.BDiemTiepNhan.Name)
                         ).ToList();
            this.AddMapDistrict(_lstDistrict);
            //List<v_Unit> _lstUnit = lstUnit.Where(a => _lstPHNC.Any(p => p.UnitCode.Equals(a.UnitCode))).ToList();
            List<v_Unit> _lstUnit = (from sc in lstUnit
                                     join soc in _lstPHNC
                                     on sc.UnitCode equals soc.UnitCode
                                     select new v_Unit(sc, soc.DiemTiepNhanId, soc.BDiemTiepNhan.Name)
                         ).ToList();

            this.AddMapUnit(_lstUnit);

        }

        public List<v_Unit> getUnitNotMap(string[] lstids, string typeid)
        {

            List<v_Unit> result = new List<v_Unit>();
            if (typeid.Trim() == "1")
            {
                result = lstTinhThanh.Where(a => lstids.Any(p => p.Equals(a.ProvinceCode))).SelectMany(a => a.lstUnitNotMap).ToList();
            }
            else if (typeid.Trim() == "2")
            {
                result = lstQuanHuyen.Where(a => lstids.Any(p => p.Equals(a.DistrictCode))).SelectMany(a => a.lstUnitNotMap).OrderBy(t => t.DistrictName).ToList();
            }
            else if (typeid.Trim() == "3")
            {
                result = lstUnit.Where(a => lstids.Any(p => p.Equals(a.UnitCode))).ToList();
            }
            return result;
        }

        public List<v_Unit> getUnitMap(string[] lstids, string typeid)
        {

            List<v_Unit> result = new List<v_Unit>();
            if (typeid.Trim() == "1")
            {
                result = lstTinhThanh.Where(a => lstids.Any(p => p.Equals(a.ProvinceCode))).SelectMany(a => a.lstUnitMap).ToList();
            }
            else if (typeid.Trim() == "2")
            {
                result = lstQuanHuyen.Where(a => lstids.Any(p => p.Equals(a.DistrictCode))).SelectMany(a => a.lstUnit).OrderBy(t => t.DistrictName).ToList();
            }
            else if (typeid.Trim() == "3")
            {
                result = lstUnit.Where(a => lstids.Any(p => p.Equals(a.UnitCode))).ToList();
            }
            return result;
        }

        public List<QuanHuyen> getDistrictNotMap(string[] lstids, string typeid)
        {

            List<QuanHuyen> result = new List<QuanHuyen>();
            //result = lstQuanHuyen.Where(t => lstids.Contains(t.ProvinceCode) && t.lstUnitNotMap.Count > 0).OrderBy(t => t.DistrictName).OrderBy(t => t.ProvinceName).ToList();
            if (lstids.Count() > 0 && typeid == "1")
            {
                result = lstQuanHuyen.Where(t => lstids.Contains(t.ProvinceCode) && string.IsNullOrWhiteSpace(t.DiemTiepNhanId)).ToList();
            }
            else if (lstids.Count() > 0 && typeid == "2")
            {
                result = lstQuanHuyen.Where(t => lstids.Contains(t.DistrictCode) && string.IsNullOrWhiteSpace(t.DiemTiepNhanId)).ToList();
            }
            else
            {
                result = lstQuanHuyen.Where(t =>  string.IsNullOrWhiteSpace(t.DiemTiepNhanId)).ToList();
            }
            return result;
        }

        public List<QuanHuyen> getDistrictMap(string[] lstids,string typeid)
        {

            List<QuanHuyen> result = new List<QuanHuyen>();
            //result = lstQuanHuyen.Where(t => lstids.Contains(t.ProvinceCode) && t.lstUnitMap.Count > 0).OrderBy(t => t.DistrictName).OrderBy(t => t.ProvinceName).ToList();
            if (lstids.Count() > 0 && typeid == "1")
            {
                result = lstQuanHuyen.Where(t => lstids.Contains(t.ProvinceCode) && !string.IsNullOrWhiteSpace(t.DiemTiepNhanId)).ToList();
            }
            else if (lstids.Count() > 0 && typeid == "2")
            {
                result = lstQuanHuyen.Where(t => lstids.Contains(t.DistrictCode) && !string.IsNullOrWhiteSpace(t.DiemTiepNhanId)).ToList();
            }
            else
            {
                result = lstQuanHuyen.Where(t => !string.IsNullOrWhiteSpace(t.DiemTiepNhanId)).ToList();
            }
            return result;
        }
        public List<QuanHuyen> getDistrictMapByProvinceCode(string provincecode)
        {
            //return lstQuanHuyen.Where(t => t.ProvinceCode.CompareTo(provincecode) == 0 && t.lstUnitMap.Count <= t.lstUnit.Count && t.lstUnitMap.Count > 0).OrderBy(x => x.ProvinceName).OrderBy(x => x.DistrictName).ToList();
            return lstQuanHuyen.Where(t => !string.IsNullOrWhiteSpace(t.DiemTiepNhanId) && t.ProvinceCode.Equals(provincecode)).ToList();
        }
        public List<QuanHuyen> getDistrictNotMapByProvinceCode(string provincecode)
        {
            //return lstQuanHuyen.Where(t => t.ProvinceCode.CompareTo(provincecode) == 0 && t.lstUnitNotMap.Count > 0).OrderBy(x => x.ProvinceName).OrderBy(x => x.DistrictName).ToList();
            return lstQuanHuyen.Where(t => string.IsNullOrWhiteSpace(t.DiemTiepNhanId) && t.ProvinceCode.Equals(provincecode)).ToList();
        }
        public List<QuanHuyen> getDistrictMapByDistrictCode(string districtcode)
        {
            //return lstQuanHuyen.Where(t => t.DistrictCode.CompareTo(districtcode) == 0 && t.lstUnitMap.Count <= t.lstUnit.Count && t.lstUnitMap.Count > 0).ToList();
            return lstQuanHuyen.Where(t => !string.IsNullOrWhiteSpace(t.DiemTiepNhanId) && t.DistrictCode.Equals(districtcode)).ToList();
        }
        public List<TinhThanh> getAllProvinceNotMap(string provincecode = null)
        {
            List<TinhThanh> result = new List<TinhThanh>();
            if (string.IsNullOrWhiteSpace(provincecode))
            {
                result = lstTinhThanh.Where(t => t.lstQuanHuyen.Where(x => string.IsNullOrWhiteSpace(x.DiemTiepNhanId)).Count() > 0).OrderBy(t => t.ProvinceCode).ToList();
            }
            else
            {
                result = lstTinhThanh.Where(t => t.ProvinceCode.CompareTo(provincecode) == 0 && t.lstUnitMap.Count < t.lstUnit.Count).OrderBy(t => t.ProvinceName).ToList();
            }
            return result;
        }
        public List<TinhThanh> getAllProvince()
        {
            return lstTinhThanh.OrderBy(t => t.ProvinceName).OrderBy(t => t.CountDistrictMap2 == t.lstQuanHuyen.Count ? 0 : 1).OrderBy(t => t.CountDistrictMap2 > 0 ? 0 : 1).ToList();
        }
        public List<v_Unit> getUnitNotMap(string[] arProvince, string[] arDistrict, string[] arUnit)
        {

            List<v_Unit> result = new List<v_Unit>();
            if (arProvince == null || arProvince.Length == 0)
                return result;
            if (arDistrict == null || arDistrict.Length == 0)
                return lstTinhThanh.Where(a => arProvince.Any(p => p.Equals(a.ProvinceCode))).SelectMany(a => a.lstUnitNotMap).ToList();
            else if (arUnit == null || arUnit.Length == 0)
            {
                var _lstQH = lstTinhThanh.Where(a => arProvince.Any(p => p.Equals(a.ProvinceCode))).SelectMany(a => a.lstQuanHuyen).ToList();
                return _lstQH.Where(a => arDistrict.Any(p => p.Equals(a.DistrictCode))).SelectMany(a => a.lstUnitNotMap).ToList();
            }
            else
            {
                var lstQH = lstTinhThanh.Where(a => arProvince.Any(p => p.Equals(a.ProvinceCode))).SelectMany(a => a.lstQuanHuyen).ToList();
                var lstU = lstQH.Where(a => arDistrict.Any(p => p.Equals(a.DistrictCode))).SelectMany(a => a.lstUnitNotMap).ToList();
                return lstU.Where(a => arUnit.Any(p => p.Equals(a.UnitCode))).ToList();
            }
        }
        private void build()
        {
            lstTinhThanh = new List<TinhThanh>();
            lstQuanHuyen = new List<QuanHuyen>();
            string cProvinceCode = "";
            string cDistrictCode = "";
            TinhThanh objTinhThanh = new TinhThanh();
            QuanHuyen objQuanHuyen = new QuanHuyen();
            foreach (v_Unit item in this.lstUnit)
            {
                if (!item.ProvinceCode.Equals(cProvinceCode))
                {
                    cProvinceCode = item.ProvinceCode;
                    objTinhThanh = new TinhThanh();
                    objTinhThanh.ProvinceCode = item.ProvinceCode;
                    objTinhThanh.ProvinceName = lstProvince.Find(p => p.ProvinceCode.Equals(objTinhThanh.ProvinceCode)).ProvinceName;
                    lstTinhThanh.Add(objTinhThanh);
                }
                if (!item.DistrictCode.Equals(cDistrictCode))
                {
                    cDistrictCode = item.DistrictCode;
                    objQuanHuyen = new QuanHuyen();
                    objQuanHuyen.DistrictCode = item.DistrictCode;
                    objQuanHuyen.DistrictName = item.DistrictName;
                    objQuanHuyen.ProvinceCode = item.ProvinceCode;
                    objQuanHuyen.ProvinceName = objTinhThanh.ProvinceName;
                    lstQuanHuyen.Add(objQuanHuyen);
                    objTinhThanh.lstQuanHuyen.Add(objQuanHuyen);
                }
                objTinhThanh.lstUnit.Add(item);
                objTinhThanh.lstUnitNotMap.Add(item);

                objQuanHuyen.lstUnit.Add(item);
                objQuanHuyen.lstUnitNotMap.Add(item);
            }
        }
        private void AddMapUnit(List<v_Unit> _lstUnit, string DiemTiepNhanId = null)
        {
            string cProvinceCode = "";
            string cDistrictCode = "";
            TinhThanh objTinhThanh = new TinhThanh();
            QuanHuyen objQuanHuyen = new QuanHuyen();

            BDiemTiepNhan objDTN = new BDiemTiepNhan();
            objDTN = lstDTN.FirstOrDefault(t => t.Id.Equals(DiemTiepNhanId));

            foreach (v_Unit item in _lstUnit)
            {
                if (!String.IsNullOrWhiteSpace(DiemTiepNhanId))
                {
                    item.DiemTiepNhanId = DiemTiepNhanId;
                    item.DiemTiepNhanName = objDTN.Name;                    
                }
                
                if (!item.ProvinceCode.Equals(cProvinceCode))
                {
                    cProvinceCode = item.ProvinceCode;
                    objTinhThanh = this.lstTinhThanh.Find(a => a.ProvinceCode.Equals(cProvinceCode));
                }
                if (!item.DistrictCode.Equals(cDistrictCode))
                {
                    cDistrictCode = item.DistrictCode;
                    objQuanHuyen = objTinhThanh.lstQuanHuyen.Find(a => a.DistrictCode.Equals(cDistrictCode));
                }
                v_Unit objLstUnit = objQuanHuyen.lstUnit.Find(a => a.UnitCode == item.UnitCode);
                objLstUnit.DiemTiepNhanId = item.DiemTiepNhanId;
                objLstUnit.DiemTiepNhanName = item.DiemTiepNhanName;

                objTinhThanh.lstUnitMap.Add(item);
                int index = objTinhThanh.lstUnitNotMap.FindIndex(a => a.UnitCode == item.UnitCode);
                objTinhThanh.lstUnitNotMap.RemoveAt(index);

                objQuanHuyen.lstUnitMap.Add(item);
                index = objQuanHuyen.lstUnitNotMap.FindIndex(a => a.UnitCode == item.UnitCode);
                objQuanHuyen.lstUnitNotMap.RemoveAt(index);                

            }
        }
        private void AddMapDistrict(List<QuanHuyen> _lstDistrict, string DiemTiepNhanId = null)
        {
            string cProvinceCode = "";
            TinhThanh objTinhThanh = new TinhThanh();
            QuanHuyen objQuanHuyen = new QuanHuyen();

            BDiemTiepNhan objDTN = new BDiemTiepNhan();
            objDTN = lstDTN.FirstOrDefault(t => t.Id.Equals(DiemTiepNhanId));

            foreach (QuanHuyen item in _lstDistrict)
            {
                if (!String.IsNullOrWhiteSpace(DiemTiepNhanId))
                {
                    item.DiemTiepNhanId = DiemTiepNhanId;
                    item.DiemTiepNhanName = objDTN.Name;
                }
                if (!item.ProvinceCode.Equals(cProvinceCode))
                {
                    cProvinceCode = item.ProvinceCode;
                    objTinhThanh = this.lstTinhThanh.Find(a => a.ProvinceCode.Equals(cProvinceCode));
                }
                foreach (v_Unit unit in item.lstUnit)
                {
                    unit.DiemTiepNhanId = item.DiemTiepNhanId;
                    unit.DiemTiepNhanName = item.DiemTiepNhanName;
                }
                objQuanHuyen = objTinhThanh.lstQuanHuyen.Find(a => a.DistrictCode.Equals(item.DistrictCode));
                objQuanHuyen.DiemTiepNhanId = item.DiemTiepNhanId;
                objQuanHuyen.DiemTiepNhanName = item.DiemTiepNhanName;
                
            }
        }
        private void UpdateMapUnit(List<v_Unit> _lstUnit, string DiemTiepNhanId)
        {
            string cProvinceCode = "";
            string cDistrictCode = "";
            TinhThanh objTinhThanh = new TinhThanh();
            QuanHuyen objQuanHuyen = new QuanHuyen();
            //objNew = item;
            string tenDiemTiepNhan = lstDTN.FirstOrDefault(t => t.Id.Equals(DiemTiepNhanId)).Name;

            //_lstUnit.ForEach(a => { a.DiemTiepNhanId = DiemTiepNhanId; a.DiemTiepNhanName = objDTN.Name; });
            foreach (v_Unit item in _lstUnit)
            {
                if (!item.ProvinceCode.Equals(cProvinceCode))
                {
                    cProvinceCode = item.ProvinceCode;
                    objTinhThanh = this.lstTinhThanh.Find(a => a.ProvinceCode.Equals(cProvinceCode));
                }
                if (!item.DistrictCode.Equals(cDistrictCode))
                {
                    cDistrictCode = item.DistrictCode;
                    objQuanHuyen = objTinhThanh.lstQuanHuyen.Find(a => a.DistrictCode.Equals(cDistrictCode));
                }
                v_Unit setUnit = objTinhThanh.lstUnitMap.Find(a => a.UnitCode == item.UnitCode);
                if (setUnit != null) {
                    item.DiemTiepNhanId = DiemTiepNhanId;
                    item.DiemTiepNhanName = tenDiemTiepNhan;
                    setUnit.DiemTiepNhanId = DiemTiepNhanId;
                    setUnit.DiemTiepNhanName = tenDiemTiepNhan;
                    setUnit = objQuanHuyen.lstUnitMap.Find(a => a.UnitCode == item.UnitCode);
                    setUnit.DiemTiepNhanId = DiemTiepNhanId;
                    setUnit.DiemTiepNhanName = tenDiemTiepNhan;
                    //_lstUnit.Remove(item);
                }
                else
                {
                    item.DiemTiepNhanId = DiemTiepNhanId;
                    item.DiemTiepNhanName = tenDiemTiepNhan;
                    objTinhThanh.lstUnitMap.Add(item);
                    objQuanHuyen.lstUnitMap.Add(item);
                    objTinhThanh.lstUnitNotMap.RemoveAt(objTinhThanh.lstUnitNotMap.FindIndex(a => a.UnitCode == item.UnitCode));
                    objQuanHuyen.lstUnitNotMap.RemoveAt(objQuanHuyen.lstUnitNotMap.FindIndex(a => a.UnitCode == item.UnitCode));
                }
                //objTinhThanh.lstUnitMap.Remove(item);
                //objQuanHuyen.lstUnitMap.Add(item);
            }
            if (_lstUnit.Count > 0)
            { 
                //AddMapUnit(_lstUnit, DiemTiepNhanId);
            }
        }
        private void UpdateMapDistrict(List<QuanHuyen> _lstQuanHuyen, string DiemTiepNhanId)
        {
            string cProvinceCode = "";
            string cDistrictCode = "";
            TinhThanh objTinhThanh = new TinhThanh();
            QuanHuyen objQuanHuyen = new QuanHuyen();
            //objNew = item;
            string tenDiemTiepNhan = lstDTN.FirstOrDefault(t => t.Id.Equals(DiemTiepNhanId)).Name;

            //_lstUnit.ForEach(a => { a.DiemTiepNhanId = DiemTiepNhanId; a.DiemTiepNhanName = objDTN.Name; });
            foreach (QuanHuyen item in _lstQuanHuyen)
            {
                if (!item.ProvinceCode.Equals(cProvinceCode))
                {
                    cProvinceCode = item.ProvinceCode;
                    objTinhThanh = this.lstTinhThanh.Find(a => a.ProvinceCode.Equals(cProvinceCode));
                }
                foreach (v_Unit unit in item.lstUnit)
                {
                    unit.DiemTiepNhanId = item.DiemTiepNhanId;
                    unit.DiemTiepNhanName = item.DiemTiepNhanName;
                }
                cDistrictCode = item.DistrictCode;
                objQuanHuyen = objTinhThanh.lstQuanHuyen.Find(a => a.DistrictCode.Equals(item.DistrictCode));
                objQuanHuyen.DiemTiepNhanName = tenDiemTiepNhan;
                objQuanHuyen.DiemTiepNhanId = DiemTiepNhanId;

            }
        }
    }
}
