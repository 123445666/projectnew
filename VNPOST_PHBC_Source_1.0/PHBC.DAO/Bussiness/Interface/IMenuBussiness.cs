using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface IMenuBussiness
    {
        List<MenuTree> getMenuTree();
        List<MenuView> getMenuView();
        List<MenuModel> getAllMenu();
        int AddMenu(MenuModel menu);
        int Edit(MenuModel menu);
        int Delete(string menuId);
        MenuModel getMenuById(string id);
        MenuModel getMenuByIdWithChild(string id);
        List<SysAction> getAction();
        List<SysAction> getAction(int page, int pageSize, out int pageCount);
        List<SysMenu> getAllMenuIsParent();
        List<SysMenu> getAllMenuIsParent(string Id);
        List<SysMenu> checkChild(string Id);
        void Dispose();
    }
}
