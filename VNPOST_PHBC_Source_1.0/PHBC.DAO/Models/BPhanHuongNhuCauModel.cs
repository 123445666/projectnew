using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PHBC.DAO.Common;
using System.Linq;

namespace PHBC.DAO.Models
{
    public class BPhanHuongNhuCauModel
    {
        public BPhanHuongNhuCauModel()
        {
        }

        public BPhanHuongNhuCauModel(BPhanHuongNhuCau BPhanHuongNhuCau)
        {
            this.Id = BPhanHuongNhuCau.Id;
            this.UnitCode = BPhanHuongNhuCau.UnitCode;
            if (BPhanHuongNhuCau.Unit != null)
            {
                this.UnitName = BPhanHuongNhuCau.Unit.UnitName;
            }
            this.DiemTiepNhanId = BPhanHuongNhuCau.DiemTiepNhanId;
            if (BPhanHuongNhuCau.BDiemTiepNhan != null)
            {
                this.TenDiemTiepNhan = BPhanHuongNhuCau.BDiemTiepNhan.Name;
            }
            this.ThongTinBaoId = BPhanHuongNhuCau.ThongTinBaoId;
            if (BPhanHuongNhuCau.BThongTinBao != null)
            {
                this.TenBao = BPhanHuongNhuCau.BThongTinBao.TenBao;
            }
            this.BDiemTiepNhan = BPhanHuongNhuCau.BDiemTiepNhan;
            this.BThongTinBao = BPhanHuongNhuCau.BThongTinBao;
            this.Unit = BPhanHuongNhuCau.Unit;
            this.CreateBy = BPhanHuongNhuCau.CreateBy;
            this.CreateDate = BPhanHuongNhuCau.CreateDate;
            this.ModifyBy = BPhanHuongNhuCau.ModifyBy;
            this.ModifyDate = BPhanHuongNhuCau.ModifyDate;
        }

