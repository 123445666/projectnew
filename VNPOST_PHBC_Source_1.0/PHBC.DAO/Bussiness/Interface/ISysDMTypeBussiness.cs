using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface ISysDMTypeBussiness
    {
        List<SysDMType> getAll();
        List<SysDMType> getAll(int page, int pageSize, out int pageCount);
        List<SysDMTypeModel> getAllModel();
        List<SysDMTypeModel> getAllModel(int page, int pageSize, out int pageCount);
        List<SysDMType> search(string SysDMTypeName);
        List<SysDMType> search(string SysDMTypeName, int page, int pageSize, out int pageCount);
        List<SysDMTypeModel> searchModel(string SysDMTypeName, int page, int pageSize, out int pageCount);
        SysDMType getById(int? id);
        int Add(SysDMType SysDMType);
        int Update(SysDMType SysDMType);
        int Delete(int? id);
        bool checkSysDMType(int id);
        void Dispose();
    }
}
