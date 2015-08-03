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
using PHBC.Web.Base;
using PHBC.DAO.Models;
using PHBC.Web.Constants;
using Webdiyer.WebControls.Mvc;
using System.Dynamic;
using Newtonsoft.Json;
using PHBC.DAO.Common;

namespace PHBC.Web.Areas.Bao
{
    public class ThongTinGiaBaoController : BaseController
    {
        private int pageSize = 10;
        private IThongTinGiaBaoBussiness db;        
        public ThongTinGiaBaoController(IThongTinGiaBaoBussiness _db)
        {
            this.db = _db;            
        }

        // GET: Bao/ThongTinGiaBao
        public ActionResult Index(string id, string pageIndex = "")
        {

            ViewBag.UserLevel = userInfo.Level;
            string provinceCode = string.Empty;
            if (userInfo.Level > (int)Enums.RoleLevel.PHBC_TW)
            { 
                provinceCode = userInfo.ProvinCode;
                permisson.RemovePermisson("GiaMua");
            }
            DanhSachGiaBaoModel danhSachGiaBaoModel = db.getDanhSachGiaBaoModel(id, provinceCode);
            
            Session[Application.Session.ModelSearch] = null;
            int pageNum = 0;
            if (!String.IsNullOrEmpty(pageIndex))
            {
                pageNum = int.Parse(pageIndex.Replace('/', '\0'));
            }
            else pageNum = 1;
            int pageCount = 0;
            //int totalitem = 0;
            //var value = thongTinBaoModel.BaoGiaBan;
            ViewBag.Page = pageNum;
            ViewBag.PageCount = pageCount;
            return View(danhSachGiaBaoModel);
        }

        // GET: Bao/ThongTinGiaBao/Details/5
        public ActionResult Details(string id, string thongTinBaoId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinGiaBaoModel thongTinGiaBaoModel= null;//= db.getThongTinGiaBaoModel(id);
            if (thongTinGiaBaoModel == null)
            {
                return HttpNotFound();
            }
            return View(thongTinGiaBaoModel);
        }

        // GET: Bao/ThongTinGiaBao/Create
        public ActionResult Create( string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ThongTinGiaBaoModel thongTinGiaBaoModel = db.createThongTinGiaBaoModel(id);
            if (thongTinGiaBaoModel == null)
                return HttpNotFound();
            return View(thongTinGiaBaoModel);
        }

