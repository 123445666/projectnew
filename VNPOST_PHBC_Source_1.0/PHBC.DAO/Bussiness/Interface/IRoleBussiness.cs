using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface IRoleBussiness
    {
        List<AspNetRole> getAll(int roleLevel=0);
        List<AspNetRole> getAll(int page, int pageSize, out int pageCount, int roleLevel = 0);
        List<AspNetRole> search(string roleName,int roleLevel=0);
        List<AspNetRole> search(string roleName, int page, int pageSize, out int pageCount,int roleLevel=0);
        AspNetRole getById(string id, bool fInclude = false);
        bool checkRole(string id, string roleName);
        ErrorObject Add(AspNetRoleModel aspNetRoleModel);
        ErrorObject Update(AspNetRoleModel aspNetRoleModel);
        int Delete(string id);
        RoleActionModel getRoleActionModel(string id);
        int updateAction(string id, string lstActionCode);
        RoleUserModel getRoleUserModel(string id);
        int updateUser(string id, string lstUserId);
        List<DefineSelectItem> buildListLevel(int curentLevel, bool fDefault = false);

        void Dispose();
    }
}
