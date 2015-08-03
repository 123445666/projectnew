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
    public class RoleBussiness : IRoleBussiness
    {
        DB_PHBCEntities db;
        public RoleBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public RoleBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }

        /// <summary>
        /// Lay danh sach role sắp xếp theo tên role
        /// </summary>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<AspNetRole> getAll(int roleLevel = 0)
        {
            return db.AspNetRoles.Where(a=>a.Level>roleLevel).OrderBy(a=>a.Name).ToList();
        }

        /// <summary>
        /// Lay danh sach role va phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<AspNetRole> getAll(int page, int pageSize, out int pageCount, int roleLevel = 0)
        {
            int count = db.AspNetRoles.Where(a => a.Level > roleLevel).Count();
            //neu khong co du lieu thi return
            if (count == 0)
            {
                pageCount = 0;
                return new List<AspNetRole>();
            }
            //pageSize nho hon 10 thi dat bang 10
            if (pageSize < 10)
                pageSize = 10;
            //tinh pagecount
            pageCount = count / pageSize;
            //Neu so luong con du thi tang pagecount len 1
            if (count % pageSize > 0) pageCount++;
            //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
            if (page >= pageCount)
                page = pageCount-1;
            else
                page = page - 1;//So trang bat dau tu 0
            return db.AspNetRoles.Where(a=>a.Level>roleLevel).OrderBy(a=>a.Name).Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Tim kiem role theo roleName.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<AspNetRole> search(string roleName, int roleLevel = 0)
        {
            return db.AspNetRoles.Where(r => r.Name.Contains(roleName) && r.Level>roleLevel).OrderBy(a=>a.Name).ToList();
        }

        /// <summary>
        /// Tim kiem role theo roleName và phân trang.
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<AspNetRole> search(string roleName, int page, int pageSize, out int pageCount, int roleLevel = 0)
        {
            int count = db.AspNetRoles.Where(r => r.Name.Contains(roleName) && r.Level > roleLevel).Count();
            //neu khong co du lieu thi return
            if (count == 0)
            {
                pageCount = 0;
                return new List<AspNetRole>();
            }
            //pageSize nho hon 10 thi dat bang 10
            if (pageSize < 10)
                pageSize = 10;
            //tinh pagecount
            pageCount = count / pageSize;
            //Neu so luong con du thi tang pagecount len 1
            if (count % pageSize > 0) pageCount++;
            //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
            if (page >= pageCount)
                page = pageCount - 1;
            else
                page = page - 1;//So trang bat dau tu 0
            return db.AspNetRoles.Where(r => r.Name.Contains(roleName) && r.Level > roleLevel).OrderBy(a => a.Name).Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Lay Role theo roleId
        /// </summary>
        /// <param name="id">Id cua role</param>
        /// <param name="fInclude">True - Load danh sach action va user</param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// Anhhn   15/6/15 Them tinh nang load danh sach action va user
        /// </modify>
        public AspNetRole getById(string id, bool fInclude = false)
        {
            if (fInclude == true)
                return db.AspNetRoles.Include(r=>r.AspNetUsers).Include(r=>r.SysActions).FirstOrDefault(a=>a.Id.CompareTo(id)==0);
            else
                return db.AspNetRoles.Find(id);
        }

        /// <summary>
        /// Kiem tra roleName đã tồn tại chưa. Nếu có roleId thì không check roleId đó
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public bool checkRole(string id, string roleName)
        {
            if (string.IsNullOrEmpty(id))
            {
                return db.AspNetRoles.Any(r => r.Name.CompareTo(roleName) == 0);
            }
            else
            {
                return db.AspNetRoles.Any(r => r.Name.CompareTo(roleName) == 0 && r.Id.CompareTo(id) != 0);
            }
        }

        /// <summary>
        /// Thêm mơi role vào database
        /// </summary>
        /// <param name="aspNetRole"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public ErrorObject Add(AspNetRoleModel aspNetRoleModel)
        {
            ErrorObject error = new ErrorObject();
            if (this.checkRole(string.Empty, aspNetRoleModel.Name))
            {
                error.HasError = true;
                error.LstError.Add("Name", String.Format(Enums.ErrorMessage.Exists, Utils.getDislayName<AspNetRoleModel>(a => a.Name)));
                return error;
            }
            AspNetRole aspNetRole = aspNetRoleModel.toAspNetRole();
            aspNetRole.CreateBy = aspNetRoleModel.userId;
            aspNetRole.CreateDate = DateTime.Now;
            db.AspNetRoles.Add(aspNetRole);
            if( db.SaveChanges() != 1)
            { 
                error.HasError = true;
                error.LstError.Add("", "Vai trò không thể thêm vào CSDL");
            }
            return error;
        }

        /// <summary>
        /// Cập nhập role vào database
        /// </summary>
        /// <param name="aspNetRole"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public ErrorObject Update(AspNetRoleModel aspNetRoleModel)
        {
            ErrorObject error = new ErrorObject();
            if (this.checkRole(aspNetRoleModel.Id, aspNetRoleModel.Name))
            {
                error.HasError = true;
                error.LstError.Add("Name", String.Format(Enums.ErrorMessage.Exists, Utils.getDislayName<AspNetRoleModel>(a=>a.Name)));
                return error;
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(aspNetRoleModel.Id);
            aspNetRole.Name = aspNetRoleModel.Name;
            aspNetRole.Discriminator = aspNetRoleModel.Discriminator;
            aspNetRole.Level = aspNetRoleModel.Level;
            aspNetRole.ModifyBy = aspNetRoleModel.userId;
            aspNetRole.ModifyDate = DateTime.Now;
            if (db.SaveChanges() != 1)
            {
                error.HasError = true;
                error.LstError.Add("", "Vai trò không thể cập nhật vào CSDL");
            }
            return error;
        }

        /// <summary>
        /// Xoa role khoi database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Delete(string id)
        {
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            db.AspNetRoles.Remove(aspNetRole);
            return db.SaveChanges();
        }

        /// <summary>
        /// Lấy model cho from cấu hình chức năng của role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public RoleActionModel getRoleActionModel(string id)
        {
            AspNetRole aspNetRole = db.AspNetRoles.Include("SysActions").FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            if (aspNetRole == null)
                return null;

            RoleActionModel result = new RoleActionModel();
            result.Id = aspNetRole.Id;
            result.Name = aspNetRole.Name;
            //lay danh sach da co quyen
            result.lstActions_Curent = aspNetRole.SysActions.OrderBy(a=>a.Code).ToList();
            //Lay danh sach chua co quyen
            List<SysAction> lstActionAll = db.SysActions.ToList();
            result.lstActions_noMap = lstActionAll.Where(a => !aspNetRole.SysActions.Any(c => c.Code.CompareTo(a.Code) == 0)).OrderBy(a=>a.Code).ToList();
            return result;
        }

        /// <summary>
        /// Cap nhat danh sach action vao role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lstActionCode"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int updateAction(string id, string lstActionCode)
        {      
            int result = 0;

            string sqlDelete = "Delete SysRoleActions Where RoleId = {0}";
            result = db.Database.ExecuteSqlCommand(sqlDelete, id);
            //Neu danh sach aciton khong co du lieu thi xoa Action map voi role
            if (!string.IsNullOrEmpty(lstActionCode))
            {
                string[] lstAction = lstActionCode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string sqlInsert = "Insert into SysRoleActions (RoleId, ActionCode) VALUES ({0}, {1});";
                foreach (string item in lstAction)
                    result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
            }
            return result;
        }
        public int updateActionOld(string id, string lstActionCode)
        {
            int result = 0;
            //Neu danh sach aciton khong co du lieu thi xoa Action map voi role
            if (string.IsNullOrEmpty(lstActionCode))
            {
                string sqlDelete = "Delete SysRoleActions Where RoleId = {0}";
                return db.Database.ExecuteSqlCommand(sqlDelete, id);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Include("AspNetUsers").Include("SysActions").FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            
            string[] lstAction = lstActionCode.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            //Sử lý trường hợp role đã có action=>Thêm và xóa action map với role
            if (aspNetRole.SysActions.Count > 0)
            {
                //Lay danh sach action hien co của role ma khong co trong dach sach action moi de xoa
                List<string> lstActionDelete = aspNetRole.SysActions.Where(a => !lstAction.Any(c => c.CompareTo(a.Code) == 0)).Select(a => a.Code).ToList();
                //lay danh sach action moi ma khong co trong danh sach action hien tai cua role de them moi
                List<string> lstActionAdd = lstAction.Where(a => !aspNetRole.SysActions.Any(c => c.Code.CompareTo(a) == 0)).ToList();
                if (lstActionDelete != null & lstActionDelete.Count > 0)
                {
                    string sqlDelete = "Delete SysRoleActions Where RoleId = {0} And ActionCode = {1};";
                    foreach (string item in lstActionDelete)
                        result += db.Database.ExecuteSqlCommand(sqlDelete, id, item);
                }
                if (lstActionAdd != null & lstActionAdd.Count > 0)
                {
                    string sqlInsert = "Insert into SysRoleActions (RoleId, ActionCode) VALUES ({0}, {1});";
                    foreach (string item in lstActionAdd)
                        result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
                }
            }
            else
            {//neu role chua co action nao thi thuc hien them moi
                string sqlInsert = "Insert into SysRoleActions (RoleId, ActionCode) VALUES ({0}, {1});";
                foreach (string item in lstAction)
                    result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
            }
            return result;
        }
        
        /// <summary>
        /// Lấy model thêm user cho role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public RoleUserModel getRoleUserModel(string id)
        {
            AspNetRole aspNetRole = db.AspNetRoles.Include("AspNetUsers").FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            if (aspNetRole == null)
                return null;

            RoleUserModel result = new RoleUserModel();
            result.Id = aspNetRole.Id;
            result.Name = aspNetRole.Name;
            int active = (int)Enums.RecordStatusCode.active;
            //lay danh sach da co quyen
            if (aspNetRole.AspNetUsers == null || aspNetRole.AspNetUsers.Count == 0)
                result.lstUser_Curent = new List<UserModel>();
            else
            {
                List<UserInfo> userInfo = db.UserInfoes.Include(c => c.Unit).ToList();
                result.lstUser_Curent = (from a in userInfo
                                         join b in aspNetRole.AspNetUsers on a.Id equals b.Id
                                         where a.Status == active
                                         select new UserModel()
                                         {
                                             UnitName = a.Unit == null ? a.UnitCode : a.Unit.UnitName,
                                             Id = a.Id,
                                             UserName = b.UserName,
                                             Email = b.Email,
                                             DislayName = a.DislayName,
                                             UnitCode = a.UnitCode
                                         }).ToList();//aspNetRole.AspNetUsers.ToList();
            }
            //Lay danh sach chua co quyen
            var query = (from b in db.AspNetUsers
                        join a in db.UserInfoes.Include(c => c.Unit) on b.Id equals a.Id
                        where a.Status == active
                        && a.Level <= aspNetRole.Level //cap user phai cao hon cap role
                        select new UserModel()
                        {
                            UnitName = a.Unit == null ? a.UnitCode : a.Unit.UnitName,
                            Id = a.Id,
                            UserName = b.UserName,
                            Email = b.Email,
                            DislayName = a.DislayName,
                            UnitCode = a.UnitCode
                        }).ToList();

            if (aspNetRole.AspNetUsers == null || aspNetRole.AspNetUsers.Count == 0)
                result.lstUser_noMap = query;
            else
                result.lstUser_noMap = query.Where(a=>!aspNetRole.AspNetUsers.Any(u=>u.Id.CompareTo(a.Id)==0)).ToList();
            return result;
        }

        /// <summary>
        /// Cập nhật danh sách user cho role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lstUserId"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int updateUser(string id, string lstUserId)
        {
            int result = 0;

            string sqlDelete = "Delete AspNetUserRoles Where RoleId = {0}";
            result = db.Database.ExecuteSqlCommand(sqlDelete, id);
            //Neu danh sach user moi khong co du lieu thi thuc hien xoa user map voi role
            if (!string.IsNullOrEmpty(lstUserId))
            {
                string[] lstUser = lstUserId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                string sqlInsert = "Insert into AspNetUserRoles (RoleId, UserId) VALUES ({0}, {1});";
                foreach (string item in lstUser)
                    result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
            }
            return result;
        }
        public int updateUserOld(string id, string lstUserId)
        {
            int result = 0;
            //Neu danh sach user moi khong co du lieu thi thuc hien xoa user map voi role
            if (string.IsNullOrEmpty(lstUserId))
            {
                string sqlDelete = "Delete AspNetUserRoles Where RoleId = {0}";
                return db.Database.ExecuteSqlCommand(sqlDelete, id);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Include("AspNetUsers").Include("SysActions").FirstOrDefault(a => a.Id.CompareTo(id) == 0);

            string[] lstUser = lstUserId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //Thuc hien truong hop role da map voi user=>Them vao xoa user
            if (aspNetRole.AspNetUsers.Count > 0)
            {
                //Lay danh sach user hien tai khong co trong danh sach moi de xoa
                List<string> lstUserDelete = aspNetRole.AspNetUsers.Where(a => !lstUser.Any(c => c.CompareTo(a.Id) == 0)).Select(a => a.Id).ToList();
                //Lay danh sach user moi khong co trong danh sach hien tai de them moi
                List<string> lstUserAdd = lstUser.Where(a => !aspNetRole.AspNetUsers.Any(c => c.Id.CompareTo(a) == 0)).ToList();
                if (lstUserDelete != null & lstUserDelete.Count > 0)
                {
                    string sqlDelete = "Delete AspNetUserRoles Where RoleId = {0} And UserId = {1};";
                    foreach (string item in lstUserDelete)
                        result += db.Database.ExecuteSqlCommand(sqlDelete, id, item);
                }
                if (lstUserAdd != null & lstUserAdd.Count > 0)
                {
                    string sqlInsert = "Insert into AspNetUserRoles (RoleId, UserId) VALUES ({0}, {1});";
                    foreach (string item in lstUserAdd)
                        result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
                }
            }
            else
            {//Neu role chua duoc map voi user thi thuc hien them moi
                string sqlInsert = "Insert into AspNetUserRoles (RoleId, UserId) VALUES ({0}, {1});";
                foreach (string item in lstUser)
                    result += db.Database.ExecuteSqlCommand(sqlInsert, id, item);
            }
            return result;
        }

        public List<DefineSelectItem> buildListLevel(int curentLevel, bool fDefault=false)
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();
            if(fDefault)
                result.Add(new DefineSelectItem() { Value = "", Text = "---Chọn Level---" });
            int sLevel = (int)Enums.RoleLevel.VN_POST;
            if(sLevel>=curentLevel)
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
        private IQueryable<AspNetRoleModel> BuildQuery(RoleSearchModel search, bool fInclude = false)
        {
            string include = string.Empty;
            IQueryable<AspNetRole> qRole;
            if (fInclude)
                qRole = db.AspNetRoles.Include(a => a.SysActions).Include(a => a.AspNetUsers);
            else
                qRole = db.AspNetRoles;
            IQueryable<AspNetRoleModel> query = from u in qRole
                                                select new AspNetRoleModel()
                                          {
                                              Id = u.Id,
                                              Name = u.Name,
                                              Discriminator = u.Discriminator,
                                              Level = u.Level
                                          };
            if (search != null)
            {
                if (string.IsNullOrWhiteSpace(search.Name))
                    query = query.Where(a => a.Name.Contains(search.Name));
                if (string.IsNullOrWhiteSpace(search.Discriminator))
                    query = query.Where(a => a.Discriminator.Contains(search.Discriminator));
                if (search.Level.HasValue && search.Level.Value > 0)
                    query = query.Where(a => a.Level == search.Level);
                if (string.IsNullOrWhiteSpace(search.Id))
                    query = query.Where(a => a.Id.CompareTo(search.Id) == 0);
            }
            return query;
        }

        //Ham huy
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
