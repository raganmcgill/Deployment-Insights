using DeploymentInsights.Models.FourKeyMetrics;

namespace DeploymentInsights.Models
{
    public class InsightSummary
    {
        public AverageMetric DeploymentFrequency { get; set; }

        public AverageMetric LeadTime { get; set; }

        public AverageMetric ChangeFailRate { get; set; }

        public AverageMetric MTTR { get; set; }

        public InsightSummary()
        {
            DeploymentFrequency = new AverageMetric();
            LeadTime = new AverageMetric();
            ChangeFailRate = new AverageMetric();
            MTTR = new AverageMetric();
        }
    }
}
