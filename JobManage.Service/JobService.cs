using JobManage.Core;
using Quartz;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JobManage.Service
{
    public class JobService : IJob
    {
        private readonly JobRepository _jobRepository;
        private readonly JobRunLogRepository _jobRunLogRepository;
        public JobService(JobRepository jobRepository, JobRunLogRepository jobRunLogRepository)
        {
            _jobRepository = jobRepository;
            _jobRunLogRepository = jobRunLogRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            List<Job> jobs = await _jobRepository.GetListAsync();

            foreach (var job in jobs)
            {
                switch (job.Status)
                {
                    case (int)JobStatusEnum.ReadyRun:
                        await JobAdd(job, context);
                        break;
                    case (int)JobStatusEnum.ReadyRecover:
                        await JobRecover(job, context);
                        break;
                    case (int)JobStatusEnum.ReadyPause:
                    case (int)JobStatusEnum.Paused:
                        await JobPause(job, context);
                        break;
                    case (int)JobStatusEnum.ReadyDelete:
                    case (int)JobStatusEnum.Deleted:
                        await JobDelete(job, context);
                        break;
                    case (int)JobStatusEnum.Running:
                        await JobRunning(job, context);
                        break;
                }
            }
        }

        /// <summary>
        /// 任务添加
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task JobAdd(Job job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (!await context.Scheduler.CheckExists(jobKey))
                {
                    IJobDetail jobDetail = JobBuilder.Create<BaseJob>()
                            .WithIdentity(jobKey)
                            .UsingJobData("RequestUrl", job.RequestUrl)
                            .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(group, name)
                        .StartNow()
                        .WithCronSchedule(job.CronExpression)
                        .Build();

                    await context.Scheduler.ScheduleJob(jobDetail, trigger);
                    await _jobRepository.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Running);
                }
            }
            catch (Exception ex)
            {
                await _jobRunLogRepository.InsertAsync(new JobRunLog(job.Group, job.Name, 0, DateTime.Now, false, "任务新增失败：" + ex.StackTrace));
            }
        }

        /// <summary>
        /// 任务暂停
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        private async Task JobPause(Job job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (await context.Scheduler.CheckExists(jobKey))
                {
                    await context.Scheduler.PauseJob(jobKey);
                }
                await _jobRepository.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Paused);
            }
            catch (Exception ex)
            {
                await _jobRunLogRepository.InsertAsync(new JobRunLog(job.Group, job.Name, 0, DateTime.Now, false, "任务暂停失败：" + ex.StackTrace));
            }
        }

        /// <summary>
        /// 任务重新启动
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        private async Task JobRecover(Job job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (await context.Scheduler.CheckExists(jobKey))
                {
                    await context.Scheduler.ResumeJob(jobKey);
                    await _jobRepository.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Running);
                }
                else
                {
                    await JobAdd(job, context);
                }
            }
            catch (Exception ex)
            {
                await _jobRunLogRepository.InsertAsync(new JobRunLog(job.Group, job.Name, 0, DateTime.Now, false, "任务重启失败：" + ex.StackTrace));
            }
        }

        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task JobDelete(Job job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (await context.Scheduler.CheckExists(jobKey))
                {
                    await context.Scheduler.DeleteJob(jobKey);
                }
                await _jobRepository.UpdateStatusAsync(job.Id, (int)JobStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                await _jobRunLogRepository.InsertAsync(new JobRunLog(job.Group, job.Name, 0, DateTime.Now, false, "任务删除失败：" + ex.StackTrace));
            }
        }

        /// <summary>
        /// 运行中任务
        /// </summary>
        /// <param name="job"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task JobRunning(Job job, IJobExecutionContext context)
        {
            try
            {
                string group = job.Group;
                string name = job.Name;
                JobKey jobKey = new JobKey(name, group);
                if (!await context.Scheduler.CheckExists(jobKey))
                {
                    await JobAdd(job, context);
                }
            }
            catch (Exception ex)
            {
                await _jobRunLogRepository.InsertAsync(new JobRunLog(job.Group, job.Name, 0, DateTime.Now, false, "任务运行失败：" + ex.StackTrace));
            }
        }

        //private async Task<Type> GetClass(string className)
        //{
        //    try
        //    {
        //        string url = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/") + $"JobDllImport/GLJob.Manage.dll";
        //        Assembly assembly = Assembly.LoadFrom(url);
        //        Type type = assembly.GetType(className);
        //        if (type == null)
        //        {
        //            await _jobRunLogRepository.InsertAsync(new JobRunLog(JobHelper.BaseJobGroup, JobHelper.BaseJobName, 0, DateTime.Now, false, $"任务实例{className}获取失败"));
        //        }
        //        return type;
        //    }
        //    catch (Exception ex)
        //    {
        //        await _jobRunLogRepository.InsertAsync(new JobRunLog(JobHelper.BaseJobGroup, JobHelper.BaseJobName, 0, DateTime.Now, false, $"任务实例{className}获取失败：" + ex.StackTrace));
        //        return null;
        //    }
        //}

    }
}
