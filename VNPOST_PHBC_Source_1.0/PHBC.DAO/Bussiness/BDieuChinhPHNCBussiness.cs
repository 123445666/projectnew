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
    /// <summary>
    /// created : 23/07/2015
    /// Author : vietvb
    /// </summary>
    public class BDieuChinhPHNCBussiness : IBDieuChinhPHNCBussiness
    {
        List<Unit> lstUnit;
        private DB_PHBCEntities db;

        /*
         * constructor function
         * */
        public BDieuChinhPHNCBussiness()
        {
            db = new DB_PHBCEntities();
        }
        public BDieuChinhPHNCBussiness(DB_PHBCEntities _db)
        {
            db = _db;
        }

        public List<BDieuChinhPhanHuongUnit> getAll()
        {
            //var query3 = db.BDiemTiepNhans.Include(a => a.BDieuChinhPhanHuongUnits)
            //    .Include("BDieuChinhPhanHuongUnits.Unit").ToList();
            //string s = "1";
            //var query1 = from a in query3
            //             select new BDiemTiepNhanModel
            //             {
            //                 Id = a.Id,
            //                 Name = a.Name,
            //                 Units = a.BDieuChinhPhanHuongUnits.Where(d=>(string.IsNullOrWhiteSpace(s) && d.DieuChinhKHXBDetailId==null) || d.DieuChinhKHXBDetailId == s).Select(b => b.Unit).Distinct()
            //             };

            //var query = db.BDieuChinhPhanHuongUnits.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit).GroupBy(b => b.DiemTiepNhanId);
            //var query2 = from p in db.BDieuChinhPhanHuongUnits.Include(a => a.BDiemTiepNhan).GroupBy(a => a.BDiemTiepNhan)
            //                 select p
            //            ;
            List<BDieuChinhPhanHuongUnit> BDieuChinhPhanHuongUnits = db.BDieuChinhPhanHuongUnits.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit).ToList();
            return BDieuChinhPhanHuongUnits;
        }

        public List<BDieuChinhPhanHuongUnitModel> getAllModelPager(int page, ref int pageSize, out int pageCount, out int totalitem, string DieuChinhKHXBDetailId = null)
        {
            List<BDieuChinhPhanHuongUnitModel> result = new List<BDieuChinhPhanHuongUnitModel>();
            IQueryable<BDieuChinhPhanHuongUnit> lstPH;
            if (string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
            {
                lstPH = db.BDieuChinhPhanHuongUnits.Where(a => a.DieuChinhKHXBDetailId == null);
            }
            else
            {
                lstPH = db.BDieuChinhPhanHuongUnits.Where(a => a.DieuChinhKHXBDetailId.Equals(DieuChinhKHXBDetailId));
            }

            IQueryable<BDieuChinhPhanHuongUnitModel> query = (from t in lstPH.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit)
                                                              join u in db.v_Unit on t.UnitCode equals u.UnitCode
                                                              join k in db.Provinces on u.ProvinceCode equals k.ProvinceCode
                                                              select new BDieuChinhPhanHuongUnitModel()
                                                              {
                                                                  Id = t.Id,
                                                                  UnitCode = t.UnitCode,
                                                                  UnitName = t.Unit.UnitName,
                                                                  DiemTiepNhanId = t.DiemTiepNhanId,
                                                                  TenDiemTiepNhan = t.BDiemTiepNhan.Name,
                                                                  DieuChinhKHXBDetailId = t.DieuChinhKHXBDetailId,
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
            result = Utils.buildPage<BDieuChinhPhanHuongUnitModel>(query, page, ref pageSize, out totalitem, out pageCount);
            return result;
        }
        public List<BDiemTiepNhanModel> getAllData(string currentunit, string DieuChinhKHXBDetailId = null)
        {
            var query3 = db.BDiemTiepNhans.Include(a => a.BDieuChinhPhanHuongUnits)
               .Include("BDieuChinhPhanHuongUnits.Unit").ToList();
            //IQueryable<string> lstUnit2 = getChildUnitCode(currentunit);
            lstUnit = getChildUnit(currentunit);
            var query = from a in query3
                        select new BDiemTiepNhanModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Units = a.BDieuChinhPhanHuongUnits.Where(d => ((string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId) && d.DieuChinhKHXBDetailId == null) || d.DieuChinhKHXBDetailId == DieuChinhKHXBDetailId) && lstUnit.Select(x => x.UnitCode).Contains(d.UnitCode)).OrderBy(d => d.UnitCode).Select(b => b.Unit).Distinct()
                        };
            return query.ToList();
        }

        public List<BDieuChinhPhanHuongUnit> getAll(int page, int pageSize, out int pageCount)
        {
            throw new NotImplementedException();
        }

        public List<BDieuChinhPhanHuongUnitModel> getAllDieuChinhPHUnitModel(int page, int pageSize, out int pageCount, out int totalitem)
        {
            List<BDieuChinhPhanHuongUnitModel> result = new List<BDieuChinhPhanHuongUnitModel>();
            int count = db.BDieuChinhPhanHuongUnits.Count();
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
            result = (from t in db.BDieuChinhPhanHuongUnits.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit)
                      select new BDieuChinhPhanHuongUnitModel()
                      {
                          Id = t.Id,
                          UnitCode = t.UnitCode,
                          DiemTiepNhanId = t.DiemTiepNhanId,
                          DieuChinhKHXBDetailId = t.DieuChinhKHXBDetailId,
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

        public List<BDieuChinhPhanHuongUnitModel> searchModel(DMToaSoanSearchModel obj, int page, int pageSize, out int pageCount, out int totalitem)
        {
            throw new NotImplementedException();
        }

        public List<BDieuChinhPhanHuongUnitModel> searchModel(DMToaSoanSearchModel obj)
        {
            throw new NotImplementedException();
        }

        public BDieuChinhPhanHuongUnit getById(string id)
        {
            return db.BDieuChinhPhanHuongUnits.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit).FirstOrDefault(x => x.Id == id);
        }
        /// <summary>
        /// Function check PHBC
        /// ErrorObject with:
        /// - check1: value 1: lỗi trùng unitcode và điểm tiếp nhận
        /// </summary>
        /// <param name="UnitCode">Mã bưu cục</param>
        /// <param name="DieuChinhKHXBDetailId">Mã báo</param>
        /// <param name="DiemTiepNhanId">Mã điểm tiếp nhận</param>
        /// <returns>ErrorObject</returns>
        public ErrorObject checkPHBC(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit)
        {
            ErrorObject err = new ErrorObject();
            if (string.IsNullOrWhiteSpace(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId))
            {
                if (db.BDieuChinhPhanHuongUnits.Any(r => r.UnitCode.CompareTo(BDieuChinhPhanHuongUnit.UnitCode.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BDieuChinhPhanHuongUnit.DiemTiepNhanId.Trim()) == 0 && r.DieuChinhKHXBDetailId == null))
                {
                    err.HasError = true;
                    err.LstError.Add("Check1", "1"); // trùng giá trị unit code , diem tiep nhan id
                }
            }
            else
            {
                if (db.BDieuChinhPhanHuongUnits.Any(r => r.DieuChinhKHXBDetailId.CompareTo(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BDieuChinhPhanHuongUnit.DiemTiepNhanId.Trim()) == 0 && r.UnitCode.CompareTo(BDieuChinhPhanHuongUnit.UnitCode.Trim()) == 0))
                {
                    err.HasError = true;
                    err.LstError.Add("Check3", "3"); // trùng cả 3 giá trị
                }
            }
            //if (db.BDieuChinhPhanHuongUnits.Any(r => r.DieuChinhKHXBDetailId.CompareTo(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BDieuChinhPhanHuongUnit.DiemTiepNhanId.Trim()) == 0))
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
        /// <param name="DieuChinhKHXBDetailId">Mã báo</param>
        /// <param name="DiemTiepNhanId">Mã điểm tiếp nhận</param>
        /// <returns>
        /// ErrorObject
        /// - check1: value 1: lỗi trùng unitcode và điểm tiếp nhận
        /// </returns>
        private ErrorObject checkPHBCNew(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit)
        {
            ErrorObject err = new ErrorObject();
            List<string> units = new List<string>();
            List<BDieuChinhPhanHuongUnit> lstPH;
            BDieuChinhPhanHuongUnit obj;

            if (string.IsNullOrWhiteSpace(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId))
            {
                lstPH = db.BDieuChinhPhanHuongUnits.Where(a => a.DieuChinhKHXBDetailId == null).ToList();
            }
            else
            {
                lstPH = db.BDieuChinhPhanHuongUnits.Where(a => a.DieuChinhKHXBDetailId.Equals(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId)).ToList();
            }
            foreach (string item in units)
            {
                obj = lstPH.FirstOrDefault(a => a.UnitCode.Equals(item));
                if (obj == null)
                {
                    obj = new BDieuChinhPhanHuongUnit();
                    obj.UnitCode = item;
                    obj.DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
                    obj.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                    obj.CreateBy = BDieuChinhPhanHuongUnit.CreateBy;
                    obj.CreateDate = BDieuChinhPhanHuongUnit.CreateDate;
                    db.BDieuChinhPhanHuongUnits.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BDieuChinhPhanHuongUnit.ModifyBy;
                    obj.ModifyDate = BDieuChinhPhanHuongUnit.ModifyDate;
                    obj.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                }
            }
            db.SaveChanges();
            //if (db.BDieuChinhPhanHuongUnits.Any(r => r.DieuChinhKHXBDetailId.CompareTo(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId.Trim()) == 0 && r.DiemTiepNhanId.CompareTo(BDieuChinhPhanHuongUnit.DiemTiepNhanId.Trim()) == 0))
            //{
            //    err.HasError = true;
            //    err.LstError.Add("Check2", "2"); // trùng giá trị ma bao, diem tiep nhan id
            //}


            return err;
        }
        public int Add(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit, List<v_Unit> units)
        {
            //new insert function
            List<BDieuChinhPhanHuongUnit> lstPH;
            BDieuChinhPhanHuongUnit obj;
            lstPH = db.BDieuChinhPhanHuongUnits.Where(a => a.DieuChinhKHXBDetailId.Equals(BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId)).ToList();
            BDieuChinhKHXBDetail objDetail = db.BDieuChinhKHXBDetails.Include(t => t.BDieuChinhKHXB).FirstOrDefault(x => x.Id == BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId);
            BDieuChinhPhanHuongUnit.SoBao = objDetail.SoBao;
            BDieuChinhPhanHuongUnit.Nam = objDetail.BDieuChinhKHXB.Nam;
            BDieuChinhPhanHuongUnit.Quy = objDetail.BDieuChinhKHXB.Quy;
            BDieuChinhPhanHuongUnit.ThongTinBaoId = objDetail.BDieuChinhKHXB.ThongTinBaoId;
            foreach (v_Unit item in units)
            {
                obj = lstPH.FirstOrDefault(a => a.UnitCode.Equals(item.UnitCode));
                if (obj == null)
                {
                    obj = new BDieuChinhPhanHuongUnit();
                    obj.Id = item.UnitCode + BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
                    //obj.Id = item.UnitCode + BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
                    obj.UnitCode = item.UnitCode;
                    obj.DieuChinhKHXBDetailId = BDieuChinhPhanHuongUnit.DieuChinhKHXBDetailId;
                    obj.ThongTinBaoId = BDieuChinhPhanHuongUnit.ThongTinBaoId;
                    obj.SoBao = BDieuChinhPhanHuongUnit.SoBao;
                    obj.Nam = BDieuChinhPhanHuongUnit.Nam;
                    obj.Quy = BDieuChinhPhanHuongUnit.Quy;
                    obj.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                    obj.CreateBy = BDieuChinhPhanHuongUnit.CreateBy;
                    obj.CreateDate = BDieuChinhPhanHuongUnit.CreateDate;
                    db.BDieuChinhPhanHuongUnits.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BDieuChinhPhanHuongUnit.ModifyBy;
                    obj.ModifyDate = BDieuChinhPhanHuongUnit.ModifyDate;
                    obj.DiemTiepNhanId = BDieuChinhPhanHuongUnit.DiemTiepNhanId;
                }
            }
            return db.SaveChanges();
        }

        public int AddDistrict(BDieuChinhPhanHuongDistrict BDieuChinhPhanHuongDistrict, List<QuanHuyen> District)
        {
            List<BDieuChinhPhanHuongDistrict> lstPH;
            BDieuChinhPhanHuongDistrict obj;
            //lstPH = db.BDieuChinhPhanHuongDistricts.Where(a => a.DieuChinhKHXBDetailId.Equals(BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId)).ToList();
            lstPH = db.BDieuChinhPhanHuongDistricts.Where(a => a.DieuChinhKHXBDetailId.Equals(BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId)).ToList();
            BDieuChinhKHXBDetail objDetail = db.BDieuChinhKHXBDetails.Include(t => t.BDieuChinhKHXB).FirstOrDefault(x => x.Id == BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId);
            BDieuChinhPhanHuongDistrict.SoBao = objDetail.SoBao;
            BDieuChinhPhanHuongDistrict.Nam = objDetail.BDieuChinhKHXB.Nam;
            BDieuChinhPhanHuongDistrict.Quy = objDetail.BDieuChinhKHXB.Quy;
            BDieuChinhPhanHuongDistrict.ThongTinBaoId = objDetail.BDieuChinhKHXB.ThongTinBaoId;
            foreach (QuanHuyen item in District)
            {
                obj = lstPH.FirstOrDefault(a => a.DistrictCode.Equals(item.DistrictCode));
                if (obj == null)
                {
                    obj = new BDieuChinhPhanHuongDistrict();
                    obj.Id = item.DistrictCode + BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId;
                    //obj.Id = item.DistrictCode + BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId;
                    obj.DistrictCode = item.DistrictCode;
                    obj.DieuChinhKHXBDetailId = BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId;
                    obj.ThongTinBaoId = BDieuChinhPhanHuongDistrict.ThongTinBaoId;
                    obj.SoBao = BDieuChinhPhanHuongDistrict.SoBao;
                    obj.Nam = BDieuChinhPhanHuongDistrict.Nam;
                    obj.Quy = BDieuChinhPhanHuongDistrict.Quy;
                    obj.DiemTiepNhanId = BDieuChinhPhanHuongDistrict.DiemTiepNhanId;
                    obj.CreateBy = BDieuChinhPhanHuongDistrict.CreateBy;
                    obj.CreateDate = BDieuChinhPhanHuongDistrict.CreateDate;
                    db.BDieuChinhPhanHuongDistricts.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BDieuChinhPhanHuongDistrict.ModifyBy;
                    obj.ModifyDate = BDieuChinhPhanHuongDistrict.ModifyDate;
                    obj.DiemTiepNhanId = BDieuChinhPhanHuongDistrict.DiemTiepNhanId;
                }
            }
            return db.SaveChanges();
        }
        public int Add(BPhanHuongNhuCauUnit BPhanHuongNhuCauUnit, List<v_Unit> units)
        {
            //new insert function
            List<BPhanHuongNhuCauUnit> lstPH;
            BPhanHuongNhuCauUnit obj;
            lstPH = db.BPhanHuongNhuCauUnits.Where(a => a.ThongTinBaoId.Equals(BPhanHuongNhuCauUnit.ThongTinBaoId)).ToList();     
            foreach (v_Unit item in units)
            {
                obj = lstPH.FirstOrDefault(a => a.UnitCode.Equals(item.UnitCode));
                if (obj == null)
                {
                    obj = new BPhanHuongNhuCauUnit();
                    obj.Id = item.UnitCode + BPhanHuongNhuCauUnit.ThongTinBaoId;
                    obj.UnitCode = item.UnitCode;
                    obj.ThongTinBaoId = BPhanHuongNhuCauUnit.ThongTinBaoId;
                    obj.DiemTiepNhanId = BPhanHuongNhuCauUnit.DiemTiepNhanId;
                    obj.CreateBy = BPhanHuongNhuCauUnit.CreateBy;
                    obj.CreateDate = BPhanHuongNhuCauUnit.CreateDate;
                    db.BPhanHuongNhuCauUnits.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BPhanHuongNhuCauUnit.ModifyBy;
                    obj.ModifyDate = BPhanHuongNhuCauUnit.ModifyDate;
                    obj.DiemTiepNhanId = BPhanHuongNhuCauUnit.DiemTiepNhanId;
                }
            }
            return db.SaveChanges();
        }

        public int AddDistrict(BPhanHuongNhuCauDistrict BPhanHuongNhuCauDistrict, List<QuanHuyen> District)
        {
            List<BPhanHuongNhuCauDistrict> lstPH;
            BPhanHuongNhuCauDistrict obj;
            //lstPH = db.BDieuChinhPhanHuongDistricts.Where(a => a.DieuChinhKHXBDetailId.Equals(BDieuChinhPhanHuongDistrict.DieuChinhKHXBDetailId)).ToList();
            lstPH = db.BPhanHuongNhuCauDistricts.Where(a => a.ThongTinBaoId.Equals(BPhanHuongNhuCauDistrict.ThongTinBaoId)).ToList();
            foreach (QuanHuyen item in District)
            {
                obj = lstPH.FirstOrDefault(a => a.DistrictCode.Equals(item.DistrictCode));
                if (obj == null)
                {
                    obj = new BPhanHuongNhuCauDistrict();
                    obj.Id = item.DistrictCode + BPhanHuongNhuCauDistrict.ThongTinBaoId;
                    obj.DistrictCode = item.DistrictCode;
                    obj.ThongTinBaoId = BPhanHuongNhuCauDistrict.ThongTinBaoId;
                    obj.DiemTiepNhanId = BPhanHuongNhuCauDistrict.DiemTiepNhanId;
                    obj.CreateBy = BPhanHuongNhuCauDistrict.CreateBy;
                    obj.CreateDate = BPhanHuongNhuCauDistrict.CreateDate;
                    db.BPhanHuongNhuCauDistricts.Add(obj);
                }
                else
                {
                    obj.ModifyBy = BPhanHuongNhuCauDistrict.ModifyBy;
                    obj.ModifyDate = BPhanHuongNhuCauDistrict.ModifyDate;
                    obj.DiemTiepNhanId = BPhanHuongNhuCauDistrict.DiemTiepNhanId;
                }
            }
            return db.SaveChanges();
        }
        public int Update(BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit)
        {
            db.Entry(BDieuChinhPhanHuongUnit).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public int Delete(string id)
        {
            BDieuChinhPhanHuongUnit BDieuChinhPhanHuongUnit = db.BDieuChinhPhanHuongUnits.Find(id);
            db.BDieuChinhPhanHuongUnits.Remove(BDieuChinhPhanHuongUnit);
            return db.SaveChanges();
        }
        private IQueryable<BDieuChinhPhanHuongUnitModel> BuildQuery(BDieuChinhPhanHuongUnitSearchModel search, bool fInclude = false)
        {
            string include = string.Empty;
            IQueryable<BDieuChinhPhanHuongUnit> qPHNC;
            if (fInclude)
                qPHNC = db.BDieuChinhPhanHuongUnits.Include(b => b.BDiemTiepNhan).Include(b => b.BThongTinBao).Include(b => b.Unit);
            else
                qPHNC = db.BDieuChinhPhanHuongUnits;
            IQueryable<BDieuChinhPhanHuongUnitModel> query = (from t in qPHNC
                                                              select new BDieuChinhPhanHuongUnitModel()
                                                              {
                                                                  Id = t.Id,
                                                                  UnitCode = t.UnitCode,
                                                                  DiemTiepNhanId = t.DiemTiepNhanId,
                                                                  DieuChinhKHXBDetailId = t.DieuChinhKHXBDetailId,
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
                if (!string.IsNullOrWhiteSpace(search.DieuChinhKHXBDetailId))
                    query = query.Where(a => a.DieuChinhKHXBDetailId.CompareTo(search.DieuChinhKHXBDetailId) == 0);
            }
            return query;
        }


        public List<Unit> getAllUnitNotInPhnc(string currentunit, string BDieuChinhKHXBDetailId = null)
        {
            List<BDieuChinhPhanHuongUnit> phanhuong;
            if (string.IsNullOrWhiteSpace(BDieuChinhKHXBDetailId))
                phanhuong = db.BDieuChinhPhanHuongUnits.Where(t => t.DieuChinhKHXBDetailId == null).ToList();
            else
                phanhuong = db.BDieuChinhPhanHuongUnits.Where(t => t.DieuChinhKHXBDetailId == BDieuChinhKHXBDetailId).ToList();
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


        public List<Unit> getAllUnitByDTNId(string currentunit, string DiemTiepNhanId, string BDieuChinhKHXBDetailId = null)
        {
            List<BDieuChinhPhanHuongUnit> phanhuong;
            if (string.IsNullOrWhiteSpace(BDieuChinhKHXBDetailId))
                phanhuong = db.BDieuChinhPhanHuongUnits.Where(t => t.DieuChinhKHXBDetailId == null).Include(a => a.Unit).ToList();
            else
                phanhuong = db.BDieuChinhPhanHuongUnits.Where(t => t.DieuChinhKHXBDetailId == BDieuChinhKHXBDetailId).Include(a => a.Unit).Include(a => a.BThongTinBao).ToList();

            //IQueryable<string> lstUnit = getChildUnitCode(currentunit);
            lstUnit = getChildUnit(currentunit);
            List<BDieuChinhPhanHuongUnit> lstPHNC = db.BDieuChinhPhanHuongUnits.ToList();
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
        public BThongTinBao getThongTinBaoById(string id)
        {
            return db.BThongTinBaos.Find(id);
        }
        public BDieuChinhKHXBDetail getDieuChinhDetailById(string id)
        {
            return db.BDieuChinhKHXBDetails.Include(t => t.BDieuChinhKHXB).Include(t => t.BDieuChinhKHXB.BThongTinBao).SingleOrDefault(x => x.Id == id);
        }
        public List<Unit> getAllUnit()
        {
            return db.Units.ToList();
        }

        public BDieuChinhPhanHuongUnit getByUnit(string UnitCode, string DieuChinhKHXBDetailId = null)
        {
            if (string.IsNullOrWhiteSpace(DieuChinhKHXBDetailId))
            {
                return db.BDieuChinhPhanHuongUnits.FirstOrDefault(t => t.UnitCode.CompareTo(UnitCode) == 0 && t.DieuChinhKHXBDetailId == null);
            }
            else
                return db.BDieuChinhPhanHuongUnits.FirstOrDefault(t => t.UnitCode.CompareTo(UnitCode) == 0 && t.DieuChinhKHXBDetailId == DieuChinhKHXBDetailId);
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

        public List<ProvinceWithCount> getListProvince(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC)
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

        public List<ProvinceWithCount> getListProvinceToEdit(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC, string provincecode = null)
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

        public List<DistrictWithCount> getListDistrict(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC, string[] lstids, bool type = true)
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

        public List<v_Unit> getListUnit(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC, string[] lstids, bool type = true)
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

        public List<ProvinceWithCount> getListProvinceConfiged(List<v_Unit> lstVUnit, List<BDieuChinhPhanHuongUnit> lstPHNC)
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


        public List<BDieuChinhPhanHuongUnit> getAllPHNCByDieuChinhKHXBDetailIdNotConfig(string DieuChinhKHXBDetailId)
        {
            return db.BDieuChinhPhanHuongUnits.Include(t => t.BDiemTiepNhan).Where(a => (!(DieuChinhKHXBDetailId == null || DieuChinhKHXBDetailId.Trim() == string.Empty) && a.DieuChinhKHXBDetailId == null) || a.DieuChinhKHXBDetailId.Equals(DieuChinhKHXBDetailId)).ToList();
        }
        public List<BDieuChinhPhanHuongUnit> getAllPHNCByDieuChinhKHXBDetailIdConfig(string DieuChinhKHXBDetailId)
        {
            return db.BDieuChinhPhanHuongUnits.Include(t => t.BDiemTiepNhan).Where(a => ((DieuChinhKHXBDetailId == null || DieuChinhKHXBDetailId.Trim() == string.Empty) && a.DieuChinhKHXBDetailId == null) || a.DieuChinhKHXBDetailId.Equals(DieuChinhKHXBDetailId)).ToList();
        }
        public List<BDieuChinhPhanHuongDistrict> getAllDistrictPHNCByDieuChinhKHXBDetailIdConfig(string DieuChinhKHXBDetailId)
        {
            return db.BDieuChinhPhanHuongDistricts.Include(t => t.BDiemTiepNhan).Where(a => ((DieuChinhKHXBDetailId == null || DieuChinhKHXBDetailId.Trim() == string.Empty) && a.DieuChinhKHXBDetailId == null) || a.DieuChinhKHXBDetailId.Equals(DieuChinhKHXBDetailId)).ToList();
        }

        public List<BPhanHuongNhuCauUnit> getAllPHNCByThongTinBaoIdNotConfig(string ThongTinBaoId = null)
        {
            return db.BPhanHuongNhuCauUnits.Include(t => t.BDiemTiepNhan).Where(a => (!(ThongTinBaoId == null || ThongTinBaoId.Trim() == string.Empty) && a.ThongTinBaoId == null) || a.ThongTinBaoId.Equals(ThongTinBaoId)).ToList();
        }
        public List<BPhanHuongNhuCauUnit> getAllPHNCByThongTinBaoIdConfig(string ThongTinBaoId = null)
        {
            return db.BPhanHuongNhuCauUnits.Include(t => t.BDiemTiepNhan).Where(a => ((ThongTinBaoId == null || ThongTinBaoId.Trim() == string.Empty) && a.ThongTinBaoId == null) || a.ThongTinBaoId.Equals(ThongTinBaoId)).ToList();
        }
        public List<BPhanHuongNhuCauDistrict> getAllDistrictPHNCByThongTinBaoIdConfig(string ThongTinBaoId = null)
        {
            return db.BPhanHuongNhuCauDistricts.Include(t => t.BDiemTiepNhan).Where(a => ((ThongTinBaoId == null || ThongTinBaoId.Trim() == string.Empty) && a.ThongTinBaoId == null) || a.ThongTinBaoId.Equals(ThongTinBaoId)).ToList();
        }

        public void Dispose()
        {
            lstUnit = null;
            db.Dispose();
        }


    }
}
