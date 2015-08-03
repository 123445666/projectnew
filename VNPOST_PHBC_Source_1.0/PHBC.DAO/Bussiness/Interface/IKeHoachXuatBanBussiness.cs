using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHBC.DAO.Models;

namespace PHBC.DAO.Bussiness
{
    public interface IKeHoachXuatBanBussiness
    {
        DB_PHBCEntities getDBSelect();
        int updateBKeHoachXuatBan(string id, int year, int soBatdau, string userID);
        BKeHoachXuatBan getBKeHoachXuatBan(string id);
        BaoKyXuatBanModel getBaoKyXuatBanModel(string id);
        List<BKeHoachXuatBanDetail> getBKeHoachXuatBanDetail(string id,int year);
        List<BDieuChinhKHXBDetailModel> getBKeHoachXuatBanDetailModel(string id);
        List<BDieuChinhKHXB> getBDieuChinhKHXB(string id, int year);
        int getBKeHoachXuatBanYearLast(string id);
        int addDieuChinhKHXB(string id, int year, int quy, string userID, string copy);
        
        List<dynamic> getAll(int page, int pageSize, out int pageCount, out int totalitem,string search,int year);
        List<dynamic> getKHXBByID(string id);
        void Dispose();
    }
    
}
