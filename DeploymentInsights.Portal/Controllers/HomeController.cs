using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using DeploymentInsights.Portal.Models;
using DeploymentInsights.Portal.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Controllers
{
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class HomeController : Controller
    {
        private readonly IDataProvider _data_provider;

        public HomeController(IDataProvider data_provider)
        {
            _data_provider = data_provider;
        }

        public IActionResult Index(string team_code, string product_code, int number_of_months, DateTime date)
        {
            if (product_code == null && team_code == null)
            {
                return View("NoData");
            }

            var homeVM = new HomeVM
            {
                TeamCode = team_code,
                ProductCode = product_code,
                NumberOfMonths = number_of_months
            };

            var reporting_date = DateTime.Now;

            if (date == DateTime.MinValue)
            {
                homeVM.ReportingDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                homeVM.ReportingDate = date.ToString("yyyy-MM-dd");
                reporting_date = date;
            }

            List<Deployment> all_deployments = null;
            List<Deployment> deployments = null;

            if (product_code != null)
            {
                var product = _data_provider.GetProduct(product_code);
                homeVM.Title = $"Product : {product.Name}";

                all_deployments = _data_provider.GetProductDeployments(product_code);
                deployments = all_deployments.Where(x => x.Date <= reporting_date).OrderBy(x => x.Date).ToList();

            }
            if (team_code != null)
            {
                var team = _data_provider.GetTeam(team_code);
                homeVM.Title = $"Team : {team.Name}";

                all_deployments = _data_provider.GetTeamDeployments(team_code);
                deployments = all_deployments.Where(x => x.Date <= reporting_date).OrderBy(x => x.Date).ToList();

            }

            var latest_deployment = deployments.LastOrDefault();

            homeVM.AverageSummary = CreateAverageSummary(latest_deployment.InsightSummary);

            Deployment last_faild_deployment = deployments.Where(x => x.WasSuccessful == false).OrderBy(x => x.Date).Last();
            homeVM.AverageSummary.MeanTimeToRecovery.DaysSinceLastFailure = (int)Math.Round((reporting_date - last_faild_deployment.Date).TotalDays, 2);

            if (homeVM.AverageSummary.MeanTimeToRecovery.DaysSinceLastFailure <= 30)
            {
                homeVM.AverageSummary.MeanTimeToRecovery.DirectionClass = "fa-check-circle text-success";
            }
            else if (homeVM.AverageSummary.MeanTimeToRecovery.DaysSinceLastFailure > 30 & homeVM.AverageSummary.MeanTimeToRecovery.DaysSinceLastFailure < 90)
            {
                homeVM.AverageSummary.MeanTimeToRecovery.DirectionClass = "fa-exclamation-circle text-warning";
            }
            else
            {
                homeVM.AverageSummary.MeanTimeToRecovery.DirectionClass = "fa-exclamation-triangle text-danger";
            }

            ConstructApiAddresses(team_code, product_code, number_of_months, date, homeVM);

            return View(homeVM);
        }


        public IActionResult DeploymentInfo(string deployment_id)
        {
            var deployment = _data_provider.GetDeployment(deployment_id);

            return View(deployment);
        }
        public IActionResult Deployments(string team_code, string product_code, DateTime date)
        {
            if (product_code == null && team_code == null)
            {
                return View("NoData");
            }

            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }

            List<Deployment> all_deployments = null;
            List<Deployment> deployments = null;

            if (product_code != null)
            {
                var product = _data_provider.GetProduct(product_code);

                all_deployments = _data_provider.GetProductDeployments(product_code);
            }
            if (team_code != null)
            {
                var team = _data_provider.GetTeam(team_code);

                all_deployments = _data_provider.GetTeamDeployments(team_code);
            }

            for (int i = 1; i < all_deployments.Count; i++)
            {
                all_deployments[i].DaysSincePreviousDeployment = Math.Round((all_deployments[i].Date - all_deployments[i - 1].Date).TotalDays,2);
            }

            deployments = all_deployments.Where(x => x.Date <= date).OrderBy(x => x.Date).ToList();

            return View(deployments);
        }
        public IActionResult MeanTimeToRecoveryInfo(string team_code, string product_code, DateTime date)
        {
            if (product_code == null && team_code == null)
            {
                return View("NoData");
            }

            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }

            var recent_failures = _data_provider.GetRecentFailures(team_code, product_code, date);

            return View(recent_failures);
        }

        private static void ConstructApiAddresses(string team_code, string product_code, int number_of_months, DateTime date, HomeVM homeVM)
        {
            if (!string.IsNullOrWhiteSpace(product_code))
            {
                homeVM.DeploymentIntervalApiAddress = $"/api/product/GetDeploymentIntervals/{product_code}/{number_of_months}";
                homeVM.DeploymentFrequencyApiAddress = $"/api/product/GetDeploymentFrequency/{product_code}/{number_of_months}";
                homeVM.LeadTimeApiAddress = $"/api/product/GetLeadTime/{product_code}/{number_of_months}";
                homeVM.ChangeFailRateApiAddress = $"/api/product/GetChangeFailRate/{product_code}/{number_of_months}";
                homeVM.TimeToRestoreApiAddress = $"/api/product/GetTimeToRecover/{product_code}/{number_of_months}";
            }
            else
            {
                homeVM.DeploymentIntervalApiAddress = $"/api/product/GetDeploymentIntervals/{team_code}/{number_of_months}";
                homeVM.DeploymentFrequencyApiAddress = $"/api/team/GetDeploymentFrequency/{team_code}/{number_of_months}";
                homeVM.LeadTimeApiAddress = $"/api/team/GetLeadTime/{team_code}/{number_of_months}";
                homeVM.ChangeFailRateApiAddress = $"/api/team/GetChangeFailRate/{team_code}/{number_of_months}";
                homeVM.TimeToRestoreApiAddress = $"/api/team/GetTimeToRecover/{team_code}/{number_of_months}";
            }

            if (date != DateTime.MinValue)
            {
                homeVM.DeploymentIntervalApiAddress = homeVM.DeploymentIntervalApiAddress + $"?date={homeVM.ReportingDate}";
                homeVM.DeploymentFrequencyApiAddress = homeVM.DeploymentFrequencyApiAddress + $"?date={homeVM.ReportingDate}";
                homeVM.LeadTimeApiAddress = homeVM.LeadTimeApiAddress + $"?date={homeVM.ReportingDate}";
                homeVM.ChangeFailRateApiAddress = homeVM.ChangeFailRateApiAddress + $"?date={homeVM.ReportingDate}";
                homeVM.TimeToRestoreApiAddress = homeVM.TimeToRestoreApiAddress + $"?date={homeVM.ReportingDate}";
            }
        }

        private AverageSummary CreateAverageSummary(InsightSummary insightSummary)
        {
            Metrics metrics = JsonConvert.DeserializeObject<Metrics>(System.IO.File.ReadAllText("FourKeyMetricsBands.json"));

            var result = new AverageSummary
            {
                AverageDeploymentFrequency = Calculate(insightSummary.DeploymentFrequency, metrics.DeploymentFrequency),
                AverageLeadTime = Calculate(insightSummary.LeadTime, metrics.LeadTime),
                AverageChangeFailRate = Calculate(insightSummary.ChangeFailRate, metrics.ChangeFailRate),
                MeanTimeToRecovery = CalculateMTTR(insightSummary.MTTR, metrics.MTTR)
            };

            return result;
        }

        private AverageMetric Calculate(AverageMetric metric, List<Threshold> thresholds)
        {
            var result = new AverageMetric
            {
                Average = metric.Average,
                PreviousAverage = metric.PreviousAverage,
                Movement = metric.Movement,
            };

            var threshold = thresholds.Where(x => x.Min <= result.Average && x.Max >= result.Average).FirstOrDefault();
            if (threshold != null)
            {
                result.AverageClass = threshold.Class;
            }

            var change = result.Average - result.PreviousAverage;

            switch (change)
            {
                case double n when n > 0:
                    result.DirectionClass = "fa-caret-up text-danger";
                    break;
                case double n when n < 0:
                    result.DirectionClass = "fa-caret-down text-success";
                    break;
                default:
                    result.DirectionClass = "fa-arrows-h";
                    break;
            }

            return result;
        }

        private AverageMetric CalculateMTTR(AverageMetric metric, List<Threshold> thresholds)
        {

            var result = new AverageMetric
            {
                Average = metric.Average,
                PreviousAverage = metric.PreviousAverage,
                Movement = metric.Movement,
            };

            var threshold = thresholds.Where(x => x.Min <= result.Average && x.Max >= result.Average).FirstOrDefault();
            if (threshold != null)
            {
                result.AverageClass = threshold.Class;
            }


            return result;
        }
    }
}
