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
using Webdiyer.WebControls.Mvc;
using PHBC.Web.Base;

namespace PHBC.Web.Areas.Admin.Controllers
{
    public class DiemInController : BaseController
    {
        //private DB_PHBCEntities db = new DB_PHBCEntities();
         IDMDiemInBussiness db;
         int pagesize = 10;
         public DiemInController(IDMDiemInBussiness _iDMDiemInBussiness)
        {
            db = _iDMDiemInBussiness;
            ViewBag.TitleName = " Điểm In ";
            //ViewBag.Permisson = base.permisson;
        }

        // GET: /Admin/DiemIn/
         public ActionResult Index(string pageIndex = "")
        {
            string sv = HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            int pagenum = 0;
            if (!String.IsNullOrEmpty(pageIndex))
            {
                pagenum = int.Parse(pageIndex.Replace('/', '\0'));
            }
            else pagenum = 1;
            int pageCount = 0;
            int totalitem = 0;
            var value = db.getAllModel(pagenum, pagesize, out pageCount, out totalitem);            
            return View(new PagedList<DMDiemInModel>(value, pagenum, pagesize, totalitem));
            //return View(value);
        }

        // GET: /Admin/DiemIn/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.Province = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName");
            DMDiemIn dmdiemin = db.getById(id);
            if (dmdiemin == null)
            {
                return HttpNotFound();
            }
            return View(new DMDiemInModel(dmdiemin));
        }

