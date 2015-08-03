using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHBC.DAO.Common;
using PHBC.DAO.Models;

namespace PHBC.DAO.Bussiness
{
    public interface IDMLoaiAnPhamBussiness
    {
        List<DMLoaiAnPham> getAll();
        List<DMLoaiAnPham> getAll(int page, int pageSize, out int pageCount);
        List<DMLoaiAnPhamModel> getAllModel();
        List<DMLoaiAnPhamModel> getAllModel(int page,ref int pageSize, out int pageCount);
        List<DMLoaiAnPhamModel> search(string TenLoaiAnPham);
        List<DMLoaiAnPham> search(string TenLoaiAnPham, int page, int pageSize, out int pageCount);
        DMLoaiAnPham getById(string id);
        ErrorObject checkDMLoaiAnPham(byte action, string Id, string TenLoaiAnPham);
        int Add(DMLoaiAnPham DMLoaiAnPham);
        int Update(DMLoaiAnPhamModel dmLoaiAnPhamModel);
        int Delete(string id);
        void Dispose();

        List<DMLoaiAnPham> search(string Id, string TenLoaiAnPham);

        
    }
}
