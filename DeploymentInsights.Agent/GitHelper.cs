using DeploymentInsights.Models;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeploymentInsights.Agent
{
    public class GitHelper
    {

        public List<Deployment> GetTeamDeployments(Team team)
        {
            var deployments = new List<Deployment>();

            foreach (var product in team.Products)
            {
                var product_deployments = GetProductDeployments(product);

                deployments.AddRange(product_deployments);
            }

            return deployments;
        }

        public List<Deployment> GetProductDeployments(Product product)
        {
            var deployments = new List<Deployment>();

            foreach (var git_location in product.GitLocations)
            {
                var product_deployments = GetDeployments(product, git_location);

                deployments.AddRange(product_deployments);
            }

            return deployments;
        }

        private List<Deployment> GetDeployments(Product product, string repo_location)
        {
            var deployments = new List<Deployment>();

            using (var repo = new Repository(repo_location))
            {
                var releases = GetReleases(repo);

                foreach (Tag t in releases.OrderBy(t => ((Commit)t.PeeledTarget).Author.When))
                {
                    if (t.Annotation != null)
                    {
                        var deployment = new Deployment
                        {
                            TeamGuid = product.TeamCode,
                            ProductGuid = product.Code,
                            Date = ((Commit)t.PeeledTarget).Author.When.LocalDateTime,
                            Version = t.FriendlyName,
                            WasSuccessful = true
                        };

                        if (t.FriendlyName.ToLower().Contains("fix"))
                        {
                            deployments.Last().WasSuccessful = false;
                        }

                        if (deployments.Any())
                        {
                            var commits = GetCommitSummaryForTag(repo, deployments.Last(), deployment);
                            deployment.SimpleCommits = commits;
                        }

                        deployments.Add(deployment);
                    }
                }
            }

            return deployments;
        }

        private List<CommitSummary> GetCommitSummaryForTag(Repository repo, Deployment from_deployment, Deployment to_deployment)
        {
            var simpleCommits = new List<CommitSummary>();

            Tag tagTo = repo.Tags[to_deployment.Version];
            Tag tagFrom = repo.Tags[from_deployment.Version];

            CommitFilter cf = new CommitFilter
            {
                SortBy = CommitSortStrategies.Reverse | CommitSortStrategies.Time,
                ExcludeReachableFrom = tagFrom.Target.Sha,
                IncludeReachableFrom = tagTo.Target.Sha
            };

            var results = repo.Commits.QueryBy(cf).ToList();

            var sanitised_commits = results;
            sanitised_commits.RemoveAll(x => x.Author.Email == "github-buildmonkey@red-gate.com");
            sanitised_commits.RemoveAll(x => x.Message.ToLower().Contains("merge pull"));

            foreach (var commit in sanitised_commits)
            {
                var simpleCommit = new CommitSummary
                {
                    Date = commit.Author.When,
                    Message = commit.MessageShort,
                    SHA = commit.Sha,
                    LeadTime = Math.Round((to_deployment.Date - commit.Author.When).TotalDays, 2)
                };

                simpleCommits.Add(simpleCommit);
            }

            return simpleCommits;
        }

        private List<Tag> GetReleases(Repository repo)
        {
            var tags = repo.Tags.OrderBy(t => ((Commit)t.PeeledTarget).Author.When).ToList();

            var releases = tags.Where(x => x.FriendlyName.ToLower().Contains("release")).ToList();

            if (!releases.Any())
            {
                releases = tags;
            }

            return releases;
        }
    }
}
