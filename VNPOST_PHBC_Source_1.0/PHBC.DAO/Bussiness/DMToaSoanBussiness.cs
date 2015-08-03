using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public class DMToaSoanBussiness : IDMToaSoanBussiness
    {
        DB_PHBCEntities db;
        public DMToaSoanBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public DMToaSoanBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }
        /***
         * function: getAll()
         * param : null
         * result: List<DMToaSoan>
         * author: vietvb
         ***/
        public List<DMToaSoan> getAll()
        {
            return db.DMToaSoans.ToList();
        }

        /***
         * function: getAll()
         * param : int page, int pagesize, out int pageCount
         * result: List<DMToaSoan>
         * author: vietvb
         * Dùng để phân trang
         * */
        public List<DMToaSoan> getAll(int page, int pageSize, out int pageCount)
        {
            throw new NotImplementedException();
        }

        /***
         * function: getAllModel()
         * param : null
         * result: List<DMToaSoanModel>
         * author: vietvb
         ***/
        public List<DMToaSoanModel> getAllModel()
        {
            List<DMToaSoanModel> result = new List<DMToaSoanModel>();
            result = (from t in db.DMToaSoans
                      where t.Status == (int)Enums.RecordStatusCode.active
                   select new DMToaSoanModel()
                   {
                       Id = t.Id,
                       MaToaSoan = t.MaToaSoan,
                       TenToaSoan = t.TenToaSoan,
                       DiaChi = t.DiaChi,
                       SoDienThoai = t.SoDienThoai,
                       Email = t.Email,
                       Web = t.Web,
                       MaSoThue = t.MaSoThue,
                       TaiKhoan = t.TaiKhoan,
                       TongBienTap = t.TongBienTap,
                       NguoiDaiDien = t.NguoiDaiDien,
                       CoQuanChuQuan = t.CoQuanChuQuan,
                       Status = t.Status,
                       KieuToaSoan = t.KieuToaSoan,
                       NganHang = t.NganHang
                   }).ToList();
            return result;
        }

        /***
         * function: getAllModel()
         * param : int page, int pagesize, out int pageCount
         * result: List<DMDiemIn>
         * author: vietvb
         * Dùng để phân trang
         * */
        public List<DMToaSoanModel> getAllModel(int page, int pageSize, out int pageCount,out int totalitem)
        {
            List<DMToaSoanModel> result = new List<DMToaSoanModel>();
            int count = db.DMToaSoans.Where(t => t.Status == (int)Enums.RecordStatusCode.active).Count();
            totalitem = count;
            //neu khong co du lieu thi return
            if (count == 0)
            {
                pageCount = 0;
                return result;
            }
            //pageSize nho hon 10 thi dat bang 10
            if (pageSize < 10)
                pageSize = 10;
            //tinh pagecount
            pageCount = count / pageSize;
            //Neu so luong con du thi tang pagecount len 1
            if (count % pageSize > 0) pageCount++;
            //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
            if (page >= pageCount)
                page = pageCount - 1;
            else
                page = page - 1;//So trang bat dau tu 0
            result = (from t in db.DMToaSoans
                      where t.Status == 1
                      select new DMToaSoanModel()
                      {
                          Id = t.Id,
                          MaToaSoan = t.MaToaSoan,
                          TenToaSoan = t.TenToaSoan,
                          DiaChi = t.DiaChi,
                          SoDienThoai = t.SoDienThoai,
                          Email = t.Email,
                          Web = t.Web,
                          MaSoThue = t.MaSoThue,
                          TaiKhoan = t.TaiKhoan,
                          TongBienTap = t.TongBienTap,
                          NguoiDaiDien = t.NguoiDaiDien,
                          CoQuanChuQuan = t.CoQuanChuQuan,
                          Status = t.Status,
                          KieuToaSoan = t.KieuToaSoan,
                          NganHang = t.NganHang
                      }).OrderBy(i => i.MaToaSoan).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        /***
        * function: Search()
        * param : string MaToaSoan, string TenToaSoan = "", string SoDienThoai =""
        * result: List<DMToaSoan>
        * author: vietvb
        * Tìm kiếm theo mã tòa soạn, tên tòa soạn và số điện thoại
        * */
        public List<DMToaSoan> search(string MaToaSoan, string TenToaSoan = "", string SoDienThoai = "")
        {
            throw new NotImplementedException();
        }
        /***
        * function: Search()
        * param : string MaToaSoan, string TenToaSoan = "", string SoDienThoai =""
        * result: List<DMToaSoan>
        * author: vietvb
        * Tìm kiếm theo mã tòa soạn, tên tòa soạn và số điện thoại
        * */
        public List<DMToaSoan> search(string MaToaSoan, int page, int pageSize, out int pageCount)
        {
            throw new NotImplementedException();
        }

        /***
        * function: SearchModel()
        * param : string MaDiemIn, int page, int pageSize, out int pageCount
        * result: List<DMDiemIn>
        * author: vietvb
        * Tìm kiếm theo mã điểm in, tên điểm in và phân trang
        * */
        public List<DMToaSoanModel> searchModel(DMToaSoanSearchModel obj, int page, int pageSize, out int pageCount, out int totalitem)
        {
            List<DMToaSoanModel> result = new List<DMToaSoanModel>();
            IEnumerable<DMToaSoanModel> query = null;
            query = (from t in db.DMToaSoans
                     where t.Status == (int)Enums.RecordStatusCode.active
                     select new DMToaSoanModel()
                     {
                         Id = t.Id,
                        MaToaSoan = t.MaToaSoan,
                        TenToaSoan = t.TenToaSoan,
                        DiaChi = t.DiaChi,
                        SoDienThoai = t.SoDienThoai,
                        Email = t.Email,
                        Web = t.Web,
                        MaSoThue = t.MaSoThue,
                        TaiKhoan = t.TaiKhoan,
                        TongBienTap = t.TongBienTap,
                        NguoiDaiDien = t.NguoiDaiDien,
                        CoQuanChuQuan = t.CoQuanChuQuan,
                        Status = t.Status,
                        KieuToaSoan = t.KieuToaSoan,
                        NganHang = t.NganHang
                     });
            if (!String.IsNullOrEmpty(obj.MaToaSoan))
            {
                query = query.Where(r => r.MaToaSoan.ToLower().Contains(obj.MaToaSoan.ToLower()));
            }
            if (!String.IsNullOrEmpty(obj.TenToaSoan))
            {
                query = query.Where(r => r.TenToaSoan.ToLower().Contains(obj.TenToaSoan.ToLower()));
            }
            if (!String.IsNullOrEmpty(obj.SoDienThoai))
            {
                query = query.Where(r => r.SoDienThoai.ToLower().Contains(obj.SoDienThoai.ToLower()));
            }
            if (query != null)
            {
                int count = query.Count();
                totalitem = count;
                //neu khong co du lieu thi return
                if (count == 0)
                {
                    pageCount = 0;
                    return result;
                }
                //pageSize nho hon 10 thi dat bang 10
                if (pageSize < 10)
                    pageSize = 2;
                //tinh pagecount
                pageCount = count / pageSize;
                //Neu so luong con du thi tang pagecount len 1
                if (count % pageSize > 0) pageCount++;
                //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
                if (page >= pageCount)
                    page = pageCount - 1;
                else
                    page = page - 1;//So trang bat dau tu 0
                result = query.OrderBy(i => i.MaToaSoan).Skip(page * pageSize).Take(pageSize).ToList();
                return result;
            }
            else
            {
                totalitem = 0;
                pageCount = 0;
                return result;
            }
        }

        /***
        * function: Search()
        * param : DMToaSoanSearchModel
        * result: List<DMDiemIn>
        * author: vietvb
        * Tìm kiếm theo mã điểm in, tên điểm in và phân trang
        * */
        public List<DMToaSoanModel> searchModel(DMToaSoanSearchModel obj)
        {
            List<DMToaSoanModel> result = new List<DMToaSoanModel>();
            IEnumerable<DMToaSoanModel> query = null;
            query = (from t in db.DMToaSoans
                     where t.Status == 1
                     select new DMToaSoanModel()
                     {
                         Id = t.Id,
                         MaToaSoan = t.MaToaSoan,
                         TenToaSoan = t.TenToaSoan,
                         DiaChi = t.DiaChi,
                         SoDienThoai = t.SoDienThoai,
                         Email = t.Email,
                         Web = t.Web,
                         MaSoThue = t.MaSoThue,
                         TaiKhoan = t.TaiKhoan,
                         TongBienTap = t.TongBienTap,
                         NguoiDaiDien = t.NguoiDaiDien,
                         CoQuanChuQuan = t.CoQuanChuQuan,
                         Status = t.Status,
                         KieuToaSoan = t.KieuToaSoan,
                         NganHang = t.NganHang
                     });
            if (!String.IsNullOrEmpty(obj.MaToaSoan))
            {
                query = query.Where(r => r.MaToaSoan.Contains(obj.MaToaSoan));
            }
            if (!String.IsNullOrEmpty(obj.TenToaSoan))
            {
                query = query.Where(r => r.TenToaSoan.Contains(obj.TenToaSoan));
            }
            if (!String.IsNullOrEmpty(obj.SoDienThoai))
            {
                query = query.Where(r => r.SoDienThoai.Contains(obj.SoDienThoai));
            }
            if (query != null)
            {
                int count = query.Count();
                //neu khong co du lieu thi return
                
                result = query.ToList();
                return result;
            }
            else
            {                
                return result;
            }
        }
        /***
        * function: getById()
        * param : string id
        * result: DMToaSoan
        * author: vietvb
        * Lấy dữ liệu theo ID
        * */
        public DMToaSoan getById(string id)
        {
            return db.DMToaSoans.Where(x => x.Status == (int)Enums.RecordStatusCode.active).SingleOrDefault(x => x.Id == id);
        }
        /***
        * function: checkDMToaSoan()
        * param : string id, string MaToaSoan, string TenToaSoan
        * result: bool and out int
        * declare: 1: trùng mã tòa soạn, 3 trùng tên tòa soan, 5 trùng id tòa soạn, lỗi ra sẽ là tổng các trường hợp
        * author: vietvb
        * Check có dữ liệu theo id, tên tòa soạn và mã tòa soạn
        * */
        public ErrorObject checkDMToaSoan(string id, string MaToaSoan, string TenToaSoan)
        {
            ErrorObject err = new ErrorObject();
            if (String.IsNullOrEmpty(id))
            {
                if (db.DMToaSoans.Any(r => r.MaToaSoan.CompareTo(MaToaSoan.Trim()) == 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("MaToaSoan", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã tòa soạn"));
                }
                if (db.DMToaSoans.Any(r => r.TenToaSoan.CompareTo(TenToaSoan.Trim()) == 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("TenToaSoan", String.Format(Enums.ErrorMessage.SameKey.ToString(),"Tên tòa soạn"));
                }
            }
            else
            {
                if (db.DMToaSoans.Any(r => r.MaToaSoan.CompareTo(MaToaSoan.Trim()) == 0 && r.Id.CompareTo(id.Trim()) != 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("MaToaSoan", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã tòa soạn"));
                }
                if (db.DMToaSoans.Any(r => r.TenToaSoan.CompareTo(TenToaSoan.Trim()) == 0 && r.Id.CompareTo(id.Trim()) != 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("TenToaSoan", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Tên tòa soạn"));
                }
            }
            return err;
        }
        /***
        * function: Add()
        * param : DMToaSoan
        * result: int
        * author: vietvb
        * Thêm dữ liệu, trả ra id
        * */
        public int Add(DMToaSoan DMToaSoan)
        {
            DMToaSoan.Status = (int)Enums.RecordStatusCode.active;
            db.DMToaSoans.Add(DMToaSoan);
            return db.SaveChanges();
        }
        /***
        * function: Update()
        * param : DMToaSoan
        * result: int
        * author: vietvb
        * Update dữ liệu, trả ra id
        * */
        public int Update(DMToaSoan dmToaSoan)
        {
            dmToaSoan.Status = (int)Enums.RecordStatusCode.active;
            //get old data
            DMToaSoan Old = db.DMToaSoans.Find(dmToaSoan.Id);
            CopyData(Old, dmToaSoan);
            Old.ModifyBy = dmToaSoan.ModifyBy;
            Old.ModifyDate = dmToaSoan.ModifyDate;
            //edit
            //db.Entry(DMToaSoan).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /***
        * function: Delete()
        * param : DMToaSoan
        * result: int
        * author: vietvb
        * Delete dữ liệu, trả ra int
        * */
        public int Delete(DMToaSoan DMToaSoan)
        {
            //DMToaSoan DMToaSoan = db.DMToaSoans.Find(id);
            //db.DMToaSoans.Remove(DMToaSoan);
            //return db.SaveChanges();

            //reset status for delete
            DMToaSoan.Status = (int)Enums.RecordStatusCode.delete;
            db.Entry(DMToaSoan).State = EntityState.Modified;
            return db.SaveChanges();
        }

        private void CopyData(DMToaSoan old, DMToaSoan newTS)
        {
            old.MaToaSoan = newTS.MaToaSoan;
            old.TenToaSoan = newTS.TenToaSoan;
            old.DiaChi = newTS.DiaChi;
            old.SoDienThoai = newTS.SoDienThoai;
            old.Email = newTS.Email;
            old.Web = newTS.Web;
            old.MaSoThue = newTS.MaSoThue;
            old.TaiKhoan = newTS.TaiKhoan;
            old.TongBienTap = newTS.TongBienTap;
            old.NguoiDaiDien = newTS.NguoiDaiDien;
            old.CoQuanChuQuan = newTS.CoQuanChuQuan;
            old.KieuToaSoan = newTS.KieuToaSoan;
            old.NganHang = newTS.NganHang;
        }

        //Hàm hủy
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
