using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Models
{
    public class ChartDataResult
    {
        public ChartRange Range { get; set; }
        public List<ChartData> Elite { get; set; }

        public List<ProductDataResult> Products { get; set; }
        public List<ChartData> DataForTrending { get; set; }

        public List<ChartData> DeploymentInterval { get; internal set; }

        public ChartDataResult()
        {
            Elite = new List<ChartData>();
            Products = new List<ProductDataResult>();
            DeploymentInterval = new List<ChartData>();
        }
    }
}
