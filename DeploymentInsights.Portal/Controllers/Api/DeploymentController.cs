using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using DeploymentInsights.Portal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeploymentInsights.Portal.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeploymentController : ControllerBase
    {
        private readonly IDataProvider _data_provider;

        public DeploymentController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }

        [HttpGet("{product_code}")]
        public IActionResult GetProductDeployments(string product_code)
        {
            var insights = _data_provider.GetProductDeployments(product_code);
            return Ok(insights);
        }


        [HttpGet("{team_code}")]
        public IActionResult GetTeamDeployments(string team_code)
        {
            var insights = _data_provider.GetTeamDeployments(team_code);
            return Ok(insights);
        }



        [HttpPost("{product_code}")]
        public IActionResult UploadProductDeployment(string product_code, [FromBody] Deployment deployment)
        {
            _data_provider.SaveProductDeployment(product_code, deployment);

            return Ok(new { Product = product_code, Date = deployment.Date });
        }

        [HttpPost("{team_code}")]
        public IActionResult UploadTeamDeployment(string team_code, [FromBody] Deployment deployment)
        {
            _data_provider.SaveTeamDeployment(team_code, deployment);

            return Ok(new { Team = team_code, Date = deployment.Date });
        }
    }
}
