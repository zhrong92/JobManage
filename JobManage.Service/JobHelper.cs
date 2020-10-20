using System;
using System.Collections.Generic;
using System.Text;

namespace JobManage.Service
{
    public class JobHelper
    {
        #region 基础任务分组名称
        public static string BaseJobGroup = "JobGroup_Base";
        public static string BaseJobName = "JobName_Base";
        public static string BaseTriggerGroup = "TriggerGroup_Base";
        public static string BaseTriggerName = "TriggerName_Base";
        #endregion

        /// <summary>
        /// 是否是基础任务
        /// </summary>
        /// <param name="group"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsBaseJob(string group, string name) =>
            string.Equals(group, BaseJobGroup) && string.Equals(name, BaseJobName);
    }
}
