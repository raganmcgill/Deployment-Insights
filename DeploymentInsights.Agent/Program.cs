using DeploymentInsights.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using DeploymentInsights.Agent.Calculators;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using LoremNET;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace DeploymentInsights.Agent
{
    partial class Program
    {
        private static readonly Random _random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("DEPLOYMENT INSIGHT AGENT");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine($"Starting : {DateTime.Now.ToShortTimeString()}");
            Console.WriteLine("Getting configuration");
            var config = GetConfiguration();

            var d = DateTime.Now;

            Console.WriteLine("");
            Console.WriteLine("Building Deployment Information");

            foreach (var team in config.Teams)
            {
                Console.WriteLine($" - Building team : {team.Name}");

                var calculator = new Calculator();
                var git_helper = new GitHelper();

                foreach (var product in team.Products)
                {
                    Console.WriteLine($"  - Building product : {product.Name} ({product.GitLocations.Count} repositories)");

                    //product.TeamCode = team.Code;

                    var deployments1 = git_helper.GetProductDeployments(product);
                    var product_deployments = deployments1.Where(x => x.Date <= d).OrderBy(x => x.Date).ToList();

                    product.Deployments = calculator.Calculate(product_deployments);
                }

                var deployments2 = git_helper.GetTeamDeployments(team);
                var team_deployments = deployments2.Where(x => x.Date <= d).OrderBy(x => x.Date).ToList();

                team.Deployments = calculator.Calculate(team_deployments);

                Console.WriteLine("");
            }

            if (config.ObfuscateData)
            {
                Console.WriteLine("Obfuscating Data");
                Console.WriteLine("");

                AnonamiseData(config.Teams);
            }

            Console.WriteLine("Uploading Data");

            string address;
            foreach (var team in config.Teams)
            {
                Console.WriteLine($" - Uploading team {team.Name}");
                address = Path.Combine(config.ServerAddress, "api/team/UploadTeam");
                UploadTeam(address, team);

                Console.WriteLine($" - Uploading {team.Deployments.Count} team deployments");
                address = Path.Combine(config.ServerAddress, $"api/deployment/UploadTeamDeployment/{team.Code}");
                UploadDeployments(address, team.Deployments);

                foreach (var product in team.Products)
                {
                    Console.WriteLine($" - Uploading {product.Deployments.Count} product deployments");
                    address = Path.Combine(config.ServerAddress, $"api/deployment/UploadProductDeployment/{product.Code}");
                    UploadDeployments(address, product.Deployments);
                }
                Console.WriteLine("");
            }

            Console.WriteLine($"Finished : {DateTime.Now.ToShortTimeString()}");

            Console.ReadLine();
        }

        private static void UploadDeployments(string address, List<Deployment> deployments)
        {
            var i = 1;

            foreach (var deployment in deployments)
            {
                var client = new RestClient(address)
                {
                    Timeout = -1
                };

                var s = JsonConvert.SerializeObject(deployment, Formatting.Indented);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", s, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                Console.Write($"\r  - Completed {i} of {deployments.Count}");
                i++;
            }
            Console.WriteLine();
        }

        private static void AnonamiseData(List<Team> teams)
        {
            string alphabet = "_ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var team_number = 1;
            var product_number = 1;

            foreach (var team in teams)
            {
                Console.WriteLine($" - {team.Name} changed to {$"Team {team_number}"}");

                team.Code = $"team_{team_number}";
                team.Name = $"Team {team_number}";

                foreach (var product in team.Products)
                {
                    var letter = alphabet[product_number];

                    Console.WriteLine($" - {product.Name} changed to {$"Product {letter}"}");


                    product.Code = $"product_{letter}";
                    product.Name = $"Product {letter}";
                    product.TeamCode = team.Code;

                    var deployment_number = _random.Next(1, 999);
                    foreach (var deployment in product.Deployments)
                    {
                        deployment.Version = $"{product.Code} release/{deployment_number}";

                        foreach (var commit in deployment.SimpleCommits)
                        {
                            string random_words = LoremNET.Lorem.Words(5, 10);
                            commit.Message = random_words;
                        }

                        deployment_number++;
                    }

                    product_number++;
                }

                team_number++;
            }
        }

        private static void UploadTeam(string address, Team team)
        {
            var client = new RestClient(address)
            {
                Timeout = -1
            };

            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();

            jsonResolver.IgnoreProperty(typeof(Team), "Deployments");
            jsonResolver.IgnoreProperty(typeof(Team), "Insights");

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = jsonResolver
            };

            var s = JsonConvert.SerializeObject(team, Formatting.Indented, serializerSettings);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
        }

        private static Config GetConfiguration()
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));

            return config;
        }
    }

    public class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
    {
        private readonly Dictionary<Type, HashSet<string>> _ignores;
        private readonly Dictionary<Type, Dictionary<string, string>> _renames;

        public PropertyRenameAndIgnoreSerializerContractResolver()
        {
            _ignores = new Dictionary<Type, HashSet<string>>();
            _renames = new Dictionary<Type, Dictionary<string, string>>();
        }

        public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignores.ContainsKey(type))
                _ignores[type] = new HashSet<string>();

            foreach (var prop in jsonPropertyNames)
                _ignores[type].Add(prop);
        }

        public void RenameProperty(Type type, string propertyName, string newJsonPropertyName)
        {
            if (!_renames.ContainsKey(type))
                _renames[type] = new Dictionary<string, string>();

            _renames[type][propertyName] = newJsonPropertyName;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.DeclaringType, property.PropertyName))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
                property.PropertyName = newJsonPropertyName;

            return property;
        }

        private bool IsIgnored(Type type, string jsonPropertyName)
        {
            if (!_ignores.ContainsKey(type))
                return false;

            return _ignores[type].Contains(jsonPropertyName);
        }

        private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
        {
            Dictionary<string, string> renames;

            if (!_renames.TryGetValue(type, out renames) || !renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
            {
                newJsonPropertyName = null;
                return false;
            }

            return true;
        }
    }
}
