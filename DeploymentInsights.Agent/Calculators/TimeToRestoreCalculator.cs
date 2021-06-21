using DeploymentInsights.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentInsights.Agent.Calculators
{
    public class TimeToRestoreCalculator
    {
        public void Calculate(List<Deployment> deployments)
        {
            var ordered_deployments = deployments.OrderBy(x => x.Date).ToList();
            var number_to_average_over = 5;

            var failed_deployments = new List<Deployment>();

            for (int i = 0; i < ordered_deployments.Count; i++)
            {
                var deployment = ordered_deployments[i];

                if (!deployment.WasSuccessful)
                {
                    var next_deployment = ordered_deployments[i + 1];
                    ordered_deployments[i].Insights.TimeToRestore = Math.Round((next_deployment.Date - deployment.Date).TotalHours,2);

                    failed_deployments.Add(ordered_deployments[i]);

                }

                var last_X_failures = failed_deployments.Where(x=>x.Date<= ordered_deployments[i].Date).OrderByDescending(x => x.Date).Take(number_to_average_over);
                if (last_X_failures.Any())
                {
                    var total = last_X_failures.Sum(x => x.Insights.TimeToRestore);

                   // ordered_deployments[i].InsightSummary.MTTR.Average = Math.Round(total / last_X_failures.Count(), 2);

                    var previous = ordered_deployments[i - 1].InsightSummary.MTTR.Average;
                    var current = Math.Round(total / last_X_failures.Count(), 2);

                    ordered_deployments[i].InsightSummary.MTTR.PreviousAverage = previous;
                    ordered_deployments[i].InsightSummary.MTTR.Average = current;
                    var movement = Math.Round(current - previous, 2);
                    ordered_deployments[i].InsightSummary.MTTR.Movement = movement;

                    var o_f = last_X_failures.OrderBy(x => x.Date).ToList();

                    var item = o_f.AsEnumerable().Reverse().FirstOrDefault();

                    if (item != null)
                    {
                        ordered_deployments[i].InsightSummary.MTTR.DaysSinceLastFailure = (int)(ordered_deployments[i].Date - item.Date).TotalDays;
                    }

                }


                //var all_failures = ordered_deployments.Where(x => x.WasSuccessful == false).OrderByDescending(x => x.Date).ToList();
                //if (all_failures.Any())
                //{
                //    ordered_deployments[i].Insights.TimeToRestore.DaysSinceLastFailure = (int)(ordered_deployments[i].Date - all_failures.First().Date).TotalDays;
                //}
            }

        }
    }
}
