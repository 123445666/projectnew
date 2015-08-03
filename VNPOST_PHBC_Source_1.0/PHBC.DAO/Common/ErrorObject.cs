using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Common
{
    public class ErrorObject
    {
        public ErrorObject()
        {
            HasError = false;
            LstError = new Dictionary<string, string>();
        }
        public bool HasError { get; set; }

        public Dictionary<string, string> LstError { get; set; }
    }
}
