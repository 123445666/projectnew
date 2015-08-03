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
using Boilerplate.Web.Mvc.Filters;

namespace PHBC.Web.Areas.Bao.Controllers
{
    /// <summary>
    /// created : 01/07/2015
    /// Author : vietvb
    /// </summary>
    public class PHNCBaoController : BaseController
    {
        //private DB_PHBCEntities db = new DB_PHBCEntities();
        IBPHNCBussiness db;
        public PHNCBaoController(IBPHNCBussiness _iBPHNCBussiness)
        {
            db = _iBPHNCBussiness;
            ViewBag.TitleName = " Phân hướng nhu cầu cho báo ";
        }
        // GET: Admin/PHNC

        public ActionResult Index(string Mabao, string pageIndex = "")
        {
            BPhanHuongNhuCauModel objPHNC = new BPhanHuongNhuCauModel();
            BThongTinBao objBao = db.getAllThongTinBaoById(Mabao);
            if (!string.IsNullOrWhiteSpace(Mabao))
            {                
                objPHNC.ThongTinBaoId = objBao.Id;
                objPHNC.TenBao = objBao.TenBao;
            }

            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            objUnitNew = BuildSession(Mabao);
            List<TinhThanh> lstTinh = objUnitNew.getAllProvince();
            ViewBag.Mabao = Mabao;
            ViewBag.ThongTinBao = objBao;
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_PHNCView", lstTinh);
            return View(lstTinh);
        }

        // GET: Admin/PHNC/Details/5
        public ActionResult Details(string provincecode, string districtcode, string Mabao)
        {
            BPhanHuongNhuCauModel objPHNC = new BPhanHuongNhuCauModel();
            if (!string.IsNullOrWhiteSpace(Mabao))
            {
                BThongTinBao objBao = db.getAllThongTinBaoById(Mabao);
                objPHNC.ThongTinBaoId = objBao.Id;
                objPHNC.TenBao = objBao.TenBao;
                ViewBag.MaBao = Mabao;
                ViewBag.ThongTinBao = objBao;
            }

            
            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            objUnitNew = BuildSession(Mabao);
            List<QuanHuyen> lstHuyen = new List<QuanHuyen>();
            if(!String.IsNullOrWhiteSpace(provincecode))
            { 
                lstHuyen = objUnitNew.getDistrictMapByProvinceCode(provincecode);
                ViewBag.UnitForm = "0";
            }
            else if (!String.IsNullOrWhiteSpace(districtcode))
            {
                lstHuyen = objUnitNew.getDistrictMapByDistrictCode(districtcode);
                ViewBag.UnitForm = "1";
                ViewBag.ProvinceCodeDetail = lstHuyen.FirstOrDefault().ProvinceCode;
            }
            ViewBag.ProvinceCode = provincecode;
            return View(lstHuyen.OrderBy(t => t.DistrictName));
        }

        // GET: Admin/PHNC/Create
        public ActionResult Create(string Mabao, string provincecode)
        {
            BPhanHuongNhuCauModel objPHNC = new BPhanHuongNhuCauModel();
            bindCombobox(1);
            ViewBag.MaBao = Mabao;
            if (!string.IsNullOrWhiteSpace(Mabao))
            {
                BThongTinBao objBao = db.getAllThongTinBaoById(Mabao);
                objPHNC.ThongTinBaoId = objBao.Id;
                objPHNC.TenBao = objBao.TenBao;
            }

            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            objUnitNew = BuildSession(Mabao);
            List<TinhThanh> lstTinh = objUnitNew.getAllProvinceNotMap(provincecode);
            if (!string.IsNullOrWhiteSpace(provincecode))
            {
                ViewBag.CheckPrv = true;
                ViewBag.QuanHuyen = objUnitNew.getDistrictNotMapByProvinceCode(provincecode);
                Session["ProvinceCode"] = provincecode;
            }
            else
            {
                if (Session["ProvinceCode"] != null)
                {
                    Session["ProvinceCode"] = null;
                }
            }
            ViewBag.ProvinceComment = "huyện chưa thiết lập phân hướng nhu cầu xong";
            TempData["DistrictComment"] = "bưu cục chưa thiết lập phân hướng nhu cầu";
            TempData["TypeUnit"] = "1";
            ViewBag.Province = lstTinh;

            if (HttpContext.Request.IsAjaxRequest())
            return PartialView("_DiemTiepNhan",objPHNC);
            return View(objPHNC);
        }

