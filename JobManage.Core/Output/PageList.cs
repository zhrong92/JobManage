using System;
using System.Collections.Generic;
using System.Text;

namespace JobManage.Core
{
    public class PageList<T>
    {
        public List<T> Datas { get; set; }
        public int TotalCount { get; set; }
    }
}
