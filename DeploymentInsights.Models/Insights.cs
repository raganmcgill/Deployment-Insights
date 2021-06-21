using DeploymentInsights.Models.FourKeyMetrics;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeploymentInsights.Models
{
    public class Insights
    {
        public double DeploymentFrequency { get; set; }
        public double AverageLeadTime { get; set; }
        public double ChangeFailRate { get; set; }
        public double TimeToRestore { get; set; }

        public Insights()
        {

        }
    }
}
