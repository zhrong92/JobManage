using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JobManage.Web.Controllers
{
    public class JobRunLogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}