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
    public class UserBussiness : IUserBussiness
    {
        private DB_PHBCEntities db;
        public UserBussiness()
        {
            this.db = new DB_PHBCEntities();
        }
        public UserBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;
        }
        public List<UserModel> getAllUserModel()
        {
            int active = (int)Enums.RecordStatusCode.active;
            return BuildQuery(null, active).ToList();
        }
        public List<UserModel> getAllUserModel(int page,ref int pageSize, out int pageCount)
        {
            int active = (int)Enums.RecordStatusCode.active;
            IQueryable<UserModel> query = BuildQuery(null, active);
            return Utils.buildPage(query, page,ref pageSize, out pageCount);
        }
        public List<UserModel> getAllUserModel(int page, ref int pageSize, out int totalItemCount, out int pageCount)
        {
            int active = (int)Enums.RecordStatusCode.active;
            IQueryable<UserModel> query = BuildQuery(null, active);
            return Utils.buildPage(query, page, ref pageSize, out totalItemCount, out pageCount);
        }
        public List<UserModel> searchUserModel(UserSearchModel search)
        {
            int active = (int)Enums.RecordStatusCode.active;
            return BuildQuery(search, active).ToList();
        }
        public List<UserModel> searchUserModel(UserSearchModel search, int page,ref int pageSize, out int pageCount)
        {
            int active = (int)Enums.RecordStatusCode.active;
            IQueryable<UserModel> query = BuildQuery(search, active);
            return Utils.buildPage(query, page,ref pageSize, out pageCount);
        }
        public List<UserModel> searchUserModel(UserSearchModel search, int page, ref int pageSize, out int totalItemCount, out int pageCount)
        {
            int active = (int)Enums.RecordStatusCode.active;
            IQueryable<UserModel> query = BuildQuery(search, active);
            return Utils.buildPage(query, page, ref pageSize, out totalItemCount, out pageCount);
        }
        public UserModel getById(string id, bool FInclude = false)
        {
            UserSearchModel search = new UserSearchModel() { Id = id };
            int active = (int)Enums.RecordStatusCode.active;
            return BuildQuery(search, active, FInclude).FirstOrDefault();
        }
        public UserModel getByUserName(string userName, bool FInclude = false)
        {
            int active = (int)Enums.RecordStatusCode.active;
            UserModel result = (from u in db.AspNetUsers
                         join uf in db.UserInfoes on u.Id equals uf.Id
                         join unit in db.v_Unit on uf.UnitCode equals unit.UnitCode
                         where uf.Status == active
                         && u.UserName.Equals(userName)
                         select new UserModel()
                         {
                             Id = u.Id,
                             Email = u.Email,
                             DislayName = uf.DislayName,
                             UnitCode = uf.UnitCode,
                             UnitName = unit.UnitName,
                             UserName = u.UserName,
                             PhoneNumber = u.PhoneNumber,
                             Level = uf.Level,
                             ProvinCode = unit.ProvinceCode,
                             DistrictCode = unit.DistrictCode
                         }).FirstOrDefault();
            return result;
        }
        public int AddUser(UserModel user)
        {
            var userInfor = new UserInfo() ;
            userInfor.Id = user.Id;
            userInfor.DislayName = user.DislayName;
            userInfor.UnitCode = user.UnitCode;
            userInfor.Level = user.Level;
            userInfor.CreateBy = user.CreateBy;
            userInfor.CreateDate = user.CreateDate;
            userInfor.Status = (int)Enums.RecordStatusCode.active;
            db.UserInfoes.Add(userInfor);
            return db.SaveChanges();  
        }
        public int DeleteUser(string id)
        {
            UserInfo userInfor = db.UserInfoes.Find(id);
            userInfor.Status = (int)Enums.RecordStatusCode.delete;
            db.Entry(userInfor).State = EntityState.Modified;
            return db.SaveChanges();
        }
        public int EditUser(UserModel user)
        {
            UserInfo userInfor = db.UserInfoes.Find(user.Id);
            userInfor.DislayName = user.DislayName;
            userInfor.UnitCode = user.UnitCode;
            userInfor.Level = user.Level;
            userInfor.Status = (int)Enums.RecordStatusCode.active;
            userInfor.ModifyBy = user.ModifyBy;
            userInfor.ModifyDate = user.ModifyDate;
            db.Entry(userInfor).State = EntityState.Modified;
            return db.SaveChanges();    
        }
        public int AddRoles(string id, string lstRole)
        {
            int result = 0;

            string sqlDelete = "Delete AspNetUserRoles Where UserId = {0}";
            result = db.Database.ExecuteSqlCommand(sqlDelete, id);
            //Neu danh sach user moi khong co du lieu thi thuc hien xoa user map voi role
            if (!string.IsNullOrEmpty(lstRole))
            {
                string[] lstRoleInsert = lstRole.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string sqlInsert = "Insert into AspNetUserRoles (UserId, RoleId) VALUES ({0}, {1});";
                foreach (string item in lstRoleInsert)
                    result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
            }
            return result;
        }
        public UserRoleModel getUserRoleModel(string id)
        {
            UserModel user = this.getById(id, true);
            if (user == null)
                return null;

            UserRoleModel result = new UserRoleModel();
            result.Id = user.Id;
            result.UserName = user.UserName;
            //result.DislayName = aspNetUser.
            //lay danh sach da co quyen
            result.Role_Current = user.LstRole.OrderBy(a => a.Name).ToList();
            //Lay danh sach chua co quyen
            List<AspNetRole> lstRoleAll = db.AspNetRoles.Where(a=>a.Level>=user.Level).ToList();//cap role phai thap hon cap user(cap cao nhat la 1)
            result.Role_NotMap = lstRoleAll.Where(a => !user.LstRole.Any(c => c.Id.CompareTo(a.Id) == 0)).OrderBy(a => a.Name).ToList();
            return result;
        }
        public List<string> getActionCodeByUserName(string userName)
        {
            List<string> result = new List<string>();
            string userId = db.AspNetUsers.Where(a=>a.UserName.ToLower().Equals(userName.ToLower())).Select(a=>a.Id).FirstOrDefault();
            int active = (int)Enums.RecordStatusCode.active;
            if (db.UserInfoes.Any(a => a.Status == active && a.Id.CompareTo(userId) == 0))
            {
                result = db.AspNetUsers.Include(a => a.AspNetRoles).Include("AspNetRoles.SysActions").Where(a => a.Id.CompareTo(userId) == 0).SelectMany(a => a.AspNetRoles.SelectMany(b => b.SysActions)).Select(a => a.Code).Distinct().ToList();
            }
            return result;
        }

        
        public List<SysAction> getActionByUserName(string userName)
        {
            string userId = db.AspNetUsers.Where(a=>a.UserName.ToLower().Equals(userName.ToLower())).Select(a=>a.Id).FirstOrDefault();
            int active = (int)Enums.RecordStatusCode.active;
            if (db.UserInfoes.Any(a => a.Status == active && a.Id.CompareTo(userId) == 0))
            {
                return db.AspNetUsers.Include(a => a.AspNetRoles).Include("AspNetRoles.SysActions").Where(a => a.Id.CompareTo(userId) == 0).SelectMany(a => a.AspNetRoles.SelectMany(b => b.SysActions)).Distinct().OrderBy(a=>a.Code).ToList();
            }
            return null;
        }
        private IQueryable<UserModel> BuildQuery(UserSearchModel search, int status = 1, bool fInclude = false)
        {
            string include = string.Empty;
            IQueryable<AspNetUser> qUser;
            if (fInclude)
                qUser = db.AspNetUsers.Include(a => a.AspNetRoles);
            else
                qUser = db.AspNetUsers;
            IQueryable<UserModel> query = (from u in qUser
                                          join uf in db.UserInfoes.Include(c => c.Unit) on u.Id equals uf.Id
                                          where uf.Status == status
                                          select new UserModel()
                                          {
                                              Id = u.Id,
                                              Email = u.Email,
                                              DislayName = uf.DislayName,
                                              UnitCode = uf.UnitCode,
                                              UnitName = uf.Unit == null ? uf.UnitCode : uf.Unit.UnitName,
                                              UserName = u.UserName,
                                              PhoneNumber = u.PhoneNumber,
                                              Level = uf.Level,
                                              LstRole = u.AspNetRoles.ToList()
                                          }).OrderBy(a=>a.UserName);
            if(search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.DislayName))
                    query = query.Where(a => a.DislayName.Contains(search.DislayName));
                if (!string.IsNullOrWhiteSpace(search.Email))
                    query = query.Where(a => a.Email.Contains(search.Email));
                if (search.Level > 0)
                    query = query.Where(a => a.Level == search.Level);
                if (!string.IsNullOrWhiteSpace(search.UnitCode))
                    query = query.Where(a => a.UnitCode.CompareTo(search.UnitCode)==0);
                if (!string.IsNullOrWhiteSpace(search.UserName))
                    query = query.Where(a => a.UserName.Contains(search.UserName));
                if (!string.IsNullOrWhiteSpace(search.Id))
                    query = query.Where(a => a.Id.CompareTo(search.Id)==0);
            }
            return query;
        }
        public List<DefineSelectItem> buildListLevel(int curentLevel, bool fDefault = false)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();
            if (fDefault)
                result.Add(new DefineSelectItem() { Value = "", Text = "---Chọn Level---" });
            int sLevel = (int)Enums.RoleLevel.VN_POST;
            if (sLevel >= curentLevel)
                result.Add(new DefineSelectItem() { Value = sLevel.ToString(), Text = Enums.RoleLevelDesc.VN_POST });
            sLevel = (int)Enums.RoleLevel.PHBC_TW;
            if (sLevel >= curentLevel)
                result.Add(new DefineSelectItem() { Value = sLevel.ToString(), Text = Enums.RoleLevelDesc.PHBC_TW });
            sLevel = (int)Enums.RoleLevel.BDT;
            if (sLevel >= curentLevel)
                result.Add(new DefineSelectItem() { Value = sLevel.ToString(), Text = Enums.RoleLevelDesc.BDT });
            sLevel = (int)Enums.RoleLevel.BDH;
            if (sLevel >= curentLevel)
                result.Add(new DefineSelectItem() { Value = sLevel.ToString(), Text = Enums.RoleLevelDesc.BDH });
            return result;
        }
        public List<Unit> getAllUnit()
        {
            return db.Units.OrderBy(a=>a.UnitCode).ToList();
        }
        public List<MenuView> getMenuByAction(List<string> lstAction, bool FAdmin = false)
        {
            List<MenuView> result = new List<MenuView>();
            List<SysMenu> lstMenu = db.SysMenus.Include(a => a.SysMenu1).Where(a => a.ParentId == null || a.ParentId == "").OrderBy(a => a.Order).ToList();
            foreach(SysMenu item in lstMenu)
            {
                if (!FAdmin && !lstAction.Any(a => a.Equals(item.ActionCode)) && !item.SysMenu1.Any(a => lstAction.Any(c => c.Equals(a.ActionCode))))
                    continue;
                MenuView menuView = new MenuView() { MenuName = item.Name, Action = item.Action, Area = item.Area, Controller = item.Controller, Id = item.Id };
                if (FAdmin)
                {
                    menuView.ChildMenu = (from a in item.SysMenu1.OrderBy(c => c.Order)
                                          select new MenuView()
                                          {
                                              MenuName = a.Name,
                                              Action = a.Action,
                                              Area = a.Area,
                                              Controller = a.Controller,
                                              Id = a.Id
                                          }).ToList();
                }
                else
                {
                    menuView.ChildMenu = (from a in item.SysMenu1.OrderBy(c => c.Order)
                                          join b in lstAction on a.ActionCode equals b
                                          select new MenuView()
                                          {
                                              MenuName = a.Name,
                                              Action = a.Action,
                                              Area = a.Area,
                                              Controller = a.Controller,
                                              Id = a.Id
                                          }).ToList();
                }
                result.Add(menuView);
            }
            return result;
        }

        private string defineLevel = "-";       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="curentUnit"></param>
        /// <returns></returns>
        public List<DefineSelectItem> getListUnitbyUnitCode(string curentUnit)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();
            int length = curentUnit.Length;
            List<Unit> lstUnit;
            if (curentUnit.Equals("00"))
            {
                lstUnit = db.Units.OrderBy(a => a.UnitCode).ToList();
                length = 0;
            }
            else
                lstUnit = db.Units.Where(a => a.UnitCode.StartsWith(curentUnit)).OrderBy(a => a.UnitCode).ToList();
            result = lstUnit.Select(a => getUnitForDrop(a,length)).ToList();
            return result;
        }

        private DefineSelectItem getUnitForDrop(Unit unit,int  level)
        {
            string s="";
            if (!string.IsNullOrEmpty(unit.ParentUnitCode))
            {
                int Ulevel = unit.UnitCode.Length;
                for (int i = level; i < Ulevel; i++)
                    s += defineLevel;
            }
            return new DefineSelectItem() { Text = s + unit.UnitName, Value = unit.UnitCode };
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
