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

namespace PHBC.Web.Areas.Bao.Controllers
{
    public class ThongTinBaoController : BaseController
    {
        private int pageSize = 10;
        private int pageSizePopup = 10;
        private IThongTinBaoBussiness db;
        public ThongTinBaoController(IThongTinBaoBussiness _iThongTinBaoBussiness)
        {
            this.db = _iThongTinBaoBussiness;
        }

        // GET: Bao/ThongTinBao
        public ActionResult Index(string pageIndex = "")
        {
            //return View(db.BThongTinBaos.ToList());
            Session[Application.Session.ModelSearch] = null;
            int pageNum = 0;
            if (!String.IsNullOrEmpty(pageIndex))
            {
                pageNum = int.Parse(pageIndex.Replace('/', '\0'));
            }
            else pageNum = 1;
            int pageCount = 0;
            int totalitem = 0;
            var value = db.getAllModel(pageNum, pageSize, out pageCount, out totalitem);
            ViewBag.Page = pageNum;
            ViewBag.PageCount = pageCount;
            getValueForDropDownList(null);
            return View(new PagedList<ThongTinBaoModel>(value, pageNum, pageSize, totalitem));
        }

        // GET: Bao/ThongTinBao/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinBaoModel thongTinBaoModel = db.getThongTinBaoById(id);
            if (thongTinBaoModel == null)
            {
                return HttpNotFound();
            }
            return View(thongTinBaoModel);
        }

        // GET: Bao/ThongTinBao/Create
        public ActionResult Create()
        {
            ThongTinBaoModel thongTinBaoModel = new ThongTinBaoModel();           
            getValueForDropDownList(null);             
            return View();            
        }
        public ActionResult GetMaBaoCha(string search, string pageIndex)
        {
            List<ThongTinBaoModel> value = new List<ThongTinBaoModel>();
            int pageCount = 0;
            int totalitem = 0;
            if(string.IsNullOrWhiteSpace(search))
                value = db.getAllModel(1, pageSizePopup, out pageCount, out totalitem);
            else
            {
                ThongTinBaoSearchModel searchModel = new ThongTinBaoSearchModel() { Search = search };
                value = db.searchThongTinBao(searchModel);
            }
            ViewBag.Search = search;
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_TimKiemMaBaoCha", new PagedList<ThongTinBaoModel>(value, 1, pageSizePopup, totalitem));
            return PartialView("_TimKiemMaBaoCha", new PagedList<ThongTinBaoModel>(value, 1, pageSizePopup, totalitem));
        }

        // POST: Bao/ThongTinBao/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBao,TenBao,MaToaSoan,BaoTrongMucLuc,CoThue,MucThue,BaoTrungUongDiaPhuong,BaoCongIchNgoaiCongIch,LoaiAnPham,BaoNgoaiVan,SoTrang,KichThuoc,TrongLuong,GiayPhep,GiaBia,ParentId,GhiChu")] ThongTinBaoModel thongTinBaoModel)
        {
            if (ModelState.IsValid)
            {                
                ErrorObject err = new ErrorObject();
                thongTinBaoModel.UnitCode = userInfo.UnitCode;
                thongTinBaoModel.userId = userInfo.Id;
                thongTinBaoModel.CreateDate = DateTime.Now;
                err = db.Create(thongTinBaoModel);                
                if (err.HasError)
                {
                    buildError(err);
                    getValueForDropDownList(thongTinBaoModel);
                    return View(thongTinBaoModel);
                }                
                return RedirectToAction("Index");
            }
            getValueForDropDownList(thongTinBaoModel);  
            return View(thongTinBaoModel);
        }

