using DeploymentInsights.Portal.Interfaces;
using DeploymentInsights.Portal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.ViewComponents
{
    public class SideMenu : ViewComponent
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDataProvider _data_provider;

        public SideMenu(IWebHostEnvironment env, IDataProvider data_provider)
        {
            _env = env;
            _data_provider = data_provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var teams = _data_provider.GetTeams();

            return await Task.FromResult((IViewComponentResult)View("SideMenu", teams));
        }
    }
}
