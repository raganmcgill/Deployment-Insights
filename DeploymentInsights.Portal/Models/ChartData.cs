using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Models
{
    public class ChartData
    {
        public string x
        {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            }
        }

        public double y { get; set; }

        public string Version { get; set; }

        public DateTime Date { get; set; }

        public string ClickThroughData { get; set; }
    }
}
