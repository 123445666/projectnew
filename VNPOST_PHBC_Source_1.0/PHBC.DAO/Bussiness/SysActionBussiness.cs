using PHBC.DAO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PHBC.DAO.Bussiness
{
    public class SysActionBussiness : ISysActionBussiness
    {
        DB_PHBCEntities db;
        public SysActionBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public SysActionBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }

        /// <summary>
        /// Cap nhat action va sua menu tuong ung voi action
        /// </summary>
        /// <param name="lstAction"></param>
        /// <returns></returns>
        public int UpdateActions(List<SysAction> lstAction)
        {
            if (lstAction == null || lstAction.Count == 0)
                return 0;
            int result = 0;
            List<SysAction> lstCurrentAction = db.SysActions.ToList();
            if (lstCurrentAction == null || lstCurrentAction.Count == 0)
            {
                db.SysActions.AddRange(lstAction);
                return db.SaveChanges();
            }
            List<String> lstCode = new List<string>();
            //Neu code da xu ly roi thi khong xu ly nua
            SysAction sAction;
            bool hashChangeMenu = false;
            string sqlMenu = "Update SysMenu set Controller = {0}, Action = {1}, Area = {2}, Params = {2} WHERE ActionCode = {4};";
            foreach (SysAction item in lstAction)
            {
                if (lstCode.Any(c => c.CompareTo(item.Code) == 0))
                    continue;
                lstCode.Add(item.Code);
                sAction = lstCurrentAction.Where(c => c.Code.CompareTo(item.Code) == 0).FirstOrDefault();
                //Neu code chua co thi the moi
                if (sAction == null)
                {
                    db.SysActions.Add(item);
                }
                else
                {
                    hashChangeMenu = false;
                    if (!Utils.stringEquals(sAction.Controller, item.Controller))
                    {
                        hashChangeMenu = true;
                        sAction.Controller = item.Controller;
                    }
                    else if (!Utils.stringEquals(sAction.Action, item.Action))
                    {
                        hashChangeMenu = true;
                        sAction.Action = item.Action;
                    }
                    else if (!Utils.stringEquals(sAction.Area, item.Area))
                    {
                        hashChangeMenu = true;
                        sAction.Area = item.Area;
                    }
                    if (!Utils.stringEquals(sAction.Description, item.Description))
                    {
                        sAction.Description = item.Description;
                    }
                    if (!Utils.stringEquals(sAction.Params, item.Params))
                    {
                        hashChangeMenu = true;
                        sAction.Params = item.Params;
                    }
                    if (hashChangeMenu)
                    {
                        //"Update SysMenu set Controller = {0}, Action = {1}, Area = {2}, Params = {2} WHERE ActionCode = {4};";
                        result += db.Database.ExecuteSqlCommand(sqlMenu, item.Controller, item.Action, item.Area, item.Params, item.Code);
                    }
                }
            }
            List<string> lstRemove = lstCurrentAction.Where(s => !lstCode.Any(c => c.CompareTo(s.Code) == 0)).Select(a=>a.Code).ToList();
            if(lstRemove != null && lstRemove.Count>0)
            { 
                string sqlDeleteAction = "Delete SysAction where code in ('{0}')";
                string codeParam = string.Join("','", lstRemove);
                result = db.Database.ExecuteSqlCommand(sqlDeleteAction, codeParam);
            }
            result += db.SaveChanges();
            db.Dispose();
            return result;
        }

        /* Repository not support EF 6
        public bool UpdateActions(List<SysAction> lstAdd, List<SysAction> lstRemove, List<SysAction> lstModify, List<SysAction> lstChangeMenu)
        {
            bool result = false;
            if(lstAdd != null && lstAdd.Count >0){
                //result = AddMultiple<SysAction>(lstAdd);
            }
            if(lstModify != null && lstModify.Count >0){
                //result = UpdateMultiple<SysAction>(lstModify);
            }
            if(lstRemove != null && lstRemove.Count >0){
               // result = DeleteMultiple<SysAction>(lstRemove);
            }
            if(lstChangeMenu != null && lstChangeMenu.Count >0){
                List<SysMenu> lstMenu = new List<SysMenu>();//GetAll<SysMenu>().ToList();
                //Neu chua co menu thi dung
                if (lstMenu == null || lstMenu.Count == 0) return result;
                List<SysMenu> lstMenuMdf = (from a in lstMenu
                                            join b in lstChangeMenu
                                            on a.ActionCode equals b.Code
                                            select new SysMenu
                                            {
                                                Id = a.Id,
                                                Area = b.Area,
                                                Action = b.Area,
                                                Controller = b.Controller,
                                                ActionCode = b.Code,
                                                Description = string.IsNullOrEmpty(a.Description)? b.Description : a.Description,
                                                MenuType = a.MenuType,
                                                Name = a.Name,
                                                Order = a.Order,
                                                ParentId = a.ParentId,
                                                Params = b.Params,
                                                Pram1 = a.Pram1,
                                                Pram2 = a.Pram2,
                                                Pram3 = a.Pram3,
                                                QuerryString = a.QuerryString,
                                                Url = a.Url
                                            }
                                           ).ToList();
                //Neu khong co menu nao can sua thi dung
                if (lstMenuMdf == null || lstMenuMdf.Count == 0) return result;
                //Sửa menu
                //result = UpdateMultiple<SysAction>(lstModify);
            }
            return result;
        }
         */
        
    }
}
