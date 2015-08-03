using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PHBC.DAO;
using PHBC.DAO.Bussiness;
using PHBC.DAO.Models;
using PHBC.Web.Base;

namespace PHBC.Web.Areas.Admin.Controllers
{
    public class SysMenuController : BaseController
    {
        //private DB_PHBCEntities db2 = new DB_PHBCEntities();
        IMenuBussiness db;
        public SysMenuController(IMenuBussiness _iMenuBussiness)
        {
            db = _iMenuBussiness;
            ViewBag.TitleName = " Menu ";
        }

        // GET: /Admin/SysMenu/
        public ActionResult Index()
        {
            //var sysmenus = db.getAllMenu();
            //return View(sysmenus);
            return RedirectToAction("Create");
        }

        // GET: /Admin/SysMenu/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModel sysmenu = db.getMenuById(id);
            if (sysmenu == null)
            {
                return HttpNotFound();
            }
            return View(sysmenu);
        }

        // GET: /Admin/SysMenu/Create
        public ActionResult Create()
        {
            ViewBag.AllMenu = db.getAllMenu();
            ViewBag.ActionCode = new SelectList(db.getAction(), "Code", "Description").ToList();
            ViewBag.ParentId = new SelectList(db.getAllMenuIsParent(), "Id", "Name").ToList();
            return View();
        }

        // POST: /Admin/SysMenu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,ActionCode,ParentId,Order,Description,MenuType")] MenuModel sysmenu)
        {
            ViewBag.AllMenu = db.getAllMenu();
            if (ModelState.IsValid)
            {
                sysmenu.CreateBy = userInfo.Id;
                sysmenu.CreateDate = DateTime.Now;
                db.AddMenu(sysmenu);
                return RedirectToAction("Index");
            }

            ViewBag.ActionCode = new SelectList(db.getAction(), "Code", "Controller", sysmenu.ActionCode).ToList();
            ViewBag.ParentId = new SelectList(db.getAllMenuIsParent(sysmenu.Id), "Id", "Name", sysmenu.ParentId).ToList();
            return View(sysmenu);
        }

        // GET: /Admin/SysMenu/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.AllMenu = db.getAllMenu();
            MenuModel sysmenu = db.getMenuById(id);
            if (sysmenu == null)
            {
                return HttpNotFound();
            }
            //SelectList item = new SelectList(db.getAction(), "Code", "Description", sysmenu.ActionCode);
            ViewBag.ActionCode = new SelectList(db.getAction(), "Code", "Description", sysmenu.ActionCode).ToList();
            if (db.checkChild(sysmenu.Id)== null || db.checkChild(sysmenu.Id).Count == 0)
            {
                ViewBag.ParentId = new SelectList(db.getAllMenuIsParent(sysmenu.Id), "Id", "Name", sysmenu.ParentId).ToList();
            }
            else
            {
                ViewBag.ParentId = new SelectList( "", "Id", "Name", sysmenu.ParentId).ToList();
            }
            return View(sysmenu);
        }

        // POST: /Admin/SysMenu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,ActionCode,Area,Controller,Action,ParentId,Order,Description")] MenuModel sysmenu)
        {
            if (ModelState.IsValid)
            {
                sysmenu.ModifyBy = userInfo.Id;
                sysmenu.ModifyDate = DateTime.Now;
                db.Edit(sysmenu);
                return RedirectToAction("Index");
            }

            ViewBag.ActionCode = new SelectList(db.getAction(), "Code", "Description", sysmenu.ActionCode).ToList();

            if (db.checkChild(sysmenu.Id) == null || db.checkChild(sysmenu.Id).Count == 0)
            {
                ViewBag.ParentId = new SelectList(db.getAllMenuIsParent(sysmenu.Id), "Id", "Name", sysmenu.ParentId).ToList();
            }
            else
            {
                ViewBag.ParentId = new SelectList("", "Id", "Name", sysmenu.ParentId).ToList();
            }
            return View(sysmenu);
        }

        // GET: /Admin/SysMenu/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuModel sysmenu = db.getMenuByIdWithChild(id);
            if (sysmenu == null)
            {
                return HttpNotFound();
            }
            return View(sysmenu);
        }

        // POST: /Admin/SysMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            db.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
