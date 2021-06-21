using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.ViewModels
{
    public class MeanTimeToRecoveryInfoVM
    {
        public List<Deployment> Deployments { get; set; }
        public string TeamCode { get; set; }
        public string ProductCode { get; set; }
        public DateTime Date { get; set; }
    }
}