        public ActionResult SuaBao(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinBaoModel thongTinBaoModel = db.getThongTinBaoById(id);
            if (thongTinBaoModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.BaoId = id;
            getValueForDropDownList(thongTinBaoModel);
            return View();
        }

        public ActionResult XemBao(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinBaoModel thongTinBaoModel = db.getThongTinBaoById(id);
            if (thongTinBaoModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.BaoId = id;
            return View();
        }
        // GET: Bao/ThongTinBao/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinBaoModel thongTinBaoModel = db.getThongTinBaoById(id);            
            if (thongTinBaoModel == null)
            {
                return HttpNotFound();
            }
            getValueForDropDownList(thongTinBaoModel);
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_Edit", thongTinBaoModel);
            return View(thongTinBaoModel);
        }
        public ActionResult ViewBao(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinBaoModel thongTinBaoModel = db.getThongTinBaoById(id);
            if (thongTinBaoModel == null)
            {
                return HttpNotFound();
            }
            getValueForDropDownList(thongTinBaoModel);
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_View", thongTinBaoModel);
            return View(thongTinBaoModel);
        }
        public ActionResult ViewDiemIn(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoDiemInModel bBaoDiemInModel = db.getBaoDiemInModel(id);
            if (bBaoDiemInModel == null)
            {
                return HttpNotFound();
            }
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_viewDiemIn", bBaoDiemInModel);
            return View(bBaoDiemInModel);
        }       

        // POST: Bao/ThongTinBao/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaBao,TenBao,MaToaSoan,BaoTrongMucLuc,CoThue,MucThue,BaoTrungUongDiaPhuong,BaoCongIchNgoaiCongIch,LoaiAnPham,BaoNgoaiVan,SoTrang,KichThuoc,TrongLuong,GiayPhep,GiaBia,GhiChu")] ThongTinBaoModel thongTinBaoModel)
        {
            if (ModelState.IsValid)
            {                
                ErrorObject err = new ErrorObject();
                thongTinBaoModel.UnitCode = userInfo.UnitCode; 
                thongTinBaoModel.userId = userInfo.Id;
                err = db.Edit(thongTinBaoModel);   
                
                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    getValueForDropDownList(thongTinBaoModel);
                    if (HttpContext.Request.IsAjaxRequest())
                    {
                        return PartialView("_Edit", thongTinBaoModel);
                    }
                    return View(thongTinBaoModel);
                }
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return this.Json(new { MSG = "Thêm thành công" });
                }
                return RedirectToAction("Index");
            }
            getValueForDropDownList(thongTinBaoModel);

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_Edit", thongTinBaoModel);
            }
            return View(thongTinBaoModel);
        }

        // GET: Bao/ThongTinBao/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThongTinBaoModel thongTinBaoModel = db.getThongTinBaoById(id);
            if (thongTinBaoModel == null)
            {
                return HttpNotFound();
            }
            return View(thongTinBaoModel);
        }

        // POST: Bao/ThongTinBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            db.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDiemIn(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoDiemInModel bBaoDiemInModel = db.getBaoDiemInModel(id);
            if (bBaoDiemInModel == null)
            {
                return HttpNotFound();
            }
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_UpdateDiemIn", bBaoDiemInModel);
            return View(bBaoDiemInModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDiemIn(string id, string lstDiemIn)
        {
            db.CapNhatBaoDiemIn(id, lstDiemIn);
            if (HttpContext.Request.IsAjaxRequest())
                return this.Json(new { MSG = "Thành công" });
            return RedirectToAction("Index");
            //return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateKyXuatBan(string id, string KyConfig)
        {
            dynamic _data = JsonConvert.DeserializeObject<ExpandoObject>(KyConfig);
            int LoaiKy = Convert.ToInt32(_data.id);
            db.UpdateKyXuatBan(id, LoaiKy, KyConfig, userInfo.Id);
            if (HttpContext.Request.IsAjaxRequest())
                return this.Json(new { MSG = "Thành công" });
            return RedirectToAction("Index");
            //return View();
        }
        public ActionResult KyXuatBan(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(id);
            if (baoKyXuatBanModel == null)
            {
                return HttpNotFound();
            }
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_KyXuatBan", baoKyXuatBanModel);
            return View(baoKyXuatBanModel);
        }

        private void getValueForDropDownList(ThongTinBaoModel thongTinBaoModel)
        {
            if(thongTinBaoModel != null)
            {
                List<DefineSelectItem> toaSoan = db.getListDMToanSoan();
                ViewBag.MaToaSoan = new SelectList(toaSoan, "Value", "Text", thongTinBaoModel.MaToaSoan);

                List<DefineSelectItem> loaiAnPham = db.getListLoaiAnPham();
                ViewBag.LoaiAnPham = new SelectList(loaiAnPham, "Value", "Text", thongTinBaoModel.TenLoaiAnPham);                
            }
            else
            {
                List<DefineSelectItem> toaSoan = db.getListDMToanSoan();
                ViewBag.MaToaSoan = new SelectList(toaSoan, "Value", "Text");

                List<DefineSelectItem> loaiAnPham = db.getListLoaiAnPham();
                ViewBag.LoaiAnPham = new SelectList(loaiAnPham, "Value", "Text");               
            }            
        }
        public ActionResult AdvancedSearch(string page)
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
        public ActionResult AdvancedSearch([Bind(Include = "GiaBia,SoTrang,TrongLuong,KichThuoc,GiayPhep")] ThongTinBaoSearchModel thongTinBaoSearchModel, int? page)
        {
            int pagenum = 0;
            int pageCount = 0;
            if (String.IsNullOrEmpty(thongTinBaoSearchModel.GiaBia) && String.IsNullOrEmpty(thongTinBaoSearchModel.SoTrang)
                && String.IsNullOrEmpty(thongTinBaoSearchModel.TrongLuong) && String.IsNullOrEmpty(thongTinBaoSearchModel.KichThuoc)
                && String.IsNullOrEmpty(thongTinBaoSearchModel.GiayPhep))
            {
                return RedirectToAction("Index");
            }
            if (!String.IsNullOrEmpty(Convert.ToString(page)))
            {
                pagenum = page.Value;
            }
            else pagenum = 1;
            int totalitem = 0;
            var value = db.searchThongTinBao(thongTinBaoSearchModel);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            Session[Constants.Application.Session.ModelSearch] = thongTinBaoSearchModel;
            return View("Index", new PagedList<ThongTinBaoModel>(value, pagenum, pageSize, totalitem));
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
        public ActionResult Search([Bind(Include = "Search")] ThongTinBaoSearchModel thongTinBaoSearchModel, int? pageIndex)
        {
            int pagenum = 0;
            int pageCount = 0;

            if (!String.IsNullOrEmpty(Convert.ToString(pageIndex)))
            {
                pagenum = pageIndex.Value;
            }
            else pagenum = 1;
            int totalitem = 0;
            var value = db.searchThongTinBao(thongTinBaoSearchModel);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            Session[Constants.Application.Session.ModelSearch] = thongTinBaoSearchModel;
            return View("Index",new PagedList<ThongTinBaoModel>(value, pagenum, pageSize, totalitem));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiemMaBaoCha(string search, string pageIndex)
        {
            return this.GetMaBaoCha(search, pageIndex);
        }
        public ActionResult AjaxPaging(string pageIndex)
        {
            return this.GetMaBaoCha("", pageIndex);
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
