using System;
using System.Collections.Generic;
using System.Text;

namespace JobManage.Core
{
    public class JobAddInput
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public string Description { get; set; }
        public string RequestUrl { get; set; }
    }
}