        public BPhanHuongNhuCau toBPhanHuongNhuCau()
        {
            BPhanHuongNhuCau result = new BPhanHuongNhuCau();
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

    public class BPhanHuongNhuCauSearchModel
    {
        public BPhanHuongNhuCauSearchModel()
        {
        }
        public string UnitCode { get; set; }
        public string ThongTinBaoId { get; set; }
        public string DiemTiepNhanId { get; set; }
        public virtual BDiemTiepNhan BDiemTiepNhan { get; set; }
        public virtual BThongTinBao BThongTinBao { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }

    public partial class BDiemTiepNhanModel
    {
        public BDiemTiepNhanModel()
        {
            this.BPhanHuongNhuCaus = new HashSet<BPhanHuongNhuCau>();
            this.BDieuChinhPhanHuongDistricts = new HashSet<BDieuChinhPhanHuongDistrict>();
            this.BDieuChinhPhanHuongUnits = new HashSet<BDieuChinhPhanHuongUnit>();
            this.Units = new HashSet<Unit>();
        }
        public BDiemTiepNhanModel(BDiemTiepNhan bDiemTiepNhan)
        {
            if (bDiemTiepNhan != null)
            { 
                this.Id = bDiemTiepNhan.Id;
                this.Code = bDiemTiepNhan.Code;
                this.Name = bDiemTiepNhan.Name;
                this.UnitCode = bDiemTiepNhan.UnitCode;
                this.Unit = bDiemTiepNhan.Unit;
                this.CreateBy = bDiemTiepNhan.CreateBy;
                this.CreateDate = bDiemTiepNhan.CreateDate;
                this.ModifyBy = bDiemTiepNhan.ModifyBy;
                this.ModifyDate = bDiemTiepNhan.ModifyDate;
                this.Status = bDiemTiepNhan.Status;
                this.Unit = bDiemTiepNhan.Unit;
                this.BPhanHuongNhuCaus = bDiemTiepNhan.BPhanHuongNhuCaus;
                this.BDieuChinhPhanHuongDistricts = bDiemTiepNhan.BDieuChinhPhanHuongDistricts;
                this.BDieuChinhPhanHuongUnits = bDiemTiepNhan.BDieuChinhPhanHuongUnits;
            }
        }
        public BDiemTiepNhan toBDiemTiepNhan()
        {
            BDiemTiepNhan result = new BDiemTiepNhan();
            result.Id = string.IsNullOrEmpty(this.Id) ? Guid.NewGuid().ToString() : this.Id;
            result.Code = this.Code;
            result.Name = this.Name;
            result.UnitCode = this.UnitCode;
            result.Unit = this.Unit;
            result.CreateBy = this.CreateBy;
            result.CreateDate = this.CreateDate;
            result.ModifyBy = this.ModifyBy;
            result.ModifyDate = this.ModifyDate;
            result.Status = this.Status;
            result.BPhanHuongNhuCaus = this.BPhanHuongNhuCaus;
            result.BDieuChinhPhanHuongDistricts = this.BDieuChinhPhanHuongDistricts;
            result.BDieuChinhPhanHuongUnits = this.BDieuChinhPhanHuongUnits;
            return result;
        }
        [Display(Name = "Mã Điểm Đặt Nhu Cầu")]
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Display(Name = "Mã Điểm Đặt Nhu Cầu")]
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        public string Code { get; set; }
        [Display(Name = "Tên Điểm Đặt Nhu Cầu")]
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        public string Name { get; set; }
        [Required(ErrorMessage = Enums.ErrorMessage.Required)]
        [Display(Name = "Mã Bưu Cục")]
        public string UnitCode { get; set; }
        private string _UnitName;
        [Display(Name = "Tên Bưu Cục")]
        public string UnitName {
            get { if (this.Unit != null) return this.Unit.UnitName; else return _UnitName; }
            set { 
                if (value != null) 
                    _UnitName = value; 
                else { 
                    if (this.Unit != null) 
                        _UnitName = this.Unit.UnitName; 
                    else _UnitName = ""; 
                }
            }
        }
        [Display(Name = "Ngày tạo")]
        public Nullable<System.DateTime> CreateDate { get; set; }
        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày sửa")]
        public Nullable<System.DateTime> ModifyDate { get; set; }
        [Display(Name = "Người sửa")]
        public string ModifyBy { get; set; }
        [Display(Name = "Trạng thái")]
        public int Status { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual IEnumerable<Unit> Units { get; set; }
        public virtual ICollection<BPhanHuongNhuCau> BPhanHuongNhuCaus { get; set; }
        public virtual ICollection<BDieuChinhPhanHuongDistrict> BDieuChinhPhanHuongDistricts { get; set; }
        public virtual ICollection<BDieuChinhPhanHuongUnit> BDieuChinhPhanHuongUnits { get; set; }
    }

    public class DistrictWithUnit
    {
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public virtual IEnumerable<v_Unit> Units { get; set; }
        public virtual IEnumerable<ProvinceWithCode> ProvinceWithCode { get; set; }
    }

    public class ProvinceWithCount
    {
        public string ProvinceName { get; set; }
        public string ProvinceCode { get; set; }
        public int CountDistrictConfig { get; set; }
        public int CountDistrictNotConfig { get; set; }
        public int CountDistrict { get; set; }
        public virtual ICollection<District> Districts { get; set; }
        public virtual ICollection<ProvinceWithCode> ProDistricts { get; set; }
        public virtual ICollection<DistrictWithUnit> DistrictWithUnit { get; set; }
        public DistrictWithCount DistrictWithCount { get; set; }
    }

    public class DistrictWithCount
    {
        public string DistrictName { get; set; }
        public string DistrictCode { get; set; }
        public string ProvinceName { get; set; }
        public int CountUnitConfig { get; set; }
        public int CountUnitNotConfig { get; set; }
        public int CountUnit { get; set; }
    }

    public class ProvinceWithCode
    {
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string UnitCode { get; set; }
    }

    public class UnitModel
    {
        public string ThongTinBaoId;
        private List<v_Unit> lstUnit;
        private List<Province> lstProvince;
        public List<TinhThanh> lstTinhThanh { get; set; }
        public List<QuanHuyen> lstQuanHuyen { get; set; }
        public List<BPhanHuongNhuCau> lstPHNC { get; set; }
        public List<BDiemTiepNhan> lstDTN { get; set; }
        public UnitModel()
        {

        }
        public UnitModel(List<Province> _lstProvince)
        {
            lstProvince = _lstProvince;
        }
        public UnitModel(List<v_Unit> _lstUnit, List<Province> _lstProvince, List<BDiemTiepNhan> _lstBDiemTiepNhan)
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
            this.AddMap(_lstUnit, DiemTiepNhanId);
        }

        public void UpdateListUnitMap(List<v_Unit> _lstUnit, string DiemTiepNhanId)
        {
            this.UpdateMap(_lstUnit, DiemTiepNhanId);
        }

        public void setListPHNC(List<BPhanHuongNhuCau> _lstPHNC)
        {
            //List<v_Unit> _lstUnit = lstUnit.Where(a => _lstPHNC.Any(p => p.UnitCode.Equals(a.UnitCode))).ToList();
            List<v_Unit> _lstUnit = (from sc in lstUnit
                                     join soc in _lstPHNC
                                     on sc.UnitCode equals soc.UnitCode
                                     select new v_Unit(sc, soc.DiemTiepNhanId, soc.BDiemTiepNhan.Name)
                         ).ToList();

            this.AddMap(_lstUnit);
            lstPHNC = _lstPHNC;
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
                result = lstQuanHuyen.Where(a => lstids.Any(p => p.Equals(a.DistrictCode))).SelectMany(a => a.lstUnitMap).OrderBy(t => t.DistrictName).ToList();
            }
            else if (typeid.Trim() == "3")
            {
                result = lstUnit.Where(a => lstids.Any(p => p.Equals(a.UnitCode))).ToList();
            }
            return result;
        }

        public List<QuanHuyen> getDistrictNotMap(string[] lstids)
        {

            List<QuanHuyen> result = new List<QuanHuyen>();
            result = lstQuanHuyen.Where(t => lstids.Contains(t.ProvinceCode) && t.lstUnitNotMap.Count > 0).OrderBy(t => t.DistrictName).OrderBy(t => t.ProvinceName).ToList();
            return result;
        }

        public List<QuanHuyen> getDistrictMap(string[] lstids)
        {

            List<QuanHuyen> result = new List<QuanHuyen>();
            result = lstQuanHuyen.Where(t => lstids.Contains(t.ProvinceCode) && t.lstUnitMap.Count > 0).OrderBy(t => t.DistrictName).OrderBy(t => t.ProvinceName).ToList();
            return result;
        }
        public List<QuanHuyen> getDistrictMapByProvinceCode(string provincecode)
        {
            return lstQuanHuyen.Where(t => t.ProvinceCode.CompareTo(provincecode) == 0 && t.lstUnitMap.Count <= t.lstUnit.Count && t.lstUnitMap.Count > 0).OrderBy(x => x.ProvinceName).OrderBy(x => x.DistrictName).ToList();
        }
        public List<QuanHuyen> getDistrictNotMapByProvinceCode(string provincecode)
        {
            return lstQuanHuyen.Where(t => t.ProvinceCode.CompareTo(provincecode) == 0 && t.lstUnitNotMap.Count > 0).OrderBy(x => x.ProvinceName).OrderBy(x => x.DistrictName).ToList();
        }
        public List<QuanHuyen> getDistrictMapByDistrictCode(string districtcode)
        {
            return lstQuanHuyen.Where(t => t.DistrictCode.CompareTo(districtcode) == 0 && t.lstUnitMap.Count <= t.lstUnit.Count && t.lstUnitMap.Count > 0).ToList();
        }
        public List<TinhThanh> getAllProvinceNotMap(string provincecode = null)
        {
            List<TinhThanh> result = new List<TinhThanh>();
            if (string.IsNullOrWhiteSpace(provincecode))
            {
                result = lstTinhThanh.Where(t => t.lstUnitMap.Count < t.lstUnit.Count).OrderBy(t => t.ProvinceName).ToList();
            }
            else
            {
                result = lstTinhThanh.Where(t => t.ProvinceCode.CompareTo(provincecode) == 0 && t.lstUnitMap.Count < t.lstUnit.Count).OrderBy(t => t.ProvinceName).ToList();
            }
            return result;
        }
        public List<TinhThanh> getAllProvince()
        {
            return lstTinhThanh.OrderBy(t => t.ProvinceName).OrderBy(t => t.CountDistrictMap == t.lstQuanHuyen.Count ? 0 : 1).OrderBy(t => t.CountDistrictMap > 0 ? 0 : 1).ToList();
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
        private void AddMap(List<v_Unit> _lstUnit, string DiemTiepNhanId = null)
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
                objTinhThanh.lstUnitMap.Add(item);
                int index = objTinhThanh.lstUnitNotMap.FindIndex(a => a.UnitCode == item.UnitCode);
                objTinhThanh.lstUnitNotMap.RemoveAt(index);

                objQuanHuyen.lstUnitMap.Add(item);
                index = objQuanHuyen.lstUnitNotMap.FindIndex(a => a.UnitCode == item.UnitCode);
                objQuanHuyen.lstUnitNotMap.RemoveAt(index);

            }
        }

