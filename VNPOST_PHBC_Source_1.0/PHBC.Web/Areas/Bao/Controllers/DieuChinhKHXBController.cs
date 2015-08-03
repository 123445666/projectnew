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
using PHBC.Web.Base;
using PHBC.DAO.Bussiness;
using Newtonsoft.Json;
using PHBC.DAO.Common;
using System.Dynamic;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using PHBC.Web.Constants;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Webdiyer.WebControls.Mvc;
using Boilerplate.Web.Mvc.Filters;


namespace PHBC.Web.Areas.Bao.Controllers
{
    public class DieuChinhKHXBController : BaseController
    {
        IDieuChinhKHXBBussiness db;
        IBDieuChinhPHNCBussiness dbDieuChinhPHNC;

        string subAction = "DieuChinhPHNC";
        public DieuChinhKHXBController(IDieuChinhKHXBBussiness _db, IBDieuChinhPHNCBussiness _IBDieuChinhPHNCBussiness)
        {
            this.db = _db;
            dbDieuChinhPHNC = _IBDieuChinhPHNCBussiness;
            ViewBag.TitleNamePHNC = " Phân hướng nhu cầu cho điều chỉnh KHXB ";
            ViewBag.SubAction = subAction;
        }

        // GET: Bao/DieuChinhKHXB
        public ActionResult Index(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BDieuChinhKHXB _dieuChinh = db.getDBSelect().BDieuChinhKHXBs.Where(d=>d.Id.Equals(id)).FirstOrDefault();
            if (_dieuChinh == null)
            {
                return HttpNotFound();
                //return Redirect(Request.UrlReferrer.ToString());
            }
            //return RedirectToAction("bao/kehoachxuatban");
            int sDonSo = (int)Enums.KeHoachXuatBan.group;

            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(_dieuChinh.ThongTinBaoId);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = _dieuChinh.ChiTietKy;
            _kehoach.LoaiKy = Convert.ToInt32(_dieuChinh.LoaiKy);
            _kehoach.ThongTinBaoId = _dieuChinh.ThongTinBaoId;
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = id;
            _kehoach.Nam = _dieuChinh.Nam;
            _kehoach.Quy = _dieuChinh.Quy;
            _kehoach.detailKHXB = db.getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(_dieuChinh.ThongTinBaoId) && d.Nam == _dieuChinh.Nam && d.Quy == _dieuChinh.Quy && (d.status != sDonSo || d.Number < 0)).OrderBy(d => d.Thang).OrderBy(d => d.Ngay).ToList();
            _kehoach.dieuchinhdetailKHXB = db.getBKeHoachXuatBanDetailModel(_dieuChinh.Id).ToList();
            ViewBag.KyXuatBan = JsonConvert.DeserializeObject<ExpandoObject>(_dieuChinh.ChiTietKy);
            if (_kehoach == null)
            {
                return HttpNotFound();
            }

            return View(_kehoach);
        }

        [AllowAnonymous]
        public string getsoxuatban(string id, string idkehoach, int typekhxb, string data, int typeDieuChinh)
        {
            dynamic _data = JsonConvert.DeserializeObject<ExpandoObject>(data);
            BDieuChinhKHXB _dieuChinh = db.getDBSelect().BDieuChinhKHXBs.Where(d => d.ThongTinBaoId.Equals(id) && d.Id.Equals(idkehoach)).FirstOrDefault();
            var querySo = db.getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(id) && d.Nam == _dieuChinh.Nam && d.Quy == _dieuChinh.Quy);

            if (typeDieuChinh == (int)Enums.KeHoachXuatBan.cancel)
                querySo = querySo.Where(d => d.status != (int)Enums.KeHoachXuatBan.group && d.status != (int)Enums.KeHoachXuatBan.cancel && d.status != (int)Enums.KeHoachXuatBan.add);
            else if (typeDieuChinh == (int)Enums.KeHoachXuatBan.group)
                querySo = querySo.Where(d => d.status != (int)Enums.KeHoachXuatBan.cancel && d.status != (int)Enums.KeHoachXuatBan.add);
            else if (typeDieuChinh == (int)Enums.KeHoachXuatBan.add)
                querySo = querySo.Where(d => d.status != (int)Enums.KeHoachXuatBan.cancel && (d.status == (int)Enums.KeHoachXuatBan.group && d.Number == -1));
            else if (typeDieuChinh == (int)Enums.KeHoachXuatBan.changed)
                querySo = querySo.Where(d => ((d.status == (int)Enums.KeHoachXuatBan.group || d.status == (int)Enums.KeHoachXuatBan.add) && d.Number == -1) || d.status == (int)Enums.KeHoachXuatBan.approved);
            else
                querySo = querySo.Where(d => d.status == (int)Enums.KeHoachXuatBan.approved);

