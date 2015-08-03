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

    public class BThongTinGiaBaoBussiness : IBThongTinGiaBaoBussiness
    {
        private DB_PHBCEntities db;
        public BThongTinGiaBaoBussiness()
        {
            this.db = new DB_PHBCEntities();
        }
        public BThongTinGiaBaoBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;
        }

        public List<BThongTinGiaBao> getAllBThongTinGiaBao()
        {
            return db.BThongTinGiaBaos.ToList();
            //throw new NotImplementedException();
        }

        private IQueryable<BThongTinGiaBaoModel> BuildQuery(ThongTinGiaBaoSearchModel search, int status = 1, List<string> lstInclude = null)
        {
            string include = string.Empty;
            IQueryable<BThongTinGiaBao> qThongTinGiaBao = db.BThongTinGiaBaos;
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qThongTinGiaBao = qThongTinGiaBao.Include(item);

            IQueryable<BThongTinGiaBaoModel> query = (from u in qThongTinGiaBao//.Include(a=>a.)
                                                      where u.Status == status
                                                      select new BThongTinGiaBaoModel()
                                                      {
                                                          Id = u.Id,
                                                          ThongTinBaoId = u.ThongTinBaoId,
                                                          NgayHieuLuc = u.NgayHieuLuc,
                                                          NgayHetHieuLuc = u.NgayHetHieuLuc,
                                                          ProvinceCode = u.ProvinceCode,
                                                          QuyetDinh = u.QuyetDinh,
                                                          ValueType = u.ValueType,
                                                          Value = u.Value,
                                                          MaBao = u.BThongTinBao != null ? u.BThongTinBao.MaBao : "",
                                                          TenBao = u.BThongTinBao != null ? u.BThongTinBao.TenBao : ""
                                                      }).OrderBy(a => a.ThongTinBaoId);
            //Neu co include thi them include vao 
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qThongTinGiaBao = qThongTinGiaBao.Include(item);
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.Id))
                    query = query.Where(a => a.Id.CompareTo(search.Id) == 0);
                if (!string.IsNullOrWhiteSpace(search.ThongTinBaoId))
                    query = query.Where(a => a.ThongTinBaoId.Contains(search.ThongTinBaoId));

            }
            return query;
        }

        public List<BThongTinGiaBaoModel> getAllBThongTinGiaBaoModel()
        {
            return BuildQuery(null).ToList();
            //throw new NotImplementedException();
        }

        public List<BThongTinGiaBao> getAll(int page, int pageSize, out int pageCount)
        {
            return Utils.buildPage(db.BThongTinGiaBaos, page, ref pageSize, out pageCount);
            //throw new NotImplementedException();
        }


        public List<BThongTinGiaBaoModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem)
        {
            var query = BuildQuery(null);
            List<BThongTinGiaBaoModel> lst = Utils.buildPage(query, page, ref pageSize, out totalitem, out pageCount);
            foreach (var item in lst)
            {

            }
            return lst;
            //throw new NotImplementedException();
        }

        private ErrorObject checkThongTinGiaBao(string Id, string ThongTinBaoId)
        {
            ErrorObject err = new ErrorObject();
            if (string.IsNullOrEmpty(Id))
            {
                if (db.BThongTinGiaBaos.Any(r => r.Id.CompareTo(Id.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("MaGiaBao", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã Giá Báo"));
                }
                if (db.BThongTinGiaBaos.Any(r => r.ThongTinBaoId.CompareTo(ThongTinBaoId.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("ThongTinBaoId", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã Thông Tin Báo"));
                }
            }
            else
            {
                if (db.BThongTinGiaBaos.Any(r => r.Id.CompareTo(Id.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("MaGiaBao", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã Giá Báo"));
                }
                if (db.BThongTinGiaBaos.Any(r => r.ThongTinBaoId.CompareTo(ThongTinBaoId.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("ThongTinBaoId", String.Format(Enums.ErrorMessage.SameKey.ToString(), "Mã Thông Tin Báo"));
                }
            }
            return err;
        }
        public ErrorObject Create(BThongTinGiaBaoModel bthongtingiabaoModel)
        {
            ErrorObject err = new ErrorObject();
            err = checkThongTinGiaBao(bthongtingiabaoModel.Id, bthongtingiabaoModel.ThongTinBaoId);
            if (err.HasError)
            {
                return err;
            }
            else
            {
                ////BThongTinGiaBao bThongTinGiaBao = new BThongTinGiaBao();
                //mapObject(bthongtingiabaoModel, bThongTinGiaBao);
                db.BThongTinGiaBaos.Add(bthongtingiabaoModel.toCreate());
                int i = db.SaveChanges();
                if (i < 1)
                {
                    err.HasError = false;
                }
            }
            return err;
            // throw new NotImplementedException();
        }

        public BThongTinGiaBaoModel getBThongTinGiaBaoModel(string id)
        {
            BThongTinGiaBao thongtingiabao = db.BThongTinGiaBaos.Include(a => a.ThongTinBaoId).FirstOrDefault(a => a.Id.CompareTo(id) == 0);
            if (thongtingiabao == null)
                return null;
            BThongTinGiaBaoModel result = new BThongTinGiaBaoModel();
            result.Id = thongtingiabao.Id;
            result.ThongTinBaoId = thongtingiabao.ThongTinBaoId;
            int status = (int)Enums.RecordStatusCode.active;
            return result;
            //throw new NotImplementedException();
        }

        public List<BThongTinGiaBaoModel> searchThongTinGiaBao(ThongTinGiaBaoSearchModel thongTinGiaBaoSearchModel)
        {
            int active = (int)Enums.RecordStatusCode.active;
            return BuildQuery(thongTinGiaBaoSearchModel, active, null).ToList();
            // throw new NotImplementedException();
        }

        public void Dispose()
        {
            db.Dispose();
            //throw new NotImplementedException();
        }
    }
}