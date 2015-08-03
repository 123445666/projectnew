using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHBC.DAO.Models;

namespace PHBC.DAO.Bussiness
{
    public interface IUserBussiness
    {
        List<UserModel> getAllUserModel();
        List<UserModel> getAllUserModel(int page, ref int pageSize, out int pageCount);
        List<UserModel> getAllUserModel(int page, ref int pageSize, out int totalItemCount, out int pageCount);
        List<UserModel> searchUserModel(UserSearchModel search);
        List<UserModel> searchUserModel(UserSearchModel search, int page, ref int pageSize, out int pageCount);
        List<UserModel> searchUserModel(UserSearchModel search, int page, ref int pageSize, out int totalItemCount, out int pageCount);
        UserModel getById(string id, bool FInclude = false);
        UserModel getByUserName(string userName, bool FInclude = false);    
        int AddUser(UserModel user);        
        int DeleteUser(string id);
        int EditUser(UserModel user);
        int AddRoles(string id, string lstRole);
        UserRoleModel getUserRoleModel(string id);
        List<string> getActionCodeByUserName(string userName);
        List<SysAction> getActionByUserName(string userName);
        List<MenuView> getMenuByAction(List<string> lstAction, bool FAdmin = false);
        List<DefineSelectItem> buildListLevel(int curentLevel, bool fDefault = false);
        List<Unit> getAllUnit();
        List<DefineSelectItem> getListUnitbyUnitCode(string curentUnit);
        void Dispose();
    }
}
