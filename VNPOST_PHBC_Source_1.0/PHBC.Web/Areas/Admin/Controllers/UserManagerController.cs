using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PHBC.DAO;
using PHBC.DAO.Common;
using PHBC.DAO.Bussiness;
using PHBC.DAO.Models;
using System.Threading.Tasks;
using PHBC.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Webdiyer.WebControls.Mvc;
using PHBC.Web.Base;
using PHBC.Web.Constants;
using PHBC.Web.Permission;

namespace PHBC.Web.Areas.Admin.Controllers
{
    
    public class UserManagerController : BaseController
    {
        private int pageSize = 20;
        private ApplicationUserManager _userManager;
        IUserBussiness iUserBussiness;
        public UserManagerController(IUserBussiness _iUserBussiness)
        {
            this.iUserBussiness = _iUserBussiness;
        }
        public UserManagerController(IUserBussiness _iUserBussiness, ApplicationUserManager userManager)
        {
            this.iUserBussiness = _iUserBussiness;
            this._userManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult LoadMenu()
        {
            if (Session[Application.Session.Permisson] == null)
            {
                return Content("");
            }
            AppPermission appPermission = Session[Application.Session.Permisson] as AppPermission;
            if (appPermission.getMenu == null || appPermission.getMenu.Count == 0)
                return Content("");
            return PartialView("_MenuH", appPermission.getMenu);
        }
               
        // GET: Admin/UserManager
        public ActionResult Index(string pageindex)
        {
            Session[Application.Session.ModelSearch] = null;
            int page = 1;
            if (!string.IsNullOrWhiteSpace(pageindex))
                page = int.Parse(pageindex.Replace('/', '\0'));
            int pageCount = 0;
            int totalItemCount = 0;
            return View(new PagedList<UserModel>(iUserBussiness.getAllUserModel(page, ref pageSize, out totalItemCount, out pageCount), page, pageSize, totalItemCount));
        }

        // GET: Admin/UserManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel user = iUserBussiness.getById(id, true);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/UserManager/Create
        public ActionResult Create()
        {
            ViewBag.Level = new SelectList(iUserBussiness.buildListLevel(0), "Value", "Text");

            List<DefineSelectItem> items = iUserBussiness.getListUnitbyUnitCode(userInfo.UnitCode);
            items.Insert(0, (new DefineSelectItem { Text = "--- Chọn đơn vị ---", Value = "" }));
            ViewBag.UnitCode = new SelectList(items, "Value", "Text");

            return View();
        }

        // POST: Admin/UserManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,DislayName,PhoneNumber,Level,UserName,UnitCode")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userModel.UserName, Email = userModel.Email, PhoneNumber = userModel.PhoneNumber };
                
                var result = await UserManager.CreateAsync(user, "P@ssw0rd");
                if(result.Succeeded)
                {                    
                    userModel.Id = UserManager.FindByName(userModel.UserName).Id;
                    userModel.CreateBy = userInfo.Id;
                    userModel.CreateDate = DateTime.Now;
                    iUserBussiness.AddUser(userModel);
                    return RedirectToAction("Index");
                }
                else
                {
                    base.buildError(result);
                    ViewBag.Level = new SelectList(iUserBussiness.buildListLevel(0), "Value", "Text", userModel.Level);

                    List<DefineSelectItem> items = iUserBussiness.getListUnitbyUnitCode(userInfo.UnitCode);
                    items.Insert(0, (new DefineSelectItem { Text = "--- Chọn đơn vị ---", Value = "" }));
                    ViewBag.UnitCode = new SelectList(items, "Value", "Text", userModel.UnitCode);
                    return View(userModel);
                }
            }
            ViewBag.Level = new SelectList(iUserBussiness.buildListLevel(0), "Value", "Text", userModel.Level);

            List<DefineSelectItem> lstitems = iUserBussiness.getListUnitbyUnitCode(userInfo.UnitCode);
            lstitems.Insert(0, (new DefineSelectItem { Text = "--- Chọn đơn vị ---", Value = "" }));
            ViewBag.UnitCode = new SelectList(lstitems, "Value", "Text", userModel.UnitCode);
            return View(userModel);
        }

