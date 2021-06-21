using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentInsights.Agent.Calculators
{
    public class ChangeFailRateCalculator
    {
        public void Calculate(List<Deployment> deployments)
        {
            var ordered_deployments = deployments.OrderBy(x => x.Date).ToList();

            var number = 10;
            for (int i = number; i < ordered_deployments.Count; i++)
            {
                var deployments_in_range = ordered_deployments.Skip(i - (number - 1)).Take(number).ToList();

                int number_of_failures = deployments_in_range.Count(x => x.WasSuccessful == false);
                decimal rate = decimal.Divide(number_of_failures, deployments_in_range.Count());

                ordered_deployments[i].Insights.ChangeFailRate = (double)(rate * 100);

                var previous = ordered_deployments[i - 1].InsightSummary.ChangeFailRate.Average;
                var current = CalculateAverage(deployments, ordered_deployments[i].Date,10);

                ordered_deployments[i].InsightSummary.ChangeFailRate.PreviousAverage = previous;
                ordered_deployments[i].InsightSummary.ChangeFailRate.Average = current;
                ordered_deployments[i].InsightSummary.ChangeFailRate.Movement = Math.Round(current - previous, 2);
            }

        }

        public double CalculateAverage(List<Deployment> deployments, DateTime date, int number)
        {
            var window = deployments.Where(x => x.Date <= date).OrderByDescending(x => x.Date).ToList();

            var lastX = window.Take(number).ToList();

            var sum = lastX.Sum(x => x.Insights.ChangeFailRate);

            var result = sum / number;

            return Math.Round(result, 2);
        }
    }
}
