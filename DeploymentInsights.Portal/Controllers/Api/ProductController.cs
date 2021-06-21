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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDataProvider _data_provider;

        public ProductController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _data_provider.GetProducts();

            return Ok(products);
        }

        [HttpDelete("{product_code}")]
        public IActionResult Delete(string product_code)
        {
            _data_provider.DeleteProduct(product_code);

            return Ok();
        }

        [HttpGet("{product_code}")]
        public IActionResult Get(string product_code, string date)
        {
            DateTime reporting_date;

            if (!string.IsNullOrWhiteSpace(date))
            {
                reporting_date = DateTime.Parse(date);
            }
            else
            {
                reporting_date = DateTime.Now;
            }

            var product = _data_provider.GetProduct(product_code);

            if (product != null)
            {
                product.Deployments = _data_provider.GetProductDeployments(product_code).Where(x => x.Date <= reporting_date).OrderBy(x => x.Date).ToList();
            }

            return Ok(product);
        }
    }
}
