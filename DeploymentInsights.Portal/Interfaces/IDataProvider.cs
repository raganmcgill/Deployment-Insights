using DeploymentInsights.Models;
using System;
using System.Collections.Generic;

namespace DeploymentInsights.Portal.Interfaces
{
    public interface IDataProvider
    {
        void DeleteProduct(string product_code);
        void DeleteTeam(string team_code);
        Deployment GetDeployment(string deployment_id);
        Product GetProduct(string product_code);
        List<Deployment> GetProductDeployments(string product_code);
        List<Product> GetProducts();
        Team GetTeam(string team_code);
        List<Deployment> GetTeamDeployments(string team_code);
        List<Team> GetTeams();
        void Reset();
        void SaveProductDeployment(string product_code, Deployment deployment);
        void SaveTeamDeployment(string team_code, Deployment deployment);
        void SaveTeam(Team team);
        List<Deployment> GetRecentFailures(string team_code, string product_code, DateTime reporting_date);
        List<Deployment> GetProductDeploymentIntervals(string product_code);
    }
}