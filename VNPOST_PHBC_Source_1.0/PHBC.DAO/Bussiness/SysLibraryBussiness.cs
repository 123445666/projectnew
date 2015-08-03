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
    public class SysLibraryBussiness : ISysLibraryBussiness
    {
        private UnitBussiness objUnitBussiness = new UnitBussiness();       
        public List<UnitModel2> getAllUnitModel()
        {
            return objUnitBussiness.getAll();
        }

        public List<UnitModel2> getUnitModelSearch(UnitModel2Search objSearch)
        {
            return objUnitBussiness.getSearch(objSearch);
        }

        public List<UnitModel2> getAllUnitModelPager(int page, int pageSize, out int pageCount)
        {
            return objUnitBussiness.getAllPager(page, pageSize,out pageCount);
        }

        public List<UnitModel2> getUnitModelSearchPager(UnitModel2Search objSearch, int page, int pageSize, out int pageCount)
        {
            return objUnitBussiness.getSearchPager(objSearch, page, pageSize,out pageCount);
        }
        
        public void Dispose()
        {
            objUnitBussiness.Dispose();
        }
    }
}
