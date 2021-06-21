using System.Collections.Generic;

namespace DeploymentInsights.Models
{
    public class Team
    {
        public List<Product> Products { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public List<Deployment> Deployments { get; set; }
    }
}
