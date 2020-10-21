using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobManage.Core;
using Microsoft.AspNetCore.Mvc;

namespace JobManage.Web.Controllers
{
    public class JobRunLogController : Controller
    {
        private readonly JobRunLogRepository _jobRunLogRepository;
        public JobRunLogController(JobRunLogRepository jobRunLogRepository)
        {
            _jobRunLogRepository = jobRunLogRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> List(int PageIndex, int PageSize, string SearchValue)
        {
            SearchValue = string.IsNullOrWhiteSpace(SearchValue) ? "" : SearchValue;
            var task = _jobRunLogRepository.GetPagedListAsync(PageIndex, PageSize, SearchValue);
            var page = await task;
            return Result.Success(ApiStatusCode.succ, page.Datas, page.TotalCount);
        }
    }
}