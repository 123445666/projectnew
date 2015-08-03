using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface IBThongTinGiaBaoBussiness
    {
        List<BThongTinGiaBao> getAllBThongTinGiaBao();
        List<BThongTinGiaBaoModel> getAllBThongTinGiaBaoModel();
        List<BThongTinGiaBao> getAll(int page, int pageSize, out int pageCount);
        List<BThongTinGiaBaoModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem);
        ErrorObject Create(BThongTinGiaBaoModel bthongtinbaoModel);
        BThongTinGiaBaoModel getBThongTinGiaBaoModel(string id);
        List<BThongTinGiaBaoModel> searchThongTinGiaBao(ThongTinGiaBaoSearchModel thongTinGiaBaoSearchModel);
        void Dispose();

    }
}
