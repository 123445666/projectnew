using PHBC.DAO.Common;
using PHBC.DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    /// <summary>
    /// created : 23/07/2015
    /// Author : vietvb
    /// </summary>
    public interface IBDiemTiepNhanBussiness
    {
        List<BDiemTiepNhanModel> getAllModel();
        BDiemTiepNhanModel getModelById(string id);
        int Add(BDiemTiepNhanModel bDiemTiepNhanModel);
        int Update(BDiemTiepNhanModel bDiemTiepNhanModel);
        int Delete(string id);
        void Dispose();
        
    }
}
