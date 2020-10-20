using System;
using Topshelf;

namespace JobManage.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.SetServiceName("JobService");
                c.SetDisplayName("JobService");
                c.SetDescription("任务管理服务");

                c.Service<TopshelfService>(s =>
                {
                    s.ConstructUsing(b => new TopshelfService());
                    s.WhenStarted(o => o.Start());
                    s.WhenStopped(o => o.Stop());
                });

                c.StartAutomatically();
                c.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(10)));
            }
            );
        }
    }
}
