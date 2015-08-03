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
    public class DMDiemInBussiness : IDMDiemInBussiness
    {
        DB_PHBCEntities db;
        public DMDiemInBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public DMDiemInBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }
        /***
         * function: getAll()
         * param : null
         * result: List<DMDiemIn>
         * author: vietvb
         ***/
        public List<DMDiemIn> getAll()
        {
            return db.DMDiemIns.ToList();
        }

        /***
         * function: getAllProvince()
         * param : null
         * result: List<Province>
         * author: vietvb
         ***/
        public List<Province> getAllProvince()
        {
            return db.Provinces.OrderBy(t => t.ProvinceName).ToList();
        }

        /***
         * function: getAll()
         * param : int page, int pagesize, out int pageCount
         * result: List<DMDiemIn>
         * author: vietvb
         * Dùng để phân trang
         * */
        public List<DMDiemIn> getAll(int page, int pageSize, out int pageCount)
        {
            int count = db.DMDiemIns.Count();
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
            return db.DMDiemIns.OrderBy(i => i.MaDiemIn).Skip(page * pageSize).Take(pageSize).ToList();
        }

        /***
         * function: getAllModel()
         * param : null
         * result: List<DMDiemInModel>
         * author: vietvb
         ***/
        public List<DMDiemInModel> getAllModel()
        {
            List<DMDiemInModel> result = new List<DMDiemInModel>();
            result = (from t in db.DMDiemIns
                      where t.Status == (int)Enums.RecordStatusCode.active
                      select new DMDiemInModel()
                      {
                          MaDiemIn = t.MaDiemIn,
                          TenDiemIn = t.TenDiemIn,
                          ProvinceCode = t.ProvinceCode,
                          DistrictCode = t.DistrictCode,
                          DiaChi = t.DiaChi,
                      }).ToList();
            return result;
        }

        /***
         * function: getAll()
         * param : int page, int pagesize, out int pageCount
         * result: List<DMDiemIn>
         * author: vietvb
         * Dùng để phân trang
         * */
        public List<DMDiemInModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem)
        {
            List<DMDiemInModel> result = new List<DMDiemInModel>();
            int count = db.DMDiemIns.Where(x => x.Status == (int)Enums.RecordStatusCode.active).Count();
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
            result = (from t in db.DMDiemIns.Include(a=>a.Province).Include(a=>a.District)
                      where t.Status == (int)Enums.RecordStatusCode.active
                      select new DMDiemInModel()
                      {
                          Id = t.Id,
                          MaDiemIn = t.MaDiemIn,
                          TenDiemIn = t.TenDiemIn,
                          ProvinceCode = t.ProvinceCode,
                          DistrictCode = t.DistrictCode,
                          DiaChi = t.DiaChi,
                          ProvinceName = t.Province != null ? t.Province.ProvinceName : "",
                          DistrictName = t.District != null ? t.District.DistrictName : ""
                      }).OrderBy(i => i.MaDiemIn).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        /***
        * function: Search()
        * param : string MaDiemIn, string MaDiemIn = "", string TenDiemIn =""
        * result: List<DMDiemIn>
        * author: vietvb
        * Tìm kiếm theo mã điểm in và tên điểm in
        * */
        public List<DMDiemIn> search(string MaDiemIn = "", string TenDiemIn = "")
        {
            IEnumerable<DMDiemIn> query = null;
            if (!String.IsNullOrEmpty(TenDiemIn))
            {
                query = db.DMDiemIns.Where(r => r.TenDiemIn.ToLower().Contains(TenDiemIn.ToLower()));
            }
            if (!String.IsNullOrEmpty(MaDiemIn))
            {
                query = db.DMDiemIns.Where(r => r.MaDiemIn.ToLower().Contains(MaDiemIn.ToLower()));
            }
            if (query == null) return null;
            return query.ToList();
        }
        /***
        * function: Search()
        * param : string MaDiemIn, int page, int pageSize, out int pageCount
        * result: List<DMDiemIn>
        * author: vietvb
        * Tìm kiếm theo mã điểm in, tên điểm in và phân trang
        * */
        public List<DMDiemIn> search(string MaDiemIn, string TenDiemIn, int page, int pageSize, out int pageCount)
        {
            IEnumerable<DMDiemIn> query = null;
            if (!String.IsNullOrEmpty(TenDiemIn))
            {
                query = db.DMDiemIns.Where(r => r.TenDiemIn.ToLower().Contains(TenDiemIn.ToLower()));
            }
            else if (!String.IsNullOrEmpty(MaDiemIn))
            {
                query = db.DMDiemIns.Where(r => r.MaDiemIn.ToLower().Contains(MaDiemIn.ToLower()));
            }
            if (query != null)
            {
                int count = query.Where(x => x.Status == (int)Enums.RecordStatusCode.active).Count();
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

        /***
        * function: Search()
        * param : string MaDiemIn, int page, int pageSize, out int pageCount
        * result: List<DMDiemIn>
        * author: vietvb
        * Tìm kiếm theo mã điểm in, tên điểm in và phân trang
        * */
        public List<DMDiemInModel> searchModel(string MaDiemIn, string TenDiemIn, int page, int pageSize, out int pageCount, out int totalitem)
        {
            List<DMDiemInModel> result = new List<DMDiemInModel>();
            IEnumerable<DMDiemInModel> query = null;
            query = (from t in db.DMDiemIns
                     where t.Status == (int)Enums.RecordStatusCode.active
                     select new DMDiemInModel()
                     {
                         Id = t.Id,
                         MaDiemIn = t.MaDiemIn,
                         TenDiemIn = t.TenDiemIn,
                         ProvinceCode = t.ProvinceCode,
                         DiaChi = t.DiaChi,
                     });
            if (!String.IsNullOrEmpty(TenDiemIn))
            {
                query = query.Where(r => r.TenDiemIn.ToLower().Contains(TenDiemIn.ToLower()));
            }
            if (!String.IsNullOrEmpty(MaDiemIn))
            {
                query = query.Where(r => r.MaDiemIn.ToLower().Contains(MaDiemIn.ToLower()));
            }
            if (query != null)
            {
                int count = query.Count();
                //neu khong co du lieu thi return
                totalitem = count;
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
                result = query.OrderBy(i => i.MaDiemIn).Skip(page * pageSize).Take(pageSize).ToList();
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
        * function: getById()
        * param : string id
        * result: DMDiemIn
        * author: vietvb
        * Lấy dữ liệu theo ID
        * */
        public DMDiemIn getById(string id)
        {
            return db.DMDiemIns.Where(x => x.Status == (int)Enums.RecordStatusCode.active).Include("Province").SingleOrDefault(x => x.Id == id);
        }
        /***
        * function: checkDMDiemIn()
        * param : string id, string MaDiemIn, string TenDiemIn
        * result: bool
        * author: vietvb
        * Check có dữ liệu theo id, tên tòa soạn và mã tòa soạn
        * */
        public ErrorObject checkDMDiemIn(string id, string MaDiemIn, string TenDiemIn)
        {
            ErrorObject err = new ErrorObject();
            if (String.IsNullOrEmpty(id))
            {
                if (db.DMDiemIns.Any(r => r.MaDiemIn.CompareTo(MaDiemIn.Trim()) == 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("MaDiemIn", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã điểm in"));
                }
                if (db.DMDiemIns.Any(r => r.TenDiemIn.CompareTo(TenDiemIn.Trim()) == 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("TenDiemIn", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Tên điểm in"));
                }
            }
            else
            {
                if (db.DMDiemIns.Any(r => r.MaDiemIn.CompareTo(MaDiemIn.Trim()) == 0 && r.Id.CompareTo(id.Trim()) != 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("MaDiemIn", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã điểm in"));
                }
                if (db.DMDiemIns.Any(r => r.TenDiemIn.CompareTo(TenDiemIn.Trim()) == 0 && r.Id.CompareTo(id.Trim()) != 0 && r.Status != 4))
                {
                    err.HasError = true;
                    err.LstError.Add("TenDiemIn", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Tên điểm in"));
                }
            }
            return err;

        }
        /***
        * function: Add()
        * param : DMDiemIn
        * result: int
        * author: vietvb
        * Thêm dữ liệu, trả ra id
        * */
        public int Add(DMDiemInModel DMDiemIn)
        {
            db.DMDiemIns.Add(DMDiemIn.toCreate());
            return db.SaveChanges();
        }
        /***
        * function: Update()
        * param : DMDiemIn
        * result: int
        * author: vietvb
        * Update dữ liệu, trả ra id
        * */
        public int Update(DMDiemInModel diemIn)
        {
            //get old data
            DMDiemIn Old = db.DMDiemIns.Find(diemIn.Id);
            diemIn.changeEdit(Old);
            //edit
            db.Entry(Old).State = EntityState.Modified;
            return db.SaveChanges();
        }
        /***
        * function: Delete()
        * param : DMDiemIn
        * result: int
        * author: vietvb
        * Delete dữ liệu, trả ra int
        * */
        public int Delete(DMDiemIn DMDiemIn)
        {       
            DMDiemIn.Status = (int)Enums.RecordStatusCode.delete;
            db.Entry(DMDiemIn).State = EntityState.Modified;
            return db.SaveChanges();

            //real del data 

            //DMDiemIn DMDiemIn = db.DMDiemIns.Find(id);
            //db.DMDiemIns.Remove(DMDiemIn);
            //return db.SaveChanges();
        }

        /*
         * buildquery : query sử dụng chung cho business
         * TODO: new build query
         * */
        private IQueryable<DMDiemInModel> BuildQuery(DMDiemInSearchModel search, int status = 1, bool fInclude = false)
        {
            string include = string.Empty;
            IQueryable<DMDiemIn> pQuery;
            if (fInclude)
                pQuery = db.DMDiemIns.Include(a => a.Province);
            else
                pQuery = db.DMDiemIns;
            IQueryable<DMDiemInModel> query = (from t in pQuery                                           
                                           where t.Status == status
                                          select new DMDiemInModel()
                                           {
                                               Id = t.Id,
                                               MaDiemIn = t.MaDiemIn,
                                               TenDiemIn = t.TenDiemIn,
                                               ProvinceCode = t.ProvinceCode,
                                               DistrictCode = t.DistrictCode,
                                               DiaChi = t.DiaChi,
                                           }).OrderBy(i => i.MaDiemIn);
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.MaDiemIn))
                    query = query.Where(a => a.MaDiemIn.Contains(search.MaDiemIn));
                if (!string.IsNullOrWhiteSpace(search.TenDiemIn))
                    query = query.Where(a => a.TenDiemIn.Contains(search.TenDiemIn));           
            }
            return query;
        }

        //Hàm hủy
        public void Dispose()
        {
            db.Dispose();
        }





        public List<District> getDistrictByProvince(string Province)
        {
            //throw new NotImplementedException();
            return db.Districts.Where(a => a.ProvinceCode.Equals(Province)).OrderBy(a=>a.DistrictName).ToList();
        }


        public List<District> getAllDistrict()
        {
            //throw new NotImplementedException();
            return db.Districts.OrderBy(t => t.DistrictName).ToList();
        }
    }
}
