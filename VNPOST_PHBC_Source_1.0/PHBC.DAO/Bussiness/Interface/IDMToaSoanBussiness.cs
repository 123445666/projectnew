using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
   public interface IDMToaSoanBussiness
    {
        List<DMToaSoan> getAll();
        List<DMToaSoanModel> getAllModel();
        List<DMToaSoan> getAll(int page, int pageSize, out int pageCount);
        List<DMToaSoanModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem);
        List<DMToaSoan> search(string MaToaSoan, string TenToaSoan = "", string SoDienThoai ="");
        List<DMToaSoanModel> searchModel(DMToaSoanSearchModel obj, int page, int pageSize, out int pageCount, out int totalitem);
        List<DMToaSoanModel> searchModel(DMToaSoanSearchModel obj);
        List<DMToaSoan> search(string roleName, int page, int pageSize, out int pageCount);
        DMToaSoan getById(string id);
        ErrorObject checkDMToaSoan(string id, string MaToaSoan, string TenToaSoan);
        int Add(DMToaSoan DMToaSoan);
        int Update(DMToaSoan dmToaSoan);
        int Delete(DMToaSoan DMToaSoan);
        void Dispose();
    }
}