            if (_data.type == 1 && typeDieuChinh != (int)Enums.KeHoachXuatBan.group)
            {
                int[] arrNum = JsonConvert.DeserializeObject<int[]>(JsonConvert.SerializeObject(_data.data));
                if (typekhxb == 0 || typekhxb == 1)
                    querySo = querySo.Where(d => arrNum.Contains(d.Thu));
                else if (typekhxb == 2 || typekhxb == 3)
                    querySo = querySo.Where(d => arrNum.Contains(d.Ngay));
            }
            querySo = querySo.OrderBy(d => d.Number);

            return JsonConvert.SerializeObject(typeDieuChinh == (int)Enums.KeHoachXuatBan.changed ? querySo.ToList().OrderBy(d => new DateTime(d.Nam, d.Thang, d.Ngay)).ToList<BKeHoachXuatBanDetail>() : querySo.ToList<BKeHoachXuatBanDetail>());
        }

        // GET: Bao/DieuChinhKHXB/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDieuChinhKHXBDetail bDieuChinhKHXBDetail = null;
            if (bDieuChinhKHXBDetail == null)
            {
                return HttpNotFound();
            }
            return View(bDieuChinhKHXBDetail);
        }

        // GET: Bao/DieuChinhKHXB/Create
        public ActionResult Create(string type, string id)
        {
            if (type == null || id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BDieuChinhKHXB _dieuChinh = db.getDBSelect().BDieuChinhKHXBs.Where(d => d.Id.Equals(id)).FirstOrDefault();
            if (_dieuChinh == null)
            {
                return HttpNotFound();
                //return Redirect(Request.UrlReferrer.ToString());
            }

            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(_dieuChinh.ThongTinBaoId);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = _dieuChinh.ChiTietKy;
            _kehoach.LoaiKy = Convert.ToInt32(_dieuChinh.LoaiKy);
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = id;
            _kehoach.Nam = _dieuChinh.Nam;
            _kehoach.Quy = _dieuChinh.Quy;
            _kehoach.data.Add("TypeText", Enums.KieuDieuChinhKHXB(Enums.getKeHoachXuatBan(type)));
            int typeDieuCHinh = ((int)Enums.getKeHoachXuatBan(type));
            _kehoach.data.Add("Type", typeDieuCHinh);
            if (_kehoach.LoaiKy >= 0 && _kehoach.LoaiKy <= 3)
            {
                List<string> listConfig = JsonConvert.DeserializeObject<List<string>>((JsonConvert.DeserializeObject<dynamic>(_kehoach.KyConfig)).data.ToString());
                if (_kehoach.LoaiKy == 0)
                    listConfig = (new string[] { "2", "3", "4", "5", "6", "7", "8" }).ToList();
                List<string> ngayThuLoai = new List<string>();

                foreach (string _item in db.getDBSelect().BDieuChinhKHXBDetails.Where(d => d.DieuChinhKHXBId.Equals(id) && d.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.cancel && d.NgayOrThu != null).Select(d => d.NgayOrThu))
                {
                    int[] itemArr = JsonConvert.DeserializeObject<int[]>(_item);
                    foreach (string itemC in listConfig)
                    {
                        if (itemArr.Contains(Convert.ToInt32(itemC)) && !ngayThuLoai.Contains(itemC))
                            ngayThuLoai.Add(itemC);
                    }

                }
                if (typeDieuCHinh != (int)Enums.KeHoachXuatBan.cancel)
                {
                    foreach (string _item in db.getDBSelect().BDieuChinhKHXBDetails.Where(d => d.DieuChinhKHXBId.Equals(id) && d.LoaiDieuChinh == typeDieuCHinh && d.NgayOrThu != null).Select(d => d.NgayOrThu))
                    {
                        int[] itemArr = JsonConvert.DeserializeObject<int[]>(_item);
                        foreach (string itemC in listConfig)
                        {
                            if (itemArr.Contains(Convert.ToInt32(itemC)) && !ngayThuLoai.Contains(itemC))
                                ngayThuLoai.Add(itemC);
                        }

                    }
                }
                else
                {
                    foreach (string _item in db.getDBSelect().BDieuChinhKHXBDetails.Where(d => d.DieuChinhKHXBId.Equals(id) && d.LoaiDieuChinh == (int)Enums.KeHoachXuatBan.group && d.NgayOrThu != null).Select(d => d.NgayOrThu))
                    {
                        int[] itemArr = JsonConvert.DeserializeObject<int[]>(_item);
                        foreach (string itemC in listConfig)
                        {
                            if (itemArr.Contains(Convert.ToInt32(itemC)) && !ngayThuLoai.Contains(itemC))
                                ngayThuLoai.Add(itemC);
                        }

                    }
                }
                foreach (var item in ngayThuLoai)
                {
                    listConfig.Remove(item);
                }
                _kehoach.data.Add("KyConfig", listConfig.ToArray());
            }
            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            return View(_kehoach);
        }

        // POST: Bao/DieuChinhKHXB/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IEnumerable<DCThongTinBaoEditModel> lstEdit, string Config, int LoaiDieuChinh, string id, string GhiChu)
        {
            List<DCThongTinBaoModel> lstDCThongTinBao = new List<DCThongTinBaoModel>();
            if (ModelState.IsValid && CheckListDCThongTinBao(lstEdit) && !string.IsNullOrWhiteSpace(GhiChu))
            {
                if (lstEdit != null)
                    lstDCThongTinBao = (from edit in lstEdit.Where(a => a.IsActive == true)
                                        select new DCThongTinBaoModel()
                                        {
                                            Dislay = edit.Dislay,
                                            Key = edit.Key,
                                            KieuDuLieu = edit.KieuDuLieu,
                                            Value = edit.Value
                                        }
                                       ).ToList();
                ErrorObject err = db.CreateDieuChinhKHXBDetail(id, LoaiDieuChinh, Config, lstDCThongTinBao, GhiChu,userInfo.Id);
                if (err.HasError)
                {
                    base.buildError(err);
                }
                else
                {
                    buildMessage("Tạo điều chỉnh thành công");
                    return this.Json(new { MSG = "Tạo điều chỉnh thành công" });
                }
            }
            if(string.IsNullOrWhiteSpace(GhiChu))
            {
                ModelState.AddModelError("GhiChu", "Ghi chú không được trống");
            }
            ViewBag.Config = Config;
            ViewBag.LoaiDieuChinh = LoaiDieuChinh;
            ViewBag.Id = id;
            ViewBag.GhiChu = GhiChu;
            lstEdit = lstEdit ?? new List<DCThongTinBaoEditModel>();
            return PartialView("_CreateDCThongTinBao", lstEdit);
        }

        // GET: Bao/DieuChinhKHXB/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDieuChinhKHXBDetail _dieuchinhDetail = db.getDBSelect().BDieuChinhKHXBDetails.Where(d => d.Id.Equals(id)).FirstOrDefault();
            if (_dieuchinhDetail == null)
            {
                return HttpNotFound();
                //return Redirect(Request.UrlReferrer.ToString());
            }

            int typeDieuCHinh = _dieuchinhDetail.LoaiDieuChinh;
            BDieuChinhKHXB _dieuChinh = db.getDBSelect().BDieuChinhKHXBs.Where(d => d.Id.Equals(_dieuchinhDetail.DieuChinhKHXBId)).FirstOrDefault();
            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(_dieuChinh.ThongTinBaoId);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = _dieuChinh.ChiTietKy;
            _kehoach.LoaiKy = Convert.ToInt32(_dieuChinh.LoaiKy);
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = _dieuchinhDetail.DieuChinhKHXBId;
            _kehoach.Nam = _dieuChinh.Nam;
            _kehoach.Quy = _dieuChinh.Quy;
            _kehoach.data.Add("IdDieuChinh", id);
            _kehoach.data.Add("TypeText", Enums.KieuDieuChinhKHXB((Enums.KeHoachXuatBan)typeDieuCHinh));
            _kehoach.data.Add("Type", typeDieuCHinh);
            dynamic dataConfig = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);
            _kehoach.data.Add("DataConfig", dataConfig);
            _kehoach.data.Add("SoBao", _dieuchinhDetail.SoBao);
            _kehoach.data.Add("DataNumber", db.getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(_dieuChinh.ThongTinBaoId) && d.Nam == _dieuChinh.Nam && d.Quy == d.Quy && (_dieuchinhDetail.SoBao.IndexOf("\"" + d.SoBao + "\"") > -1 || (d.Number == -1 && d.status == (int)Enums.KeHoachXuatBan.group))).ToList<BKeHoachXuatBanDetail>());


            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            return View(_kehoach);
        }

        // POST: Bao/DieuChinhKHXB/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IEnumerable<DCThongTinBaoEditModel> lstEdit, string id, string GhiChu)
        {
            List<DCThongTinBaoModel> lstDCThongTinBao = null;
            if (ModelState.IsValid && CheckListDCThongTinBao(lstEdit) && !string.IsNullOrWhiteSpace(GhiChu))
            {
                if (lstEdit != null)
                    lstDCThongTinBao = (from edit in lstEdit.Where(a => a.IsActive == true)
                                       select new DCThongTinBaoModel() { 
                                           Dislay = edit.Dislay,
                                           Key = edit.Key,
                                           KieuDuLieu = edit.KieuDuLieu,
                                           Value = edit.Value
                                       }
                                       ).ToList();

                ErrorObject err = db.EditDieuChinhKHXBDetail(id, lstDCThongTinBao, GhiChu);
                if (err.HasError)
                {
                    base.buildError(err);
                }
                else
                {
                    buildMessage("Sửa điều chỉnh thành công");
                    return this.Json(new { MSG = "Sửa điều chỉnh thành công" });
                }
            }

            if (string.IsNullOrWhiteSpace(GhiChu))
            {
                ModelState.AddModelError("GhiChu", "Ghi chú không được trống");
            }
            //ViewBag.Config = Config;
            //ViewBag.LoaiDieuChinh = LoaiDieuChinh;
            ViewBag.Id = id;
            ViewBag.GhiChu = GhiChu;
            lstEdit = lstEdit ?? new List<DCThongTinBaoEditModel>();
            return PartialView("_EditDCThongTinBao", lstEdit);
        }

        public ActionResult View(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDieuChinhKHXBDetail _dieuchinhDetail = db.getDBSelect().BDieuChinhKHXBDetails.Where(d => d.Id.Equals(id)).FirstOrDefault();
            if (_dieuchinhDetail == null)
            {
                return HttpNotFound();
                //return Redirect(Request.UrlReferrer.ToString());
            }

            int typeDieuCHinh = _dieuchinhDetail.LoaiDieuChinh;
            BDieuChinhKHXB _dieuChinh = db.getDBSelect().BDieuChinhKHXBs.Where(d => d.Id.Equals(_dieuchinhDetail.DieuChinhKHXBId)).FirstOrDefault();
            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(_dieuChinh.ThongTinBaoId);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = _dieuChinh.ChiTietKy;
            _kehoach.LoaiKy = Convert.ToInt32(_dieuChinh.LoaiKy);
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = id;
            _kehoach.Nam = _dieuChinh.Nam;
            _kehoach.Quy = _dieuChinh.Quy;
            _kehoach.data.Add("IdDieuChinh", id);
            _kehoach.data.Add("TypeText", Enums.KieuDieuChinhKHXB((Enums.KeHoachXuatBan)typeDieuCHinh));
            _kehoach.data.Add("Type", typeDieuCHinh);
            dynamic dataConfig = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);
            _kehoach.data.Add("DataConfig", dataConfig);
            _kehoach.data.Add("SoBao", _dieuchinhDetail.SoBao);
            _kehoach.data.Add("DataNumber", db.getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(_dieuChinh.ThongTinBaoId) && d.Nam == _dieuChinh.Nam && d.Quy == d.Quy && (_dieuchinhDetail.SoBao.IndexOf("\"" + d.SoBao + "\"") > -1 || (d.Number == -1 && d.status == (int)Enums.KeHoachXuatBan.group))).ToList<BKeHoachXuatBanDetail>());
            ViewBag.ThongTinBaoId = _dieuChinh.Id;

            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            return View(_kehoach);
        }
        // GET: Bao/DieuChinhKHXB/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BDieuChinhKHXBDetail _dieuchinhDetail = db.getDBSelect().BDieuChinhKHXBDetails.Where(d => d.Id.Equals(id)).FirstOrDefault();
            if (_dieuchinhDetail == null)
            {
                return HttpNotFound();
                //return Redirect(Request.UrlReferrer.ToString());
            }

            int typeDieuCHinh = _dieuchinhDetail.LoaiDieuChinh;
            BDieuChinhKHXB _dieuChinh = db.getDBSelect().BDieuChinhKHXBs.Where(d => d.Id.Equals(_dieuchinhDetail.DieuChinhKHXBId)).FirstOrDefault();
            BaoKyXuatBanModel baoKyXuatBanModel = db.getBaoKyXuatBanModel(_dieuChinh.ThongTinBaoId);
            KeHoachXuatBanModel _kehoach = new KeHoachXuatBanModel();
            _kehoach.Id = baoKyXuatBanModel.Id;
            _kehoach.KyConfig = _dieuChinh.ChiTietKy;
            _kehoach.LoaiKy = Convert.ToInt32(_dieuChinh.LoaiKy);
            _kehoach.MaBao = baoKyXuatBanModel.MaBao;
            _kehoach.TenBao = baoKyXuatBanModel.TenBao;
            _kehoach.IdKeHoachXuatBan = id;
            _kehoach.Nam = _dieuChinh.Nam;
            _kehoach.Quy = _dieuChinh.Quy;
            _kehoach.data.Add("IdDieuChinh", id);
            _kehoach.data.Add("TypeText", Enums.KieuDieuChinhKHXB((Enums.KeHoachXuatBan)typeDieuCHinh));
            _kehoach.data.Add("Type", typeDieuCHinh);
            dynamic dataConfig = JsonConvert.DeserializeObject<ExpandoObject>(_dieuchinhDetail.Config);
            _kehoach.data.Add("DataConfig", dataConfig);
            _kehoach.data.Add("SoBao", _dieuchinhDetail.SoBao);
            _kehoach.data.Add("DataNumber", db.getDBSelect().BKeHoachXuatBanDetails.Where(d => d.ThongTinBaoId.Equals(_dieuChinh.ThongTinBaoId) && d.Nam == _dieuChinh.Nam && d.Quy == d.Quy && (_dieuchinhDetail.SoBao.IndexOf("\"" + d.SoBao + "\"") > -1 || (d.Number == -1 && d.status == (int)Enums.KeHoachXuatBan.group))).ToList<BKeHoachXuatBanDetail>());
            ViewBag.ThongTinBaoId = _dieuChinh.Id;

            if (_kehoach == null)
            {
                return HttpNotFound();
            }
            return View(_kehoach);
        }

        // POST: Bao/DieuChinhKHXB/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            string dcId = db.getDieuChinhKHXBDetail(id).DieuChinhKHXBId;
            db.deleteDieuChinhKHXBDetail(id);
            base.buildMessage("Bạn đã xóa điều chỉnh thành công");
            return RedirectToAction("Index", new { id = dcId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id dieuchinhKHXBDetail</param>
        /// <param name="config"></param>
        /// <returns></returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult EditDCThongTinBao(string id)
        {
            BDieuChinhKHXBDetail dcKHXBDetail = db.getDieuChinhKHXBDetail(id);
            if (dcKHXBDetail == null)
            {
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new { MSG = "Chi tiết Điều chỉnh kế hoạch xuất bản không tồn tại"});
                }
                return HttpNotFound();
            }
            List<DCThongTinBaoEditModel> lstEdit = null;
            if (dcKHXBDetail.LoaiDieuChinh != (int)Enums.KeHoachXuatBan.cancel)
            {
                lstEdit = db.getListDCThongTinBao(dcKHXBDetail.BDieuChinhKHXB.ThongTinBaoId);
                string content = dcKHXBDetail.NoiDung;
                //Doc lai du lieu cu
                if (!string.IsNullOrWhiteSpace(content))
                {
                    List<DCThongTinBaoModel> lstOld = JsonConvert.DeserializeObject<List<DCThongTinBaoModel>>(content);
                    lstEdit.Where(a => lstOld.Any(b => b.Key.Equals(a.Key))).ToList().ForEach(a =>
                    {
                        a.Value = lstOld.FirstOrDefault(b => b.Key.Equals(a.Key)).Value;
                        a.IsActive = true;
                    });
                }
            }
            ViewBag.Id = id;
            ViewBag.GhiChu = dcKHXBDetail.GhiChu;
            return PartialView("_EditDCThongTinBao", lstEdit);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult ViewDCThongTinBao(string id)
        {
            BDieuChinhKHXBDetail dcKHXBDetail = db.getDieuChinhKHXBDetail(id);
            if (dcKHXBDetail == null)
            {
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new { MSG = "Chi tiết Điều chỉnh kế hoạch xuất bản không tồn tại" });
                }
                return HttpNotFound();
            }
            List<DCThongTinBaoModel> lstView = null;
            if (dcKHXBDetail.LoaiDieuChinh != (int)Enums.KeHoachXuatBan.cancel)
            {
                string content = dcKHXBDetail.NoiDung;
                //Doc lai du lieu cu
                if (!string.IsNullOrWhiteSpace(content))
                {
                    lstView = JsonConvert.DeserializeObject<List<DCThongTinBaoModel>>(content);
                }
            }
            ViewBag.GhiChu = dcKHXBDetail.GhiChu;
            return PartialView("_ViewDCThongTinBao", lstView);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id DieuChinhKHXB</param>
        /// <param name="config"></param>
        /// <returns></returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult CreateDCThongTinBao(string id, int loaiDieuChinh)
        {
            BDieuChinhKHXB dcKHCB = db.getDieuChinhKHXB(id);
            if (dcKHCB == null)
            {
                if (HttpContext.Request.IsAjaxRequest())
                {
                    return Json(new { MSG = "Điều chỉnh kế hoạch xuất bản không tồn tại" });
                }
                return Content("Điều chỉnh kế hoạch xuất bản không tồn tại");
            }
            List<DCThongTinBaoEditModel> lstEdit = null;
            if (loaiDieuChinh != (int)Enums.KeHoachXuatBan.cancel)
            {
                lstEdit = db.getListDCThongTinBao(dcKHCB.ThongTinBaoId);
            }

            ViewBag.Config = "";
            ViewBag.LoaiDieuChinh = loaiDieuChinh;
            ViewBag.Id = id;
            ViewBag.GhiChu = "";
            return PartialView("_CreateDCThongTinBao", lstEdit);
        }
        /// <summary>
        /// Kiem tra du lieu
        /// </summary>
        /// <param name="lstEdit"></param>
        /// <returns></returns>
        private bool CheckListDCThongTinBao(IEnumerable<DCThongTinBaoEditModel> lstEdit)
        {
            if (lstEdit == null) return true;
            //"[0].IntergerValue"
            bool hasError = true;
            for (int i = 0; i < lstEdit.Count(); i++)
            {
                var item = lstEdit.Skip(i).Take(1).First();
                if(item.IsActive)
                {
                    if(string.IsNullOrWhiteSpace(item.Value))
                    {
                        hasError = false;
                        ModelState.AddModelError("[" + i + "]." + item.getNameKieuDuLieu(), item.Dislay + " không được để trống");
                    }
                }
            }
            return hasError;
        }
        public ActionResult EditMulti()
        {
            List<DCThongTinBaoModel> lstEdit = new List<DCThongTinBaoModel>();
            for (int i = 1; i < 10; i++ )
            {
                lstEdit.Add(new DCThongTinBaoModel() { Key = "Key" + i, Dislay = "Key " + i , KieuDuLieu = i%5});
            }
            return View(lstEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMulti(IEnumerable<DCThongTinBaoEditModel> lstEdit, string Config)
        {
            if(ModelState.IsValid)
            {
                return View(lstEdit);
            }
            return View(lstEdit);
        }
        #region DieuChinhPHNC

        //private dbDieuChinhPHNC_PHBCEntities dbDieuChinhPHNC = new dbDieuChinhPHNC_PHBCEntities();
        //string subAction = "DieuChinhPHNC";
        //public DieuChinhPHNCController(IBDieuChinhPHNCBussiness _IBDieuChinhPHNCBussiness)
        //{
        //    dbDieuChinhPHNC = _IBDieuChinhPHNCBussiness;
        //    ViewBag.TitleName = " Phân hướng nhu cầu cho điều chỉnh KHXB ";
        //    ViewBag.SubAction = subAction;
        //}
        // GET: Bao/DieuChinhPHNC

        public ActionResult IndexDieuChinhPHNC(string DieuChinhKHXBDetailId)
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
        public ActionResult CreateDieuChinhPHNC(string DieuChinhKHXBDetailId, string provincecode)
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
            objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
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
                UnitModelDieuChinh objUnitNew = BuildSessionDieuChinhPHNC(DieuChinhKHXBDetailId);
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
                else if(!string.IsNullOrWhiteSpace(typeid))
                {
                    lstQuanHuyen = objUnitNew.getDistrictNotMap(lstids, typeid);
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
                    objUnitNew.setListDistrictMapNew(lstQuanHuyen, bDieuChinhPhanHuongDistrict.DiemTiepNhanId);
                    Session[Application.Session.UnitModelDieuChinh] = objUnitNew;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                dbDieuChinhPHNC.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
