using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Controllers
{
    public class AdviceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DeploymentFrequency()
        {
            return View();
        }
        public IActionResult LeadTime()
        {
            return View();
        }
        public IActionResult ChangeFailRate()
        {
            return View();
        }
        public IActionResult TimeToRecover()
        {
            return View();
        }

    }
}
