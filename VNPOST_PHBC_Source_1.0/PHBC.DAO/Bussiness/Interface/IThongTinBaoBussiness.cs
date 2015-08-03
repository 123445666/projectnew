using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface IThongTinBaoBussiness
    {
        List<BThongTinBao> getAllThongTinBao();
        List<ThongTinBaoModel> getAllThongTinBaoModel();
        List<BThongTinBao> getAll(int page, int pageSize, out int pageCount);
        List<ThongTinBaoModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem);        
        ErrorObject Create(ThongTinBaoModel thongTinBaoModel);
        ErrorObject Edit(ThongTinBaoModel thongTinBaoModel);
        ErrorObject Delete(string id);
        BaoDiemInModel getBaoDiemInModel(string id);
        int CapNhatBaoDiemIn(string id, string lstDiemIn);
        BaoKyXuatBanModel getBaoKyXuatBanModel(string id);
        int UpdateKyXuatBan(string id, int loaiky, string chitiet, string userID);        
        ThongTinBaoModel getThongTinBaoById(string Id);        
        List<DefineSelectItem> getListLoaiAnPham();
        List<DefineSelectItem> getListBaoNgoaiVan(int currentValue);
        List<DefineSelectItem> getListTWDP(int currentValue);
        List<DefineSelectItem> getListBaoTrongDanhMuc(int currentValue);
        List<DefineSelectItem> getListBaoCongIchNgoaiCongIch(int currentValue);
        List<DefineSelectItem> getListBaoCoThueKhongThue(int currentValue);
        List<DefineSelectItem> getListDMToanSoan();
        List<ThongTinBaoModel> searchThongTinBao(ThongTinBaoSearchModel thongTinBaoSearchModel);
        void Dispose();
    }
}
