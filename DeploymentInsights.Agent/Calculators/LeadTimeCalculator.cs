using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentInsights.Agent.Calculators
{
    public class LeadTimeCalculator
    {
        public void Calculate(List<Deployment> deployments)
        {
            var ordered_deployments = deployments.OrderBy(x => x.Date).ToList();

            for (int i = 1; i < ordered_deployments.Count; i++)
            {
                if (ordered_deployments[i].SimpleCommits.Any())
                {
                    ordered_deployments[i].Insights.AverageLeadTime = CalculateMedianDeliveryLeadTime(ordered_deployments[i].SimpleCommits, ordered_deployments[i].Date);
                    
                    var previous = ordered_deployments[i - 1].InsightSummary.LeadTime.Average;
                    var current = CalculateAverage(deployments, ordered_deployments[i].Date, 10);

                    ordered_deployments[i].InsightSummary.LeadTime.PreviousAverage = Math.Round(previous, 2);
                    ordered_deployments[i].InsightSummary.LeadTime.Average = Math.Round(current, 2);
                    ordered_deployments[i].InsightSummary.LeadTime.Movement = Math.Round(current - previous, 2);
                }
            }
        }

        public double CalculateAverage(List<Deployment> deployments, DateTime date, int number)
        {
            var window = deployments.Where(x => x.Date <= date).OrderByDescending(x => x.Date).ToList();

            var lastX = window.Take(number).ToList();

            var sum = lastX.Sum(x => x.Insights.AverageLeadTime);

            var result = sum / number;

            return Math.Round(result, 2);
        }

        private double CalculateMedianDeliveryLeadTime(List<CommitSummary> commits, DateTime date)
        {
            List<double> lead_times = new List<double>();

            foreach (var commit in commits)
            {
                var lead_time = (date - commit.Date).TotalDays;
                lead_times.Add(lead_time);
            }

            var orderedLeadTimes = lead_times.OrderBy(d => d).ToList(); ;

            var median_value = GetMedian(orderedLeadTimes);
            return (double) Math.Round(median_value, 2);
        }
        private double CalculateAverageDeliveryLeadTime(List<CommitSummary> commits, DateTime date)
        {
            List<double> lead_times = new List<double>();

            foreach (var commit in commits)
            {
                var lead_time = (date - commit.Date).TotalDays;
                lead_times.Add(lead_time);
            }

            var total_lead_time = lead_times.Sum(x => x);
            var count = lead_times.Count();

            return (double)(total_lead_time / count);
        }
        private double CalculateMaximumDeliveryLeadTime(List<CommitSummary> commits, DateTime date)
        {
            List<double> lead_times = new List<double>();

            foreach (var commit in commits)
            {
                var lead_time = (date - commit.Date).TotalDays;
                lead_times.Add(lead_time);
            }

            var orderedLeadTimes = lead_times.OrderBy(d => d).ToList();

            return orderedLeadTimes.Last();
        }

        public decimal GetMedian(List<double> list)
        {
            double[] tempArray = list.ToArray();
            int count = tempArray.Length;

            Array.Sort(tempArray);

            decimal medianValue = 0;

            if (count % 2 == 0)
            {
                // count is even, need to get the middle two elements, add them together, then divide by 2
                var middleElement1 = tempArray[count / 2 - 1];
                var middleElement2 = tempArray[count / 2];
                medianValue = (decimal)((middleElement1 + middleElement2) / 2);
            }
            else
            {
                // count is odd, simply get the middle element.
                medianValue = (decimal)tempArray[count / 2];
            }

            return Math.Round(medianValue,2);
        }
    }
}
