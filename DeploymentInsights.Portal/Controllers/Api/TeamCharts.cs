using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using DeploymentInsights.Portal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Controllers.Api
{
    [Route("api/team/[action]")]
    [ApiController]
    public class TeamChartingController : ControllerBase
    {
        private readonly IDataProvider _data_provider;

        public TeamChartingController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }


        [HttpGet("{team_code}/{months}")]
        public IActionResult GetDeploymentFrequency(string team_code, int months, string date)
        {
            var factory = new ChartingFactory();

            var products = _data_provider.GetTeam(team_code).Products;

            ChartDataResult result = factory.BuildDeploymentFrequencyResult(date, months, _data_provider, products);

            return Ok(result);
        }


        [HttpGet("{team_code}/{months}")]
        public IActionResult GetLeadTime(int months, string date, string team_code)
        {
            var factory = new ChartingFactory();

            var products = _data_provider.GetTeam(team_code).Products;

            ChartDataResult result = factory.BuildLeadTimeResult(date, months, _data_provider, products);

            return Ok(result);
        }

        [HttpGet("{team_code}/{months}")]
        public IActionResult GetChangeFailRate(int months, string date, string team_code)
        {
            var factory = new ChartingFactory();

            var products = _data_provider.GetTeam(team_code).Products;

            ChartDataResult result = factory.BuildChangeFailRateResult(date, months, _data_provider, products);

            return Ok(result);
        }

        [HttpGet("{team_code}/{months}")]
        public IActionResult GetTimeToRecover(int months, string date, string team_code)
        {
            var factory = new ChartingFactory();

            var products = _data_provider.GetTeam(team_code).Products;

            ChartDataResult result = factory.BuildTimeToRecoverResult(date, months, _data_provider, products);

            return Ok(result);
        }

    }
}
