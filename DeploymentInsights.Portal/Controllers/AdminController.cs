using DeploymentInsights.Portal.Interfaces;
using DeploymentInsights.Portal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDataProvider _data_provider;

        public AdminController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }
        public IActionResult Reset()
        {
            _data_provider.Reset();

            return RedirectToAction("Index", "Home");
        }
    }
}
