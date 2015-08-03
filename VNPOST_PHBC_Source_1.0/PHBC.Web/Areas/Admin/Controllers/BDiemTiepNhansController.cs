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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using PHBC.Web.Constants;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PHBC.Web.Base;
using Webdiyer.WebControls.Mvc;

namespace PHBC.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// TODO: code edit va create da xong . Chua co Phan quyen, Chua co giao dien
    /// </summary>
    public class BDiemTiepNhansController : BaseController
    {
        IBDiemTiepNhanBussiness db;
        public BDiemTiepNhansController(IBDiemTiepNhanBussiness _iBDiemTiepNhanBussiness)
        {
            db = _iBDiemTiepNhanBussiness;
            ViewBag.TitleName = " Điểm TIếp Nhận ";
            //ViewBag.Permisson = base.permisson;
        }
        // GET: Admin/BDiemTiepNhans
        public ActionResult Index()
        {
            var bDiemTiepNhans = db.getAllModel();
            return View(bDiemTiepNhans.ToList());
        }

        // GET: Admin/BDiemTiepNhans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDiemTiepNhanModel bDiemTiepNhanModel = db.getModelById(id);
            if (bDiemTiepNhanModel == null)
            {
                return HttpNotFound();
            }
            return View(bDiemTiepNhanModel);
        }

        // GET: Admin/BDiemTiepNhans/Create
        public ActionResult Create()
        {
            //ViewBag.UnitCode = new SelectList(db.Units, "UnitCode", "UnitName");
            return View();
        }

        // POST: Admin/BDiemTiepNhans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,UnitCode,UnitName,CreateDate,CreateBy,ModifyDate,ModifyBy,Status")] BDiemTiepNhanModel bDiemTiepNhanModel)
        {
            if (ModelState.IsValid)
            {
                bDiemTiepNhanModel.CreateBy = userInfo.Id;
                bDiemTiepNhanModel.CreateDate = DateTime.Now;
                if (db.Add(bDiemTiepNhanModel) == -1)
                {
                    buildMessage(Application.FormMessage.CreateUnSuccess.ToString());
                    return View(bDiemTiepNhanModel);
                }
                else
                {
                    buildMessage(Application.FormMessage.CreateSuccess.ToString());
                    return RedirectToAction("Index");
                }
            }

            //ViewBag.UnitCode = new SelectList(db.Units, "UnitCode", "UnitName", bDiemTiepNhan.UnitCode);
            buildMessage(Application.FormMessage.CreateUnSuccess.ToString());
            return View(bDiemTiepNhanModel);
        }

        // GET: Admin/BDiemTiepNhans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDiemTiepNhanModel bDiemTiepNhanModel = db.getModelById(id);
            if (bDiemTiepNhanModel == null)
            {
                return HttpNotFound();
            }
            //ViewBag.UnitCode = new SelectList(db.Units, "UnitCode", "UnitName", bDiemTiepNhan.UnitCode);
            return View(bDiemTiepNhanModel);
        }

        // POST: Admin/BDiemTiepNhans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,UnitCode,CreateDate,CreateBy,ModifyDate,ModifyBy,Status")] BDiemTiepNhanModel bDiemTiepNhanModel)
        {
            if (ModelState.IsValid)
            {
                bDiemTiepNhanModel.ModifyBy = userInfo.Id;
                bDiemTiepNhanModel.ModifyDate = DateTime.Now;
                if (db.Update(bDiemTiepNhanModel) == -1)
                {
                    buildMessage(Application.FormMessage.EditUnSuccess.ToString());
                    return View(bDiemTiepNhanModel);
                }
                else
                {
                    buildMessage(Application.FormMessage.EditSuccess.ToString());
                    return RedirectToAction("Index");
                }
            }
            //ViewBag.UnitCode = new SelectList(db.Units, "UnitCode", "UnitName", bDiemTiepNhan.UnitCode);
            buildMessage(Application.FormMessage.EditUnSuccess.ToString());
            return View(bDiemTiepNhanModel);
        }

        // GET: Admin/BDiemTiepNhans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDiemTiepNhanModel bDiemTiepNhanModel = db.getModelById(id);
            if (bDiemTiepNhanModel == null)
            {
                return HttpNotFound();
            }
            return View(bDiemTiepNhanModel);
        }

        // POST: Admin/BDiemTiepNhans/Delete/5
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
