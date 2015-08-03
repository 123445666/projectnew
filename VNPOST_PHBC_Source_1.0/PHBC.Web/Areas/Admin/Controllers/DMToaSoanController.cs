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

namespace PHBC.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// author : vietvb
    /// </summary>
    public class DMToaSoanController : BaseController
    {
        //private DB_PHBCEntities db = new DB_PHBCEntities();
        IDMToaSoanBussiness db;
        int pagesize = 10;
        public DMToaSoanController(IDMToaSoanBussiness _iDMToaSoanBussiness)
        {
            db = _iDMToaSoanBussiness;
            ViewBag.TitleName = " Toà Soạn ";
            //ViewBag.Permisson = base.permisson;
        }

        // GET: /Admin/DMToaSoan/
        public ActionResult Index(string pageIndex = "")
        {
            Session[Application.Session.ModelSearch] = null;
            int pagenum = 0;
            if (!String.IsNullOrEmpty(pageIndex))
            {
                pagenum = int.Parse(pageIndex.Replace('/', '\0'));
            }
            else pagenum = 1;
            int pageCount = 0;
            int totalitem = 0;
            var value = db.getAllModel(pagenum, pagesize, out pageCount, out totalitem);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            return View(new PagedList<DMToaSoanModel>(value, pagenum, pagesize, totalitem));
            //return View(value);
        }

        // GET: /Admin/DMToaSoan/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMToaSoan dmtoasoan = db.getById(id);
            if (dmtoasoan == null)
            {
                return HttpNotFound();
            }
            return View(new DMToaSoanModel(dmtoasoan));
        }

        // GET: /Admin/DMToaSoan/Create
        public ActionResult Create()
        {
            ViewBag.KieuToaSoan = new SelectList(listKieuToaSoan(), "Value", "Text");
            return View();
        }

        // POST: /Admin/DMToaSoan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaToaSoan,TenToaSoan,DiaChi,SoDienThoai,Email,Web,MaSoThue,TaiKhoan,TongBienTap,NguoiDaiDien,CoQuanChuQuan,KieuToaSoan,NganHang")] DMToaSoanModel dmtoasoanmodel)
        {
            if (ModelState.IsValid)
            {
                //check mã tòa soạn và tên tòa soạn
                ErrorObject err = new ErrorObject();
                err = db.checkDMToaSoan(dmtoasoanmodel.Id, dmtoasoanmodel.MaToaSoan, dmtoasoanmodel.TenToaSoan);
                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    ViewBag.KieuToaSoan = new SelectList(listKieuToaSoan(), "Value", "Text", dmtoasoanmodel.KieuToaSoan);
                    return View(dmtoasoanmodel);
                }
                
                dmtoasoanmodel.CreateBy = userInfo.Id;
                dmtoasoanmodel.CreateDate = DateTime.Now;
                db.Add(dmtoasoanmodel.toDMToaSoan());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.KieuToaSoan = new SelectList(listKieuToaSoan(), "Value", "Text", dmtoasoanmodel.KieuToaSoan);
            }

            return View(dmtoasoanmodel);
        }

        // GET: /Admin/DMToaSoan/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMToaSoan dmtoasoan = db.getById(id);
            SelectList sl = new SelectList(listKieuToaSoan(), "Value", "Text", Convert.ToString(dmtoasoan.KieuToaSoan));
            ViewBag.KieuToaSoan = sl;
            if (dmtoasoan == null)
            {
                return HttpNotFound();
            }
            return View(new DMToaSoanModel(dmtoasoan));
        }

        // POST: /Admin/DMToaSoan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaToaSoan,TenToaSoan,DiaChi,SoDienThoai,Email,Web,MaSoThue,TaiKhoan,TongBienTap,NguoiDaiDien,CoQuanChuQuan,KieuToaSoan,NganHang")] DMToaSoanModel dmtoasoanmodel)
        {
            ViewBag.KieuToaSoan = new SelectList(listKieuToaSoan(), "Value", "Text", Convert.ToString(dmtoasoanmodel.KieuToaSoan));
            if (ModelState.IsValid)
            {
                ErrorObject err = new ErrorObject();
                err = db.checkDMToaSoan(dmtoasoanmodel.Id, dmtoasoanmodel.MaToaSoan, dmtoasoanmodel.TenToaSoan);

                if (err.HasError)
                {
                    foreach (var item in err.LstError)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                    }
                    ViewBag.KieuToaSoan = new SelectList(listKieuToaSoan(), "Value", "Text", Convert.ToString(dmtoasoanmodel.KieuToaSoan));
                    return View(dmtoasoanmodel);
                }
                dmtoasoanmodel.ModifyBy = userInfo.Id;
                dmtoasoanmodel.ModifyDate = DateTime.Now;                
                db.Update(dmtoasoanmodel.toDMToaSoan());

                return RedirectToAction("Index");
            }
            return View(dmtoasoanmodel);
        }

        // GET: /Admin/DMToaSoan/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMToaSoan dmtoasoan = db.getById(id);
            if (dmtoasoan == null)
            {
                return HttpNotFound();
            }
            return View(new DMToaSoanModel(dmtoasoan));
        }

        // POST: /Admin/DMToaSoan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DMToaSoan DMToaSoan = db.getById(id);
            DMToaSoan.ModifyBy = userInfo.Id;
            DMToaSoan.ModifyDate = DateTime.Now;
            db.Delete(DMToaSoan);
            return RedirectToAction("Index");
        }

        public ActionResult Search(string page)
        {
            int pagenum = 1;
            DMToaSoanSearchModel search = Session[Constants.Application.Session.ModelSearch] as DMToaSoanSearchModel;
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
        public ActionResult Search([Bind(Include = "MaToaSoan,TenToaSoan,SoDienThoai")] DMToaSoanSearchModel dmToaSoan, int? page)
        {
            int pagenum = 0;
            int pageCount = 0;
            if (String.IsNullOrEmpty(dmToaSoan.MaToaSoan) && String.IsNullOrEmpty(dmToaSoan.TenToaSoan) && String.IsNullOrEmpty(dmToaSoan.SoDienThoai))
            {
                return RedirectToAction("Index");
            }
            if (!String.IsNullOrEmpty(Convert.ToString(page)))
            {
                pagenum = page.Value;
            }
            else pagenum = 1;
            int totalitem = 0;
            List<DMToaSoanModel> value = db.searchModel(dmToaSoan, pagenum, pagesize, out pageCount, out totalitem);
            ViewBag.Page = pagenum;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchModel = dmToaSoan;
            Session[Constants.Application.Session.ModelSearch] = dmToaSoan;
            return View("Index", new PagedList<DMToaSoanModel>(value, pagenum, pagesize, totalitem));
        }

        /*
         * Function Print
         * param id toa soan
         * author vietvb
         * function dùng để chọn những tòa soạn cho việc print
         * */
        public ActionResult Checked(string id, string page, string actionts)
        {
            //string old_id = String.IsNullOrEmpty(Convert.ToString(Session["Print"])) ? Convert.ToString(Session["Print"]) : "";
            List<DMToaSoanModel> lsDM = new List<DMToaSoanModel>();
            if (Session["CheckedToaSoan"] != null)
            {
                lsDM = (List<DMToaSoanModel>)Session["CheckedToaSoan"];
            }
            //lstid.Add(id);
            //Session["Print"] = lstid;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DMToaSoanModel dmtoasoan = new DMToaSoanModel();
            if (id == "0")
            {
                if (Session[Application.Session.ModelSearch] != null)
                {
                    lsDM = db.searchModel((DMToaSoanSearchModel)Session[Application.Session.ModelSearch]);
                }
                else
                {
                    lsDM = db.getAllModel();
                }
            }
            else
            {
                dmtoasoan = new DMToaSoanModel(db.getById(id));
                if (dmtoasoan == null)
                {
                    return HttpNotFound();
                }
                if ((lsDM.Where(item => item.Id == dmtoasoan.Id).Count() == 0))
                {
                    lsDM.Add(dmtoasoan);
                }
                else
                {
                    lsDM.Remove(lsDM.FirstOrDefault(item => item.Id == dmtoasoan.Id));
                }
            }

            Session["CheckedToaSoan"] = lsDM;

            return RedirectToAction(actionts, new { page = page });
        }

        /*
         * Function PrintAll
         * param id toa soan
         * author vietvb
         * function dùng để chọn những tòa soạn cho việc print
         * */
        public ActionResult PrintAll()
        {
            //string old_id = String.IsNullOrEmpty(Convert.ToString(Session["Print"])) ? Convert.ToString(Session["Print"]) : "";
            List<DMToaSoanModel> lsDM = new List<DMToaSoanModel>();
            if (Session["CheckedToaSoan"] != null)
            {
                lsDM = (List<DMToaSoanModel>)Session["CheckedToaSoan"];
            }
            else
            {
                return RedirectToAction("Index");
            }

            return View(lsDM);
        }

        /*
         * Function BackFromPrint
         * param id (để check trạng thái đã in (1), chưa in (0)
         * author vietvb
         * function dùng để chọn những tòa soạn cho việc print
         * */
        public ActionResult BackFromPrint(int? id)
        {
            //string old_id = String.IsNullOrEmpty(Convert.ToString(Session["Print"])) ? Convert.ToString(Session["Print"]) : "";
            if (id != null)
            {
                if (id == 1)
                {
                    Session["CheckedToaSoan"] = null;
                }
            }
            return RedirectToAction("Index");
        }

        /*
         * Function ExportExcel
         * param id (để check trạng thái đã in (1), chưa in (0)
         * author vietvb
         * function dùng để chọn những tòa soạn cho việc print
         * */
        [HttpPost]
        public ActionResult ExportExcel(string values)
        {
            //string old_id = String.IsNullOrEmpty(Convert.ToString(Session["Print"])) ? Convert.ToString(Session["Print"]) : "";
            List<DMToaSoanModel> lstTS = new List<DMToaSoanModel>();
            string[] lstids = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (values == null)
            {
                return RedirectToAction("Index");
            }
            if (String.IsNullOrEmpty(values))
            {
                if (Session[Application.Session.ModelSearch] != null)
                {
                    lstTS = db.searchModel((DMToaSoanSearchModel)Session[Application.Session.ModelSearch]);
                }
                else
                {
                    lstTS = db.getAllModel();
                }
            }
            else
            {
                foreach (var item in lstids)
                {
                    lstTS.Add(new DMToaSoanModel(db.getById(item)));
                }
            }

            FileStream fs = new FileStream(Server.MapPath(@"\template\exceltemplate\DMToaSoanTemplate.xls"), FileMode.Open, FileAccess.Read);
            HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);
            HSSFSheet sheet1 = (HSSFSheet)templateWorkbook.GetSheet("DMToaSoan");

            for (int i = 2; i < (lstTS.Count + 2); i++)
            {
                sheet1.CreateRow(i);
                sheet1.GetRow(i).CreateCell(0).SetCellValue(lstTS[i - 2].Id);
                sheet1.GetRow(i).CreateCell(1).SetCellValue(lstTS[i - 2].MaToaSoan);
                sheet1.GetRow(i).CreateCell(2).SetCellValue(lstTS[i - 2].TenToaSoan);
                sheet1.GetRow(i).CreateCell(3).SetCellValue(lstTS[i - 2].DiaChi);
                sheet1.GetRow(i).CreateCell(4).SetCellValue(lstTS[i - 2].SoDienThoai);
                sheet1.GetRow(i).CreateCell(5).SetCellValue(lstTS[i - 2].Email);
                sheet1.GetRow(i).CreateCell(6).SetCellValue(lstTS[i - 2].Web);
                sheet1.GetRow(i).CreateCell(7).SetCellValue(lstTS[i - 2].MaSoThue);
                sheet1.GetRow(i).CreateCell(8).SetCellValue(lstTS[i - 2].TaiKhoan);
                sheet1.GetRow(i).CreateCell(9).SetCellValue(lstTS[i - 2].TongBienTap);
                sheet1.GetRow(i).CreateCell(10).SetCellValue(lstTS[i - 2].NguoiDaiDien);
            }
            //Force Excel to recalculate all formulas in the table
            sheet1.ForceFormulaRecalculation = true;
            //#endregion
            //# region Set response header ( file name and file format )
            //Set the response type to Excel
            Response.ContentType = "application/vnd.ms-excel";
            //Set to download Excel file name
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Danh sách tòa soạn.xls"));
            //Clear method removes all cached HTML output. However, this method only deletes Response shows the input information , do not delete the Response header information. So as not to affect the integrity of the exported data.
            Response.Clear();
            //#endregion
            //# region write to the client
            using (MemoryStream ms = new MemoryStream())
            {
                //The contents of the workbook into memory stream
                templateWorkbook.Write(ms);
                //The memory stream into an array of bytes sent to the client
                Response.BinaryWrite(ms.GetBuffer());
                Response.End();
                //GridView gv = new GridView();
                //gv.AutoGenerateColumns = false;
                //gv.Caption = "Danh sách tòa soạn ";
                //gv = ChangeTemplateGrid(gv);
                //if (values != null)
                //{
                //    gv.DataSource = lstTS;
                //    //gv.DataSource = id;
                //}
                //else
                //{
                //    return RedirectToAction("Index");
                //}

                //gv.DataBind();

                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment; filename=DanhSachToaSoan.xls");
                //Response.ContentType = "application/ms-excel";
                //Response.Charset = "";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);
                //gv.RenderControl(htw);
                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();
                return RedirectToAction("Index");
            }
        }

        public GridView ChangeTemplateGrid(GridView gv)
        {
            BoundField column;

            column = new BoundField();
            column.DataField = "id";
            column.HeaderText = "ID Toà Soạn";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "MaToaSoan";
            column.HeaderText = "Mã Tòa Soạn";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "TenToaSoan";
            column.HeaderText = "Tên Tòa Soạn";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "DiaChi";
            column.HeaderText = "Địa Chỉ";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "SoDienThoai";
            column.HeaderText = "Số điện thoại";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "Email";
            column.HeaderText = "Email";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "Web";
            column.HeaderText = "Web";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "MaSoThue";
            column.HeaderText = "Mã số thuế";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "TaiKhoan";
            column.HeaderText = "Tài khoản";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "TongBienTap";
            column.HeaderText = "Tổng Biên Tập";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "NguoiDaiDien";
            column.HeaderText = "Người Đại Diện";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "CoQuanChuQuan";
            column.HeaderText = "Cơ quan chủ quản";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "KieuToaSoan";
            column.HeaderText = "Kiểu Tòa Soạn";
            gv.Columns.Add(column);

            column = new BoundField();
            column.DataField = "NganHang";
            column.HeaderText = "Ngân Hàng";
            gv.Columns.Add(column);


            return gv;
        }

        /*
        * Function Print
        * param values (list string được đưa vào ngăn bởi dấu ,)
        * author vietvb
        * function dùng để chọn những tòa soạn cho việc print
        * */
        [HttpPost]
        public ActionResult Print(string values)
        {
            //string old_id = String.IsNullOrEmpty(Convert.ToString(Session["Print"])) ? Convert.ToString(Session["Print"]) : "";
            List<DMToaSoanModel> lstTS = new List<DMToaSoanModel>();
            string[] lstids = values.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (values == null)
            {
                return RedirectToAction("Index");
            }
            if (String.IsNullOrEmpty(values))
            {
                if (Session[Application.Session.ModelSearch] != null)
                {
                    lstTS = db.searchModel((DMToaSoanSearchModel)Session[Application.Session.ModelSearch]);
                }
                else
                {
                    lstTS = db.getAllModel();
                }
            }
            else
            {
                foreach (var item in lstids)
                {
                    lstTS.Add(new DMToaSoanModel(db.getById(item)));
                }
            }

            //FileStream fs = new FileStream(Server.MapPath(@"\template\exceltemplate\DMToaSoanTemplate.xls"), FileMode.Open, FileAccess.Read);
            HSSFWorkbook templateWorkbook = new HSSFWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)templateWorkbook.CreateSheet("DanhSachToaSoan");
            HSSFCellStyle cellStyle = (HSSFCellStyle)templateWorkbook.CreateCellStyle();
            cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium;
            cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
            cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;
            cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
            cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            cellStyle.ShrinkToFit = true;
            //HSSFFont font = (HSSFFont)templateWorkbook.CreateFont();

            HSSFFont font = ExcelNpoi.createFont(templateWorkbook, (short)FontBoldWeight.Bold, 64, 12, "Times New Roman", false, FontUnderlineType.None, FontSuperScript.None);
            cellStyle.SetFont(font);
            cellStyle.WrapText = true;
            sheet1.SetColumnWidth(1, 10000);
            sheet1.SetColumnWidth(2, 10000);
            int k = 0;
            int count = 0;
            if (lstTS.Count%2==0)
            {
                count = (lstTS.Count) / 2;
            }
            else
            {
                count = (lstTS.Count) / 2 + 1;
            }
            for (int i = 0; i < count; i++)
            {
                    k = i * 2;
                    sheet1.CreateRow(k);
                    sheet1.GetRow(k).Height = 5000;
                    sheet1.GetRow(k).CreateCell(1).SetCellValue(lstTS[k].MaToaSoan + Environment.NewLine + lstTS[k].TenToaSoan);
                    sheet1.GetRow(k).GetCell(1).CellStyle = cellStyle;
                    if (k + 1 < lstTS.Count)
                    { 
                    sheet1.GetRow(k).Height = 5000;
                    sheet1.GetRow(k).CreateCell(2).SetCellValue(lstTS[k + 1].MaToaSoan + Environment.NewLine + lstTS[k + 1].TenToaSoan);
                    sheet1.GetRow(k).GetCell(2).CellStyle = cellStyle;
                    }
                
                
                //sheet1.GetRow(i).CreateCell(2).SetCellValue(lstTS[i - 2].TenToaSoan);
            }
            //Force Excel to recalculate all formulas in the table
            sheet1.ForceFormulaRecalculation = true;
            //#endregion
            //# region Set response header ( file name and file format )
            //Set the response type to Excel
            Response.ContentType = "application/vnd.ms-excel";
            //Set to download Excel file name
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "Nhãn danh sách tòa soạn.xls"));
            //Clear method removes all cached HTML output. However, this method only deletes Response shows the input information , do not delete the Response header information. So as not to affect the integrity of the exported data.
            Response.Clear();
            //#endregion
            //# region write to the client
            using (MemoryStream ms = new MemoryStream())
            {
                //The contents of the workbook into memory stream
                templateWorkbook.Write(ms);
                //The memory stream into an array of bytes sent to the client
                Response.BinaryWrite(ms.GetBuffer());
                Response.End();
                //GridView gv = new GridView();
                //gv.AutoGenerateColumns = false;
                //gv.Caption = "Danh sách tòa soạn ";
                //gv = ChangeTemplateGrid(gv);
                //if (values != null)
                //{
                //    gv.DataSource = lstTS;
                //    //gv.DataSource = id;
                //}
                //else
                //{
                //    return RedirectToAction("Index");
                //}

                //gv.DataBind();

                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment; filename=DanhSachToaSoan.xls");
                //Response.ContentType = "application/ms-excel";
                //Response.Charset = "";
                //StringWriter sw = new StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(sw);
                //gv.RenderControl(htw);
                //Response.Output.Write(sw.ToString());
                //Response.Flush();
                //Response.End();
                return RedirectToAction("Index");
            }
        }

        public List<DefineSelectItem> listKieuToaSoan()
        {
            List<DefineSelectItem> result = new List<DefineSelectItem>();
            int val1 = (int)Enums.KieuToaSoan.TrungUong;
            int val2 = (int)Enums.KieuToaSoan.DiaPhuong;
            result.Add(new DefineSelectItem() { Value = val1.ToString(), Text = "Tòa soạn Trung Ương" });
            result.Add(new DefineSelectItem() { Value = val2.ToString(), Text = "Tòa soạn Địa Phương" });
            return result;
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
