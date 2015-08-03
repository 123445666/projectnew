using PHBC.DAO;
using PHBC.DAO.Bussiness;
using PHBC.DAO.Models;
using PHBC.Web.Constants;
using PHBC.Web.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PHBC.Web.Base
{
    public class BaseController : Controller
    {
        public PermissonController permisson;
        public UserModel userInfo;
        public BaseController()
            : base()
        {
            this.permisson = new PermissonController();
            ViewBag.Permisson = this.permisson;
            userInfo = null;
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                 || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
                return;

            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            if (Session[Application.Session.Permisson] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
            AppPermission appPermission = Session[Application.Session.Permisson] as AppPermission;
            this.userInfo = appPermission.UserInfo;
            string areaName = filterContext.RequestContext.RouteData.DataTokens["Area"] as string?? "";
            string controllnerName = filterContext.RequestContext.RouteData.Values["Controller"].ToString();
            string actionName = filterContext.RequestContext.RouteData.Values["Action"].ToString();
            this.permisson = appPermission.getPermision(areaName, controllnerName);
            permisson.appPermisson = appPermission;
            ViewBag.Permisson = this.permisson;
            if (!permisson.hasPermisson(actionName))
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                return;
            } 
            if (Session[Application.Session.Message] != null)
            {
                ViewBag.Message = Session[Application.Session.Message];
                Session[Application.Session.Message] = null;
            }
            base.OnAuthorization(filterContext);
        }

        public bool buildError(Microsoft.AspNet.Identity.IdentityResult Error)
        {
            if (Error.Succeeded)
                return Error.Succeeded;
            foreach(var item in Error.Errors)
                ModelState.AddModelError("", item);
            return Error.Succeeded;
        }

        public bool buildError(PHBC.DAO.Common.ErrorObject Error)
        {

            if (Error.HasError)
                foreach (var item in Error.LstError)
                    ModelState.AddModelError(item.Key, item.Value);
            return Error.HasError;
        }

        public void buildMessage(string Message)
        {
            Session[Application.Session.Message] = Message;
            ViewBag.Message = Message;
        }

        #region PhanHuongNhuCau
        private UnitModelDieuChinh InsertCacheDieuChinhPHNC()
        {
            BDieuChinhPHNCBussiness dbDieuChinhPHNC = new BDieuChinhPHNCBussiness();       
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            List<v_Unit> lstVUnit = dbDieuChinhPHNC.getAllVunit();
            List<Province> lstAllPrv = dbDieuChinhPHNC.getAllProvince();
            List<BDiemTiepNhan> lstDTN = dbDieuChinhPHNC.getAllDiemTiepNhan();
            objUnitNew = new UnitModelDieuChinh(lstVUnit, lstAllPrv, lstDTN);
            List<BPhanHuongNhuCauUnit> lstPHNC = dbDieuChinhPHNC.getAllPHNCByThongTinBaoIdConfig();
            List<BPhanHuongNhuCauDistrict> lstDistrict = dbDieuChinhPHNC.getAllDistrictPHNCByThongTinBaoIdConfig();
            objUnitNew.setListPHNC(lstPHNC, lstDistrict);
            Session[Application.Session.UnitModelDieuChinh] = objUnitNew;

            return objUnitNew;
        }

        public UnitModelDieuChinh BuildSessionDieuChinhPHNC(string ThongTinBaoId = null,string DieuChinhKHXBDetailId= null)
        {
            BDieuChinhPHNCBussiness dbDieuChinhPHNC = new BDieuChinhPHNCBussiness();
            UnitModelDieuChinh objUnitNew = new UnitModelDieuChinh();
            if (Session[Application.Session.UnitModelDieuChinh] != null)
            {
                objUnitNew = (UnitModelDieuChinh)Session[Application.Session.UnitModelDieuChinh];
                if (!String.IsNullOrWhiteSpace(ThongTinBaoId))
                {
                    List<BPhanHuongNhuCauUnit> lstPHNC = dbDieuChinhPHNC.getAllPHNCByThongTinBaoIdConfig(ThongTinBaoId);
                    List<BPhanHuongNhuCauDistrict> lstDistrict = dbDieuChinhPHNC.getAllDistrictPHNCByThongTinBaoIdConfig(ThongTinBaoId);
                    objUnitNew.setListPHNC(lstPHNC, lstDistrict);
                    
                    if (!string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
                    { 
                        List<BDieuChinhPhanHuongUnit> _lstPHNC = dbDieuChinhPHNC.getAllPHNCByDieuChinhKHXBDetailIdConfig(DieuChinhKHXBDetailId);
                        List<BDieuChinhPhanHuongDistrict> _lstDistrict = dbDieuChinhPHNC.getAllDistrictPHNCByDieuChinhKHXBDetailIdConfig(DieuChinhKHXBDetailId);
                        objUnitNew.setListDieuChinhPHNC(_lstPHNC, _lstDistrict);
                    }
                    Session[Application.Session.UnitModelDieuChinh] = objUnitNew;
                }
            }
            else
            {
                objUnitNew = InsertCacheDieuChinhPHNC();
                BuildSessionDieuChinhPHNC(ThongTinBaoId, DieuChinhKHXBDetailId);
            }
            return objUnitNew;
        }
        #endregion
    }
}