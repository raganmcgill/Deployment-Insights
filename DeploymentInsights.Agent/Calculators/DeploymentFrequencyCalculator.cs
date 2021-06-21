using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentInsights.Agent.Calculators
{
    public class DeploymentFrequencyCalculator
    {
        public void Calculate(List<Deployment> deployments)
        {
            var ordered_deployments = deployments.OrderBy(x => x.Date).ToList();
            var number = 10;

            for (int i = number; i < ordered_deployments.Count; i++)
            {
                var start = ordered_deployments[i - number].Date;
                var to = ordered_deployments[i].Date;

                var span = (to - start).TotalDays;

                var frequency = span / number;
                ordered_deployments[i].Insights.DeploymentFrequency = Math.Round(frequency,2);

                var previous = ordered_deployments[i-1].InsightSummary.DeploymentFrequency.Average;
                var current = CalculateAverage(ordered_deployments, ordered_deployments[i].Date, number);
                
                ordered_deployments[i].InsightSummary.DeploymentFrequency.PreviousAverage = previous;
                ordered_deployments[i].InsightSummary.DeploymentFrequency.Average = current;
                ordered_deployments[i].InsightSummary.DeploymentFrequency.Movement = Math.Round(current - previous, 2);
            }
        }

        public double CalculateAverage(List<Deployment> deployments, DateTime date, int number)
        {
            var window = deployments.Where(x => x.Date <= date).OrderByDescending(x => x.Date).ToList();

            var lastX = window.Take(number).ToList();

            var sum = lastX.Sum(x => x.Insights.DeploymentFrequency);

            var result = sum / number;

            return Math.Round(result, 2);
        }
    }
}
