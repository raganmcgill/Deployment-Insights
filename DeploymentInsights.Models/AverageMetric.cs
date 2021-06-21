namespace DeploymentInsights.Models
{
    public class AverageMetric
    {
        public double Average { get; set; }
        public double PreviousAverage { get; set; }
        public double Movement { get; set; }

        public string DirectionClass { get; set; }

        public string AverageClass { get; set; }
        public int DaysSinceLastFailure { get; set; }
    }
}