        // POST: Admin/PHNC/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ThongTinBaoId,DiemTiepNhanId")] BPhanHuongNhuCauModel bPhanHuongNhuCau, string listId, string typeid)
        {   
            if (ModelState.IsValid)
            {
                //check session unitmodel
                string Mabao = bPhanHuongNhuCau.ThongTinBaoId;
                UnitModel objUnitNew = BuildSession(Mabao);
                List<v_Unit> lstUnits = new List<v_Unit>();
                //lstids typeid: 1: list province, 2: list district, 3: list unit                
                string[] lstids = listId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (lstids.Count() == 0)
                {
                    return this.Json(new { MSG = "Bạn chưa chọn quận huyện hay bưu cục nào !" });
                } 
                lstUnits = objUnitNew.getUnitNotMap(lstids, typeid);
                //bind data
                bPhanHuongNhuCau.CreateBy = userInfo.Id;
                bPhanHuongNhuCau.CreateDate = DateTime.Now;
                bPhanHuongNhuCau.ModifyBy = userInfo.Id;
                bPhanHuongNhuCau.ModifyDate = DateTime.Now;
                //add action
                db.Add(bPhanHuongNhuCau.toBPhanHuongNhuCau(), lstUnits);
                //map lai list vua tao cho object
                objUnitNew.setListUnitMapNew(lstUnits, bPhanHuongNhuCau.DiemTiepNhanId);
                Session[Application.Session.UnitModel] = objUnitNew;
                if (Session["ProvinceCode"] != null)
                {
                    return Json(new { MSG = "Bạn đã thiết lập thành công !" });
                }
                else
                {
                    return Json(new { MSG = "Bạn đã thiết lập thành công !" });
                }
            }

            return this.Json(new { MSG = "Có lỗi xảy ra, dữ liệu không hợp lệ !" });
        }

        // GET: Admin/PHNC/delete/5
        //id : DiemTiepNhanId
        public ActionResult Delete(string id, string Mabao)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get BDiemTiepNhan with DiemTiepNhanId 
            BDiemTiepNhan bDiemTiepNhan = db.getByDiemTiepNhanId(id);

            //tạo model mới để return ra view DiemTiepNhanId và ThongTinBaoId
            BPhanHuongNhuCauModel bPhanHuongNhuCau = new BPhanHuongNhuCauModel();
            bPhanHuongNhuCau.DiemTiepNhanId = id;
            bPhanHuongNhuCau.ThongTinBaoId = Mabao;

            //check lỗi không có điểm tiếp nhận
            if (bDiemTiepNhan == null)
            {
                return HttpNotFound();
            }

            //Viewbag chung để return ra view input            
            ViewBag.DTNBC = db.getAllData(userInfo.UnitCode, Mabao); //left tree data
            ViewBag.DiemTiepNhanId = new SelectList(db.getAllDiemTiepNhan(), "Id", "Name", bPhanHuongNhuCau.DiemTiepNhanId);
            ViewBag.ThongTinBaoId = new SelectList(db.getAllThongTinBao(), "Id", "MaBao", bPhanHuongNhuCau.ThongTinBaoId);
            ViewBag.UnitCode = db.getAllUnitByDTNId(userInfo.UnitCode, id, Mabao);
            ViewBag.MaBao = Mabao;
            return View(bPhanHuongNhuCau);
        }

        // POST: Admin/PHNC/Delete/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "Id,UnitCode,ThongTinBaoId,DiemTiepNhanId")] BPhanHuongNhuCauModel bPhanHuongNhuCau, string Units)
        {
            string Mabao = null;
            Mabao = bPhanHuongNhuCau.ThongTinBaoId; // lấy thông tin mã báo
            if (ModelState.IsValid)
            {
                //object temp để lưu dữ liệu PHNC
                BPhanHuongNhuCau temp = new BPhanHuongNhuCau();

                string[] lstids = Units.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in lstids)
                {
                    //lấy thông tin đổ vào temp param : UnitCode và ThongTinBaoId
                    temp = db.getByUnit(item, bPhanHuongNhuCau.ThongTinBaoId);
                    db.Delete(temp.Id);
                }
                return RedirectToAction("Index", new { Mabao = bPhanHuongNhuCau.ThongTinBaoId });
            }

