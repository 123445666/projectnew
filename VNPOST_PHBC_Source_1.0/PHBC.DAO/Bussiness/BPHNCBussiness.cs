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
    public class BPHNCBussiness : IBPHNCBussiness
    {
        List<Unit> lstUnit;
        private DB_PHBCEntities db;

        /*
         * constructor function
         * */
        public BPHNCBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public BPHNCBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }

        public List<BPhanHuongNhuCau> getAll()
        {
            //var query3 = db.BDiemTiepNhans.Include(a => a.BPhanHuongNhuCaus)
            //    .Include("BPhanHuongNhuCaus.Unit").ToList();
            //string s = "1";
            //var query1 = from a in query3
            //             select new BDiemTiepNhanModel
            //             {
            //                 Id = a.Id,
            //                 Name = a.Name,
            //                 Units = a.BPhanHuongNhuCaus.Where(d=>(string.IsNullOrWhiteSpace(s) && d.ThongTinBaoId==null) || d.ThongTinBaoId == s).Select(b => b.Unit).Distinct()
            //             };

            //var query = db.BPhanHuongNhuCaus.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit).GroupBy(b => b.DiemTiepNhanId);
            //var query2 = from p in db.BPhanHuongNhuCaus.Include(a => a.BDiemTiepNhan).GroupBy(a => a.BDiemTiepNhan)
            //                 select p
            //            ;
            List<BPhanHuongNhuCau> bPhanHuongNhuCaus = db.BPhanHuongNhuCaus.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit).ToList();
            return bPhanHuongNhuCaus;
        }

        public List<BPhanHuongNhuCauModel> getAllModelPager(int page, ref int pageSize, out int pageCount, out int totalitem, string Mabao = null)
        {
            List<BPhanHuongNhuCauModel> result = new List<BPhanHuongNhuCauModel>();
            IQueryable<BPhanHuongNhuCau> lstPH;
            if (string.IsNullOrWhiteSpace(Mabao))
            {
                lstPH = db.BPhanHuongNhuCaus.Where(a => a.ThongTinBaoId == null);
            }
            else
            {
                lstPH = db.BPhanHuongNhuCaus.Where(a => a.ThongTinBaoId.Equals(Mabao));
            }

            IQueryable<BPhanHuongNhuCauModel> query = (from t in lstPH.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit)
                                                       join u in db.v_Unit on t.UnitCode equals u.UnitCode
                                                       join k in db.Provinces on u.ProvinceCode equals k.ProvinceCode
                                                       select new BPhanHuongNhuCauModel()
                                                       {
                                                           Id = t.Id,
                                                           UnitCode = t.UnitCode,
                                                           UnitName = t.Unit.UnitName,
                                                           DiemTiepNhanId = t.DiemTiepNhanId,
                                                           TenDiemTiepNhan = t.BDiemTiepNhan.Name,
                                                           ThongTinBaoId = t.ThongTinBaoId,
                                                           TenBao = t.BThongTinBao.TenBao,
                                                           BDiemTiepNhan = t.BDiemTiepNhan,
                                                           BThongTinBao = t.BThongTinBao,
                                                           Unit = t.Unit,
                                                           CreateBy = t.CreateBy,
                                                           CreateDate = t.CreateDate,
                                                           ModifyBy = t.ModifyBy,
                                                           ModifyDate = t.ModifyDate,
                                                           ProvinceCode = u.ProvinceCode,
                                                           ProvinceName = k.ProvinceName,
                                                           DistrictCode = u.DistrictCode,
                                                           DistrictName = u.DistrictName
                                                       }).OrderBy(x => x.ProvinceCode).OrderBy(x => x.DistrictCode);
            result = Utils.buildPage<BPhanHuongNhuCauModel>(query, page, ref pageSize, out totalitem, out pageCount);
            return result;
        }
        public List<BDiemTiepNhanModel> getAllData(string currentunit, string ThongTinBaoId = null)
        {
            var query3 = db.BDiemTiepNhans.Include(a => a.BPhanHuongNhuCaus)
               .Include("BPhanHuongNhuCaus.Unit").ToList();
            //IQueryable<string> lstUnit2 = getChildUnitCode(currentunit);
            lstUnit = getChildUnit(currentunit);
            var query = from a in query3
                        select new BDiemTiepNhanModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Units = a.BPhanHuongNhuCaus.Where(d => ((string.IsNullOrWhiteSpace(ThongTinBaoId) && d.ThongTinBaoId == null) || d.ThongTinBaoId == ThongTinBaoId) && lstUnit.Select(x => x.UnitCode).Contains(d.UnitCode)).OrderBy(d => d.UnitCode).Select(b => b.Unit).Distinct()
                        };
            return query.ToList();
        }

        public List<BPhanHuongNhuCau> getAll(int page, int pageSize, out int pageCount)
        {
            throw new NotImplementedException();
        }

        public List<BPhanHuongNhuCauModel> getAllModel(int page, int pageSize, out int pageCount, out int totalitem)
        {
            List<BPhanHuongNhuCauModel> result = new List<BPhanHuongNhuCauModel>();
            int count = db.BPhanHuongNhuCaus.Count();
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
            result = (from t in db.BPhanHuongNhuCaus.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit)
                      select new BPhanHuongNhuCauModel()
                      {
                          Id = t.Id,
                          UnitCode = t.UnitCode,
                          DiemTiepNhanId = t.DiemTiepNhanId,
                          ThongTinBaoId = t.ThongTinBaoId,
                          BDiemTiepNhan = t.BDiemTiepNhan,
                          BThongTinBao = t.BThongTinBao,
                          Unit = t.Unit,
                          CreateBy = t.CreateBy,
                          CreateDate = t.CreateDate,
                          ModifyBy = t.ModifyBy,
                          ModifyDate = t.ModifyDate
                      }).OrderBy(i => i.Id).Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }

        public List<BPhanHuongNhuCauModel> searchModel(DMToaSoanSearchModel obj, int page, int pageSize, out int pageCount, out int totalitem)
        {
            throw new NotImplementedException();
        }

        public List<BPhanHuongNhuCauModel> searchModel(DMToaSoanSearchModel obj)
        {
            throw new NotImplementedException();
        }

        public BPhanHuongNhuCau getById(string id)
        {
            return db.BPhanHuongNhuCaus.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit).FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// Function check PHBC
        /// ErrorObject with:
        /// - check1: value 1: lỗi trùng unitcode và điểm tiếp nhận
        /// </summary>
        /// <param name="UnitCode">Mã bưu cục</param>
        /// <param name="ThongTinBaoId">Mã báo</param>
        /// <param name="DiemTiepNhanId">Mã điểm tiếp nhận</param>
        /// <returns>ErrorObject</returns>
        public ErrorObject checkPHBC(BPhanHuongNhuCau BPhanHuongNhuCau)
        {
            ErrorObject err = new ErrorObject();
            if (string.IsNullOrWhiteSpace(BPhanHuongNhuCau.ThongTinBaoId))
            {
                if (db.BPhanHuongNhuCaus.Any(r => r.UnitCode.CompareTo(BPhanHuongNhuCau.UnitCode.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BPhanHuongNhuCau.DiemTiepNhanId.Trim()) == 0 && r.ThongTinBaoId == null))
                {
                    err.HasError = true;
                    err.LstError.Add("Check1", "1"); // trùng giá trị unit code , diem tiep nhan id
                }
            }
            else
            {
                if (db.BPhanHuongNhuCaus.Any(r => r.ThongTinBaoId.CompareTo(BPhanHuongNhuCau.ThongTinBaoId.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BPhanHuongNhuCau.DiemTiepNhanId.Trim()) == 0 && r.UnitCode.CompareTo(BPhanHuongNhuCau.UnitCode.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("Check3", "3"); // trùng cả 3 giá trị
                }
            }
            //if (db.BPhanHuongNhuCaus.Any(r => r.ThongTinBaoId.CompareTo(BPhanHuongNhuCau.ThongTinBaoId.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BPhanHuongNhuCau.DiemTiepNhanId.Trim()) == 0))
            //{
            //    err.HasError = true;
            //    err.LstError.Add("Check2", "2"); // trùng giá trị ma bao, diem tiep nhan id
            //}


            return err;
        }
        /// <summary>
        /// Function check PHBC
        /// ErrorObject with:  
        /// TODO: Hàm thêm mới BP
        /// </summary>
        /// <param name="UnitCode">Mã bưu cục</param>
        /// <param name="ThongTinBaoId">Mã báo</param>
        /// <param name="DiemTiepNhanId">Mã điểm tiếp nhận</param>
        /// <returns>
        /// ErrorObject
        /// - check1: value 1: lỗi trùng unitcode và điểm tiếp nhận
        /// </returns>
        private ErrorObject checkPHBCNew(BPhanHuongNhuCau BPhanHuongNhuCau)
        {
            ErrorObject err = new ErrorObject();
            List<string> units = new List<string>();
            List<BPhanHuongNhuCau> lstPH;
            BPhanHuongNhuCau obj;

            if (string.IsNullOrWhiteSpace(BPhanHuongNhuCau.ThongTinBaoId))
            {
                lstPH = db.BPhanHuongNhuCaus.Where(a => a.ThongTinBaoId == null).ToList();
            }
            else
            {
                lstPH = db.BPhanHuongNhuCaus.Where(a => a.ThongTinBaoId.Equals(BPhanHuongNhuCau.ThongTinBaoId)).ToList();
            }
            foreach (string item in units)
            {
                obj = lstPH.FirstOrDefault(a => a.UnitCode.Equals(item));
                if (obj == null)
                {
                    obj = new BPhanHuongNhuCau();
                    obj.UnitCode = item;
                    obj.ThongTinBaoId = BPhanHuongNhuCau.ThongTinBaoId;
                    obj.DiemTiepNhanId = BPhanHuongNhuCau.DiemTiepNhanId;
                    obj.CreateBy = BPhanHuongNhuCau.CreateBy;
                    obj.CreateDate = BPhanHuongNhuCau.CreateDate;
                    db.BPhanHuongNhuCaus.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BPhanHuongNhuCau.ModifyBy;
                    obj.ModifyDate = BPhanHuongNhuCau.ModifyDate;
                    obj.DiemTiepNhanId = BPhanHuongNhuCau.DiemTiepNhanId;
                }
            }
            db.SaveChanges();
            //if (db.BPhanHuongNhuCaus.Any(r => r.ThongTinBaoId.CompareTo(BPhanHuongNhuCau.ThongTinBaoId.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BPhanHuongNhuCau.DiemTiepNhanId.Trim()) == 0))
            //{
            //    err.HasError = true;
            //    err.LstError.Add("Check2", "2"); // trùng giá trị ma bao, diem tiep nhan id
            //}


            return err;
        }
        public int Add(BPhanHuongNhuCau BPhanHuongNhuCau, List<v_Unit> units)
        {
            //db.BPhanHuongNhuCaus.Add(BPhanHuongNhuCau);
            //return db.SaveChanges();

            //new insert function
            List<BPhanHuongNhuCau> lstPH;
            BPhanHuongNhuCau obj;

            if (string.IsNullOrWhiteSpace(BPhanHuongNhuCau.ThongTinBaoId))
            {
                lstPH = db.BPhanHuongNhuCaus.Where(a => a.ThongTinBaoId == null).ToList();
            }
            else
            {
                lstPH = db.BPhanHuongNhuCaus.Where(a => a.ThongTinBaoId.Equals(BPhanHuongNhuCau.ThongTinBaoId)).ToList();
            }
            foreach (v_Unit item in units)
            {
                obj = lstPH.FirstOrDefault(a => a.UnitCode.Equals(item.UnitCode));
                if (obj == null)
                {
                    obj = new BPhanHuongNhuCau();
                    obj.Id = item.UnitCode + BPhanHuongNhuCau.ThongTinBaoId;
                    obj.UnitCode = item.UnitCode;
                    obj.ThongTinBaoId = BPhanHuongNhuCau.ThongTinBaoId;
                    obj.DiemTiepNhanId = BPhanHuongNhuCau.DiemTiepNhanId;
                    obj.CreateBy = BPhanHuongNhuCau.CreateBy;
                    obj.CreateDate = BPhanHuongNhuCau.CreateDate;
                    db.BPhanHuongNhuCaus.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BPhanHuongNhuCau.ModifyBy;
                    obj.ModifyDate = BPhanHuongNhuCau.ModifyDate;
                    obj.DiemTiepNhanId = BPhanHuongNhuCau.DiemTiepNhanId;
                }
            }
            return db.SaveChanges();
        }

        public int Update(BPhanHuongNhuCau BPhanHuongNhuCau)
        {
            db.Entry(BPhanHuongNhuCau).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(string id)
        {
            BPhanHuongNhuCau bPhanHuongNhuCau = db.BPhanHuongNhuCaus.Find(id);
            db.BPhanHuongNhuCaus.Remove(bPhanHuongNhuCau);
            return db.SaveChanges();
        }
        private IQueryable<BPhanHuongNhuCauModel> BuildQuery(BPhanHuongNhuCauSearchModel search, bool fInclude = false)
        {
            string include = string.Empty;
            IQueryable<BPhanHuongNhuCau> qPHNC;
            if (fInclude)
                qPHNC = db.BPhanHuongNhuCaus.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit);
            else
                qPHNC = db.BPhanHuongNhuCaus;
            IQueryable<BPhanHuongNhuCauModel> query = (from t in qPHNC
                                                       select new BPhanHuongNhuCauModel()
                                                       {
                                                           Id = t.Id,
                                                           UnitCode = t.UnitCode,
                                                           DiemTiepNhanId = t.DiemTiepNhanId,
                                                           ThongTinBaoId = t.ThongTinBaoId,
                                                           BDiemTiepNhan = t.BDiemTiepNhan,
                                                           BThongTinBao = t.BThongTinBao,
                                                           Unit = t.Unit,
                                                           CreateBy = t.CreateBy,
                                                           CreateDate = t.CreateDate,
                                                           ModifyBy = t.ModifyBy,
                                                           ModifyDate = t.ModifyDate
                                                       }).OrderBy(a => a.DiemTiepNhanId).OrderBy(a => a.UnitCode);
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.BDiemTiepNhan.Name))
                    query = query.Where(a => a.BDiemTiepNhan.Name.Contains(search.BDiemTiepNhan.Name));
                if (!string.IsNullOrWhiteSpace(search.Unit.UnitName))
                    query = query.Where(a => a.Unit.UnitName.Contains(search.Unit.UnitName));
                if (!string.IsNullOrWhiteSpace(search.BThongTinBao.TenBao))
                    query = query.Where(a => a.BThongTinBao.TenBao.Contains(search.BThongTinBao.TenBao));
                if (!string.IsNullOrWhiteSpace(search.UnitCode))
                    query = query.Where(a => a.UnitCode.CompareTo(search.UnitCode) == 0);
                if (!string.IsNullOrWhiteSpace(search.DiemTiepNhanId))
                    query = query.Where(a => a.DiemTiepNhanId.CompareTo(search.DiemTiepNhanId) == 0);
                if (!string.IsNullOrWhiteSpace(search.ThongTinBaoId))
                    query = query.Where(a => a.ThongTinBaoId.CompareTo(search.ThongTinBaoId) == 0);
            }
            return query;
        }


        public List<Unit> getAllUnitNotInPhnc(string currentunit, string BThongTinBaoId = null)
        {
            List<BPhanHuongNhuCau> phanhuong;
            if (string.IsNullOrWhiteSpace(BThongTinBaoId))
                phanhuong = db.BPhanHuongNhuCaus.Where(t => t.ThongTinBaoId == null).ToList();
            else
                phanhuong = db.BPhanHuongNhuCaus.Where(t => t.ThongTinBaoId == BThongTinBaoId).ToList();
            List<Unit> ls = new List<Unit>();
            //check unit of current user
            lstUnit = getChildUnit(currentunit);
            //ls.Add(lstUnit);

            var query = (from c in lstUnit
                         where !phanhuong.Any(a => a.UnitCode == c.UnitCode)
                         select c);

            return query.OrderBy(r => r.UnitCode).ToList();
            //return db.Units.Where("!(from o in dc.Orders select o.CustomerID).Contains(c.CustomerID)").ToList();
        }


        public List<Unit> getAllUnitByDTNId(string currentunit, string DiemTiepNhanId, string BThongTinBaoId = null)
        {
            List<BPhanHuongNhuCau> phanhuong;
            if (string.IsNullOrWhiteSpace(BThongTinBaoId))
                phanhuong = db.BPhanHuongNhuCaus.Where(t => t.ThongTinBaoId == null).Include(a => a.Unit).ToList();
            else
                phanhuong = db.BPhanHuongNhuCaus.Where(t => t.ThongTinBaoId == BThongTinBaoId).Include(a => a.Unit).Include(a => a.BThongTinBao).ToList();

            //IQueryable<string> lstUnit = getChildUnitCode(currentunit);
            lstUnit = getChildUnit(currentunit);
            List<BPhanHuongNhuCau> lstPHNC = db.BPhanHuongNhuCaus.ToList();
            List<Unit> query = phanhuong.Where(t => (lstUnit.Select(c => c.UnitCode).Contains(t.UnitCode)) && t.DiemTiepNhanId.CompareTo(DiemTiepNhanId) == 0).OrderBy(r => r.UnitCode).Select(b => b.Unit).Distinct().ToList();

            return query;
        }
        /// <summary>
        /// author: vietvb
        /// desc : function to get all child unit of current
        /// </summary>
        /// <param name="currentunit"></param>
        /// <returns>IQueryable Unit</returns>
        public List<Unit> getChildUnit(string currentunit)
        {
            lstUnit = new List<Unit>();
            List<Unit> lstAllUnit = db.Units.ToList();
            Unit curent = lstAllUnit.Find(a => a.UnitCode.Equals(currentunit));
            if (curent != null)
            {
                getChild(curent, 1);
            }
            lstAllUnit = null;
            return lstUnit;
        }

        private void getChild(Unit curent, int level)
        {
            lstUnit.Add(curent);
            if (curent.Unit1 != null && curent.Unit1.Count > 0)
            {
                foreach (var item in curent.Unit1)
                    getChild(item, level++);
            }
        }

        /// <summary>
        /// author: vietvb
        /// desc : function to get all child unit of current
        /// </summary>
        /// <param name="currentunit"></param>
        /// <returns>IQueryable Unit</returns>
        public IQueryable<string> getChildUnitCode(string currentunit)
        {
            IQueryable<string> lstUnit;
            if (currentunit.Equals("00"))
            {
                lstUnit = db.Units.Select(a => a.UnitCode);
            }
            else
                lstUnit = db.Units.Where(a => a.ParentUnitCode.Contains(currentunit)).Select(a => a.UnitCode);
            return lstUnit;
        }

        public BDiemTiepNhan getByDiemTiepNhanId(string id)
        {
            return db.BDiemTiepNhans.Find(id);
        }

        public List<BDiemTiepNhan> getAllDiemTiepNhan()
        {
            return db.BDiemTiepNhans.ToList();
        }
        public BDiemTiepNhan getAllDiemTiepNhanById(string id)
        {
            return db.BDiemTiepNhans.Find(id);
        }

        public List<BThongTinBao> getAllThongTinBao()
        {
            return db.BThongTinBaos.ToList();
        }
        public BThongTinBao getAllThongTinBaoById(string id)
        {
            return db.BThongTinBaos.Find(id);
        }
        public List<Unit> getAllUnit()
        {
            return db.Units.ToList();
        }

        public BPhanHuongNhuCau getByUnit(string UnitCode, string Mabao = null)
        {
            if (string.IsNullOrWhiteSpace(Mabao))
            {
                return db.BPhanHuongNhuCaus.FirstOrDefault(t => t.UnitCode.CompareTo(UnitCode) == 0 && t.ThongTinBaoId == null);
            }
            else
                return db.BPhanHuongNhuCaus.FirstOrDefault(t => t.UnitCode.CompareTo(UnitCode) == 0 && t.ThongTinBaoId == Mabao);
        }


        public List<District> getDiStrictByArrayId(string[] lstids)
        {
            return db.Districts.Where(c => lstids.Contains(c.ProvinceCode)).Include("Province").ToList();
        }

        public List<v_Unit> getUnitByArrayId(string[] lstids, string typeid)
        {

            List<v_Unit> lstUnits = new List<v_Unit>();
            int count = lstids.Length;
            int forCount = 1;
            int size = 200;
            if (lstids.Length > size)
            {
                forCount = count / size;
                if (count % size > 0)
                    forCount++;
            }
            for (int i = 0; i < forCount; i++)
            {
                List<string> temp = lstids.OrderBy(o => o).Skip(i * size).Take(size).ToList();
                if (typeid.Trim() == "1")
                {

                    lstUnits.AddRange(db.v_Unit.Where(t => temp.Contains(t.ProvinceCode)).ToList());
                }
                else if (typeid.Trim() == "2")
                {
                    lstUnits.AddRange(db.v_Unit.Where(t => temp.Contains(t.DistrictCode)).ToList());
                }
                else if (typeid.Trim() == "3")
                {
                    lstUnits.AddRange(db.v_Unit.Where(t => temp.Contains(t.UnitCode)).ToList());
                }
            }
            return lstUnits;
        }

        public List<ProvinceWithCount> getListProvince(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC)
        {
            List<ProvinceWithCount> result = new List<ProvinceWithCount>();
            var lst = (from u in lstVUnit
                       where !lstPHNC.Any(p => p.UnitCode == u.UnitCode)
                       select new
                       {
                           ProvinceCode = u.ProvinceCode,
                           DistrictCode = u.DistrictCode
                       }).Distinct();
            result = (from u in lst
                      group u by u.ProvinceCode into g
                      select new ProvinceWithCount
                      {
                          CountDistrictNotConfig = g.Count(),
                          ProvinceCode = g.Key
                      }).ToList();

            List<Province> lstProvince = getAllProvince();
            result = (from p in result
                      join pr in lstProvince on p.ProvinceCode equals pr.ProvinceCode
                      select new ProvinceWithCount()
                      {
                          ProvinceName = pr.ProvinceName,
                          ProvinceCode = pr.ProvinceCode,
                          CountDistrict = pr.Districts.Count,
                          CountDistrictNotConfig = p.CountDistrictNotConfig
                      }).ToList();

            return result;
        }

        public List<ProvinceWithCount> getListProvinceToEdit(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC, string provincecode = null)
        {
            List<ProvinceWithCount> result = new List<ProvinceWithCount>();
            var lst = (from u in lstVUnit
                       where lstPHNC.Any(p => p.UnitCode == u.UnitCode)
                       select new
                       {
                           ProvinceCode = u.ProvinceCode,
                           DistrictCode = u.DistrictCode
                       }).Distinct();
            result = (from u in lst
                      group u by u.ProvinceCode into g
                      where g.Key.CompareTo(provincecode) == 0
                      select new ProvinceWithCount
                      {
                          CountDistrictNotConfig = g.Count(),
                          ProvinceCode = g.Key
                      }).ToList();

            List<Province> lstProvince = getAllProvince();
            result = (from p in result
                      join pr in lstProvince on p.ProvinceCode equals pr.ProvinceCode
                      select new ProvinceWithCount()
                      {
                          ProvinceName = pr.ProvinceName,
                          ProvinceCode = pr.ProvinceCode,
                          CountDistrict = pr.Districts.Count,
                          CountDistrictNotConfig = p.CountDistrictNotConfig
                      }).ToList();

            return result;
        }

        public List<DistrictWithCount> getListDistrict(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC, string[] lstids, bool type = true)
        {
            List<DistrictWithCount> result = new List<DistrictWithCount>();
            var lst = (from u in lstVUnit
                       where !lstPHNC.Any(p => p.UnitCode == u.UnitCode) == type
                       select new
                       {
                           UnitCode = u.UnitCode,
                           DistrictCode = u.DistrictCode
                       }).Distinct();
            result = (from u in lst
                      group u by u.DistrictCode into g
                      select new DistrictWithCount
                      {
                          CountUnitNotConfig = g.Count(),
                          DistrictCode = g.Key
                      }).ToList();

            List<District> lstDistrict = getDiStrictByArrayId(lstids);
            result = (from p in result
                      join pr in lstDistrict on p.DistrictCode equals pr.DistrictCode
                      select new DistrictWithCount()
                      {
                          DistrictName = pr.DistrictName,
                          DistrictCode = pr.DistrictCode,
                          ProvinceName = pr.Province.ProvinceName,
                          CountUnitNotConfig = p.CountUnitNotConfig
                      }).ToList();

            return result;
        }

        public List<v_Unit> getListUnit(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC, string[] lstids, bool type = true)
        {
            List<v_Unit> result = new List<v_Unit>();
            var lst = (from u in lstVUnit
                       where !lstPHNC.Any(p => p.UnitCode == u.UnitCode) == type
                       select new v_Unit
                       {
                           UnitCode = u.UnitCode,
                           UnitName = u.UnitName,
                           DistrictCode = u.DistrictCode,
                           DistrictName = u.DistrictName,
                       });

            result = lst.Where(t => lstids.Contains(t.DistrictCode)).ToList();
            return result;
        }

        public List<ProvinceWithCount> getListProvinceConfiged(List<v_Unit> lstVUnit, List<BPhanHuongNhuCau> lstPHNC)
        {
            //lấy province với thông tin district
            List<ProvinceWithCount> result = new List<ProvinceWithCount>();
            var lst = (from u in lstVUnit
                       where lstPHNC.Any(p => p.UnitCode == u.UnitCode)
                       select new ProvinceWithCode
                       {
                           ProvinceCode = u.ProvinceCode,
                           DistrictCode = u.DistrictCode,
                           DistrictName = u.DistrictName,
                           UnitCode = u.UnitCode
                       }).GroupBy(a => new { a.ProvinceCode, a.DistrictCode, a.DistrictName }, (key, g) => new DistrictWithUnit { DistrictCode = key.DistrictCode, ProvinceCode = key.ProvinceCode, DistrictName = key.DistrictName, ProvinceWithCode = g.ToList() });
            result = (from u in lst
                      group u by u.ProvinceCode into g
                      select new ProvinceWithCount
                      {
                          CountDistrictConfig = g.Count(),
                          DistrictWithUnit = g.ToList(),
                          ProvinceCode = g.Key
                      }).ToList();

            //join với bảng province để lấy các thông tin khác
            List<Province> lstProvince = getAllProvince();
            result = (from p in result
                      join pr in lstProvince on p.ProvinceCode equals pr.ProvinceCode
                      select new ProvinceWithCount()
                      {
                          ProvinceName = pr.ProvinceName,
                          ProvinceCode = pr.ProvinceCode,
                          DistrictWithUnit = p.DistrictWithUnit,
                          CountDistrict = pr.Districts.Count,
                          CountDistrictConfig = p.CountDistrictConfig
                      }).ToList();

            return result;
        }

        public List<Province> getAllProvince(string id = null)
        {
            List<Province> result = new List<Province>();
            if (string.IsNullOrWhiteSpace(id))
            {
                result = db.Provinces.Include(a => a.Districts).OrderBy(t => t.ProvinceCode).ToList();
            }
            else
            {
                result.Add(db.Provinces.Include(a => a.Districts).FirstOrDefault(x => x.ProvinceCode.CompareTo(id) == 0));
            }
            return result;
        }

        public List<District> getAllDistrict()
        {
            return db.Districts.Include(a => a.Province).ToList();
        }


        public List<v_Unit> getAllVunit()
        {
            return db.v_Unit.OrderBy(a => a.ProvinceCode).OrderBy(a => a.DistrictCode).ToList();
        }


        public List<BPhanHuongNhuCau> getAllPHNCByMaBaoNotConfig(string Mabao)
        {
            return db.BPhanHuongNhuCaus.Include(t=>t.BDiemTiepNhan).Where(a => (!(Mabao == null || Mabao.Trim() == string.Empty) && a.ThongTinBaoId == null) || a.ThongTinBaoId.Equals(Mabao)).ToList();
        }
        public List<BPhanHuongNhuCau> getAllPHNCByMaBaoConfig(string Mabao)
        {
            return db.BPhanHuongNhuCaus.Include(t => t.BDiemTiepNhan).Where(a => ((Mabao == null || Mabao.Trim() == string.Empty) && a.ThongTinBaoId == null) || a.ThongTinBaoId.Equals(Mabao)).ToList();
        }

        public void Dispose()
        {
            lstUnit = null;
            db.Dispose();
        }


    }
}
