using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public interface ISysActionBussiness
    {
        //bool UpdateActions(List<SysAction> lstAdd, List<SysAction> lstRemove, List<SysAction> lstModify, List<SysAction> lstChangeMenu);
        int UpdateActions(List<SysAction> lstAction);


    }
}
