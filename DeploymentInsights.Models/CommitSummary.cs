using System;

namespace DeploymentInsights.Models
{
    public class CommitSummary
    {
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
        public string SHA { get; set; }
        public double LeadTime { get; set; }
    }
}
