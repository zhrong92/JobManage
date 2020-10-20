using JobManage.Core;
using Quartz;
using Quartz.Listener;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class JobListener : JobListenerSupport
    {
        private readonly JobRepository _jobRepository;
        private readonly JobRunLogRepository _jobRunLogRepository;

        public JobListener(JobRepository jobRepository, JobRunLogRepository jobRunLogRepository)
        {
            _jobRepository = jobRepository;
            _jobRunLogRepository = jobRunLogRepository;
        }

        public override string Name
        {
            get { return "jobListener"; }
        }

        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            string group = context.JobDetail.Key.Group;
            string name = context.JobDetail.Key.Name;
            DateTime fireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.DateTime, TimeZoneInfo.Local);

            DateTime? nextFireTimeUtc = null;
            if (context.NextFireTimeUtc != null)
            {
                nextFireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
            }

            if (!JobHelper.IsBaseJob(group, name))
            {
                //更新任务运行情况
                await _jobRepository.UpdateExecuteAsync(group, name, fireTimeUtc, nextFireTimeUtc);

                //记录运行日志
                double totalSeconds = context.JobRunTime.TotalSeconds;
                bool succ = true;
                string exception = string.Empty;
                if (jobException != null)
                {
                    succ = false;
                    exception = jobException.ToString();
                }

                JobRunLog log = new JobRunLog(group, name, totalSeconds, fireTimeUtc, succ, exception);
                await _jobRunLogRepository.InsertAsync(log);
            }
        }

    }
}
