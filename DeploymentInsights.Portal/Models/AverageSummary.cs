using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Models
{
    public class Threshold
    {
        public string Name { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        public string Class { get; set; }
    }



    public class AverageSummary
    {
        public AverageMetric AverageDeploymentFrequency { get; set; }

        public AverageMetric AverageLeadTime { get; set; }

        public AverageMetric AverageChangeFailRate { get; set; }

        public AverageMetric MeanTimeToRecovery { get; set; }
    }

    public class Metrics
    {
        public List<Threshold> DeploymentFrequency { get; set; }
        public List<Threshold> LeadTime { get; set; }
        public List<Threshold> ChangeFailRate { get; set; }
        public List<Threshold> MTTR { get; set; }

        public Metrics()
        {
            DeploymentFrequency = new List<Threshold>();
            LeadTime = new List<Threshold>();
            ChangeFailRate = new List<Threshold>();
            MTTR = new List<Threshold>();
        }
    }
}