        // GET: Admin/UserManager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            UserModel user = iUserBussiness.getById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            ViewBag.Level = new SelectList(iUserBussiness.buildListLevel(userInfo.Level), "Value", "Text", user.Level);

            List<DefineSelectItem> items = iUserBussiness.getListUnitbyUnitCode(userInfo.UnitCode);
            items.Insert(0, (new DefineSelectItem { Text = "--- Chọn đơn vị ---", Value = "" }));
            ViewBag.UnitCode = new SelectList(items, "Value", "Text", user.UnitCode);

            return View(user);
        }

        // POST: Admin/UserManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,DislayName,PhoneNumber,Level,UserName,UnitCode")] UserModel user)
        {
            if (ModelState.IsValid)
            {
                user.ModifyBy = userInfo.Id;
                user.ModifyDate = DateTime.Now;
                iUserBussiness.EditUser(user);
                return RedirectToAction("Index");
            }

            ViewBag.Level = new SelectList(iUserBussiness.buildListLevel(userInfo.Level), "Value", "Text", user.Level);

            List<DefineSelectItem> items = iUserBussiness.getListUnitbyUnitCode(userInfo.UnitCode);
            items.Insert(0, (new DefineSelectItem { Text = "--- Chọn đơn vị ---", Value = "" }));
            ViewBag.UnitCode = new SelectList(items, "Value", "Text", user.UnitCode);

            return View(user);
        }

        // GET: Admin/UserManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel user = iUserBussiness.getById(id, true);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/UserManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            iUserBussiness.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public ActionResult SetPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel user = iUserBussiness.getById(id, true);
            SetPasswordViewModel setPassModel = new SetPasswordViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                DislayName = user.DislayName,
                UnitName = user.UnitName
            };
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(setPassModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result=null;
                if(UserManager.HasPassword(model.Id))
                {
                    result = await UserManager.RemovePasswordAsync(model.Id);
                }
                if (result == null || result.Succeeded)
                {
                    result = await UserManager.AddPasswordAsync(model.Id, model.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                }
                base.buildError(result);
            }
            return View(model);
        }
        // GET: Admin/UserManager/Search/5
        public ActionResult Search(string pageindex)
        {
            int page = 1;
            UserSearchModel search = Session[Constants.Application.Session.ModelSearch] as UserSearchModel;
            if (!String.IsNullOrEmpty(pageindex))
            {
                page = int.Parse(pageindex.Replace('/', '\0'));
            }
            if (search == null)
            {
                return RedirectToAction("Index");
            }

            ActionResult ars;
            ars = this.Search(search, page);
            return ars;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "UserName, Email, UnitCode, DislayName")] UserSearchModel userModel, int? pageIndex)
        {
            int page = 0;
            int pageCount = 0;
            if (String.IsNullOrEmpty(userModel.Id) && String.IsNullOrEmpty(userModel.UserName))
            {
                return RedirectToAction("Index");
            }
            if (!String.IsNullOrEmpty(Convert.ToString(pageIndex)))
            {
                page = pageIndex.Value;
            }
            else page = 1;
            int a = 2;
            int totalitem = 0;
            UserSearchModel userSearch = new UserSearchModel();
            userSearch.Id = userModel.Id;
            userSearch.UserName = userModel.UserName;

            List<UserModel> value = iUserBussiness.searchUserModel(userSearch, page, ref a , out pageCount, out totalitem);
            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchModel = userModel;
            Session[Constants.Application.Session.ModelSearch] = userModel;
            return View("Index", new PagedList<UserModel>(value, page, pageCount, totalitem));
        }
        public ActionResult AddRole(string id)
        {
            return View(iUserBussiness.getUserRoleModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRole(string id, string lstRole)
        {
            int n = iUserBussiness.AddRoles(id, lstRole);
            buildMessage("Thêm thành công " + n + " Vai trò");
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                iUserBussiness.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
