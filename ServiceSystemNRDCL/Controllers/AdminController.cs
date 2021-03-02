using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL.Controllers
{
    public class AdminController : Controller
    {
        [Route("Admin/index",Name ="Admin")]
        public ViewResult Index()
        {
            return View();
        }
    }
}
