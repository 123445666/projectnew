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
    /// created : 28/07/2015
    /// Author : vietvb
    /// </summary>
    public class UnitBussiness : IUnitBussiness
    {
        private DB_PHBCEntities db;
        public UnitBussiness()
        {
            this.db = new DB_PHBCEntities();
        }
        public UnitBussiness(DB_PHBCEntities _db)
        {
            this.db = _db;
        }
        public List<UnitModel2> getAll()
        {
            List<UnitModel2> lst = new List<UnitModel2>();
            lst = BuildQuery(null).ToList();
            return lst;
        }        

        public List<UnitModel2> getSearch(UnitModel2Search objSearch)
        {
            List<UnitModel2> lst = new List<UnitModel2>();
            lst = BuildQuery(objSearch).ToList();
            return lst;
        }

        public List<UnitModel2> getAllPager(int page, int pageSize, out int pageCount)
        {
            return Utils.buildPage(BuildQuery(null), page, ref pageSize, out pageCount);
        }

        public List<UnitModel2> getSearchPager(UnitModel2Search objSearch, int page, int pageSize, out int pageCount)
        {
            return Utils.buildPage(BuildQuery(objSearch), page, ref pageSize, out pageCount);
        }

        private IQueryable<UnitModel2> BuildQuery(UnitModel2Search search, List<string> lstInclude = null)
        {
            string include = string.Empty;
            IQueryable<Unit> qUnit = db.Units;
            //Neu co include thi them include vao 
            if (lstInclude != null && lstInclude.Count > 0)
                foreach (string item in lstInclude)
                    qUnit = qUnit.Include(item);

            IQueryable<UnitModel2> query = (from u in qUnit
                                            select new UnitModel2()
                                            {
                                                UnitCode = u.UnitCode,
                                                UnitName = u.UnitName,
                                                ParentUnitCode = u.ParentUnitCode,
                                                CommuneCode = u.CommuneCode,
                                                UnitTypeCode = u.UnitTypeCode,
                                                Commune = u.Commune,
                                                POS = u.POS,
                                                Unit1 = u.Unit1,
                                                Unit2 = u.Unit2,
                                                UnitType = u.UnitType,
                                                UserInfoes = u.UserInfoes,
                                                BDiemTiepNhans = u.BDiemTiepNhans,
                                                BPhanHuongNhuCaus = u.BPhanHuongNhuCaus,
                                                BThongTinBaos = u.BThongTinBaos,
                                                BDieuChinhPhanHuongUnits = u.BDieuChinhPhanHuongUnits
                                            });
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.UnitCode))
                    query = query.Where(a => a.UnitCode.Contains(search.UnitCode));
                if (!string.IsNullOrWhiteSpace(search.UnitName))
                    query = query.Where(a => a.UnitName.Contains(search.UnitName));
            }
            return query;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
