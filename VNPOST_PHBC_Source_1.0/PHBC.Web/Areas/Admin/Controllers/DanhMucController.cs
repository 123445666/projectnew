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
using PHBC.Web.Permission;
using PHBC.Web.Base;

namespace PHBC.Web.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        //private DB_PHBCEntities db = new DB_PHBCEntities();
        ISysDMTypeBussiness iSysDMTypeBussiness;
        public DanhMucController(ISysDMTypeBussiness _iSysDMTypeBussiness)
        {
            iSysDMTypeBussiness = _iSysDMTypeBussiness;
            ViewBag.TitleName = " Loại Danh Mục ";
            //ViewBag.Permisson = base.permisson;

        }
        // GET: /Admin/DanhMuc/
        public ActionResult Index(string page = "")
        {

            int pagenum = 0;
            if (!String.IsNullOrEmpty(page))
            {
                pagenum = int.Parse(page.Replace('/', '\0'));
            }
            else pagenum = 1;
            int pageCount = 0;
            var value = iSysDMTypeBussiness.getAllModel(pagenum, 2, out pageCount);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            return View(value);
        }

        // GET: /Admin/DanhMuc/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDMType sysdmtype = iSysDMTypeBussiness.getById(id);
            if (sysdmtype == null)
            {
                return HttpNotFound();
            }
            return View(new SysDMTypeModel(sysdmtype));
        }

        // GET: /Admin/DanhMuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/DanhMuc/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,Description")] SysDMTypeModel sysdmtypemodel)
        {
            if (ModelState.IsValid)
            {
                //Check role da ton tai chua
                if (iSysDMTypeBussiness.checkSysDMType(sysdmtypemodel.Id))
                {
                    ModelState.AddModelError("Error", "Mã này đã được sử dụng. ");
                    return View(sysdmtypemodel);
                }
                iSysDMTypeBussiness.Add(sysdmtypemodel.toSysDMType());
                return RedirectToAction("Index");
            }

            return View(sysdmtypemodel);
        }

        // GET: /Admin/DanhMuc/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDMType sysdmtype = iSysDMTypeBussiness.getById(id);
            if (sysdmtype == null)
            {
                return HttpNotFound();
            }
            return View(new SysDMTypeModel(sysdmtype));
        }

        // POST: /Admin/DanhMuc/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Description")] SysDMType sysdmtype)
        {
            if (ModelState.IsValid)
            {
                iSysDMTypeBussiness.Update(sysdmtype);
                return RedirectToAction("Index");
            }
            return View(new SysDMTypeModel(sysdmtype));
        }

        // GET: /Admin/DanhMuc/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SysDMType sysdmtype = iSysDMTypeBussiness.getById(id);
            if (sysdmtype == null)
            {
                return HttpNotFound();
            }
            return View(new SysDMTypeModel(sysdmtype));
        }

        // POST: /Admin/DanhMuc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            iSysDMTypeBussiness.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Search(string page)
        {
            int pagenum = 1;
            SysDMTypeSearchModel search = Session[Constants.Application.Session.ModelSearch] as SysDMTypeSearchModel;
            if (!String.IsNullOrEmpty(page))
            {
                pagenum = int.Parse(page.Replace('/', '\0'));
            }
            if (search == null)
            {
                return RedirectToAction("Index");
            }

            ActionResult ars;
            ars = this.Search(search, pagenum);
            return ars;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind(Include = "Name")] SysDMTypeSearchModel sysdmtype, int? page)
        {
            int pagenum = 0;
            int pageCount = 0;
            if (String.IsNullOrEmpty(sysdmtype.Name))
            {
                return RedirectToAction("Index");
            }
            if (!String.IsNullOrEmpty(Convert.ToString(page)))
            {
                pagenum = page.Value;
            }
            else pagenum = 1;

            List<SysDMTypeModel> value = iSysDMTypeBussiness.searchModel(sysdmtype.Name, pagenum, 2, out pageCount);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchModel = sysdmtype;
            Session[Constants.Application.Session.ModelSearch] = sysdmtype;
            return View("Index", value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                iSysDMTypeBussiness.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
