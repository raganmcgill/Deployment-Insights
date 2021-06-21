using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DeploymentInsights.Models
{
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<string> GitLocations { get; set; }

        public string TeamCode { get; set; }
        public List<Deployment> Deployments { get; set; }
    }
}
