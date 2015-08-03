using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHBC.DAO.Common
{
    public class ReturnJson
    {
        public ReturnJson()
        {

        }
        public ReturnJson(string _Msg, string _titleMsg = "", string _backLinh = "", int _status = 1)
        {
            this.Status = _status;
            this.MSG = _Msg;
            this.TitleMsg = _titleMsg;
            this.BackLink = _backLinh;
        }
        public int Status { set; get; }
        public string BackLink { set; get; }
        public string TitleMsg { set; get; }
        public string MSG { set; get; }

        public string toJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
