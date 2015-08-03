using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PHBC.DAO.Models;

namespace PHBC.DAO.Bussiness
{
    /// <summary>
    /// created : 28/07/2015
    /// Author : vietvb
    /// </summary>
    public interface IUnitBussiness
    {
        /// <summary>
        /// get all unit from db
        /// </summary>
        /// <returns></returns>
        List<UnitModel2> getAll();
        /// <summary>
        /// get all unit with pager
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        List<UnitModel2> getAllPager(int page, int pageSize, out int pageCount);
        /// <summary>
        /// get all unit with search
        /// </summary>
        /// <param name="objSearch">UnitModel2Search</param>
        /// <returns></returns>
        List<UnitModel2> getSearch(UnitModel2Search objSearch);
        /// <summary>
        /// get all unit with search and pager
        /// </summary>
        /// <param name="objSearch">UnitModel2Search</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        List<UnitModel2> getSearchPager(UnitModel2Search objSearch, int page, int pageSize, out int pageCount);
        /// <summary>
        /// Dispose
        /// </summary>
        void Dispose();
    }
}
