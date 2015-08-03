using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public class MenuBussiness : IMenuBussiness
    {
        DB_PHBCEntities db;
        public MenuBussiness()
        {
            this.db = new DB_PHBCEntities();
        }

        public MenuBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public List<Models.MenuTree> getMenuTree()
        {
            throw new NotImplementedException();
        }

        public List<MenuModel> getAllMenu()
        {
            List<MenuModel> result = new List<MenuModel>();
            result = (from t in db.SysMenus.Include("SysAction").Include("SysMenu2")
                      select new Models.MenuModel()
                      {
                        Id = t.Id,
                        Name = t.Name,
                        ActionCode = t.ActionCode,
                        Area = t.Area,
                        Controller = t.Controller,
                        Action = t.Action,
                        Params = t.Params,
                        Pram1 = t.Pram1,
                        Pram2 = t.Pram2,
                        Pram3 = t.Pram3,
                        QuerryString = t.QuerryString,
                        ParentId = t.ParentId,
                        Order = t.Order,
                        Description = t.Description,
                        MenuType = t.MenuType,
                        SysAction = t.SysAction,
                        SysMenu1 = t.SysMenu1,
                        SysMenu2 = t.SysMenu2
                      }).OrderBy(x =>x.Order).ToList();
            return result;
        }


        public List<Models.MenuView> getMenuView()
        {
            throw new NotImplementedException();
        }

        public int AddMenu(Models.MenuModel menu)
        {
            SysAction sysaction = new SysAction();
            sysaction = db.SysActions.Find(menu.ActionCode);
            if (sysaction != null)
            {
                menu.Action = sysaction.Action;
                menu.Area = sysaction.Area;
                menu.Controller = sysaction.Controller;
                menu.Icon = sysaction.Icon;
            }
            db.SysMenus.Add(menu.toSysMenu());
            return db.SaveChanges();
        }

        public int Edit(Models.MenuModel menu)
        {
            SysAction sysaction = new SysAction();
            sysaction = db.SysActions.Find(menu.ActionCode);
            if (sysaction != null)
            {
                menu.Action = sysaction.Action;
                menu.Area = sysaction.Area;
                menu.Controller = sysaction.Controller;
                menu.Icon = sysaction.Icon;
            }
            db.Entry(menu.toSysMenu()).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(string menuId)
        {
            SysMenu sysmenu = db.SysMenus.Find(menuId);
            db.SysMenus.RemoveRange(db.SysMenus.Where(t => t.ParentId.Equals(menuId)));
            db.SysMenus.Remove(sysmenu);
            return db.SaveChanges();
        }

        public Models.MenuModel getMenuById(string id)
        {
            return new MenuModel(db.SysMenus.Find(id));
        }

        public Models.MenuModel getMenuByIdWithChild(string id)
        {
            return new MenuModel(db.SysMenus.Include("SysMenu1").SingleOrDefault(x => x.Id == id));
        }

        public List<SysAction> getAction()
        {
            return db.SysActions.Where(t => t.IsMenu == true).ToList();
        }

        public List<SysAction> getAction(int page, int pageSize, out int pageCount)
        {
            throw new NotImplementedException();
        }

        public List<SysMenu> getAllMenuIsParent()
        {
            return db.SysMenus.Where(t => t.ParentId == null ).ToList();
        }
        public List<SysMenu> getAllMenuIsParent(string Id)
        {
            return db.SysMenus.Where(t => t.ParentId == null && t.Id != Id).ToList();
        }
        public List<SysMenu> checkChild(string Id)
        {
            return db.SysMenus.Where(t => t.ParentId == Id).ToList();
        }

    }
}
