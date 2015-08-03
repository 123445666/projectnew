using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Bussiness
{
    public class Export : IExport
    {
        public bool ExportExcelNPOI(FileStream fs)
        {
            bool result = false;
            HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);
            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);
            result = true;

            return result;
        }
    }
}
