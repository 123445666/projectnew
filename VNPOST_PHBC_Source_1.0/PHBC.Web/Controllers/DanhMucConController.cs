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
using PHBC.Web.Base;

namespace PHBC.Web.Controllers
{
    public class DanhMucConController : BaseController
    {
        //private DB_PHBCEntities db = new DB_PHBCEntities();
        ISysDMPublicBussiness iSysDMPublicBussiness;
        public DanhMucConController(ISysDMPublicBussiness _iSysDMPublicBussiness)
        {
            iSysDMPublicBussiness = _iSysDMPublicBussiness;
            ViewBag.TitleName = " Danh Mục Con ";
        }
        // GET: /Admin/DanhMuc/
        public ActionResult Index(int? id, string page = "")
        {
            ViewBag.DmTypeId = id;
            //check xem co id loai danh muc khong
            // neu khong co tra ve index loai danh muc
            if(id == null || id == 0)
            {
                return RedirectToAction("Index", "DanhMuc", new { Area = "Admin" });
            }

            //xu ly phan trang
            int pagenum = 0;
            if (!String.IsNullOrEmpty(page))
            {
                pagenum = int.Parse(page.Replace('/', '\0'));
            }
            else pagenum = 1;
            int pageCount = 0;
            var value = iSysDMPublicBussiness.getAllModelByTypeId(id.Value, pagenum, 2, out pageCount);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            SysDMTypeBussiness dbDanhMuc = new SysDMTypeBussiness();
            ViewBag.lstDanhMuc = dbDanhMuc.getAll();
            return View(value);
        }

        // GET: /Admin/DanhMuc/Details/5
        public ActionResult Details(int? typeid, string code)
        {
            if (typeid == null || String.IsNullOrEmpty(code))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDMPublic SysDMPublic = iSysDMPublicBussiness.getByIdAndCode(typeid, code);
            if (SysDMPublic == null)
            {
                return HttpNotFound();
            }
            return View(new SysDMPublicModel(SysDMPublic));
        }

        // GET: /Admin/DanhMuc/Create
        public ActionResult Create(int? id)
        {
            SysDMPublicModel sysdmpublicmodel = new SysDMPublicModel();
            if (id != null)
            {
                sysdmpublicmodel.TypeId = Int32.Parse(Convert.ToString(id));
            }
            else
            {
                return RedirectToAction("Index", "DanhMuc", new { Area="Admin" });
            }
            return View(sysdmpublicmodel);
        }

        // POST: /Admin/DanhMuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeId,Code,Name,IsLock,Description,bLock")] SysDMPublicModel SysDMPublicModel)
        {
            if (ModelState.IsValid)
            {
                //Check code da ton tai chua
                ErrorObject err = new ErrorObject();
                err = iSysDMPublicBussiness.checkSysDMPublic(SysDMPublicModel.Code);
                if(err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    return View(SysDMPublicModel);
                }
                iSysDMPublicBussiness.Add(SysDMPublicModel.toSysDMPublic());
                return RedirectToAction("Index", new { id = SysDMPublicModel.TypeId });
            }

            return View(SysDMPublicModel);
        }

        // GET: /Admin/DanhMuc/Edit/5
        public ActionResult Edit(int? typeid, string code="")
        {
            if (typeid == null || code == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDMPublic SysDMPublic = iSysDMPublicBussiness.getByIdAndCode(typeid, code);
            if (SysDMPublic == null)
            {
                return HttpNotFound();
            }
            return View(new SysDMPublicModel(SysDMPublic));
        }

        // POST: /Admin/DanhMuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeId,Code,Name,IsLock,Description,bLock")] SysDMPublicModel SysDMPublicModel)
        {
            if (ModelState.IsValid)
            {
                ErrorObject err = new ErrorObject();
                err = iSysDMPublicBussiness.checkSysDMPublic(SysDMPublicModel.TypeId, SysDMPublicModel.Code);
                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    return View(SysDMPublicModel);
                }
                iSysDMPublicBussiness.Update(SysDMPublicModel.toSysDMPublic());
                return RedirectToAction("Index", new { id = SysDMPublicModel.TypeId });
            }
            return View(SysDMPublicModel);
        }

        // GET: /Admin/DanhMuc/Delete/5
        public ActionResult Delete(int? typeid, string code = "")
        {
            if (typeid == null || String.IsNullOrEmpty(code))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDMPublic SysDMPublic = iSysDMPublicBussiness.getByIdAndCode(typeid, code);
            if (SysDMPublic == null)
            {
                return HttpNotFound();
            }
            return View(new SysDMPublicModel(SysDMPublic));
        }

        // POST: /Admin/DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int typeid,string code)
        {
            iSysDMPublicBussiness.Delete(typeid, code);
            return RedirectToAction("Index", new { id = typeid });
        }

        public ActionResult Search(int? id, string page)
        {
            int pagenum = 1;
            SysDMPublicSearchModel search = Session[Constants.Application.Session.ModelSearch] as SysDMPublicSearchModel;
            if (!String.IsNullOrEmpty(page))
            {
                pagenum = int.Parse(page.Replace('/', '\0'));
            }
            if (search == null)
            {
                return RedirectToAction("Index");
            }

            ActionResult ars;
            ars = this.Search(search,id.Value, pagenum);
            return ars;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "TypeId, Name")] SysDMPublicSearchModel sysdmpublic,int id, int? page)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "DanhMuc", new { Area = "Admin" });
            }
                
            if (string.IsNullOrEmpty(sysdmpublic.Name))
                return RedirectToAction("Index", new { id = sysdmpublic.TypeId });

            int pagenum = 0;
            int pageCount = 0;
            if (!String.IsNullOrEmpty(Convert.ToString(page)))
            {
                pagenum = page.Value;
            }
            else pagenum = 1;
            sysdmpublic.TypeId = id;
            List<SysDMPublicModel> value = iSysDMPublicBussiness.searchModel(sysdmpublic, pagenum, 2, out pageCount);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            ViewBag.DmTypeId = id;
            ViewBag.SearchModel = sysdmpublic;
            Session[Constants.Application.Session.ModelSearch] = sysdmpublic;
            SysDMTypeBussiness dbDanhMuc = new SysDMTypeBussiness();
            ViewBag.lstDanhMuc = dbDanhMuc.getAll();
            return View("Index", value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                iSysDMPublicBussiness.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
