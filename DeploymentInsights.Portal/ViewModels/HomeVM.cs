using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Models
{
    public class HomeVM
    {
        public string Title { get; set; }
        public string TeamCode { get; set; }
        public string ProductCode { get; set; }
        public int NumberOfMonths { get; set; }
        public string ReportingDate { get; set; }
        public AverageSummary AverageSummary { get; set; }

        public string DeploymentFrequencyApiAddress { get; set; }
        public string LeadTimeApiAddress { get; set; }
        public string ChangeFailRateApiAddress { get; set; }
        public string TimeToRestoreApiAddress { get; set; }
        public string DeploymentIntervalApiAddress { get; set; }
    }
}
