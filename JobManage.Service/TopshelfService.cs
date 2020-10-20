using JobManage.Core;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class TopshelfService
    {
        private IScheduler _scheduler;

        public async Task Start()
        {
            IServiceProvider serviceProvider = GetServiceProvider();

            ISchedulerFactory sf = new StdSchedulerFactory();
            _scheduler = await sf.GetScheduler();
            await _scheduler.Start();

            JobListener listener = serviceProvider.GetService<JobListener>();
            _scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());//注册监听器

            _scheduler.JobFactory = serviceProvider.GetService<JobFactory>();

            IJobDetail job = JobBuilder.Create<JobService>()
                .WithIdentity(JobHelper.BaseJobName, JobHelper.BaseJobGroup)
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(JobHelper.BaseTriggerName, JobHelper.BaseTriggerGroup)
              .StartNow()
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(60)
                  .RepeatForever())
              .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public void Stop()
        {
            _scheduler.Shutdown(false);
        }

        public void Continue()
        {
            _scheduler.ResumeAll();
        }

        public void Pause()
        {
            _scheduler.PauseAll();
        }

        private IServiceProvider GetServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            //services.AddHttpClient();
            //services.AddGLHttp();

            services.AddCore("mongodb://127.0.0.1:27017", "db_job");

            //services.AddScoped<JobRepository>();
            //services.AddScoped<JobRunLogRepository>();
            //services.AddScoped<JobFactory>();
            //services.AddScoped<JobService>();
            //services.AddScoped<JobListener>();

            //services.AddSingleton<JobRepository>();
            services.AddSingleton<JobFactory>();
            services.AddSingleton<JobRunLogRepository>();
            //services.AddSingleton<IJob,TestJob>();
            //services.AddTransient<BaseJob>();

            //注册业务Job
            //var type = typeof(IJob);
            //Assembly assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/") + $"JobDllImport/GLJob.Manage.dll");
            //var implements = assembly.GetTypes().Where(t => t.IsClass && type.IsAssignableFrom(t));
            //foreach (var item in implements)
            //{
            //    services.AddSingleton(item);
            //}

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
