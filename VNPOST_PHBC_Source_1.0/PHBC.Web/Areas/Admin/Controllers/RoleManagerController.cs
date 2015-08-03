using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PHBC.DAO;
using PHBC.DAO.Models;
using PHBC.DAO.Bussiness;
using PHBC.DAO.Common;
using PHBC.Web.Permission;
using NWebsec.Mvc.HttpHeaders.Csp;
using PHBC.Web.Base;

namespace PHBC.Web.Areas.Admin.Controllers
{

    [Authorize]
    public class RoleManagerController : BaseController
    {

        IRoleBussiness iRoleBussiness;

        public RoleManagerController(IRoleBussiness _iRoleBussiness)
        {
            iRoleBussiness = _iRoleBussiness;
        }

        // GET: Admin/RoleManager
        public ActionResult Index()
        {
            
            return View(iRoleBussiness.getAll(userInfo.Level));
        }

        // GET: Admin/RoleManager/Details/5
        
        public ActionResult Details(string id)
        {
            //PermissonRole perRole = ViewBag.Permisson as PermissonRole;
            //if (!perRole.IsIndex)
                //return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                //return new HttpUnauthorizedResult();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = iRoleBussiness.getById(id, true);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // GET: Admin/RoleManager/Create
        public ActionResult Create()
        {
            ViewBag.Level = new SelectList(iRoleBussiness.buildListLevel(userInfo.Level), "Value", "Text");
            return View();
        }

        // POST: Admin/RoleManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Discriminator,Level")] AspNetRoleModel aspNetRoleModel)
        {
            if (ModelState.IsValid)
            {
                ErrorObject error = iRoleBussiness.Add(aspNetRoleModel);
                if(error.HasError)
                {
                    this.buildError(error);
                    ViewBag.Level = new SelectList(iRoleBussiness.buildListLevel(userInfo.Level), "Value", "Text", aspNetRoleModel.Level);
                    return View(aspNetRoleModel);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Level = new SelectList(iRoleBussiness.buildListLevel(0), "Value", "Text", aspNetRoleModel.Level);
            return View(aspNetRoleModel);
        }

        // GET: Admin/RoleManager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = iRoleBussiness.getById(id);

            ViewBag.Level = new SelectList(iRoleBussiness.buildListLevel(userInfo.Level), "Value", "Text", aspNetRole.Level);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(new AspNetRoleModel(aspNetRole));
        }

        // POST: Admin/RoleManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Discriminator,Level")] AspNetRoleModel aspNetRoleModel)
        {
            if (ModelState.IsValid)
            {
                ErrorObject error = iRoleBussiness.Update(aspNetRoleModel);
                if (error.HasError)
                {
                    this.buildError(error);
                    ViewBag.Level = new SelectList(iRoleBussiness.buildListLevel(userInfo.Level), "Value", "Text", aspNetRoleModel.Level);
                    return View(aspNetRoleModel);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Level = new SelectList(iRoleBussiness.buildListLevel(userInfo.Level), "Value", "Text", aspNetRoleModel.Level);
            return View(aspNetRoleModel);
        }

        // GET: Admin/RoleManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = iRoleBussiness.getById(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        // POST: Admin/RoleManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            iRoleBussiness.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string roleName)
        {
            if(string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index");

            return View("Index", iRoleBussiness.search(roleName, userInfo.Level));
        }

        // GET: Admin/RoleManager/Create
        public ActionResult AddAction(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleActionModel roleActionModel = iRoleBussiness.getRoleActionModel(id);
            if (roleActionModel == null)
            {
                return HttpNotFound();
            }
            return View(roleActionModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAction(string id, string lstAction)
        {
            iRoleBussiness.updateAction(id, lstAction);
            return RedirectToAction("Index");
        }

        // GET: Admin/RoleManager/Create
        public ActionResult AddUser(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleUserModel roleUserModel = iRoleBussiness.getRoleUserModel(id);
            if (roleUserModel == null)
            {
                return HttpNotFound();
            }
            return View(roleUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(string id, string lstUser)
        {
            iRoleBussiness.updateUser(id, lstUser);
            return RedirectToAction("Index");
        }
       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                iRoleBussiness.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