        // GET: /Admin/DiemIn/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName").ToList();
            items.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
            ViewBag.Provinces = items;
            ViewBag.Districts = new List<SelectListItem>();
            return View();
        }

        // POST: /Admin/DiemIn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,MaDiemIn,TenDiemIn,ProvinceCode,DiaChi,DistrictCode")] DMDiemInModel dmdieminmodel)
        {
            if (ModelState.IsValid)
            {
                ErrorObject err = new ErrorObject();
                err = db.checkDMDiemIn(dmdieminmodel.Id, dmdieminmodel.MaDiemIn, dmdieminmodel.TenDiemIn);
                if (err.HasError)
                {
                    buildError(err);
                    List<SelectListItem> items1 = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName", dmdieminmodel.ProvinceCode).ToList();
                    items1.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
                    ViewBag.Provinces = items1;

                    List<SelectListItem> item2 = new SelectList(db.getDistrictByProvince(dmdieminmodel.ProvinceCode), "DistrictCode", "DistrictName", dmdieminmodel.DistrictCode).ToList();
                    item2.Insert(0, (new SelectListItem { Text = "--- Chọn huyện ---", Value = "" }));
                    ViewBag.Districts = item2;
                    return View(dmdieminmodel);

                }
                dmdieminmodel.userId = userInfo.Id;
                db.Add(dmdieminmodel);
                return RedirectToAction("Index");
            }

            List<SelectListItem> items = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName", dmdieminmodel.ProvinceCode).ToList();
            items.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
            ViewBag.Provinces = items;

            List<SelectListItem> itemDistricts = new SelectList(db.getDistrictByProvince(dmdieminmodel.ProvinceCode), "DistrictCode", "DistrictName", dmdieminmodel.DistrictCode).ToList();
            itemDistricts.Insert(0, (new SelectListItem { Text = "--- Chọn huyện ---", Value = "" }));
            ViewBag.Districts = itemDistricts;
            return View(dmdieminmodel);
            
        }

        // GET: /Admin/DiemIn/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMDiemIn dmdiemin = db.getById(id); //get item by id
            if (dmdiemin == null)
            {
                return HttpNotFound();
            }

            //load all province
            List<SelectListItem> items = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName", dmdiemin.ProvinceCode).ToList();
            items.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
            ViewBag.Provinces = items;
            List<SelectListItem> itemDistricts = new SelectList(db.getDistrictByProvince(dmdiemin.ProvinceCode), "DistrictCode", "DistrictName", dmdiemin.DistrictCode).ToList();
            itemDistricts.Insert(0, (new SelectListItem { Text = "--- Chọn huyện ---", Value = "" }));
            ViewBag.Districts = itemDistricts?? new List<SelectListItem>();
            return View(new DMDiemInModel(dmdiemin));
        }

        // POST: /Admin/DiemIn/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaDiemIn,TenDiemIn,ProvinceCode,DiaChi,DistrictCode")] DMDiemInModel dmdiemin)
        {
            //lấy ra tỉnh được chọn
            //List<SelectListItem> items = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName", dmdiemin.ProvinceCode).ToList();
            //items.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
            //ViewBag.Provinces = items;

            if (ModelState.IsValid)
            {
                ErrorObject err = new ErrorObject();
                err = db.checkDMDiemIn(dmdiemin.Id, dmdiemin.MaDiemIn, dmdiemin.TenDiemIn);
                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    List<SelectListItem> items1 = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName", dmdiemin.ProvinceCode).ToList();
                    items1.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
                    ViewBag.Provinces = items1;

                    List<SelectListItem> itemDistricts = new SelectList(db.getDistrictByProvince(dmdiemin.ProvinceCode), "DistrictCode", "DistrictName", dmdiemin.DistrictCode).ToList();
                    itemDistricts.Insert(0, (new SelectListItem { Text = "--- Chọn huyện ---", Value = "" }));
                    ViewBag.Districts = itemDistricts ?? new List<SelectListItem>();
                    return View(dmdiemin);
                }
                dmdiemin.userId = userInfo.Id;
                db.Update(dmdiemin);
                return RedirectToAction("Index");
            }
            List<SelectListItem> items2 = new SelectList(db.getAllProvince(), "ProvinceCode", "ProvinceName", dmdiemin.ProvinceCode).ToList();
            items2.Insert(0, (new SelectListItem { Text = "--- Chọn tỉnh ---", Value = "" }));
            ViewBag.Provinces = items2;

            List<SelectListItem> itemDistricts1 = new SelectList(db.getDistrictByProvince(dmdiemin.ProvinceCode), "DistrictCode", "DistrictName", dmdiemin.DistrictCode).ToList();
            itemDistricts1.Insert(0, (new SelectListItem { Text = "--- Chọn huyện ---", Value = "" }));
            ViewBag.Districts = itemDistricts1 ?? new List<SelectListItem>();
            return View(dmdiemin);
        }

        // GET: /Admin/DiemIn/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMDiemIn dmdiemin = db.getById(id);
            if (dmdiemin == null)
            {
                return HttpNotFound();
            }
            return View(new DMDiemInModel(dmdiemin));
        }

        // POST: /Admin/DiemIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DMDiemIn DMDiemIn = db.getById(id);
            DMDiemIn.ModifyBy = userInfo.Id;
            DMDiemIn.ModifyDate = DateTime.Now;
            db.Delete(DMDiemIn);
            return RedirectToAction("Index");
        }

        public ActionResult Search(string page)
        {
            int pagenum = 1;
            DMDiemInSearchModel search = Session[Constants.Application.Session.ModelSearch] as DMDiemInSearchModel;
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
        public ActionResult Search([Bind(Include = "MaDiemIn,TenDiemIn")] DMDiemInSearchModel dmdiemin, int? page)
        {
            int pagenum = 0;
            int pageCount = 0;
            if (String.IsNullOrEmpty(dmdiemin.MaDiemIn) && String.IsNullOrEmpty(dmdiemin.TenDiemIn))
            {
                return RedirectToAction("Index");
            }
            if (!String.IsNullOrEmpty(Convert.ToString(page)))
            {
                pagenum = page.Value;
            }
            else pagenum = 1;
            int totalitem = 0;
            List<DMDiemInModel> value = db.searchModel(dmdiemin.MaDiemIn, dmdiemin.TenDiemIn, pagenum, pagesize, out pageCount, out totalitem);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchModel = dmdiemin;
            Session[Constants.Application.Session.ModelSearch] = dmdiemin;
            return View("Index", new PagedList<DMDiemInModel>(value, pagenum, pagesize, totalitem));
            //return View("Index", value);
        }


        public ActionResult getDistrict(string id)
        {
            var districts = db.getDistrictByProvince(id);
            if (HttpContext.Request.IsAjaxRequest())
            {
                return this.Json(new SelectList(districts, "DistrictCode", "DistrictName"),
                    JsonRequestBehavior.AllowGet);
            }

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
