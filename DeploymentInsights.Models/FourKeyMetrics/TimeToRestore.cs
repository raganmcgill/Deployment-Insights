namespace DeploymentInsights.Models.FourKeyMetrics
{
    public class TimeToRestore
    {
        public double Time { get; set; }

        public int DaysSinceLastFailure { get; set; }
    }
}
