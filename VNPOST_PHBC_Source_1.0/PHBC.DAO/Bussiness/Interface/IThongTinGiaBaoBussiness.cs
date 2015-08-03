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
  public interface IThongTinGiaBaoBussiness
    {
      List<BThongTinGiaBao> getAllBThongTinGiaBao();
      List<ThongTinGiaBaoModel> getAllThongTinGiaBaoModel(string idBao);
      List<BThongTinGiaBao> getAll(int page, int pageSize, out int pageCount);
      List<ThongTinGiaBaoModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem, string idBao);
      ErrorObject Create(ThongTinGiaBaoModel bthongtinbaoModel);
      ErrorObject GiaMua(ThongTinGiaBaoModel bthongtingiabaoModel);
      DanhSachGiaBaoModel getDanhSachGiaBaoModel(string id, string provinceCode);
      ThongTinGiaBaoModel createThongTinGiaBaoModel(string idBao);
      ThongTinGiaBaoModel getThongTinGiaBaoById(string id);
      List<ThongTinGiaBaoModel> searchThongTinGiaBao(ThongTinGiaBaoSearchModel thongTinGiaBaoSearchModel);
      ErrorObject Edit(ThongTinGiaBaoModel thongTinGiaBaoModel);
      ErrorObject Delete(string id);
      void Dispose();

    }
}
