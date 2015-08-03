using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System.Data.Entity;

namespace PHBC.DAO.Bussiness
{
    public class DMLoaiAnPhamBussiness : IDMLoaiAnPhamBussiness
    {
        DB_PHBCEntities db;

        public DMLoaiAnPhamBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public DMLoaiAnPhamBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }
        public List<DMLoaiAnPham> getAll()
        {
            //throw new NotImplementedException();
            return db.DMLoaiAnPhams.ToList();
        }

        public List<DMLoaiAnPham> getAll(int page, int pageSize, out int pageCount)
        {
            //throw new NotImplementedException();
            int count = db.DMLoaiAnPhams.Count();
            //neu khong co du lieu thi return
            if (count == 0)
            {
                pageCount = 0;
                return null;
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
            return db.DMLoaiAnPhams.OrderBy(i => i.Id).Skip(page * pageSize).Take(pageSize).ToList();
        }

        public List<DMLoaiAnPhamModel> getAllModel()
        {
            //throw new NotImplementedException();
            List<DMLoaiAnPhamModel> result = new List<DMLoaiAnPhamModel>();
            int active = (int)Enums.RecordStatusCode.active;
            result = (from t in db.DMLoaiAnPhams
                      where t.Status == active
                      select new DMLoaiAnPhamModel()
                      {
                          Id = t.Id,
                          TenLoaiAnPham = t.TenLoaiAnPham,
                          CoKyXuatBan = t.CoKyXuatBan,
                          Status = t.Status,
                      }).OrderBy(a=>a.TenLoaiAnPham).ToList();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<DMLoaiAnPhamModel> getAllModel(int page,ref int pageSize, out int pageCount)
        {
            //throw new NotImplementedException();
            List<DMLoaiAnPhamModel> result = new List<DMLoaiAnPhamModel>();
            int count = db.DMLoaiAnPhams.Count();
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
            result = (from t in db.DMLoaiAnPhams
                      select new DMLoaiAnPhamModel()
                      {
                          Id = t.Id,
                          TenLoaiAnPham = t.TenLoaiAnPham,
                          CoKyXuatBan = t.CoKyXuatBan,
                          Status = t.Status,
                      }).OrderBy(i => i.TenLoaiAnPham).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }
        public List<DMLoaiAnPham> search(string Id, string TenLoaiAnPham, int page, int pageSize, out int pageCount)
        {
            //throw new NotImplementedException();
            IEnumerable<DMLoaiAnPham> query = null;
            if (!String.IsNullOrEmpty(TenLoaiAnPham))
            {
                query = db.DMLoaiAnPhams.Where(r => r.TenLoaiAnPham.Contains(TenLoaiAnPham));
            }
            else if (!String.IsNullOrEmpty(Id))
            {
                query = db.DMLoaiAnPhams.Where(r => r.Id.Contains(Id));
            }
            if (query != null)
            {
                int count = query.Count();
                //neu khong co du lieu thi return
                if (count == 0)
                {
                    pageCount = 0;
                    return null;
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
                return query.Skip(page * pageSize).Take(pageSize).ToList();
            }
            else
            {
                pageCount = 0;
                return null;
            }
        }
        public DMLoaiAnPham getById(string id)
        {
            //throw new NotImplementedException();
            return db.DMLoaiAnPhams.Find(id);
        }
        public ErrorObject checkDMLoaiAnPham(byte action, string Id, string TenLoaiAnPham)
        {
            //throw new NotImplementedException();
            ErrorObject err = new ErrorObject();
            err.HasError = false;
            if (action == Convert.ToByte(Common.Enums.FormAction.Edit))
            {
                if (db.DMLoaiAnPhams.Any(r => r.TenLoaiAnPham.CompareTo(TenLoaiAnPham.Trim()) == 0 && r.Id.CompareTo(Id.Trim()) != 0))
                {
                    err.HasError = true;
                    err.LstError.Add("TenLoaiAnPham", "Tên loại ấn phẩm có trong hệ thống !");
                }
            }
            else if (action == Convert.ToByte(Common.Enums.FormAction.Create))
            {
                if (db.DMLoaiAnPhams.Any(r => r.Id.CompareTo(Id.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("Id", "Mã loại ấn phẩm này đã có trong hệ thống!");
                }
                if (db.DMLoaiAnPhams.Any(r => r.TenLoaiAnPham.CompareTo(TenLoaiAnPham.Trim()) == 0 && r.Id.CompareTo(Id.Trim()) != 0))
                {
                    err.HasError = true;
                    err.LstError.Add("TenLoaiAnPham", "Tên loại ấn phẩm này đã có trong hệ thống  !");
                }
            }
            return err;
        }
        public int Add(DMLoaiAnPham DMLoaiAnPham)
        {
            //throw new NotImplementedException();
            db.DMLoaiAnPhams.Add(DMLoaiAnPham);
            return db.SaveChanges();
        }
        public int Update(DMLoaiAnPhamModel dmLoaiAnPhamModel)
        {
            DMLoaiAnPham entry =db.DMLoaiAnPhams.Find(dmLoaiAnPhamModel.Id);
            entry.CoKyXuatBan = dmLoaiAnPhamModel.CoKyXuatBan;
            entry.TenLoaiAnPham = dmLoaiAnPhamModel.TenLoaiAnPham;
            db.Entry(entry).State = EntityState.Modified;
            return db.SaveChanges();
        }
        public int Delete(string id)
        {
            //throw new NotImplementedException();
            DMLoaiAnPham entry = db.DMLoaiAnPhams.Find(id);           
            entry.Status = (int) Enums.RecordStatusCode.delete;
            db.Entry(entry).State = EntityState.Modified;
            return db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }
        public List<DMLoaiAnPham> search(string Id = "", string TenLoaiAnPham = "")
        {
            //throw new NotImplementedException();
            IEnumerable<DMLoaiAnPham> query = null;
            if (!String.IsNullOrEmpty(TenLoaiAnPham))
            {
                query = db.DMLoaiAnPhams.Where(r => r.TenLoaiAnPham.Contains(TenLoaiAnPham));
            }
            if (!String.IsNullOrEmpty(Id))
            {
                query = db.DMLoaiAnPhams.Where(r => r.Id.Contains(Id));
            }
            if (query == null) return null;
            return query.ToList();
        }
        public List<DMLoaiAnPham> search(string TenLoaiAnPham, int page, int pageSize, out int pageCount)
        {
           // throw new NotImplementedException();
            int count = db.DMLoaiAnPhams.Where(r => r.TenLoaiAnPham.Contains(TenLoaiAnPham)).Count();
            //neu khong co du lieu thi return
            if (count == 0)
            {
                pageCount = 0;
                return null;
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
            return db.DMLoaiAnPhams.Where(r => r.TenLoaiAnPham.Contains(TenLoaiAnPham)).Skip(page * pageSize).Take(pageSize).ToList();
        }
        public List<DMLoaiAnPhamModel> search(string TenLoaiAnPham)
        {
            //throw new NotImplementedException();
            var result = (from r in db.DMLoaiAnPhams
                         where r.TenLoaiAnPham.Contains(TenLoaiAnPham)
                         select new DMLoaiAnPhamModel()
                         {
                             TenLoaiAnPham = r.TenLoaiAnPham,
                             CoKyXuatBan = r.CoKyXuatBan,
                             Id = r.Id,
                             Status = r.Status
                         }).ToList();
            return result;
        }


        //public List<DMLoaiAnPhamModel> search(DMLoaiAnPhamModel dmloaianpham, int pagenum, int p, out int pageCount)
        //{
        //    //throw new NotImplementedException();
        //    List<DMLoaiAnPhamModel> result = new List<DMLoaiAnPhamModel>();
        //    IEnumerable<DMLoaiAnPhamModel> query = null;
        //    query = (from t in db.DMLoaiAnPhams
        //             where t.Status == 1
        //             select new DMLoaiAnPhamModel()
        //             {
        //                 TenLoaiAnPham = t.TenLoaiAnPham
                         
        //             });
        //    if (!String.IsNullOrEmpty(dmloaianpham.TenLoaiAnPham))
        //    {
        //        query = query.Where(r => r.TenLoaiAnPham.ToLower().Contains(dmloaianpham.TenLoaiAnPham.ToLower()));
        //    }
            
        //    if (query != null)
        //    {
        //        int count = query.Count();
        //        //neu khong co du lieu thi return
        //        if (count == 0)
        //        {
        //            pageCount = 0;
        //            return result;
        //        }
        //        //pageSize nho hon 10 thi dat bang 10
        //        //if (pageSize < 10)
        //        //    pageSize = 2;
        //        //tinh pagecount
        //        //pageCount = count / pageSize;
        //        //Neu so luong con du thi tang pagecount len 1
        //        if (count % pageSize > 0) pageCount++;
        //        //Neu trang hien tai lon hon pagecount thi thiet lap bang pageCount-1
        //        if (page >= pageCount)
        //            page = pageCount - 1;
        //        else
        //            page = page - 1;//So trang bat dau tu 0
        //        result = query.OrderBy(i => i.TenLoaiAnPham).Skip(page * pageSize).Take(pageSize).ToList();
        //        return result;
        //    }
        //    else
        //    {
        //        pageCount = 0;
        //        return result;
        //    }
        //}
    }
}
