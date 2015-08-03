using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public class SysDMTypeBussiness : ISysDMTypeBussiness
    {
        DB_PHBCEntities db;
        public SysDMTypeBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public SysDMTypeBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }

        /// <summary>
        /// Lay danh sach SysDMType
        /// </summary>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMType> getAll()
        {
            return db.SysDMTypes.ToList();
        }

        /// <summary>
        /// Lay danh sach SysDMType va phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMType> getAll(int page, int pageSize, out int pageCount)
        {
            int count = db.SysDMTypes.Count();
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
            return db.SysDMTypes.Skip(page * pageSize).Take(pageSize).ToList();
        }

        /***
         * function: getAllModel()
         * param : null
         * result: List<DMDiemInModel>
         * author: vietvb
         ***/
        public List<SysDMTypeModel> getAllModel()
        {
            List<SysDMTypeModel> result = new List<SysDMTypeModel>();
            result = (from t in db.SysDMTypes
                      select new SysDMTypeModel()
                      {
                          Id = t.Id,
                          Name = t.Name,
                          Description = t.Description,
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
        public List<SysDMTypeModel> getAllModel(int page, int pageSize, out int pageCount)
        {
            List<SysDMTypeModel> result = new List<SysDMTypeModel>();
            int count = db.SysDMTypes.Count();
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
            result = (from t in db.SysDMTypes
                      select new SysDMTypeModel()
                      {
                          Id = t.Id,
                          Name = t.Name,
                          Description = t.Description,
                      }).OrderBy(i => i.Id).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        /// <summary>
        /// Tim kiem SysDMType theo SysDMTypeName.(Đang phân biệt hoa thường)
        /// </summary>
        /// <param name="SysDMTypeName"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMType> search(string SysDMTypeName)
        {
            return db.SysDMTypes.Where(r => r.Name.Contains(SysDMTypeName)).ToList();
        }

        /// <summary>
        /// Tim kiem SysDMType theo SysDMTypeName và phân trang.(Đang phân biệt hoa thường)
        /// </summary>
        /// <param name="SysDMTypeName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMType> search(string SysDMTypeName, int page, int pageSize, out int pageCount)
        {
            int count = db.SysDMTypes.Where(r => r.Name.Contains(SysDMTypeName)).Count();
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
            return db.SysDMTypes.Where(r => r.Name.Contains(SysDMTypeName)).Skip(page * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// Tim kiem SysDMTypeModel theo SysDMTypeName và phân trang.(Đang phân biệt hoa thường)
        /// </summary>
        /// <param name="SysDMTypeName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMTypeModel> searchModel(string SysDMTypeName, int page, int pageSize, out int pageCount)
        {
            List<SysDMTypeModel> result = new List<SysDMTypeModel>();
            IEnumerable<SysDMTypeModel> query = null;
            query = (from t in db.SysDMTypes
                     select new SysDMTypeModel()
                     {
                         Id = t.Id,
                         Name = t.Name,
                         Description = t.Description,
                     });
            query = query.Where(r => r.Name.ToLower().Contains(SysDMTypeName.ToLower()));
            int count = query.Count();
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
            result = query.OrderBy(i => i.Id).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }
        /// <summary>
        /// Lay SysDMType theo SysDMTypeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public SysDMType getById(int? id)
        {
            return db.SysDMTypes.Find(id);
        }

        /// <summary>
        /// Kiem tra SysDMTypeName đã tồn tại chưa. Nếu có SysDMTypeId thì không check SysDMTypeId đó
        /// </summary>
        /// <param name="id"></param>
        /// <param name="SysDMTypeName"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public bool checkSysDMType(int id, string SysDMTypeName)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return db.SysDMTypes.Any(r => r.Name.CompareTo(SysDMTypeName) == 0);
            }
            else
            {
                return db.SysDMTypes.Any(r => r.Name.CompareTo(SysDMTypeName) == 0 && r.Id == id);
            }
        }

        /// <summary>
        /// Thêm mơi SysDMType vào database
        /// </summary>
        /// <param name="SysDMType"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Add(SysDMType SysDMType)
        {
            db.SysDMTypes.Add(SysDMType);
            return db.SaveChanges();
        }

        /// <summary>
        /// Cập nhập SysDMType vào database
        /// </summary>
        /// <param name="SysDMType"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Update(SysDMType SysDMType)
        {
            db.Entry(SysDMType).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /// <summary>
        /// Xoa SysDMType khoi database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Delete(int? id)
        {
            SysDMType SysDMType = db.SysDMTypes.Find(id);
            db.SysDMTypes.Remove(SysDMType);
            return db.SaveChanges();
        }

        /// <summary>
        /// Kiem tra id đã tồn tại chưa
        /// </summary>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public bool checkSysDMType(int id)
        {
            //int _id = int.Parse(id);
            return db.SysDMTypes.Any(c => c.Id == id);
        }
        //Ham huy
        public void Dispose()
        {
            db.Dispose();
        }

    }
}
