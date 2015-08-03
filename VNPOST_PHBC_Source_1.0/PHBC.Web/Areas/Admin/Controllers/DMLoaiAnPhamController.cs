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
using Webdiyer.WebControls.Mvc;
using PHBC.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PHBC.Web.Base;



namespace PHBC.Web.Areas.Admin.Controllers
{
    public class DMLoaiAnPhamController : BaseController
    {
        IDMLoaiAnPhamBussiness db;
      
        public DMLoaiAnPhamController(IDMLoaiAnPhamBussiness _iDMLoaiAnPhamBussiness)
        {
            db = _iDMLoaiAnPhamBussiness;
        }

        public ActionResult Index()
        {
            return View(db.getAllModel());
        }


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMLoaiAnPham dmloaianpham = db.getById(id);
            if (dmloaianpham == null)
            {
                return HttpNotFound();
            }
            return View(new DMLoaiAnPhamModel(dmloaianpham));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string TenLoaiAnPham, string CoKyXuatBan)//] DMLoaiAnPhamModel dmloaianphammodel)
        {
            DMLoaiAnPhamModel dmloaianphammodel = new DMLoaiAnPhamModel();
            dmloaianphammodel.TenLoaiAnPham = TenLoaiAnPham.ToString();
            dmloaianphammodel.CoKyXuatBan.Equals(CoKyXuatBan);
          
            if (ModelState.IsValid)
            {
                dmloaianphammodel.Id = Guid.NewGuid().ToString();
                dmloaianphammodel.Status = (int) Enums.RecordStatusCode.active;
                ErrorObject err = new ErrorObject();
                err = db.checkDMLoaiAnPham(Convert.ToByte(DAO.Common.Enums.FormAction.Create), dmloaianphammodel.Id, dmloaianphammodel.TenLoaiAnPham);
                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    return View(dmloaianphammodel);
                }
                db.Add(dmloaianphammodel.toDMLoaiAnPham());
                return RedirectToAction("Index");
            }

            return View(dmloaianphammodel);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMLoaiAnPham dmloaianpham = db.getById(id);
            if (dmloaianpham == null)
            {
                return HttpNotFound();
            }
            return View(new DMLoaiAnPhamModel(dmloaianpham));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenLoaiAnPham,CoKyXuatBan")] DMLoaiAnPhamModel dmloaianpham)
        {
           
            
            if (ModelState.IsValid)
            {
                ErrorObject err = new ErrorObject();
                err = db.checkDMLoaiAnPham(Convert.ToByte(DAO.Common.Enums.FormAction.Edit), dmloaianpham.Id, dmloaianpham.TenLoaiAnPham);
                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    return View(dmloaianpham);
                }
                db.Update(dmloaianpham);
                return RedirectToAction("Index");
            }
            return View(dmloaianpham);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMLoaiAnPham dmloaianpham = db.getById(id);
            if (dmloaianpham == null)
            {
                return HttpNotFound();
            }
            return View(new DMLoaiAnPhamModel(dmloaianpham));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            db.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string TenLoaiAnPham)
        {
            if ( String.IsNullOrEmpty(TenLoaiAnPham))
            {
                return RedirectToAction("Index");
            }
            //List<DMLoaiAnPhamModel> value = db.search(TenLoaiAnPham);

            //return View("Index", value);
            return View("Index", db.search(TenLoaiAnPham));
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
