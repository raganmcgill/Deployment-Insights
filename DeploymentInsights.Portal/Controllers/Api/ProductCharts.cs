using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using DeploymentInsights.Portal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Controllers.Api
{
    [Route("api/product/[action]")]
    [ApiController]
    public class ProductChartingController : ControllerBase
    {
        private readonly IDataProvider _data_provider;

        public ProductChartingController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{months}")]
        public IActionResult GetDeploymentFrequency(int months, string date, string product_code)
        {
            var factory = new ChartingFactory();

            var product = _data_provider.GetProduct(product_code);
            var products = new List<Product> { product };

            ChartDataResult result = factory.BuildDeploymentFrequencyResult(date, months, _data_provider, products);

            return Ok(result);
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{months}")]
        public IActionResult GetLeadTime(int months, string date, string product_code)
        {
            var factory = new ChartingFactory();

            var product = _data_provider.GetProduct(product_code);
            var products = new List<Product> { product };

            ChartDataResult result = factory.BuildLeadTimeResult(date, months, _data_provider, products);

            return Ok(result);
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{months}")]
        public IActionResult GetChangeFailRate(int months, string date, string product_code)
        {
            var factory = new ChartingFactory();

            var product = _data_provider.GetProduct(product_code);
            var products = new List<Product> { product };

            ChartDataResult result = factory.BuildChangeFailRateResult(date, months, _data_provider, products);

            return Ok(result);
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{months}")]
        public IActionResult GetTimeToRecover(int months, string date, string product_code)
        {
            var factory = new ChartingFactory();

            var product = _data_provider.GetProduct(product_code);
            var products = new List<Product> { product };

            ChartDataResult result = factory.BuildTimeToRecoverResult(date, months, _data_provider, products);

            return Ok(result);
        }

        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{date}")]
        public IActionResult GetAverageSummary(string product_code, string date)
        {
            var reporting_date = DateTime.Parse(date);

            List<Deployment> deployments = _data_provider.GetProductDeployments(product_code).Where(x => x.Date <= reporting_date).OrderBy(x => x.Date).ToList();

            var latest_deployment = deployments.LastOrDefault();

            return Ok(latest_deployment.InsightSummary);
        }


        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{date}/{number_of_failures}")]
        public IActionResult GetLastXFailures(string product_code, string date, int number_of_failures)
        {
            var reporting_date = DateTime.Parse(date);

            List<Deployment> failed_deployments = _data_provider.GetProductDeployments(product_code).Where(x => x.Date <= reporting_date && x.WasSuccessful == false).OrderBy(x => x.Date).ToList();

            var last_X_failures = failed_deployments.OrderByDescending(x => x.Date).Take(number_of_failures);

            return Ok(last_X_failures);
        }


        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
        [HttpGet("{product_code}/{months}")]
        public IActionResult GetDeploymentIntervals(int months, string date, string product_code)
        {
            var factory = new ChartingFactory();

            var deployments = _data_provider.GetProductDeploymentIntervals(product_code);

            ChartDataResult result = factory.BuidlDeploymentIntervalsResult(date, months, deployments);

            return Ok(result);
        }
    }
}
