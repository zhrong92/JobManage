using System;
using System.Collections.Generic;
using System.Text;

namespace JobManage.Core
{
    public class ApiResult
    {
        public ApiResult(bool succ, int statusCode, string msg, object data = null, object exted = null)
        {
            this.succ = succ;
            this.statusCode = statusCode;
            this.msg = msg;
            this.data = data;
            this.exted = exted;
            this.time = DateTime.Now;
        }

        public bool succ { get; set; } = true;
        public int statusCode { get; set; }
        public object data { get; set; }
        public object exted { get; set; }
        public string msg { get; set; }
        public DateTime time { get; set; }
    }
}
