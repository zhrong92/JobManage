using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobManage.Core;
using Microsoft.AspNetCore.Mvc;

namespace JobManage.Web.Controllers
{
    public class JobController : Controller
    {
        private readonly JobRepository _jobRepository;
        public JobController(JobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> List(int PageIndex, int PageSize, string SearchValue)
        {
            SearchValue = "";
            var task = _jobRepository.GetPagedListAsync(PageIndex, PageSize, SearchValue);
            var page = await task;
            return Result.Success(ApiStatusCode.succ, page.Datas, page.TotalCount);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return PartialView("_add");
        }

        public async Task<JsonResult> Add(JobAddInput input)
        {
            Job job = new Job()
            {
                Group = input.Group,
                Name = input.Name,
                Status = (int)JobStatusEnum.ReadyRun,
                CronExpression = input.CronExpression,
                Description = input.Description,
                RequestUrl = input.RequestUrl
            };
            await _jobRepository.InsertAsync(job);
            return Result.Success(ApiStatusCode.succ);
        }

        public async Task<JsonResult> UpdateStatus([FromForm]string id, int status)
        {
            bool succ = await _jobRepository.UpdateStatusAsync(id, status);
            if (succ)
            {
                return Result.Success(ApiStatusCode.succ);
            }
            else
            {
                return Result.Success(ApiStatusCode.error);
            }
        }
    }
}