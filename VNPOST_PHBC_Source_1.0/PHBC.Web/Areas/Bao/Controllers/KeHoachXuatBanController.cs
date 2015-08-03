using PHBC.DAO;
using PHBC.DAO.Bussiness;
using PHBC.DAO.Models;
using PHBC.Web.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.Dynamic;
using Webdiyer.WebControls.Mvc;
using PHBC.Web.Constants;
using Newtonsoft.Json;
using PHBC.DAO.Common;

namespace PHBC.Web.Areas.Bao.Controllers
{
    public class KeHoachXuatBanController : BaseController
    {
        private int pageSize = 20;
        private IKeHoachXuatBanBussiness db;
        string subAction = "DieuChinhPHNC";

        public KeHoachXuatBanController(IKeHoachXuatBanBussiness _iKeHoachXuatBanBussiness)
        {
            this.db = _iKeHoachXuatBanBussiness;
            ViewBag.SubAction = subAction;
        }

        public void setInFor(BDieuChinhKHXB ac)
        {
            ac.Id = "alo";
        }

        
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
            int yearSearch = -1;
            int.TryParse(Request.QueryString["keyyear"], out yearSearch);
            yearSearch = yearSearch == 0 ? DateTime.Now.Year : yearSearch;

            var value = db.getAll(pageNum, pageSize, out pageCount, out totalitem, Request.QueryString["keysearch"], yearSearch);
            ViewBag.Page = pageNum;
            ViewBag.PageCount = pageCount;

            return View(new PagedList<dynamic>(value, pageNum, pageSize, totalitem));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDieuChinhKeHoachXuatBan(string id, string IdKeHoachXuatBan, string Nam, string Quy, string Copy, string urlReturn)
        {
            db.addDieuChinhKHXB(id, Convert.ToInt32(Nam), Convert.ToInt32(Quy), userInfo.Id, Copy);
            return Redirect(urlReturn);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateKeHoachXuatBan(string id, string IdKeHoachXuatBan, string LoaiKy, string Nam, string SoBatDau, string urlReturn)
        {

            db.updateBKeHoachXuatBan(id, Convert.ToInt32(Nam), Convert.ToInt32(SoBatDau), userInfo.Id);
            if (HttpContext.Request.IsAjaxRequest())
                return this.Add(id, null);
            return Redirect(urlReturn);
        }

        public ActionResult DieuChinhKeHoachXuatBan(string id, string idkehoach)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BKeHoachXuatBan _xuatban = db.getBKeHoachXuatBan(idkehoach);
            if (_xuatban == null)
            {
                return RedirectToAction("add/" + id);
            }
            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(id);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = baoKyXuatBanModel.KyConfig;
            _kehoach.LoaiKy = baoKyXuatBanModel.LoaiKy;
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = idkehoach;
            _kehoach.Nam = _xuatban.Nam;
            _kehoach.SoBatDau = _xuatban.SoBatDau;
            if (_xuatban.Nam < DateTime.Now.Year)
                return Redirect(Request.UrlReferrer.ToString());
            //return RedirectToAction("Index");
            int[] quyCSDL = db.getBDieuChinhKHXB(id, Convert.ToInt32(_xuatban.Nam)).Select(d => d.Quy).ToArray();
            int month = DateTime.Now.Year == _xuatban.Nam ? DateTime.Now.Month : 1;
            int quyNow = month >= 1 && month <= 3 ? 1 : month > 3 && month <= 6 ? 2 : month > 6 && month <= 9 ? 3 : 4;
            quyNow = quyCSDL.Length > 0 && quyCSDL[0] >= quyNow ? quyCSDL[0] : quyNow - 1;
            List<int> arQuy = new List<int>();
            if (quyNow + 1 <= 4)
                arQuy.Add(quyNow + 1);
            //for (int i = quyNow + 1; i <= 4; i++)
            //{
            //    arQuy.Add(i);
            //}
            if (arQuy.Count <= 0)
                return Redirect(Request.UrlReferrer.ToString());
            //return RedirectToAction("Index");
            _kehoach.data.Add("arrayquy", arQuy);

            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            return View(_kehoach);
        }

        public ActionResult ChiTiet(string id, string idkehoach)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BKeHoachXuatBan _xuatban = db.getBKeHoachXuatBan(idkehoach);
            if (_xuatban == null)
                return RedirectToAction("add/" + id);
            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(id);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = baoKyXuatBanModel.KyConfig;
            _kehoach.LoaiKy = baoKyXuatBanModel.LoaiKy;
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = idkehoach;
            _kehoach.Nam = _xuatban.Nam;
            _kehoach.SoBatDau = _xuatban.SoBatDau;
            _kehoach.detailKHXB = db.getBKeHoachXuatBanDetail(id, Convert.ToInt32(_xuatban.Nam));

            _kehoach.data.Add("BDieuChinhKHXB", JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonConvert.SerializeObject((from p in db.getBDieuChinhKHXB(id, Convert.ToInt32(_xuatban.Nam)) select new { p.Id, p.ThongTinBaoId, p.ModifyDate, p.Quy, ModifyBy = (from u in db.getDBSelect().UserInfoes where u.Id.Equals(p.ModifyBy) select u.DislayName).FirstOrDefault() }))));




            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            return View(_kehoach);
        }

        public ActionResult Add(string id, string idkehoach)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //year = year == null ? DateTime.Now.Year : year < DateTime.Now.Year ? DateTime.Now.Year : year;


            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(id);

            //BKeHoachXuatBan _xuatban = db.getBKeHoachXuatBan(id, Convert.ToInt32(year));



            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = baoKyXuatBanModel.KyConfig;
            _kehoach.LoaiKy = baoKyXuatBanModel.LoaiKy;
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            if (idkehoach == null)
            {
                _kehoach.Nam = db.getBKeHoachXuatBanYearLast(id);
                _kehoach.SoBatDau = -1;
                _kehoach.IdKeHoachXuatBan = "-1";
                _kehoach.detailKHXB = new List<BKeHoachXuatBanDetail>();
            }
            else
            {
                BKeHoachXuatBan _xuatban = db.getBKeHoachXuatBan(idkehoach);
                if (_xuatban == null)
                    return RedirectToAction("add/" + id);
                _kehoach.IdKeHoachXuatBan = idkehoach;
                _kehoach.Nam = _xuatban.Nam;
                _kehoach.SoBatDau = 1;
            }
            _kehoach.khxb = db.getKHXBByID(id);
            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_Add", _kehoach);
            return View(_kehoach);
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