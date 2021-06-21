using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.Models
{
    public class ChartingFactory
    {

        public ChartDataResult BuildDeploymentFrequencyResult(string date, int months, IDataProvider data_provider, List<Product> products)
        {
            var result = new ChartDataResult();

            Metrics metrics = JsonConvert.DeserializeObject<Metrics>(System.IO.File.ReadAllText("FourKeyMetricsBands.json"));
            var high = metrics.DeploymentFrequency.Where(x => x.Name.ToLower() == "high").First();

            var current_date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(date))
            {
                current_date = DateTime.Parse(date);
            }

            DateTime from = DateTime.MaxValue;
            DateTime to = DateTime.MinValue;

            foreach (var product in products)
            {
                var deployments = data_provider.GetProductDeployments(product.Code).Where(x => x.Date >= current_date.AddMonths(0 - months) && x.Date <= current_date).OrderBy(x => x.Date);

                if (deployments.First().Date < from)
                {
                    from = deployments.First().Date;
                }

                if (deployments.Last().Date > to)
                {
                    to = deployments.Last().Date;
                }

                var pdr = new ProductDataResult
                {
                    Code = product.Code,
                    Name = product.Name
                };

                foreach (var deployment in deployments.Where(x => x.WasSuccessful == false))
                {
                    pdr.Failures.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.DeploymentFrequency, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                foreach (var deployment in deployments)
                {
                    pdr.Data.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.DeploymentFrequency, Version = deployment.Version, ClickThroughData = deployment.Guid });

                    result.DeploymentInterval.Add(new ChartData { Date = deployment.Date, y = deployment.DaysSincePreviousDeployment, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                result.Products.Add(pdr);
            }

            result.Elite = new List<ChartData>
            {
                new ChartData { Date = from.AddDays(0 - 10), y = high.Max },
                new ChartData { Date = to.AddDays(10), y = high.Max }
            };

            result.DataForTrending = new List<ChartData>();
            var temp = new List<ChartData>();
            foreach (var product in result.Products)
            {
                temp.AddRange(product.Data);
            }
            result.DataForTrending = temp.OrderBy(x => x.Date).ToList();

            return result;
        }

        public ChartDataResult BuildLeadTimeResult(string date, int months, IDataProvider data_provider, List<Product> products)
        {
            var result = new ChartDataResult();

            Metrics metrics = JsonConvert.DeserializeObject<Metrics>(System.IO.File.ReadAllText("FourKeyMetricsBands.json"));
            var high = metrics.LeadTime.Where(x => x.Name.ToLower() == "high").First();

            var current_date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(date))
            {
                current_date = DateTime.Parse(date);
            }

            DateTime from = DateTime.MaxValue;
            DateTime to = DateTime.MinValue;

            foreach (var product in products)
            {
                var deployments = data_provider.GetProductDeployments(product.Code).Where(x => x.Date >= current_date.AddMonths(0 - months) && x.Date <= current_date).OrderBy(x => x.Date);

                if (deployments.First().Date < from)
                {
                    from = deployments.First().Date;
                }

                if (deployments.Last().Date > to)
                {
                    to = deployments.Last().Date;
                }

                var pdr = new ProductDataResult
                {
                    Code = product.Code,
                    Name = product.Name
                };

                foreach (var deployment in deployments.Where(x => x.WasSuccessful == false))
                {
                    pdr.Failures.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.AverageLeadTime, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                foreach (var deployment in deployments)
                {
                    pdr.Data.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.AverageLeadTime, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                result.Products.Add(pdr);
            }

            result.Elite = new List<ChartData>
            {
                new ChartData { Date = from.AddDays(0 - 10), y = high.Max },
                new ChartData { Date = to.AddDays(10), y = high.Max }
            };

            result.DataForTrending = new List<ChartData>();
            var temp = new List<ChartData>();
            foreach (var product in result.Products)
            {
                temp.AddRange(product.Data);
            }
            result.DataForTrending = temp.OrderBy(x => x.Date).ToList();

            return result;
        }

        public ChartDataResult BuidlDeploymentIntervalsResult(string date, int months, List<Deployment> deployments)
        {
            var result = new ChartDataResult();

            var current_date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(date))
            {
                current_date = DateTime.Parse(date);
            }

            DateTime from = DateTime.MaxValue;
            DateTime to = DateTime.MinValue;


            var deployment_window = deployments.Where(x => x.Date >= current_date.AddMonths(0 - months) && x.Date <= current_date).OrderBy(x => x.Date);

            var pdr = new ProductDataResult();
            foreach (var deployment in deployment_window)
            {
                pdr.Data.Add(new ChartData { Date = deployment.Date, y = deployment.DaysSincePreviousDeployment, Version = deployment.Version, ClickThroughData = deployment.Guid });

            }
            result.Products.Add(pdr);

            return result;
        }

        public ChartDataResult BuildChangeFailRateResult(string date, int months, IDataProvider data_provider, List<Product> products)
        {
            var result = new ChartDataResult();

            Metrics metrics = JsonConvert.DeserializeObject<Metrics>(System.IO.File.ReadAllText("FourKeyMetricsBands.json"));
            var high = metrics.ChangeFailRate.Where(x => x.Name.ToLower() == "high").First();

            var current_date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(date))
            {
                current_date = DateTime.Parse(date);
            }

            DateTime from = DateTime.MaxValue;
            DateTime to = DateTime.MinValue;

            foreach (var product in products)
            {
                var deployments = data_provider.GetProductDeployments(product.Code).Where(x => x.Date >= current_date.AddMonths(0 - months) && x.Date <= current_date).OrderBy(x => x.Date);

                if (deployments.First().Date < from)
                {
                    from = deployments.First().Date;
                }

                if (deployments.Last().Date > to)
                {
                    to = deployments.Last().Date;
                }

                var pdr = new ProductDataResult
                {
                    Code = product.Code,
                    Name = product.Name
                };

                foreach (var deployment in deployments.Where(x => x.WasSuccessful == false))
                {
                    pdr.Failures.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.ChangeFailRate, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                foreach (var deployment in deployments)
                {
                    pdr.Data.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.ChangeFailRate, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                result.Products.Add(pdr);
            }

            result.Elite = new List<ChartData>
            {
                new ChartData { Date = from.AddDays(0 - 10), y = high.Max },
                new ChartData { Date = to.AddDays(10), y = high.Max }
            };

            result.DataForTrending = new List<ChartData>();
            var temp = new List<ChartData>();
            foreach (var product in result.Products)
            {
                temp.AddRange(product.Data);
            }
            result.DataForTrending = temp.OrderBy(x => x.Date).ToList();

            return result;
        }

        public ChartDataResult BuildTimeToRecoverResult(string date, int months, IDataProvider data_provider, List<Product> products)
        {
            var result = new ChartDataResult();

            Metrics metrics = JsonConvert.DeserializeObject<Metrics>(System.IO.File.ReadAllText("FourKeyMetricsBands.json"));
            var high = metrics.MTTR.Where(x => x.Name.ToLower() == "high").First();

            var current_date = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(date))
            {
                current_date = DateTime.Parse(date);
            }

            result.Range = new ChartRange
            {
                Start = current_date.AddMonths(0 - months).ToString("yyyy-MM-dd"),
                End = current_date.ToString("yyyy-MM-dd")
            };

            foreach (var product in products)
            {
                var deployments = data_provider.GetProductDeployments(product.Code).Where(x => x.Date >= current_date.AddMonths(0 - months) && x.Date <= current_date).OrderBy(x => x.Date);

                var pdr = new ProductDataResult
                {
                    Code = product.Code,
                    Name = product.Name
                };

                foreach (var deployment in deployments.Where(x => x.WasSuccessful == false))
                {
                    pdr.Failures.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.TimeToRestore, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                foreach (var deployment in deployments)
                {
                    pdr.Data.Add(new ChartData { Date = deployment.Date, y = deployment.Insights.TimeToRestore, Version = deployment.Version, ClickThroughData = deployment.Guid });
                }

                result.Products.Add(pdr);
            }

            result.Elite = new List<ChartData>
            {
                new ChartData { Date = current_date.AddMonths(0 - months).AddDays(0 - 10), y = high.Max },
                new ChartData { Date = current_date.AddDays(10), y = high.Max }
            };

            result.DataForTrending = new List<ChartData>();
            var temp = new List<ChartData>();
            foreach (var product in result.Products)
            {
                temp.AddRange(product.Failures);
            }
            result.DataForTrending = temp.OrderBy(x => x.Date).ToList();

            return result;
        }
    }
}
