using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    /// <summary>
    /// created : 23/07/2015
    /// Author : vietvb
    /// </summary>
    public interface IBDieuChinhPHNCBussiness
    {
        /// <summary>
        /// get all data from table BDieuChinhPhanHuongUnit
        /// </summary>
        /// <returns>List<BDieuChinhPhanHuongUnit></returns>
        List<BDieuChinhPhanHuongUnit> getAll();
        /// <summary>
        /// get all data from table BDieuChinhPhanHuongUnit
        /// </summary>
        /// <returns>List<BDieuChinhPhanHuongUnitModel></returns>
        List<BDieuChinhPhanHuongUnitModel> getAllDieuChinhPHUnitModel(int page, int pageSize, out int pageCount, out int totalitem);
        /// <summary>
        /// get all data from table BDieuChinhPhanHuongUnit distinct by DiemTiepNhanId return data with list Units
        /// dữ liệu dựa vào Mã báo
        /// </summary>
        /// <param name="DieuChinhKHXBDetailId"></param>
        /// <returns>List BDiemTiepNhanModel</returns>
        List<BDiemTiepNhanModel> getAllData(string currentunit, string DieuChinhKHXBDetailId = null);
        /// <summary>
        /// get all data from table BDieuChinhPhanHuongUnit with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns>List<BDieuChinhPhanHuongUnit></returns>
        List<BDieuChinhPhanHuongUnit> getAll(int page, int pageSize, out int pageCount);
        /// <summary>
        /// get all data from table BDieuChinhPhanHuongUnit with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalitem"></param>
        /// <returns>List<BDieuChinhPhanHuongUnitModel></returns>
        List<BDieuChinhPhanHuongUnitModel> getAllModelPager(int page, ref int pageSize, out int pageCount, out int totalitem,string DieuChinhKHXBDetailId = null);
        /// <summary>
        /// get all data with search object (have paging)
        /// </summary>
        /// <param name="obj">DMToaSoanSearchModel</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalitem"></param>
        /// <returns>List<BDieuChinhPhanHuongUnitModel></returns>
        List<BDieuChinhPhanHuongUnitModel> searchModel(DMToaSoanSearchModel obj, int page, int pageSize, out int pageCount, out int totalitem);
        /// <summary>
        /// get all data with search object
        /// </summary>
        /// <param name="obj">DMToaSoanSearchModel</param>
        /// <returns>List<BDieuChinhPhanHuongUnitModel></returns>
        List<BDieuChinhPhanHuongUnitModel> searchModel(DMToaSoanSearchModel obj);
        /// <summary>
        /// get data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BDieuChinhPhanHuongUnit</returns>
        BDieuChinhPhanHuongUnit getById(string id);
        /// <summary>
        /// check data 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MaToaSoan"></param>
        /// <param name="TenToaSoan"></param>
        /// <returns>ErrorObject</returns>
        ErrorObject checkPHBC(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit);
        /// <summary>
        /// get all unit not in table BDieuChinhPhanHuongUnit
        /// </summary>
        /// <param name="BDieuChinhKHXBDetailId">Mã báo</param>
        /// <returns>List<Unit></returns>
        List<Unit> getAllUnitNotInPhnc(string currentUnit, string BDieuChinhKHXBDetailId = null);
        /// <summary>
        /// get all unit not in table BDieuChinhPhanHuongUnit
        /// </summary>
        /// <param name="BDieuChinhKHXBDetailId">Mã báo</param>
        /// <returns></returns>
        List<Unit> getAllUnitByDTNId(string currentunit, string DiemTiepNhanId, string BDieuChinhKHXBDetailId = null);
        /// <summary>
        /// get all BDiemTiepNhan
        /// </summary>
        /// <returns>List<BDiemTiepNhan></returns>
        List<BDiemTiepNhan> getAllDiemTiepNhan();
        /// <summary>
        /// lấy toàn bộ điểm tiếp nhận theo id điểm tiếp nhận
        /// </summary>
        /// <returns>List<BDiemTiepNhan></returns>
        BDiemTiepNhan getAllDiemTiepNhanById(string id);
        /// <summary>
        /// get all BThongTinBao
        /// </summary>
        /// <returns>List<BThongTinBao></returns>
        List<BThongTinBao> getAllThongTinBao();
        /// <summary>
        /// lấy thông tin của báo theo id báo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BThongTinBao getThongTinBaoById(string id);
        /// <summary>
        /// lấy thông tin chỉnh sửa của DieuChinhKHXB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BDieuChinhKHXBDetail getDieuChinhDetailById(string id);
        /// <summary>
        /// author: vietvb
        /// desc: get BDiemTiepNhan from DiemTiepNhanId and DieuChinhKHXBDetailId
        /// </summary>
        /// <param name="id">DiemTiepNhanId</param>
        /// <param name="DieuChinhKHXBDetailId">DieuChinhKHXBDetailId</param>
        /// <returns></returns>
        BDiemTiepNhan getByDiemTiepNhanId(string id);
        /// <summary>
        /// author: vietvb
        /// desc: get BDieuChinhPhanHuongUnit from DiemTiepNhanId and DieuChinhKHXBDetailId
        /// </summary>
        /// <param name="id">DiemTiepNhanId</param>
        /// <param name="DieuChinhKHXBDetailId">DieuChinhKHXBDetailId</param>
        /// <returns></returns>
        BDieuChinhPhanHuongUnit getByUnit(string UnitCode, string DieuChinhKHXBDetailId = null);
        /// <summary>
        /// get all Unit
        /// </summary>
        /// <returns>List Unit</returns>
        List<Unit> getAllUnit();
        /// <summary>
        /// get all districts by array provinecode
        /// </summary>
        /// <param name="lstids">string array</param>
        /// <returns></returns>
        List<District> getDiStrictByArrayId(string[] lstids);
        /// <summary>
        /// get all units by array districtcode, provincecode
        /// </summary>
        /// <param name="lstids">string array ids</param>
        /// <param name="typeid">type of array: 1:provincecode, 2: districtcode, 3 : unitcode </param>
        /// <returns></returns>
        List<v_Unit> getUnitByArrayId(string[] lstids, string typeid);
        /// <summary>
        /// get all province
        /// </summary>
        /// <returns></returns>
        List<Province> getAllProvince(string id = null);
        /// <summary>
        /// get all data of v_unit
        /// </summary>
        /// <returns></returns>
        List<v_Unit> getAllVunit();
       
       /// <summary>
        /// lấy tất cả BDieuChinhPhanHuongUnit theo mã báo dùng cho bảng chưa config
       /// </summary>
       /// <param name="DieuChinhKHXBDetailId">DieuChinhKHXBDetailId</param>
       /// <returns></returns>
        List<BDieuChinhPhanHuongUnit> getAllPHNCByDieuChinhKHXBDetailIdNotConfig(string DieuChinhKHXBDetailId);
        /// <summary>
        /// lấy tất cả BDieuChinhPhanHuongUnit theo mã báo dùng cho bảng đã config
        /// </summary>
        /// <param name="DieuChinhKHXBDetailId">DieuChinhKHXBDetailId</param>
        /// <returns></returns>
        List<BDieuChinhPhanHuongUnit> getAllPHNCByDieuChinhKHXBDetailIdConfig(string DieuChinhKHXBDetailId);
        /// <summary>
        /// lấy tất cả BDieuChinhPhanHuongDistrict theo mã báo dùng cho bảng đã config
        /// </summary>
        /// <param name="DieuChinhKHXBDetailId">DieuChinhKHXBDetailId</param>
        /// <returns></returns>
        List<BDieuChinhPhanHuongDistrict> getAllDistrictPHNCByDieuChinhKHXBDetailIdConfig(string DieuChinhKHXBDetailId);
        /// <summary>
        /// get province
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <returns></returns>
        List<ProvinceWithCount> getListProvince(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <param name="provincecode"></param>
        /// <returns></returns>
        List<ProvinceWithCount> getListProvinceToEdit(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC, string provincecode = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <param name="lstids">string array Province</param>
        /// <returns></returns>
        List<DistrictWithCount> getListDistrict(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC, string[] lstids, bool type = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <param name="lstids">string array District</param>
        /// <returns></returns>
        List<v_Unit> getListUnit(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC, string[] lstids, bool type = true);
        /// <summary>
        /// get all province configed
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <returns></returns>
        List<ProvinceWithCount> getListProvinceConfiged(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC);
        int Add(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit, List<v_Unit> units);
        int AddDistrict(BDieuChinhPhanHuongDistrict BDieuChinhPhanHuongDistrict, List<QuanHuyen> District);
        int Add(BPhanHuongNhuCauUnit BPhanHuongNhuCauUnit, List<v_Unit> units);
        int AddDistrict(BPhanHuongNhuCauDistrict BPhanHuongNhuCauDistrict, List<QuanHuyen> District);
        int Update(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit);
        int Delete(string id);
        void Dispose();
        
    }
}
