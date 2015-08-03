using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface ISysDMPublicBussiness
    {
        List<SysDMPublic> getAll();
        List<SysDMPublic> getAllById(int? id);
        List<SysDMPublic> getAll(int page, int pageSize, out int pageCount);
        List<SysDMPublicModel> getAllModelByTypeId(int typeid, int page, int pageSize, out int pageCount);
        List<SysDMPublic> search(string SysDMPublicName);
        List<SysDMPublic> search(string SysDMPublicName, int page, int pageSize, out int pageCount);
        List<SysDMPublicModel> searchModel(SysDMPublicSearchModel obj, int page, int pageSize, out int pageCount);
        SysDMPublic getByIdAndCode(int? id, string code="");
        int Add(SysDMPublic SysDMPublic);
        int Update(SysDMPublic SysDMPublic);
        int Delete(int? id, string code="");
        ErrorObject checkSysDMPublic(int id, string code);
        ErrorObject checkSysDMPublic(string code);
        void Dispose();
    }
}
