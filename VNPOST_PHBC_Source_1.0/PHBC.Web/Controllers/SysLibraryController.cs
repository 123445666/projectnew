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

namespace PHBC.Web.Controllers
{
    /// <summary>
    /// created : 28/07/2015
    /// Author : vietvb
    /// </summary>
    [AllowAnonymous]
    public class SysLibraryController : BaseController
    {
        ISysLibraryBussiness db;
        public SysLibraryController(ISysLibraryBussiness _iSysLibraryBussiness)
        {
            db = _iSysLibraryBussiness;
        }
        // GET: SysLibrary
        [AllowAnonymous]
        public ActionResult LoadProvince(string objid, string objname)
        {
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            List<TinhThanh> lstTinhThanh = new List<TinhThanh>();
            TempData["ObjId"] = objid;
            TempData["ObjName"] = objname;

            if (Session[Application.Session.UnitModelDieuChinh] != null)
            {
                objUnitNew = (UnitModelDieuChinh)Session[Application.Session.UnitModelDieuChinh];
                lstTinhThanh = objUnitNew.lstTinhThanh;
            }
            return PartialView("_LoadProvince", lstTinhThanh);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadDistrict(string provincecode)
        {
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            List<QuanHuyen> lstQuanHuyen = new List<QuanHuyen>();
            if (Session[Application.Session.UnitModelDieuChinh] != null)
            {
                objUnitNew = (UnitModelDieuChinh)Session[Application.Session.UnitModelDieuChinh];
                lstQuanHuyen = objUnitNew.lstTinhThanh.FirstOrDefault(t => t.ProvinceCode.Equals(provincecode)).lstQuanHuyen;
            }
            return PartialView("_LoadDistrict", lstQuanHuyen);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LoadUnit(string districtcode)
        {
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            List<v_Unit> lstUnit = new List<v_Unit>();
            if (Session[Application.Session.UnitModelDieuChinh] != null)
            {
                objUnitNew = (UnitModelDieuChinh)Session[Application.Session.UnitModelDieuChinh];
                lstUnit = objUnitNew.lstQuanHuyen.FirstOrDefault(t => t.DistrictCode.Equals(districtcode)).lstUnit;
            }
            return PartialView("_LoadUnit", lstUnit);
        }

    }
}