using DeploymentInsights.Models;
using DeploymentInsights.Portal.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeploymentInsights.Portal.DataProviders
{
    public class FileBasedDataProvider : IDataProvider
    {
        private readonly IWebHostEnvironment _env;

        public FileBasedDataProvider(IWebHostEnvironment env)
        {
            _env = env;
        }

        public List<Product> GetProducts()
        {
            var results = new List<Product>();
            var data_path = Path.Combine(_env.WebRootPath, "data/teams");

            if (Directory.Exists(data_path))
            {
                var files = Directory.GetFiles(data_path, "*.json", SearchOption.AllDirectories).ToList();

                foreach (var file in files)
                {
                    Team t = JsonConvert.DeserializeObject<Team>(File.ReadAllText(file));
                    foreach (var product in t.Products)
                    {
                        results.Add(product);
                    }
                }
            }

            return results;
        }
        public Product GetProduct(string product_code)
        {
            var products = GetProducts();
            var product = products.FirstOrDefault(x => x.Code.ToLower() == product_code.ToLower());
            for (int i = 1; i < product.Deployments.Count; i++)
            {
                product.Deployments[i].DaysSincePreviousDeployment = Math.Round((product.Deployments[i].Date - product.Deployments[i - 1].Date).TotalDays, 2);
            }

            return product;
        }
        public void DeleteProduct(string product_code)
        {
            var data_path = Path.Combine(_env.WebRootPath, $"data/deployments/products/{product_code}");

            Directory.Delete(data_path, true);
        }

        public List<Team> GetTeams()
        {
            var results = new List<Team>();
            var data_path = Path.Combine(_env.WebRootPath, "data/teams");

            if (Directory.Exists(data_path))
            {
                var files = Directory.GetFiles(data_path, "*.json", SearchOption.AllDirectories).ToList();

                foreach (var file in files)
                {
                    Team t = JsonConvert.DeserializeObject<Team>(File.ReadAllText(file));

                    for (int i = 0; i < t.Products.Count; i++)
                    {
                        var product_path = Path.Combine(_env.WebRootPath, $"data/deployments/products/{t.Products[i].Code}");
                        if (!Directory.Exists(product_path))
                        {
                            t.Products.RemoveAt(i);
                        }
                    }

                    results.Add(t);
                }

            }

            return results;
        }
        public Team GetTeam(string team_code)
        {
            var teams = GetTeams();
            return teams.FirstOrDefault(x => x.Code.ToLower() == team_code.ToLower());
        }
        public void SaveTeam(Team team)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, $"data/teams");
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{team.Code}.json";
            var filepath = Path.Combine(uploadPath, fileName);

            var s = JsonConvert.SerializeObject(team, Formatting.Indented);
            File.WriteAllText(filepath, s);
        }
        public void DeleteTeam(string team_code)
        {
            var team = GetTeam(team_code);

            foreach (var product in team.Products)
            {
                DeleteProduct(product.Code);
            }

            var file_path = Path.Combine(_env.WebRootPath, "data/teams", $"{team_code}.json");

            File.Delete(file_path);

            var data_path = Path.Combine(_env.WebRootPath, $"data/deployments/teams/{team_code}");

            Directory.Delete(data_path, true);
        }

        public void Reset()
        {

            var data_path = Path.Combine(_env.WebRootPath, "data");
            DirectoryInfo di = new DirectoryInfo(data_path);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public Deployment GetDeployment(string deployment_id)
        {
            var insights = new List<Deployment>();
            Deployment result = null;

            var data_path = Path.Combine(_env.WebRootPath, $"data/deployments/");

            if (Directory.Exists(data_path))
            {
                var files = Directory.GetFiles(data_path, "*.json", SearchOption.AllDirectories).ToList();
                foreach (var file in files)
                {
                    Deployment insight = JsonConvert.DeserializeObject<Deployment>(File.ReadAllText(file));
                    insights.Add(insight);
                }

                result = insights.FirstOrDefault(x => x.Guid == deployment_id);
            }

            return result;
        }

        public List<Deployment> GetProductDeployments(string product_code)
        {
            var insights = new List<Deployment>();

            var data_path = Path.Combine(_env.WebRootPath, $"data/deployments/products/{product_code}");
            if (Directory.Exists(data_path))
            {
                var files = Directory.GetFiles(data_path, "*.json", SearchOption.AllDirectories).ToList();
                foreach (var file in files)
                {
                    Deployment insight = JsonConvert.DeserializeObject<Deployment>(File.ReadAllText(file));
                    insights.Add(insight);
                }
            }

            var result = insights.OrderBy(x => x.Date).ToList();

            for (int i = 1; i < result.Count; i++)
            {
                result[i].DaysSincePreviousDeployment = Math.Round((result[i].Date - result[i - 1].Date).TotalDays, 2);
            }

            return result;
        }
        public List<Deployment> GetTeamDeployments(string team_code)
        {
            var insights = new List<Deployment>();

            var data_path = Path.Combine(_env.WebRootPath, $"data/deployments/teams/{team_code}");
            if (Directory.Exists(data_path))
            {
                var files = Directory.GetFiles(data_path, "*.json", SearchOption.AllDirectories).ToList();
                foreach (var file in files)
                {
                    Deployment insight = JsonConvert.DeserializeObject<Deployment>(File.ReadAllText(file));
                    insights.Add(insight);
                }
            }

            return insights;
        }

        public void SaveProductDeployment(string product_code, Deployment deployment)
        {
            deployment.Guid = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(_env.WebRootPath, $"data/deployments/products/{product_code}");
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{deployment.Date.ToString("yyyy MM dd HH mm ss")}.json";
            var filepath = Path.Combine(uploadPath, fileName);

            var s = JsonConvert.SerializeObject(deployment, Formatting.Indented);
            File.WriteAllText(filepath, s);

        }
        public void SaveTeamDeployment(string team_code, Deployment deployment)
        {
            deployment.Guid = Guid.NewGuid().ToString();

            var uploadPath = Path.Combine(_env.WebRootPath, $"data/deployments/teams/{team_code}");
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{deployment.Date.ToString("yyyy MM dd HH mm ss")}.json";
            var filepath = Path.Combine(uploadPath, fileName);

            var s = JsonConvert.SerializeObject(deployment, Formatting.Indented);
            File.WriteAllText(filepath, s);
        }

        public List<Deployment> GetRecentFailures(string team_code, string product_code, DateTime date)
        {
            List<Deployment> deployments = null;
            List<Deployment> result = null;

            
            if (!string.IsNullOrEmpty(product_code))
            {
                deployments = GetProductDeployments(team_code);
            }
            else if (!string.IsNullOrEmpty(team_code))
            {
                deployments = GetTeamDeployments(team_code);
            }

            result = deployments.Where(x=>x.Date<=date && x.WasSuccessful == false).OrderBy(x => x.Date).Reverse().Take(5).Reverse().ToList();

            return result;
        }

        public List<Deployment> GetProductDeploymentIntervals(string product_code)
        {
            List<Deployment> deployments = null;

            if (!string.IsNullOrEmpty(product_code))
            {
                deployments = GetProductDeployments(product_code);
            }

            for (int i = 1; i < deployments.Count; i++)
            {
                deployments[i].DaysSincePreviousDeployment = Math.Round((deployments[i].Date - deployments[i - 1].Date).TotalDays, 2);
            }

            return deployments;
        }
    }
}
