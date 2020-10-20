using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobManage.Service
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                IJobDetail jobDetail = bundle.JobDetail;
                Console.Out.WriteLineAsync($"jobDetailCode：{jobDetail.GetHashCode()}");
                Type jobType = typeof(TestJob);
                Console.Out.WriteLineAsync($"jobTypeCode：{jobType.GetHashCode()}");
                var temp = _serviceProvider.GetServices<IJob>().First();
                Console.Out.WriteLineAsync($"NewJobCode：{temp.GetHashCode()}");
                return temp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public virtual IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        //{
        //    try
        //    {
        //        IJobDetail jobDetail = bundle.JobDetail;
        //        Type jobType = jobDetail.JobType;
        //        return _serviceProvider.GetService(jobType) as IJob;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public virtual void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
