using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface IDieuChinhKHXBBussiness
    {
        BaoKyXuatBanModel getBaoKyXuatBanModel(string id);
        List<DCThongTinBaoEditModel> getListDCThongTinBao(string thongTinBaoId);
        BDieuChinhKHXBDetail getDieuChinhKHXBDetail(string id);
        BDieuChinhKHXB getDieuChinhKHXB(string id);
        ErrorObject CreateDieuChinhKHXBDetail(string dcId, int LoaiDieuChinh, string config, List<DCThongTinBaoModel> lstThongTin, string GhiChu,string userCreate);
        ErrorObject EditDieuChinhKHXBDetail(string dcDetailId, List<DCThongTinBaoModel> lstThongTin, string GhiChu);
        void deleteDieuChinhKHXBDetail(string dcIDDetail);
        DB_PHBCEntities getDBSelect();
        List<BDieuChinhKHXBDetailModel> getBKeHoachXuatBanDetailModel(string id);
        void Dispose();
    }
}