        private void UpdateMap(List<v_Unit> _lstUnit, string DiemTiepNhanId)
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
                setUnit.DiemTiepNhanId = DiemTiepNhanId;
                setUnit.DiemTiepNhanName = tenDiemTiepNhan;
                setUnit = objQuanHuyen.lstUnitMap.Find(a => a.UnitCode == item.UnitCode);
                setUnit.DiemTiepNhanId = DiemTiepNhanId;
                setUnit.DiemTiepNhanName = tenDiemTiepNhan;
                //objTinhThanh.lstUnitMap.Remove(item);
                //objQuanHuyen.lstUnitMap.Add(item);

            }
        }
    }

    public class TinhThanh
    {
        public TinhThanh()
        {
            lstQuanHuyen = new List<QuanHuyen>();
            lstUnit = new List<v_Unit>();
            lstUnitMap = new List<v_Unit>();
            lstUnitNotMap = new List<v_Unit>();
        }

        public int CountDistrictNotMap
        {
            get
            {
                return lstQuanHuyen.Where(a => a.lstUnitNotMap.Count > 0).Count();
                //return lstQuanHuyen.Where(a => string.IsNullOrWhiteSpace(a.DiemTiepNhanId)).Count();
            }
        }
        public int CountDistrictNotMap2
        {
            get
            {
                return lstQuanHuyen.Where(a => string.IsNullOrWhiteSpace(a.DiemTiepNhanId)).Count();
            }
        }
        public int CountDistrictMap
        {
            get
            {
                return lstQuanHuyen.Where(a => a.lstUnitMap.Count > 0).Count();
                //return lstQuanHuyen.Where(a => !string.IsNullOrWhiteSpace(a.DiemTiepNhanId)).Count();
            }
        }
        public int CountDistrictMap2
        {
            get
            {
                return lstQuanHuyen.Where(a => !string.IsNullOrWhiteSpace(a.DiemTiepNhanId)).Count();
            }
        }
        public string ProvinceMapPoint
        {
            get
            {
                string point = "";
                foreach (var item in lstQuanHuyen.Where(t => !string.IsNullOrWhiteSpace(t.DiemTiepNhanName)).OrderBy(t => t.DiemTiepNhanName).Select(t => t.DiemTiepNhanName).Distinct())
                {
                    point += item + " ,";
                }
                if (point.Length > 0)
                {
                    return point.Substring(0, point.Length - 1);
                }
                else return "";
            }
        }
        [Display(Name = "Mã Tỉnh")]
        public string ProvinceCode { get; set; }
        [Display(Name = "Tên Tỉnh")]
        public string ProvinceName { get; set; }
        public List<QuanHuyen> lstQuanHuyen { get; set; }
        public List<v_Unit> lstUnit { get; set; }
        public List<v_Unit> lstUnitMap { get; set; }
        public List<v_Unit> lstUnitNotMap { get; set; }

    }

    public class QuanHuyen
    {
        public QuanHuyen()
        {
            lstUnit = new List<v_Unit>();
            lstUnitMap = new List<v_Unit>();
            lstUnitNotMap = new List<v_Unit>();
        }

        public QuanHuyen(QuanHuyen QuanHuyen, string diemTiepNhanId, string diemTiepNhanName)
        {
            this.DistrictCode = QuanHuyen.DistrictCode;
            this.DistrictName = QuanHuyen.DistrictName;
            this.DiemTiepNhanId = diemTiepNhanId;
            this.DiemTiepNhanName = diemTiepNhanName;
            this.lstUnit = QuanHuyen.lstUnit;
            this.lstUnitMap = QuanHuyen.lstUnitMap;
            this.lstUnitNotMap = QuanHuyen.lstUnitNotMap;
            this.ProvinceCode = QuanHuyen.ProvinceCode;
            this.ProvinceName = QuanHuyen.ProvinceName;
        }
        public string DistrictMapPoint
        {
            get
            {
                string point = "";
                foreach (var item in lstUnitMap.Select(t => t.DiemTiepNhanName).Distinct())
                {
                    point += item + " ,";
                }
                if (point.Length > 0)
                {
                    return point.Substring(0, point.Length - 1);
                }
                else return "";
            }
        }
        public string DistrictMapPoint2
        {
            get
            {
                return this.DiemTiepNhanName;
            }
        }

        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string DiemTiepNhanId { get; set; }
        public string DiemTiepNhanName { get; set; }
        public List<v_Unit> lstUnit { get; set; }
        public List<v_Unit> lstUnitMap { get; set; }
        public List<v_Unit> lstUnitNotMap { get; set; }
    }
}
