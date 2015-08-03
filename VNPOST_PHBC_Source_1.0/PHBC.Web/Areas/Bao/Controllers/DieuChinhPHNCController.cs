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
    /// created : 23/07/2015
    /// Author : vietvb
    /// </summary>
    public class DieuChinhPHNCController : BaseController
    {
        //private dbDieuChinhPHNC_PHBCEntities dbDieuChinhPHNC = new dbDieuChinhPHNC_PHBCEntities();
        IBDieuChinhPHNCBussiness dbDieuChinhPHNC;
        string subAction = "DieuChinhPHNC";
        public DieuChinhPHNCController(IBDieuChinhPHNCBussiness _IBDieuChinhPHNCBussiness)
        {
            dbDieuChinhPHNC = _IBDieuChinhPHNCBussiness;
            ViewBag.TitleName = " Phân hướng nhu cầu cho điều chỉnh KHXB ";
            ViewBag.SubAction = subAction;
        }
        // GET: Admin/PHNC

        public ActionResult IndexDieuChinhPHNC(string DieuChinhKHXBDetailId, string pageIndex = "")
        {
            BDieuChinhPhanHuongUnitModel objPHNC = new BDieuChinhPhanHuongUnitModel();
            BDieuChinhKHXBDetail objDetail = dbDieuChinhPHNC.getDieuChinhDetailById(DieuChinhKHXBDetailId);
            if (!string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
            {
                objPHNC.ThongTinBaoId = objDetail.BDieuChinhKHXB.ThongTinBaoId;
                objPHNC.DieuChinhKHXBDetailId = objDetail.Id;
                objPHNC.SoBao = objDetail.SoBao;
            }

            //check session unitmodel
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
            List<TinhThanh> lstTinh = objUnitNew.getAllProvince();
            ViewBag.ThongTinDetail = objDetail;
            if (HttpContext.Request.IsAjaxRequest())
                return PartialView("_PHNCView", lstTinh);
            return View(lstTinh);
        }

        // GET: Admin/PHNC/Details/5
        public ActionResult DetailsDieuChinhPHNC(string provincecode, string districtcode, string DieuChinhKHXBDetailId)
        {
            BDieuChinhPhanHuongUnitModel objPHNC = new BDieuChinhPhanHuongUnitModel();
            if (!string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
            {
                BDieuChinhKHXBDetail objDetail = dbDieuChinhPHNC.getDieuChinhDetailById(DieuChinhKHXBDetailId);
                objPHNC.DieuChinhKHXBDetailId = objDetail.Id;
                objPHNC.ThongTinBaoId = objDetail.BDieuChinhKHXB.ThongTinBaoId;
                ViewBag.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
            }
            
            //check session unitmodel
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
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
        public ActionResult CreateDieuChinhPHNC(string ThongTinBaoId, string DieuChinhKHXBDetailId, string provincecode)
        {
            BDieuChinhPhanHuongUnitModel objPHNC = new BDieuChinhPhanHuongUnitModel();
            bindComboboxDieuChinhPHNC(1);
            ViewBag.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
            if (!string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
            {
                BDieuChinhKHXBDetail objDetail = dbDieuChinhPHNC.getDieuChinhDetailById(DieuChinhKHXBDetailId);
                objPHNC.DieuChinhKHXBDetailId = objDetail.Id;
            }

            //check session unitmodel
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            objUnitNew = BuildSessionDieuChinhPHNC(ThongTinBaoId, DieuChinhKHXBDetailId);
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
        public ActionResult CreateDieuChinhPHNC([Bind(Include = "Id,ThongTinBaoId,DieuChinhKHXBDetailId,DiemTiepNhanId")] BDieuChinhPhanHuongUnitModel BDieuChinhPhanHuongUnit, string listId, string typeid)
        {   
            if (ModelState.IsValid)
            {
                //check session unitmodel
                string DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
                UnitModelDieuChinh objUnitNew = BuildSessionDieuChinhPHNC(BDieuChinhPhanHuongUnit.ThongTinBaoId, DieuChinhKHXBDetailId);
                List<v_Unit> lstUnits = new List<v_Unit>();
                List<QuanHuyen> lstQuanHuyen = new List<QuanHuyen>();
                //lstids typeid: 1: list province, 2: list district, 3: list unit                
                string[] lstids = listId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (lstids.Count() == 0)
                {
                    return this.Json(new { MSG = "Bạn chưa chọn quận huyện hay bưu cục nào !" });
                }
                if (!string.IsNullOrWhiteSpace(typeid) && typeid == "3")
                {
                    if (DieuChinhKHXBDetailId != null)
                    {
                        lstUnits = objUnitNew.getUnitNotMap(lstids, typeid);
                        //bind data
                        BDieuChinhPhanHuongUnit.CreateBy = userInfo.Id;
                        BDieuChinhPhanHuongUnit.CreateDate = DateTime.Now;
                        BDieuChinhPhanHuongUnit.ModifyBy = userInfo.Id;
                        BDieuChinhPhanHuongUnit.ModifyDate = DateTime.Now;
                        BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
                        //add action
                        dbDieuChinhPHNC.Add(BDieuChinhPhanHuongUnit.toBDieuChinhPhanHuongUnit(), lstUnits);
                        //map lai list vua tao cho object
                        objUnitNew.setListUnitMapNew(lstUnits, BDieuChinhPhanHuongUnit.DiemTiepNhanId);
                        Session[Application.Session.UnitModelDieuChinh] = objUnitNew;
                    }
                    else
                    {
                        BDieuChinhPhanHuongUnitModel bDieuChinhPhanHuongUnitModel = new BDieuChinhPhanHuongUnitModel();
                        lstUnits = objUnitNew.getUnitNotMap(lstids, typeid);
                        //bind data for district
                        bDieuChinhPhanHuongUnitModel.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                        bDieuChinhPhanHuongUnitModel.ThongTinBaoId = BDieuChinhPhanHuongUnit.ThongTinBaoId;
                        bDieuChinhPhanHuongUnitModel.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
                        bDieuChinhPhanHuongUnitModel.CreateBy = userInfo.Id;
                        bDieuChinhPhanHuongUnitModel.CreateDate = DateTime.Now;
                        bDieuChinhPhanHuongUnitModel.ModifyBy = userInfo.Id;
                        bDieuChinhPhanHuongUnitModel.ModifyDate = DateTime.Now;
                        //add action
                        dbDieuChinhPHNC.Add(bDieuChinhPhanHuongUnitModel.toBDieuChinhPhanHuongUnit(), lstUnits);
                    }
                }
                else if(!string.IsNullOrWhiteSpace(typeid))
                {
                    lstQuanHuyen = objUnitNew.getDistrictNotMap(lstids, typeid);
                    if(DieuChinhKHXBDetailId != null)
                    { 
                        BDieuChinhPhanHuongDistrictModel bDieuChinhPhanHuongDistrict = new BDieuChinhPhanHuongDistrictModel();
                        //bind data for district
                        bDieuChinhPhanHuongDistrict.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                        bDieuChinhPhanHuongDistrict.ThongTinBaoId = BDieuChinhPhanHuongUnit.ThongTinBaoId;
                        bDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
                        bDieuChinhPhanHuongDistrict.CreateBy = userInfo.Id;
                        bDieuChinhPhanHuongDistrict.CreateDate = DateTime.Now;
                        bDieuChinhPhanHuongDistrict.ModifyBy = userInfo.Id;
                        bDieuChinhPhanHuongDistrict.ModifyDate = DateTime.Now;
                        //add action
                        dbDieuChinhPHNC.AddDistrict(bDieuChinhPhanHuongDistrict.toBDieuChinhPhanHuongDistrict(), lstQuanHuyen);
                    }
                    else
                    {
                        BPhanHuongNhuCauDistrictModel bPhanHuongNhuCauDistrictModel = new BPhanHuongNhuCauDistrictModel();
                        //bind data for district
                        bPhanHuongNhuCauDistrictModel.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                        bPhanHuongNhuCauDistrictModel.ThongTinBaoId = BDieuChinhPhanHuongUnit.ThongTinBaoId;
                        bPhanHuongNhuCauDistrictModel.CreateBy = userInfo.Id;
                        bPhanHuongNhuCauDistrictModel.CreateDate = DateTime.Now;
                        bPhanHuongNhuCauDistrictModel.ModifyBy = userInfo.Id;
                        bPhanHuongNhuCauDistrictModel.ModifyDate = DateTime.Now;
                        //add action
                        dbDieuChinhPHNC.AddDistrict(bPhanHuongNhuCauDistrictModel.toBPhanHuongNhuCauDistrict(), lstQuanHuyen);
                    }
                }
                
                if (Session["ProvinceCode"] != null)
                {
                    return Json(new { MSG = "Bạn đã thiết lập thông tin phân hướng thành công !" });
                }
                else
                {
                    return Json(new { MSG = "Bạn đã thiết lập thông tin phân hướng thành công !" });
                }
            }

            return this.Json(new { MSG = "Có lỗi xảy ra, dữ liệu không hợp lệ !" });
        }

        // GET: Admin/PHNC/delete/5
        //id : DiemTiepNhanId
        public ActionResult DeleteDieuChinhPHNC(string id, string DieuChinhKHXBDetailId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get BDiemTiepNhan with DiemTiepNhanId 
            BDiemTiepNhan bDiemTiepNhan = dbDieuChinhPHNC.getByDiemTiepNhanId(id);

            //tạo model mới để return ra view DiemTiepNhanId và ThongTinBaoId
            BDieuChinhPhanHuongUnitModel BDieuChinhPhanHuongUnit = new BDieuChinhPhanHuongUnitModel();
            BDieuChinhPhanHuongUnit.DiemTiepNhanId = id;
            BDieuChinhPhanHuongUnit.ThongTinBaoId = DieuChinhKHXBDetailId;

            //check lỗi không có điểm tiếp nhận
            if (bDiemTiepNhan == null)
            {
                return HttpNotFound();
            }

            //Viewbag chung để return ra view input            
            ViewBag.DTNBC = dbDieuChinhPHNC.getAllData(userInfo.UnitCode, DieuChinhKHXBDetailId); //left tree data
            ViewBag.DiemTiepNhanId = new SelectList(dbDieuChinhPHNC.getAllDiemTiepNhan(), "Id", "Name", BDieuChinhPhanHuongUnit.DiemTiepNhanId);
            ViewBag.ThongTinBaoId = new SelectList(dbDieuChinhPHNC.getAllThongTinBao(), "Id", "DieuChinhKHXBDetailId", BDieuChinhPhanHuongUnit.ThongTinBaoId);
            ViewBag.UnitCode = dbDieuChinhPHNC.getAllUnitByDTNId(userInfo.UnitCode, id, DieuChinhKHXBDetailId);
            ViewBag.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
            return View(BDieuChinhPhanHuongUnit);
        }

        // POST: Admin/PHNC/Delete/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDieuChinhPHNC([Bind(Include = "Id,UnitCode,ThongTinBaoId,DiemTiepNhanId")] BDieuChinhPhanHuongUnitModel BDieuChinhPhanHuongUnit, string Units)
        {
            string DieuChinhKHXBDetailId = null;
            DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.ThongTinBaoId; // lấy thông tin mã báo
            if (ModelState.IsValid)
            {
                //object temp để lưu dữ liệu PHNC
                BDieuChinhPhanHuongUnit temp = new BDieuChinhPhanHuongUnit();

                string[] lstids = Units.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in lstids)
                {
                    //lấy thông tin đổ vào temp param : UnitCode và ThongTinBaoId
                    temp = dbDieuChinhPHNC.getByUnit(item, BDieuChinhPhanHuongUnit.ThongTinBaoId);
                    dbDieuChinhPHNC.Delete(temp.Id);
                }
                return RedirectToAction("Index" + subAction, new { DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.ThongTinBaoId });
            }

            //Viewbag chung để return ra view input            
            ViewBag.DTNBC = dbDieuChinhPHNC.getAllData(userInfo.UnitCode, DieuChinhKHXBDetailId); //left tree data
            ViewBag.DiemTiepNhanId = new SelectList(dbDieuChinhPHNC.getAllDiemTiepNhan(), "Id", "Name", BDieuChinhPhanHuongUnit.DiemTiepNhanId);
            ViewBag.ThongTinBaoId = new SelectList(dbDieuChinhPHNC.getAllThongTinBao(), "Id", "DieuChinhKHXBDetailId", BDieuChinhPhanHuongUnit.ThongTinBaoId);
            ViewBag.UnitCode = dbDieuChinhPHNC.getAllUnitByDTNId(userInfo.UnitCode, BDieuChinhPhanHuongUnit.DiemTiepNhanId, DieuChinhKHXBDetailId);
            ViewBag.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
            return View(BDieuChinhPhanHuongUnit);
        }


        // GET: Admin/PHNC/Edit/5?provinecode=1
        public ActionResult EditDieuChinhPHNC(string provincecode, string DieuChinhKHXBDetailId)
        {
            //ViewBag.DTNBC = dbDieuChinhPHNC.getAllData(userInfo.UnitCode, DieuChinhKHXBDetailId);
            BDieuChinhPhanHuongUnitModel objPHNC = new BDieuChinhPhanHuongUnitModel();
            bindComboboxDieuChinhPHNC(1);
            ViewBag.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
            if (!string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
            {
                BDieuChinhKHXBDetail objDetail = dbDieuChinhPHNC.getDieuChinhDetailById(DieuChinhKHXBDetailId);
                objPHNC.DieuChinhKHXBDetailId = objDetail.Id;
            }

            //check session unitmodel
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
            List<QuanHuyen> lstHuyen = objUnitNew.getDistrictMapByProvinceCode(provincecode);

            TempData["TypeUnit"] = "2";
            TempData["DistrictComment"] = "bưu cục đã thiết lập phân hướng nhu cầu";

            ViewBag.QuanHuyen = lstHuyen;
            return View(objPHNC);
        }

        // POST: Admin/PHNC/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDieuChinhPHNC([Bind(Include = "Id,UnitCode,DieuChinhKHXBDetailId,DiemTiepNhanId,CreateDate,CreateBy")] BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit, string listId, string typeid)
        {
            string DieuChinhKHXBDetailId = null;
            //check session unitmodel
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            List<v_Unit> lstUnits = new List<v_Unit>();
            List<QuanHuyen> lstQuanHuyen = new List<QuanHuyen>();
            string currentprv = "";
            if (ModelState.IsValid)
            {
                DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
                objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
                //lstids typeid: 1: list province, 2: list district, 3: list unit  
                string[] lstids = listId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (lstids.Count() == 0)
                {
                    return this.Json(new { MSG = "Bạn chưa chọn quận huyện hay bưu cục nào !" });
                }
                if (!string.IsNullOrWhiteSpace(typeid) && typeid == "3")
                {
                    lstUnits = objUnitNew.getUnitMap(lstids, typeid);
                    //get current province to back
                    currentprv = lstUnits.Select(t => t.ProvinceCode).FirstOrDefault();
                    //bind data
                    BDieuChinhPhanHuongUnit.CreateBy = userInfo.Id;
                    BDieuChinhPhanHuongUnit.CreateDate = DateTime.Now;
                    BDieuChinhPhanHuongUnit.ModifyBy = userInfo.Id;
                    BDieuChinhPhanHuongUnit.ModifyDate = DateTime.Now;
                    BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;  
                    //add action
                    dbDieuChinhPHNC.Add(BDieuChinhPhanHuongUnit, lstUnits);
                    //remap unit lai cho object
                    objUnitNew.UpdateListUnitMap(lstUnits, BDieuChinhPhanHuongUnit.DiemTiepNhanId);
                    Session[Application.Session.UnitModelDieuChinh] = objUnitNew;
                }
                else if (!string.IsNullOrWhiteSpace(typeid))
                {
                    lstQuanHuyen = objUnitNew.getDistrictMap(lstids, typeid);
                    currentprv = lstQuanHuyen.FirstOrDefault().ProvinceCode;
                    BDieuChinhPhanHuongDistrictModel bDieuChinhPhanHuongDistrict = new BDieuChinhPhanHuongDistrictModel();
                    //bind data for district
                    bDieuChinhPhanHuongDistrict.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                    bDieuChinhPhanHuongDistrict.ThongTinBaoId = BDieuChinhPhanHuongUnit.ThongTinBaoId;
                    bDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId = DieuChinhKHXBDetailId;
                    bDieuChinhPhanHuongDistrict.CreateBy = userInfo.Id;
                    bDieuChinhPhanHuongDistrict.CreateDate = DateTime.Now;
                    bDieuChinhPhanHuongDistrict.ModifyBy = userInfo.Id;
                    bDieuChinhPhanHuongDistrict.ModifyDate = DateTime.Now;
                    //add action
                    dbDieuChinhPHNC.AddDistrict(bDieuChinhPhanHuongDistrict.toBDieuChinhPhanHuongDistrict(), lstQuanHuyen);
                    //map lai list vua tao cho object
                    objUnitNew.UpdateListDistrictMap(lstQuanHuyen, bDieuChinhPhanHuongDistrict.DiemTiepNhanId);
                    Session[Application.Session.UnitModelDieuChinh] = objUnitNew;
                }
                buildMessage("Bạn đã sửa thông tin phân hướng thành công .");
                return RedirectToAction("Edit" + subAction, new { provincecode = currentprv, DieuChinhKHXBDetailId = DieuChinhKHXBDetailId });
            }

            buildMessage("Bạn đã sửa thông tin phân hướng thât bại !");
            return RedirectToAction("Edit" + subAction, new { provincecode = currentprv, DieuChinhKHXBDetailId = DieuChinhKHXBDetailId });
        }

        // GET: Admin/PHNC/Edit/5
        public ActionResult EditPHNCDieuChinhPHNC(string id)
        {
            string districtcode = "";
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit = dbDieuChinhPHNC.getById(id);
            if (BDieuChinhPhanHuongUnit == null)
            {
                buildMessage("Bưu cục chưa được cấu hình riêng nên không thể chỉnh sửa !");
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
            bindComboboxDieuChinhPHNC(1, BDieuChinhPhanHuongUnit);
            ViewBag.UnitCode = new SelectList(dbDieuChinhPHNC.getAllUnit(), "UnitCode", "UnitName", BDieuChinhPhanHuongUnit.UnitCode);
            string[] lstids = { BDieuChinhPhanHuongUnit.UnitCode };
            objUnitNew = BuildSessionDieuChinhPHNC(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId);
            List<v_Unit> lstUnits = objUnitNew.getUnitMap(lstids, "3");
            districtcode = lstUnits.FirstOrDefault().DistrictCode;
            ViewBag.DistrictCodeDetail = districtcode;
            return View(new BDieuChinhPhanHuongUnitModel(BDieuChinhPhanHuongUnit));
        }

        // POST: Admin/PHNC/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPHNCDieuChinhPHNC([Bind(Include = "Id,DieuChinhKHXBDetailId,SoBao,Nam,Quy,UnitCode,ThongTinBaoId,DieuChinhKHXBDetailId,DiemTiepNhanId,CreateDate,CreateBy")] BDieuChinhPhanHuongUnitModel BDieuChinhPhanHuongUnit)
        {
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            if (ModelState.IsValid)
            {
                BDieuChinhPhanHuongUnit.ModifyDate = DateTime.Now;
                BDieuChinhPhanHuongUnit.ModifyBy = userInfo.Id;
                dbDieuChinhPHNC.Update(BDieuChinhPhanHuongUnit.toBDieuChinhPhanHuongUnit());
                objUnitNew = BuildSessionDieuChinhPHNC(BDieuChinhPhanHuongUnit.ThongTinBaoId);
                string[] lstids = { BDieuChinhPhanHuongUnit.UnitCode };
                List<v_Unit> lstUnits = objUnitNew.getUnitMap(lstids, "3");
                objUnitNew.UpdateListUnitMap(lstUnits, BDieuChinhPhanHuongUnit.DiemTiepNhanId);
                buildMessage("Bạn đã sửa thông tin phân hướng thành công");
                return RedirectToAction("Details" + subAction, new { DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId, districtcode = lstUnits.FirstOrDefault().DistrictCode });
            }
            bindComboboxDieuChinhPHNC(0, BDieuChinhPhanHuongUnit.toBDieuChinhPhanHuongUnit());
            ViewBag.UnitCode = new SelectList(dbDieuChinhPHNC.getAllUnit(), "UnitCode", "UnitName", BDieuChinhPhanHuongUnit.UnitCode);
            buildMessage("Bạn đã sửa thông tin phân hướng thât bại !");
            return View(BDieuChinhPhanHuongUnit);
        }
        [HttpPost]
        public ActionResult LoadDistrictDieuChinhPHNC(string lstTinh, string DieuChinhKHXBDetailId, string typeid)
        {
           string[] lstids = lstTinh.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //check session unitmodel
           UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            //check session để xem thông tin
            objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
            List<QuanHuyen> lstPrv = new List<QuanHuyen>();
            lstPrv = objUnitNew.getDistrictNotMap(lstids, typeid);
            TempData["TypeUnit"] = "1";
            TempData["DistrictComment"] = "bưu cục chưa thiết lập phân hướng nhu cầu";

            return PartialView("_inputFormHuyen", lstPrv);
        }

        [HttpPost]
        public ActionResult LoadUnitDieuChinhPHNC(string lstUnit, string DieuChinhKHXBDetailId, string typestr)
        {
            string[] lstids = lstUnit.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            //check session unitmodel
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            //check session để xem thông tin
            objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
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
        

        /// <summary>
        /// bind combobox cho diem tiep nhan va thong tin bao 
        /// </summary>
        /// <param name="BDieuChinhPhanHuongUnit"></param>
        /// <param name="check">check == 1 chi load dientiepnhan</param>
        private void bindComboboxDieuChinhPHNC(int check = 0, BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit = null)
        {
            if (BDieuChinhPhanHuongUnit != null)
            {
                ViewBag.DiemTiepNhanId = new SelectList(dbDieuChinhPHNC.getAllDiemTiepNhan(), "Id", "Name", BDieuChinhPhanHuongUnit.DiemTiepNhanId);
                if (check != 0)
                {
                    ViewBag.ThongTinBaoId = new SelectList(dbDieuChinhPHNC.getAllThongTinBao(), "Id", "TenBao", BDieuChinhPhanHuongUnit.ThongTinBaoId);
                }
            }
            else
            {
                ViewBag.DiemTiepNhanId = new SelectList(dbDieuChinhPHNC.getAllDiemTiepNhan(), "Id", "Name");
                if (check != 0)
                {
                    ViewBag.ThongTinBaoId = new SelectList(dbDieuChinhPHNC.getAllThongTinBao(), "Id", "TenBao");
                }

            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbDieuChinhPHNC.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
