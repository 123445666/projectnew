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
    public class SysDMPublicBussiness : ISysDMPublicBussiness
    {
        DB_PHBCEntities db;
        public SysDMPublicBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public SysDMPublicBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }

        /// <summary>
        /// Lay danh sach SysDMPublic
        /// </summary>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMPublic> getAll()
        {
            return db.SysDMPublics.ToList();
        }

        /// <summary>
        /// Lay danh sach SysDMPublic theo TypeID
        /// </summary>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Vietvb   10/6/15 create
        /// </modify>
        public List<SysDMPublic> getAllById(int? id)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                return db.SysDMPublics.Where(r => r.TypeId == id).ToList();
            }
            else
            {
                return db.SysDMPublics.ToList();
            }
        }

        /// <summary>
        /// Lay danh sach SysDMPublic va phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMPublic> getAll(int page, int pageSize, out int pageCount)
        {
            int count = db.SysDMPublics.Count();
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
            return db.SysDMPublics.Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Lay danh sach SysDMPublic va phan trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMPublicModel> getAllModelByTypeId(int typeid, int page, int pageSize, out int pageCount)
        {
            pageCount = 0;
            List<SysDMPublicModel> result = new List<SysDMPublicModel>();
            if(typeid==0)
            {
                return result;
            }
            int count = db.SysDMPublics.Where(t => t.TypeId == typeid).Count();
            //neu khong co du lieu thi return
            if (count == 0)
            {                
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
            result = (from t in db.SysDMPublics
                      where t.TypeId == typeid
                      select new SysDMPublicModel()
                      {
                        TypeId = t.TypeId,
                        Code = t.Code,
                        Name = t.Name,
                        IsLock= t.IsLock, 
                        bLock = (t.IsLock == 1),
                        Description = t.Description,
                      }).OrderBy(i => i.TypeId).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        /// <summary>
        /// Tim kiem SysDMPublic theo SysDMPublicName.(Đang phân biệt hoa thường)
        /// </summary>
        /// <param name="SysDMPublicName"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMPublic> search(string SysDMPublicName)
        {
            return db.SysDMPublics.Where(r => r.Name.Contains(SysDMPublicName)).ToList();
        }

        /// <summary>
        /// Tim kiem SysDMPublic theo SysDMPublicName và phân trang.(Đang phân biệt hoa thường)
        /// </summary>
        /// <param name="SysDMPublicName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMPublic> search(string SysDMPublicName, int page, int pageSize, out int pageCount)
        {
            int count = db.SysDMPublics.Where(r => r.Name.Contains(SysDMPublicName)).Count();
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
            return db.SysDMPublics.Where(r => r.Name.Contains(SysDMPublicName)).Skip(page * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// Tim kiem SysDMPublic theo SysDMPublicName và phân trang.(Đang phân biệt hoa thường)
        /// </summary>
        /// <param name="SysDMPublicName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public List<SysDMPublicModel> searchModel(SysDMPublicSearchModel obj, int page, int pageSize, out int pageCount)
        {
            List<SysDMPublicModel> result = new List<SysDMPublicModel>();
            IEnumerable<SysDMPublicModel> query = null;
            query = (from t in db.SysDMPublics
                     where t.TypeId == obj.TypeId
                     select new SysDMPublicModel()
                     {
                        TypeId = t.TypeId,
                        Code = t.Code,
                        Name = t.Name,
                        IsLock= t.IsLock, 
                        bLock = (t.IsLock == 1),
                        Description = t.Description,
                     });
            query = query.Where(r => r.Name.ToLower().Contains(obj.Name.ToLower()));
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
            result = query.OrderBy(i => i.Code).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        /// <summary>
        /// Lay SysDMPublic theo SysDMPublicId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public SysDMPublic getByIdAndCode(int? id, string code = "")
        {
            return db.SysDMPublics.Find(id, code);
        }

        /// <summary>
        /// Kiem tra SysDMPublicName đã tồn tại chưa. Nếu có SysDMPublicId thì không check SysDMPublicId đó
        /// </summary>
        /// <param name="id"></param>
        /// <param name="SysDMPublicName"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public ErrorObject checkSysDMPublic(string code)
        {
            ErrorObject err = new ErrorObject();
            if (db.SysDMPublics.Any(r => r.Code.CompareTo(code.Trim()) == 0))
            {
                err.HasError = true;
                err.LstError.Add("Code", "Mã danh mục này đã được sử dụng !");
            }
            return err;
        }

        /// <summary>
        /// Thêm mơi SysDMPublic vào database
        /// </summary>
        /// <param name="SysDMPublic"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Add(SysDMPublic SysDMPublic)
        {
            db.SysDMPublics.Add(SysDMPublic);
            return db.SaveChanges();
        }

        /// <summary>
        /// Cập nhập SysDMPublic vào database
        /// </summary>
        /// <param name="SysDMPublic"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Update(SysDMPublic SysDMPublic)
        {
            db.Entry(SysDMPublic).State = EntityState.Modified;
            return db.SaveChanges();
        }

        /// <summary>
        /// Xoa SysDMPublic khoi database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <modify>
        /// Author  date    comment
        /// Anhhn   10/6/15 create
        /// </modify>
        public int Delete(int? id, string code = "")
        {
            SysDMPublic SysDMPublic = db.SysDMPublics.Find(id, code.Trim());
            db.SysDMPublics.Remove(SysDMPublic);
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
        public ErrorObject checkSysDMPublic(int id, string code)
        {
            ErrorObject err = new ErrorObject();
            if (db.SysDMPublics.Any(c => c.TypeId != id && c.Code.CompareTo(code.Trim()) == 0))
            {
                err.HasError = true;
                err.LstError.Add("Code", "Mã danh mục này đã được sử dụng !");
            }
            return err;
        }
        //Ham huy
        public void Dispose()
        {
            db.Dispose();
        }

    }
}
