using System.Collections.Generic;

namespace DeploymentInsights.Portal.Models
{
    public class ProductDataResult { 
        public List<ChartData> Data { get; set; }

        public List<ChartData> Failures { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public ProductDataResult()
        {
            Data = new List<ChartData>();
            Failures = new List<ChartData>();
        }

    }
}
