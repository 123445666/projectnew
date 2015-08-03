using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface IBPHNCBussiness
    {
        /// <summary>
        /// get all data from table BPhanHuongNhuCau
        /// </summary>
        /// <returns>List<BPhanHuongNhuCau></returns>
        List<BPhanHuongNhuCau> getAll();
        /// <summary>
        /// get all data from table BPhanHuongNhuCau
        /// </summary>
        /// <returns>List<BPhanHuongNhuCauModel></returns>
        List<BPhanHuongNhuCauModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem);
        /// <summary>
        /// get all data from table BPhanHuongNhuCau distinct by DiemTiepNhanId return data with list Units
        /// dữ liệu dựa vào Mã báo
        /// </summary>
        /// <param name="ThongTinBaoId"></param>
        /// <returns>List BDiemTiepNhanModel</returns>
        List<BDiemTiepNhanModel> getAllData(string currentunit, string ThongTinBaoId = null);
        /// <summary>
        /// get all data from table BPhanHuongNhuCau with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns>List<BPhanHuongNhuCau></returns>
        List<BPhanHuongNhuCau> getAll(int page, int pageSize, out int pageCount);
        /// <summary>
        /// get all data from table BPhanHuongNhuCau with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalitem"></param>
        /// <returns>List<BPhanHuongNhuCauModel></returns>
        List<BPhanHuongNhuCauModel> getAllModelPager(int page, ref int pageSize, out int pageCount, out int totalitem,string Mabao = null);
        /// <summary>
        /// get all data with search object (have paging)
        /// </summary>
        /// <param name="obj">DMToaSoanSearchModel</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="totalitem"></param>
        /// <returns>List<BPhanHuongNhuCauModel></returns>
        List<BPhanHuongNhuCauModel> searchModel(DMToaSoanSearchModel obj, int page, int pageSize, out int pageCount, out int totalitem);
        /// <summary>
        /// get all data with search object
        /// </summary>
        /// <param name="obj">DMToaSoanSearchModel</param>
        /// <returns>List<BPhanHuongNhuCauModel></returns>
        List<BPhanHuongNhuCauModel> searchModel(DMToaSoanSearchModel obj);
        /// <summary>
        /// get data by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BPhanHuongNhuCau</returns>
        BPhanHuongNhuCau getById(string id);
        /// <summary>
        /// check data 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MaToaSoan"></param>
        /// <param name="TenToaSoan"></param>
        /// <returns>ErrorObject</returns>
        ErrorObject checkPHBC(BPhanHuongNhuCau BPhanHuongNhuCau);
        /// <summary>
        /// get all unit not in table BPhanHuongNhuCau
        /// </summary>
        /// <param name="BThongTinBaoId">Mã báo</param>
        /// <returns>List<Unit></returns>
        List<Unit> getAllUnitNotInPhnc(string currentUnit, string BThongTinBaoId = null);
        /// <summary>
        /// get all unit not in table BPhanHuongNhuCau
        /// </summary>
        /// <param name="BThongTinBaoId">Mã báo</param>
        /// <returns></returns>
        List<Unit> getAllUnitByDTNId(string currentunit, string DiemTiepNhanId, string BThongTinBaoId = null);
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
        BThongTinBao getAllThongTinBaoById(string id);
        /// <summary>
        /// author: vietvb
        /// desc: get BDiemTiepNhan from DiemTiepNhanId and ThongTinBaoId
        /// </summary>
        /// <param name="id">DiemTiepNhanId</param>
        /// <param name="Mabao">ThongTinBaoId</param>
        /// <returns></returns>
        BDiemTiepNhan getByDiemTiepNhanId(string id);
        /// <summary>
        /// author: vietvb
        /// desc: get BPhanHuongNhuCau from DiemTiepNhanId and ThongTinBaoId
        /// </summary>
        /// <param name="id">DiemTiepNhanId</param>
        /// <param name="Mabao">ThongTinBaoId</param>
        /// <returns></returns>
        BPhanHuongNhuCau getByUnit(string UnitCode, string Mabao = null);
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
        /// lấy tất cả BPhanHuongNhuCau theo mã báo dùng cho bảng chưa config
       /// </summary>
       /// <param name="Mabao">ThongTinBaoID</param>
       /// <returns></returns>
        List<BPhanHuongNhuCau> getAllPHNCByMaBaoNotConfig(string Mabao);
        /// <summary>
        /// lấy tất cả BPhanHuongNhuCau theo mã báo dùng cho bảng đã config
        /// </summary>
        /// <param name="Mabao">ThongTinBaoID</param>
        /// <returns></returns>
        List<BPhanHuongNhuCau> getAllPHNCByMaBaoConfig(string Mabao);
        /// <summary>
        /// get province
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <returns></returns>
        List<ProvinceWithCount> getListProvince(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <param name="provincecode"></param>
        /// <returns></returns>
        List<ProvinceWithCount> getListProvinceToEdit(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC, string provincecode = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <param name="lstids">string array Province</param>
        /// <returns></returns>
        List<DistrictWithCount> getListDistrict(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC, string[] lstids, bool type = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <param name="lstids">string array District</param>
        /// <returns></returns>
        List<v_Unit> getListUnit(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC, string[] lstids, bool type = true);
        /// <summary>
        /// get all province configed
        /// </summary>
        /// <param name="lstVUnit"></param>
        /// <param name="lstPHNC"></param>
        /// <returns></returns>
        List<ProvinceWithCount> getListProvinceConfiged(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC);
        int Add(BPhanHuongNhuCau BPhanHuongNhuCau, List<v_Unit> units);
        int Update(BPhanHuongNhuCau BPhanHuongNhuCau);
        int Delete(string id);
        void Dispose();
        
    }
}
