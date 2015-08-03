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
   public interface IDMDiemInBussiness
    {
        List<DMDiemIn> getAll();
        List<Province> getAllProvince();
        List<District> getAllDistrict();
        List<DMDiemIn> getAll(int page, int pageSize, out int pageCount);
        List<DMDiemInModel> getAllModel();
        List<DMDiemInModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem);
        List<DMDiemIn> search(string MaDiemIn, string TenDiemIn = "");
        List<DMDiemIn> search(string MaDiemIn, string TenDiemIn, int page, int pageSize, out int pageCount);
        List<DMDiemInModel> searchModel(string MaDiemIn, string TenDiemIn, int page, int pageSize, out int pageCount, out int totalitem);
        DMDiemIn getById(string id);
        ErrorObject checkDMDiemIn(string id, string MaDiemIn, string TenDiemIn);
        int Add(DMDiemInModel DMDiemIn);
        int Update(DMDiemInModel DMDiemIn);
        int Delete(DMDiemIn DMDiemIn);
        //DMDiemIn getDistrict(string Province);
        List<District> getDistrictByProvince(string Province);      
        void Dispose();
    }
}