            //Viewbag chung để return ra view input            
            ViewBag.DTNBC = db.getAllData(userInfo.UnitCode, Mabao); //left tree data
            ViewBag.DiemTiepNhanId = new SelectList(db.getAllDiemTiepNhan(), "Id", "Name", bPhanHuongNhuCau.DiemTiepNhanId);
            ViewBag.ThongTinBaoId = new SelectList(db.getAllThongTinBao(), "Id", "MaBao", bPhanHuongNhuCau.ThongTinBaoId);
            ViewBag.UnitCode = db.getAllUnitByDTNId(userInfo.UnitCode, bPhanHuongNhuCau.DiemTiepNhanId, Mabao);
            ViewBag.MaBao = Mabao;
            return View(bPhanHuongNhuCau);
        }


        // GET: Admin/PHNC/Edit/5?provinecode=1
        public ActionResult Edit(string provincecode, string Mabao)
        {
            //ViewBag.DTNBC = db.getAllData(userInfo.UnitCode, Mabao);
            BPhanHuongNhuCauModel objPHNC = new BPhanHuongNhuCauModel();
            bindCombobox(1);
            ViewBag.MaBao = Mabao;
            if (!string.IsNullOrWhiteSpace(Mabao))
            {
                BThongTinBao objBao = db.getAllThongTinBaoById(Mabao);
                objPHNC.ThongTinBaoId = objBao.Id;
                objPHNC.TenBao = objBao.TenBao;
            }

            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            objUnitNew = BuildSession(Mabao);
            List<QuanHuyen> lstHuyen = objUnitNew.getDistrictMapByProvinceCode(provincecode);

            TempData["TypeUnit"] = "2";
            TempData["DistrictComment"] = "bưu cục đã thiết lập phân hướng nhu cầu";

            ViewBag.QuanHuyen = lstHuyen;
            return View(objPHNC);
        }

        // POST: Admin/PHNC/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UnitCode,ThongTinBaoId,DiemTiepNhanId,CreateDate,CreateBy")] BPhanHuongNhuCau bPhanHuongNhuCau, string listId, string typeid)
        {
            string Mabao = null;
            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            List<v_Unit> lstUnits = new List<v_Unit>();
            string currentprv = "";
            if (ModelState.IsValid)
            {
                Mabao = bPhanHuongNhuCau.ThongTinBaoId;
                objUnitNew = BuildSession(Mabao);
                //lstids typeid: 1: list province, 2: list district, 3: list unit  
                string[] lstids = listId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                lstUnits = objUnitNew.getUnitMap(lstids, typeid);
                //get current province to back
                currentprv = lstUnits.Select(t => t.ProvinceCode).FirstOrDefault();
                //bind data
                bPhanHuongNhuCau.CreateBy = userInfo.Id;
                bPhanHuongNhuCau.CreateDate = DateTime.Now;
                bPhanHuongNhuCau.ModifyBy = userInfo.Id;
                bPhanHuongNhuCau.ModifyDate = DateTime.Now;
                //add action
                db.Add(bPhanHuongNhuCau, lstUnits);
                //remap unit lai cho object
                objUnitNew.UpdateListUnitMap(lstUnits, bPhanHuongNhuCau.DiemTiepNhanId);
                Session[Application.Session.UnitModel] = objUnitNew;
                buildMessage("Bạn đã sửa thành công .");
                return RedirectToAction("Edit", new { provincecode = currentprv, Mabao = bPhanHuongNhuCau.ThongTinBaoId });
            }

            buildMessage("Bạn đã sửa thiết lập thất bại !");
            return RedirectToAction("Edit", new { provincecode = currentprv, Mabao = bPhanHuongNhuCau.ThongTinBaoId });
        }

        // GET: Admin/PHNC/Edit/5
        public ActionResult EditPHNC(string id)
        {
            string districtcode = "";
            UnitModel objUnitNew = new UnitModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BPhanHuongNhuCau bPhanHuongNhuCau = db.getById(id);
            if (bPhanHuongNhuCau == null)
            {
                return HttpNotFound();
            }
            bindCombobox(1, bPhanHuongNhuCau);
            ViewBag.UnitCode = new SelectList(db.getAllUnit(), "UnitCode", "UnitName", bPhanHuongNhuCau.UnitCode);
            string[] lstids = { bPhanHuongNhuCau.UnitCode };
            objUnitNew = BuildSession(bPhanHuongNhuCau.ThongTinBaoId);
            List<v_Unit> lstUnits = objUnitNew.getUnitMap(lstids, "3");
            districtcode = lstUnits.FirstOrDefault().DistrictCode;
            ViewBag.DistrictCodeDetail = districtcode;
            return View(new BPhanHuongNhuCauModel(bPhanHuongNhuCau));
        }

        // POST: Admin/PHNC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPHNC([Bind(Include = "Id,UnitCode,ThongTinBaoId,DiemTiepNhanId,CreateDate,CreateBy")] BPhanHuongNhuCauModel bPhanHuongNhuCau)
        {
            UnitModel objUnitNew = new UnitModel();
            if (ModelState.IsValid)
            {
                bPhanHuongNhuCau.ModifyDate = DateTime.Now;
                bPhanHuongNhuCau.ModifyBy = userInfo.Id;
                db.Update(bPhanHuongNhuCau.toBPhanHuongNhuCau());
                objUnitNew = BuildSession(bPhanHuongNhuCau.ThongTinBaoId);
                string[] lstids = { bPhanHuongNhuCau.UnitCode };
                List<v_Unit> lstUnits = objUnitNew.getUnitMap(lstids, "3");
                objUnitNew.UpdateListUnitMap(lstUnits, bPhanHuongNhuCau.DiemTiepNhanId);
                buildMessage("Bạn đã sửa thành công");
                return RedirectToAction("Details", new { Mabao = bPhanHuongNhuCau.ThongTinBaoId, districtcode = lstUnits.FirstOrDefault().DistrictCode });
            }
            bindCombobox(0, bPhanHuongNhuCau.toBPhanHuongNhuCau());
            ViewBag.UnitCode = new SelectList(db.getAllUnit(), "UnitCode", "UnitName", bPhanHuongNhuCau.UnitCode);
            buildMessage("Bạn đã sửa thiết lập thất bại !");
            return View(bPhanHuongNhuCau);
        }
        [HttpPost]
        public ActionResult LoadDistrict(string lstTinh, string Mabao)
        {
           string[] lstids = lstTinh.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            //check session để xem thông tin
            objUnitNew = BuildSession(Mabao);
            List<QuanHuyen> lstPrv = new List<QuanHuyen>();
            lstPrv = objUnitNew.getDistrictNotMap(lstids);
            TempData["TypeUnit"] = "1";
            TempData["DistrictComment"] = "bưu cục chưa thiết lập phân hướng nhu cầu";

            return PartialView("_inputFormHuyen", lstPrv);
        }

        [HttpPost]
        public ActionResult LoadUnit(string lstUnit, string Mabao, string typestr)
        {
            string[] lstids = lstUnit.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            //check session unitmodel
            UnitModel objUnitNew = new UnitModel();
            //check session để xem thông tin
            objUnitNew = BuildSession(Mabao);
            List<v_Unit> units = new List<v_Unit>();
            //typestr : 1: getUnitNotMap, 2: getUnitMap
            if (string.IsNullOrWhiteSpace(typestr) || typestr == "1")
            {
                units = objUnitNew.getUnitNotMap(lstids, "2");
            }
            else if (!string.IsNullOrWhiteSpace(typestr) && typestr == "2")
            {
               units = objUnitNew.getUnitMap(lstids, "2");
            }
            return PartialView("_inputFormBuuCuc", units);
        }
        private UnitModel BuildSession(string Mabao)
        {
            UnitModel objUnitNew = new UnitModel();
            if (Session[Application.Session.UnitModel] != null)
            {
                objUnitNew = (UnitModel)Session[Application.Session.UnitModel];                
                if(objUnitNew.ThongTinBaoId != Mabao)
                {
                    objUnitNew = InsertSession(Mabao);
                }
            }
            else
            {
                objUnitNew = InsertSession(Mabao);
            }
            return objUnitNew;
        }

        private UnitModel InsertSession(string Mabao)
        {
            UnitModel objUnitNew = new UnitModel();
            List<v_Unit> lstVUnit = db.getAllVunit();
            List<Province> lstAllPrv = db.getAllProvince();
            List<BPhanHuongNhuCau> lstPHNC = db.getAllPHNCByMaBaoConfig(Mabao);
            List<BDiemTiepNhan> lstDTN = db.getAllDiemTiepNhan();
            objUnitNew = new UnitModel(lstVUnit, lstAllPrv, lstDTN);
            objUnitNew.setListPHNC(lstPHNC);
            objUnitNew.ThongTinBaoId = Mabao;
            Session[Application.Session.UnitModel] = objUnitNew;
            return objUnitNew;
        }
        /// <summary>
        /// bind combobox cho diem tiep nhan va thong tin bao 
        /// </summary>
        /// <param name="bPhanHuongNhuCau"></param>
        /// <param name="check">check == 1 chi load dientiepnhan</param>
        private void bindCombobox(int check = 0, BPhanHuongNhuCau bPhanHuongNhuCau = null)
        {
            if (bPhanHuongNhuCau != null)
            {
                ViewBag.DiemTiepNhanId = new SelectList(db.getAllDiemTiepNhan(), "Id", "Name", bPhanHuongNhuCau.DiemTiepNhanId);
                if (check != 0)
                {
                    ViewBag.ThongTinBaoId = new SelectList(db.getAllThongTinBao(), "Id", "MaBao", bPhanHuongNhuCau.ThongTinBaoId);
                }
            }
            else
            {
                ViewBag.DiemTiepNhanId = new SelectList(db.getAllDiemTiepNhan(), "Id", "Name");
                if (check != 0)
                {
                    ViewBag.ThongTinBaoId = new SelectList(db.getAllThongTinBao(), "Id", "MaBao");
                }

            }
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