        public ActionResult GiaMua(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ThongTinGiaBaoModel thongTinGiaBaoModel = db.createThongTinGiaBaoModel(id);
            if (thongTinGiaBaoModel == null)
                return HttpNotFound();
            return View(thongTinGiaBaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiaMua([Bind(Include = "ThongTinBaoId,_NgayHieuLuc,_NgayHetHieuLuc,ProvinceCode,QuyetDinh,Value")] ThongTinGiaBaoModel thongTinGiaBaoModel)
        {
            if (ModelState.IsValid)
            {
                thongTinGiaBaoModel.ProvinceCode = userInfo.ProvinCode;
                thongTinGiaBaoModel.userId = userInfo.Id;
                thongTinGiaBaoModel.ValueType = (int)Enums.LoaiGia.GiaMua;
                ErrorObject err = db.Create(thongTinGiaBaoModel);
                if (err.HasError)
                {
                    buildError(err);

                    return View(thongTinGiaBaoModel);
                }
                base.buildMessage("Tạo giá mua thành công");
                return RedirectToAction("Index", new { id = thongTinGiaBaoModel.ThongTinBaoId });
            }

            return View(thongTinGiaBaoModel);
        }

        public ActionResult GiaBan(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ThongTinGiaBaoModel thongTinGiaBaoModel = db.createThongTinGiaBaoModel(id);
            if (thongTinGiaBaoModel == null)
                return HttpNotFound();
            return View(thongTinGiaBaoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiaBan([Bind(Include = "ThongTinBaoId,_NgayHieuLuc,_NgayHetHieuLuc,ProvinceCode,QuyetDinh,Value,GhiChu")] ThongTinGiaBaoModel thongTinGiaBaoModel)
        {
            if (ModelState.IsValid)
            {
                string provinceCode = string.Empty;
                if (userInfo.Level > (int)Enums.RoleLevel.PHBC_TW)
                {
                    provinceCode = userInfo.ProvinCode;
                    permisson.RemovePermisson("GiaMua");
                }
                thongTinGiaBaoModel.ProvinceCode = provinceCode;
                thongTinGiaBaoModel.userId = userInfo.Id;
                thongTinGiaBaoModel.ValueType = (int)Enums.LoaiGia.GiaBan;
                ErrorObject err = db.Create(thongTinGiaBaoModel);
                if (err.HasError)
                {
                    buildError(err);
                    return View(thongTinGiaBaoModel);
                }
                base.buildMessage("Tạo giá bán thành công");
                return RedirectToAction("Index", new { id = thongTinGiaBaoModel.ThongTinBaoId });
            }
            return View(thongTinGiaBaoModel);
            //RedirectToAction("Create", new {thongTinGiaBaoModel});
            //return View(thongTinGiaBaoModel);
        }

        // POST: Bao/ThongTinGiaBao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThongTinGiaBaoModel thongTinGiaBaoModel)
        {
            if (ModelState.IsValid)
            {
                string provinceCode = string.Empty;
                if (userInfo.Level > (int)Enums.RoleLevel.PHBC_TW)
                {
                    provinceCode = userInfo.ProvinCode;
                }
                thongTinGiaBaoModel.ProvinceCode = provinceCode;
                thongTinGiaBaoModel.userId = userInfo.Id;
                thongTinGiaBaoModel.ValueType = (int)Enums.LoaiGia.GiaBan;
                ErrorObject err = db.Create(thongTinGiaBaoModel);
                if (err.HasError)
                {
                    buildError(err);
                    return View(thongTinGiaBaoModel);
                }
                base.buildMessage("Tạo giá thành công");
                return RedirectToAction("Index", new { id = thongTinGiaBaoModel.ThongTinBaoId });
            }
            return View(thongTinGiaBaoModel);
        }

        // GET: Bao/ThongTinGiaBao/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinGiaBaoModel thongTinGiaBaoModel = db.getThongTinGiaBaoById(id);
            if (thongTinGiaBaoModel == null)
            {
                return HttpNotFound();
            }
            return View(thongTinGiaBaoModel);
        }

        // POST: Bao/ThongTinGiaBao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ThongTinBaoId,_NgayHieuLuc,_NgayHetHieuLuc,ProvinceCode,QuyetDinh,ValueType,Value")] ThongTinGiaBaoModel thongTinGiaBaoModel)
        {
            if (ModelState.IsValid)
            {
                ErrorObject err = new ErrorObject();
                thongTinGiaBaoModel.ProvinceCode = userInfo.ProvinCode;
                thongTinGiaBaoModel.userId = userInfo.Id;
                err = db.Edit(thongTinGiaBaoModel);
                if (err.HasError)
                {
                    buildError(err);
                    return View(thongTinGiaBaoModel);
                }
                //ThongTinBaoModel thongTinBaoModel = db1.getThongTinBaoById(thongTinGiaBaoModel.ThongTinBaoId);
                return RedirectToAction("Index", thongTinGiaBaoModel);
            }
            return View(thongTinGiaBaoModel);
        }

        // GET: Bao/ThongTinGiaBao/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinGiaBaoModel thongTinGiaBaoModel = db.getThongTinGiaBaoById(id);
            if (thongTinGiaBaoModel == null)
            {
                return HttpNotFound();
            }
            return View(thongTinGiaBaoModel);
        }

        // POST: Bao/ThongTinGiaBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ThongTinGiaBaoModel thongTinGiaBaoModel = db.getThongTinGiaBaoById(id);
            db.Delete(id);
            return RedirectToAction("Index", new {id = thongTinGiaBaoModel.ThongTinBaoId});
        }
        public ActionResult Search(string page)
        {
            int pagenum = 1;
            ThongTinBaoSearchModel search = Session[Constants.Application.Session.ModelSearch] as ThongTinBaoSearchModel;
                       
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
        public ActionResult Search([Bind(Include = "Id,Value,_NgayHieuLuc,_NgayHetHieuLuc")] ThongTinBaoSearchModel thongTinBaoSearchModel, int? pageIndex)
        {
            int pagenum = 0;
            int pageCount = 0;
            thongTinBaoSearchModel.Id = "ec88c49f-23df-47a6-9bff-8a7776252129";
            ThongTinBaoModel thongTinBaoModel = null;//db1.getThongTinGiaBaoModel(thongTinBaoSearchModel);

            if (!String.IsNullOrEmpty(Convert.ToString(pageIndex)))
            {
                pagenum = pageIndex.Value;
            }
            else pagenum = 1;
            //int totalitem = 0;
            //var value = db1.getThongTinBaoById(thongTinBaoSearchModel.Id);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            Session[Constants.Application.Session.ModelSearch] = thongTinBaoModel;
            //return View("Index", new PagedList<ThongTinGiaBaoModel>(value, pagenum, pageSize, totalitem));            
            return View("Index",thongTinBaoModel);
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
