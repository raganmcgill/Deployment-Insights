using DeploymentInsights.Models;
using System.Collections.Generic;

namespace DeploymentInsights.Agent
{
    partial class Program
    {
        public class Config
        {
            public string ServerAddress { get; set; }

            public bool ObfuscateData { get; set; }

            public List<Team> Teams { get; set; }

            public Config()
            {
                Teams = new List<Team>();
            }
        }
    }
}
