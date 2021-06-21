using System;
using System.Collections.Generic;

namespace DeploymentInsights.Models
{
    public class Deployment
    {
        public string Guid { get; set; }
        public DateTime Date { get; set; }
        public bool WasSuccessful { get; set; }
        public string Version { get; set; }

        public List<CommitSummary> SimpleCommits { get; set; }

        public Insights Insights { get; set; }
        public InsightSummary InsightSummary { get; set; }
        public string TeamGuid { get; set; }
        public string ProductGuid { get; set; }
        public double DaysSincePreviousDeployment { get; set; }

        public Deployment()
        {
            SimpleCommits = new List<CommitSummary>();
            Insights = new Insights();
            InsightSummary = new InsightSummary();
        }
    }
}
