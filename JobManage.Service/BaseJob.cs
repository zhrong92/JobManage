using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class BaseJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var str = context.JobDetail.JobDataMap.GetString("requestUrl");
            await WriteLog(str);
        }

        public async Task WriteLog(string title)
        {
            //文件存放目录
            string logpath = "d://LogFiles";
            string logfilename = logpath + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            //路径不存在 则声称路径
            if (!System.IO.Directory.Exists(logpath))
            {
                System.IO.Directory.CreateDirectory(logpath);
            }
            //写入文件内容
            string strlog = DateTime.Now + " " + title + " " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss.fff") + "\r\n";
            try
            {
                await System.IO.File.AppendAllTextAsync(logfilename, strlog);
            }
            catch
            {

            }
        }
    }
}
